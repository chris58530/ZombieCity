using DG.Tweening;
using UnityEngine;
using Zenject;

public class BattleSurvivorView : MonoBehaviour, IView
{
    [Inject] private BattleSurvivorViewMediator mediator;
    [SerializeField] private RunnerDetectCollider runnerDetectCollider;
    public int buffRunnerCount;

    private void Awake()
    {
        ResetView();
        InjectService.Instance.Inject(this);
    }

    private void OnEnable()
    {
        mediator.Register(this);
    }

    private void OnDisable()
    {
        mediator.DeRegister(this);
    }

    public void StartDetect()
    {
        runnerDetectCollider.onDetectRunner += () =>
        {
            buffRunnerCount++;
        };
        EnableRunnerDetectCollider(true);
    }

    public void EnableRunnerDetectCollider(bool isEnable)
    {
        runnerDetectCollider.gameObject.SetActive(isEnable);
    }

    public void ResetView()
    {
        EnableRunnerDetectCollider(false);
        buffRunnerCount = 0;
        DOTween.Kill(GetHashCode());
    }
}
