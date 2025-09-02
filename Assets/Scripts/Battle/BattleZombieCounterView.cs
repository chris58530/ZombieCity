using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleZombieCounterView : MonoBehaviour, IView
{
    [Zenject.Inject] private BattleZombieCounterViewMediator mediator;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI remainingCountText;
    [SerializeField] private TextMeshProUGUI totalCountText;
    [SerializeField] private TextMeshProUGUI deadCountText;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private GameObject root;

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
        UpdateDisplay(0, 0, 0);
        root.SetActive(false);
    }

    public void ShowAndFadeIn()
    {
        root.SetActive(true);
    }

    public void UpdateDisplay(int remaining, int dead, int total)
    {
        if (remainingCountText != null)
            remainingCountText.text = $"剩餘: {remaining}";

        if (totalCountText != null)
            totalCountText.text = $"總數: {total}";

        if (deadCountText != null)
            deadCountText.text = $"擊殺: {dead}";

        if (progressSlider != null && total > 0)
            progressSlider.value = (float)dead / total;
    }
}

