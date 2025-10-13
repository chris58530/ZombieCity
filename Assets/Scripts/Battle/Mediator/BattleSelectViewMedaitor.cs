using UnityEngine;

public class BattleSelectViewMedaitor : IMediator
{
    private BattleSelectView view;

    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as BattleSelectView;
    }

    public override void DeRegister(IView view)
    {
        base.DeRegister(view);
    }
}
