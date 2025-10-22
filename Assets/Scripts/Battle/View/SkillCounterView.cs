using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class SkillCounterView : MonoBehaviour
{
    [SerializeField] private float skillTime;
    [SerializeField] private Image counterImage;
    [SerializeField] private GameObject root;

    public void ResetView()
    {
        root.SetActive(false);
    }

    public void StartSkillCoolDown(Action callBack = null)
    {
        root.SetActive(true);
        counterImage.fillAmount = 1f;
        counterImage.DOFillAmount(0f, skillTime).SetEase(Ease.Linear).onComplete = () =>
        {
            callBack?.Invoke();
            root.SetActive(false);
        };
    }
}
