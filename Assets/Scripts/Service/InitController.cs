using UnityEngine;
using System.Linq;
using TMPro;
using System.Collections;
using DG.Tweening;
using Zenject;
using System;
using System.Collections.Generic;

public class InitController : MonoBehaviour
{
    [Inject] private Listener listener;
    [Inject] private DiContainer container;
    [Inject] private GameStateProxy gameStateProxy;
    [Inject] private GameCameraProxy gameCameraProxy;
    public StateControllerSetting InitStateControllerSetting;
    public StateControllerSetting GameStateControllerSetting;
    public StateControllerSetting BattleStateControllerSetting;
    public TextMeshProUGUI progressText;
    public GameObject clickObject;
    public GameObject panel;

    private const int TARGET_FRAME_RATE = 60;
    private const float INIT_WAIT_TIME = 1f;

    private Dictionary<GameState, StateControllerSetting> stateSettings;
    private bool hasStartedCompletion = false;

    private void Awake()
    {
        stateSettings = new Dictionary<GameState, StateControllerSetting>
        {
            { GameState.Init, InitStateControllerSetting },
            { GameState.Game, GameStateControllerSetting },
            { GameState.Battle, BattleStateControllerSetting }
        };
    }

    private void OnEnable()
    {
        OnInitState();
        listener.RegisterListener(this);
        clickObject.SetActive(false);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = TARGET_FRAME_RATE;
    }

    private void OnInitState()
    {
        foreach (var cmd in InitStateControllerSetting.commands)
        {
            cmd.Initialize(this, listener, container);
            cmd.Execute(this);
        }
    }

    public void OnCmdComplete()
    {
        if (hasStartedCompletion) return;
        if (InitStateControllerSetting.commands.All(cmd => cmd.isComplete || cmd.isLazy))
        {
            hasStartedCompletion = true;
            StartCoroutine(OnCmdCompleteCoroutine());
        }
    }

    [Listener(GameEvent.REQUEST_CHANGE_STATE)]
    public void RequestChangeState()
    {
        GameState state = gameStateProxy.curState;
        if (state == GameState.Game)
        {
            HandleGameStateChange();
        }
        else if (state == GameState.Battle)
        {
            HandleBattleStateChange();
        }
    }

    private void HandleGameStateChange()
    {
        Debug.Log("Change to Game State");
        listener.BroadCast(GameEvent.ON_BATTLE_STATE_END);
        CompleteState(BattleStateControllerSetting);
        ChangeState(GameStateControllerSetting, () =>
        {
            gameCameraProxy.UseCamera(CameraType.Game);
            listener.BroadCast(GameEvent.ON_GAME_STATE_START);
        });
    }

    private void HandleBattleStateChange()
    {
        Debug.Log("Change to Battle State");
        listener.BroadCast(GameEvent.ON_GAME_STATE_END);
        clickObject.SetActive(false);
        CompleteState(GameStateControllerSetting);
        ChangeState(BattleStateControllerSetting, () =>
        {
            gameCameraProxy.UseCamera(CameraType.Battle);
            listener.BroadCast(GameEvent.ON_BATTLE_STATE_START);
        });
    }

    private void CompleteState(StateControllerSetting stateControllerSetting)
    {
        foreach (var cmd in stateControllerSetting.commands.Where(cmd => !cmd.isLazy))
        {
            cmd.SetComplete();
        }
    }

    private IEnumerator OnCmdCompleteCoroutine()
    {
        yield return new WaitForSeconds(INIT_WAIT_TIME);
        LogService.Instance.Log("ON_INIT_GAME");

        if (progressText != null)
        {
            progressText.text = $"Loading...";
        }
        clickObject.SetActive(true);
        if (progressText != null)
        {
            progressText.text = "Press to  Continue";
        }
        foreach (var cmd in GameStateControllerSetting.commands)
        {
            cmd.Initialize(this, listener, container);
            cmd.Execute(this);
        }
        listener.BroadCast(GameEvent.ON_GAME_STATE_START);
    }

    public void OnClick()
    {
        panel.SetActive(false);
        clickObject.SetActive(false);
        listener.BroadCast(CameraEvent.MOVE_TO_GAME_VIEW);
    }

    public void ChangeState(StateControllerSetting stateControllerSetting, Action callBack = null)
    {
        foreach (var cmd in stateControllerSetting.commands)
        {
            cmd.Initialize(this, listener, container);
            cmd.Execute(this);
        }
        callBack?.Invoke();
    }
}
public class GameStateProxy : IProxy
{
    public GameState curState = GameState.Init;
    public void RequestChangeState(GameState state)
    {
        curState = state;
        listener.BroadCast(GameEvent.REQUEST_CHANGE_STATE);
    }
}
public enum GameState
{
    Init,
    Game,
    Battle
}