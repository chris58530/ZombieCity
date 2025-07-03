using UnityEngine;
using Zenject;

public class BattleFlowCmd : ICommand
{
    [Inject] private SelectLevelViewMediator selectLevelViewMediator;
    [Inject] private BattleProxy battleProxy;

    public override void Execute(MonoBehaviour mono)
    {
    }
}
