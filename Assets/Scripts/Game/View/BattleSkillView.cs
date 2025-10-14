using UnityEngine;

public class BattleSkillView : MonoBehaviour, IView
{
    [Zenject.Inject] private BattleSkillViewMediator mediator;

    [SerializeField] private GameObject root;

    [SerializeField] private SelectSkillPanelView selectSkillPanelView;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnSelectSkill(SkillType.Add);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            OnSelectSkill(SkillType.Penetrate);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnSelectSkill(SkillType.FireRate);
        }
    }
    public void ShowBattleSkills()
    {
    }
    public void ResetView()
    {
    }

    public void OnSelectSkill(SkillType skillType)
    {
        mediator.OnSelectSkill(skillType);
    }
}
public enum SkillType
{
    None,

    Add, //增加新子彈
    Penetrate, //貫穿子彈
    FireRate, //提升射速

    Damage, //提升傷害 健身房老闆技能
    Lightning, //閃電 電力老闆技能
    Poison //毒素 胡蘿波老闆技能
}
