using UnityEngine;
using System.Linq;
using TMPro;
using System.Collections;
using DG.Tweening;
using Zenject;

public class InitController : MonoBehaviour
{
    [Inject] private Listener listener;
    [Inject] private DiContainer container;
    public StateControllerSetting InitStateControllerSetting;
    public StateControllerSetting GameStateControllerSetting;
    public TextMeshProUGUI progressText;
    public GameObject clickObject;
    public GameObject panel;
    private int totalCommands;
    private int completedCommands;

    private void OnEnable()
    {
        StartExecution();
        listener.RegisterListener(this);
        clickObject.SetActive(false);
    }

    private void StartExecution()
    {
        if (InitStateControllerSetting == null || InitStateControllerSetting.commands.Length == 0)
        {
            Transition();
            return;
        }

        totalCommands = InitStateControllerSetting.commands.Length;
        completedCommands = 0;
        UpdateProgress();

        foreach (var cmd in InitStateControllerSetting.commands)
        {
            cmd.Initialize(this, listener, container);
            cmd.Execute(this);
        }

        if (InitStateControllerSetting.commands.All(cmd => cmd.isLazy))
        {
            Transition();
        }
    }

    public void OnCmdComplete()
    {
        StartCoroutine(OnCmdCompleteCoroutine());
    }

    private IEnumerator OnCmdCompleteCoroutine()
    {
        yield return new WaitForSeconds(1f);

        completedCommands++;
        UpdateProgress();
        if (InitStateControllerSetting.commands.All(cmd => cmd.isComplete || cmd.isLazy))
        {
            Transition();
        }
    }

    private void UpdateProgress()
    {
        float progress = totalCommands > 0 ? (float)completedCommands / totalCommands * 100f : 100f;
        if (progressText != null)
        {
            progressText.text = $"Loading...";
        }
    }

    private void Transition()
    {
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

        listener.BroadCast(GameEvent.ON_INIT_GAME);
    }
    public void OnClick()
    {
        panel.SetActive(false);
        clickObject.SetActive(false);
        listener.BroadCast(CameraEvent.MOVE_TO_GAME_VIEW);
    }

}