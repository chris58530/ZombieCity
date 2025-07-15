using UnityEngine;

public class BattleSkillView : MonoBehaviour, IView
{
    [SerializeField] private GameObject root;
    [Zenject.Inject] private BattleSkillViewMediator mediator;
    private void Awake()
    {
        InjectService.Instance.Inject(this);
        ResetView();
    }
    private void OnEnable()
    {
        mediator.Register(this);
    }
    private void OnDisable()
    {
        mediator.DeRegister(this);
    }
    public void ShowBattleSkills()
    {
        root.SetActive(true);
    }
    public void ResetView()
    {
        root.SetActive(false);
    }
}
