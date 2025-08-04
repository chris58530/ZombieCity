using TMPro;
using UnityEngine;
using Zenject;
using DG.Tweening;

public class ResourceInfoView : MonoBehaviour, IView
{
    [Inject] private ResourceInfoMediator mediator;
    [SerializeField] private GameObject root;

    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text satisfactionText;
    [SerializeField] private TMP_Text zombieCoreText;

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
    public void Hide()
    {
        root.SetActive(false);

    }
    public void Show()
    {
        root.SetActive(true);
    }

    public void OnUpdateResource(int money, int satisfaction, int zombieCore)
    {
        moneyText.text = money.ToString();
        satisfactionText.text = satisfaction.ToString();
        zombieCoreText.text = zombieCore.ToString();

        AnimateBounce(moneyText.transform);
        AnimateBounce(zombieCoreText.transform);
        AnimateBounce(satisfactionText.transform);
    }
    public void OnAddMoney(int money)
    {
        moneyText.text = money.ToString();

        AnimateBounce(moneyText.transform);
    }
    public void OnAddSatisfaction(int satisfaction)
    {
        satisfactionText.text = satisfaction.ToString();
        AnimateBounce(satisfactionText.transform);
    }
    public void OnAddZombieCore(int zombie)
    {
        zombieCoreText.text = zombie.ToString();
        AnimateBounce(zombieCoreText.transform);
    }

    private void AnimateBounce(Transform target)
    {
        target.DOKill();
        target.localScale = new Vector3(0.8f, 0.8f, target.localScale.z);
        target.DOScale(new Vector3(1.2f, 1.2f, target.localScale.z), 0.15f).SetEase(Ease.OutBack)
              .OnComplete(() => target.DOScale(new Vector3(1f, 1f, target.localScale.z), 0.15f).SetEase(Ease.InBack));
    }
}
