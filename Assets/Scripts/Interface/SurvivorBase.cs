using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class SurvivorBase : MonoBehaviour
{
    public int id;
    public AnimationView animationView;
    public SpriteRenderer sprite;
    public bool isWorking;
    public bool isTired;
    [SerializeField] private TMP_Text workTimeText;
    [SerializeField] private GameObject emotionImage;
    private Tween workingTween;
    private Action tiredCallBack;
    public SurvivorBase GetSurvivor()
    {
        return this;
    }

    public void StartWork(int tiredTime, Action tiredCallBack = null)
    {
        // 走到設施的時候隱藏自己 播放設施對應角色動畫
        this.tiredCallBack = tiredCallBack;
        isWorking = true;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);

        float elapsedTime = 0;
        workingTween = DOTween.To(() => elapsedTime, x =>
        {
            elapsedTime = x;
            workTimeText.text = Mathf.CeilToInt(tiredTime - elapsedTime).ToString();
        }, tiredTime, tiredTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            SetCollider(true);
            sprite.color = Color.white;

            workTimeText.text = string.Empty;
            this.tiredCallBack?.Invoke();
            SetTired(true);
        }).SetId(GetHashCode());
    }
    public void Hit()
    {
        sprite.color = Color.red;
        DOVirtual.DelayedCall(0.1f, () =>
        {
            sprite.color = Color.white;
        }).SetId(GetHashCode());
    }
    public void SetFlip(bool isFlip)
    {
        sprite.flipX = isFlip;
    }
    public void OnPick(Vector3 pickPosition)
    {
        workingTween?.Kill();

        isWorking = false;
        transform.position = new Vector3(pickPosition.x, pickPosition.y, -0.01f);
    }
    public void SetCollider(bool isCollider)
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = isCollider;

    }
    public void OnDrop(Vector3 dropPos)
    {
        transform.localScale = Vector3.one;
        transform.position = dropPos;
    }
    public void SetTired(bool isTired)
    {
        this.isTired = isTired;
        Debug.Log($"SetTired: {isTired}");
        emotionImage.gameObject.SetActive(isTired);

    }


}
