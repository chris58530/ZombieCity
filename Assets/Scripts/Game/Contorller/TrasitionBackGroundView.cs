using UnityEngine;
using DG.Tweening;
using Zenject;
using UnityEngine.UI;
// View
public class TrasitionBackGroundView : MonoBehaviour, IView
{
    [Inject] private TrasitionBackGroundMediator mediator;
    [SerializeField] private Image fadeImage;
    private void OnEnable()
    {
        InjectService.Instance.Inject(this);
        mediator.Register(this);
        ShowImage(false);
    }

    private void OnDisable()
    {
        mediator.DeRegister(this);
    }
    public void RequestTrasitionBackGround()
    {
        ShowImage(true);
        fadeImage.DOFade(1, 0.5f).OnComplete(() =>
        {
            fadeImage.DOFade(0, 0.3f).OnComplete(() =>
            {
                ShowImage(false);
                mediator.OnTrasitionComplete();
            });
        });
    }
    public void ShowImage(bool active)
    {
        if (active)
            fadeImage.color = new Color(0, 0, 0, 1);
        else
            fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.gameObject.SetActive(active);
    }

}
