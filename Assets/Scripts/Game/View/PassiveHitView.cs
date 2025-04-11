using DG.Tweening;
using UnityEngine;

public class PassiveHitView : MonoBehaviour, IView
{
    [Zenject.Inject] private PassiveHitMediator mediator;

    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject hitEffectPrefab;

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
    public void SetShootStart(float shootRate)
    {
        Debug.Log("SetShootStart");
        DOTween.Sequence()
            .AppendInterval(shootRate)
            .AppendCallback(() =>
            {
                GameObject bullet = Instantiate(hitEffectPrefab, shootPoint.position, Quaternion.identity);
                bullet.transform.SetParent(shootPoint);
                ZombieBase target = mediator.GetHitTarget();
                if (target == null)
                {
                    Destroy(bullet);
                    return;
                }
                bullet.transform.DOMove(target.transform.position, 0.2f)
                    .OnComplete(() =>
                    {
                        Destroy(bullet);
                        target.manager.KillZombie(target);
                    });
            })
            .SetLoops(-1, LoopType.Restart);
    }
}
