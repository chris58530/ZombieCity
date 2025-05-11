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
    public Action<int, FloorType> onSaveStayingFloor;
    public Action<int, int> onSaveLevel;
    protected SurvivorJsonData survivorJsonData;
    public void SetData(SurvivorJsonData survivorJsonData)
    {
        this.survivorJsonData = survivorJsonData;
    }
    public void SetFlip(bool isFlip)
    {
        sprite.flipX = isFlip;
    }
    public void OnPick(Vector3 pickPosition)
    {
        workingTween?.Kill();
        sprite.color = Color.white;

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
        SetCollider(true);
        isWorking = false;
        sprite.color = Color.white;
        transform.localScale = Vector3.one;
        transform.position = dropPos;
    }
    public void SetTired(bool isTired)
    {
        this.isTired = isTired;
        Debug.Log($"SetTired: {isTired}");
        emotionImage.gameObject.SetActive(isTired);

    }
    public void OnAddLevel(int level)
    {
    }
    public void OnSetStayingFloor(FloorType floorType)
    {

    }
}
