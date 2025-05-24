using UnityEngine;

public class DropItemViewMediator : IMediator
{
    private DropItemView view;
    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as DropItemView;
    }
}
