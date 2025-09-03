using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleZombieCounterView : MonoBehaviour, IView
{
    [Zenject.Inject] private BattleZombieCounterViewMediator mediator;
    [SerializeField] private GameObject root;


    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI remainingCountText;

    private void Awake()
    {
        InjectService.Instance.Inject(this);
    }

    private void OnEnable()
    {
        mediator.Register(this);
    }

    private void OnDisable()
    {
        mediator.DeRegister(this);
    }

    public void ResetView()
    {
        root.SetActive(false);
    }

    public void ShowAndFadeIn()
    {
        root.SetActive(true);
    }

    public void UpdateDisplay(int remaining)
    {
        if (remainingCountText != null)
            remainingCountText.text = $"Zombie Left: {remaining}";
    }
}

