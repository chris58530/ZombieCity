using UnityEngine;

public class DropItemViewMediator : IMediator
{
    private DropItemView view;
    public override void Register(IView view)
    {
        base.Register(view);
        this.view = view as DropItemView;
    }
    public void OnSpanwItem()
    {
        view.OnSpawnItem(DropItemType.Money, Vector3.zero, () =>
               {
                   Debug.Log("Item collected!");
               });//TODO 
    }
}
