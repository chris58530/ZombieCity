using UnityEngine;
using Zenject;

public class BattleSurvivorViewMediator : IMediator
{
    private BattleSurvivorView view;

    public override void Register(IView view)
    {
        this.view = view as BattleSurvivorView;
    }

    //listener
    private void StartDetect()
    {
        view.StartDetect();
    }

    //listener
    private void ClaimRunner()
    {
        int runnerCount = view.buffRunnerCount;
        Debug.Log("Claim Runner: " + runnerCount);
        view.ResetView();
    }
}