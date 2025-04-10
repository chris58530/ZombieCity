using Zenject;
using UnityEngine;

public class PassiveHitCmd : ICommand
{
    [Inject] private PassiveHitProxy proxy;
    [SerializeField] private float shootRate;

    public override void Execute(MonoBehaviour mono)
    {
        isLazy = true;
        InitPassiveHit();
    }
    public void InitPassiveHit()
    {
        proxy.SetShootStart(shootRate);
    }
}
