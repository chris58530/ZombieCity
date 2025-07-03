using UnityEngine;

public class BattleCampCarView : MonoBehaviour, IView
{
    // [Zenject.Inject] private BattleCampCarViewMediator mediator;
    [SerializeField] private GameObject root;
    [SerializeField] private BattleCampCarController battleCampCarController;
    [SerializeField] private GameObject followCamera;
    [Header("Test")]
    public bool isTest = false;
    public GameObject testRoot;
    private void Awake()
    {
        if (!isTest)
        {
            Destroy(testRoot);
        }
        // InjectService.Instance.Inject(this);
        ResetView();
    }

    private void OnEnable()
    {
        // mediator.Register(this);
    }

    private void OnDisable()
    {
        // mediator.DeRegister(this);
    }

    public void ShowBattleCampCar()
    {
        root.SetActive(true);
    }

    public void HideBattleCampCar()
    {
        root.SetActive(false);
    }
    public void ResetView()
    {
        root.SetActive(false);
    }
}

