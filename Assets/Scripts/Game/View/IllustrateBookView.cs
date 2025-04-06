using UnityEngine;
using Zenject;

public class IllustrateBookView : MonoBehaviour, IView
{
    [Inject] private IllustrateBookViewMediator mediator;
    [SerializeField] private GameObject bookRoot;
    [SerializeField] private GameObject openButton;
    public void Awake()
    {
        InjectService.Instance.Inject(this);

    }
    private void OnEnable()
    {
        mediator.Register(this);
        bookRoot.SetActive(false);
    }
    private void OnDisable()
    {
        mediator.DeRegister(this);
    }
    public void OnOpenBook()
    {
        bookRoot.SetActive(true);
        openButton.SetActive(false);
        mediator.RequestStopSwipe();
    }
    public void OnCloseBook()
    {
        openButton.SetActive(true);
        bookRoot.SetActive(false);
        mediator.RequestOpenSwipe();

    }
}
