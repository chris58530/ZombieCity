using UnityEngine;
using Zenject;

public class SurvivorAnimationHandler : MonoBehaviour
{
    [Inject] private Listener listener;
    [Inject] private FloorProxy floorProxy;
    [SerializeField] private int survivorId;
    [SerializeField] private FloorType floorType;
    public void Awake()
    {
        InjectService.Instance.Inject(this);
    }
    public void OnAddProduct()
    {
        floorProxy.AddProductByAnimationEvent(survivorId, floorType);
    }
}
