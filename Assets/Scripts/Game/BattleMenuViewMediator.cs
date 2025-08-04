using UnityEngine;
using Zenject;

public class BattleMenuViewMediator : IMediator
{
    [Inject] private BattleMenuView view;

    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as BattleMenuView;
    }

    public override void DeRegister(IView view)
    {
        base.DeRegister(view);
    }

}
