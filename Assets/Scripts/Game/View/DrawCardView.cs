using UnityEngine;
using Zenject;

public class DrawCardView : MonoBehaviour, IView
{
    [Inject] private DrawCardMediator mediator;
    [SerializeField]private GameObject root;
    private void OnEnable()
    {
        InjectService.Instance.Inject(this);
        mediator.Register(this);
        root.SetActive(false);

    }
     private void OnDisable()
    {
        mediator.DeRegister(this);
    }
    public void Open()
    {
        root.SetActive(true);
    }
    public void Close() //EventTriigger
    {
        mediator.OnDrawCardClose();
        root.SetActive(false);
    }
}
