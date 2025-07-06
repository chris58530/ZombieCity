using UnityEngine;
using System.Linq;
using TMPro;
using System.Collections;
using DG.Tweening;
using Zenject;
using System;

public class InitController : MonoBehaviour
{
    [Inject] private Listener listener;
    [Inject] private DiContainer container;
    [Inject] private GameStateProxy gameStateProxy;
    public StateControllerSetting InitStateControllerSetting;
    public StateControllerSetting GameStateControllerSetting;
    public StateControllerSetting BattleStateControllerSetting;
    public TextMeshProUGUI progressText;
    public GameObject clickObject;
    public GameObject panel;
    private void OnEnable()
    {
        OnInitState();
        listener.RegisterListener(this);
        clickObject.SetActive(false);
        QualitySettings.vSyncCount = 0;   // 把垂直同步關掉
        Application.targetFrameRate = 60;
    }

    private void OnInitState()
    {
        foreach (var cmd in InitStateControllerSetting.commands)
        {
            cmd.Initialize(this, listener, container);
            cmd.Execute(this);
        }

    }
    private bool hasStartedCompletion = false;

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
        switch (state)
        {
            case GameState.Game:
                listener.BroadCast(GameEvent.ON_GAME_STATE_END);
                CompleteState(BattleStateControllerSetting);
                ChangeState(GameStateControllerSetting, () =>
                {
                    listener.BroadCast(GameEvent.ON_GAME_STATE_START);

                });
                break;

            case GameState.Battle:
                listener.BroadCast(GameEvent.ON_GAME_STATE_END);
                clickObject.SetActive(false);
                CompleteState(GameStateControllerSetting);
                ChangeState(BattleStateControllerSetting, () =>
                {
                    listener.BroadCast(GameEvent.ON_BATTLE_STATE_START);
                    //todo 串連
                });
                break;
        }
    }
    private void CompleteState(StateControllerSetting stateControllerSetting)
    {
        foreach (var cmd in stateControllerSetting.commands)
        {
            if (!cmd.isLazy)
            {
                cmd.SetComplete();
            }
        }
    }
    private IEnumerator OnCmdCompleteCoroutine()
    {
        yield return new WaitForSeconds(1f);
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