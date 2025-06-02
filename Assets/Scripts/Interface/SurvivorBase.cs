using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class SurvivorBase : MonoBehaviour
{
    [Header("Setting")]
    public int id;
    public int level;
    public FloorType stayingFloor;
    [SerializeField] private AnimationView animationView;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private GameObject emotionImage;
    public Action<int, FloorType> onSaveStayingFloor;
    public Action<int, int> onSaveLevel;
    protected SurvivorJsonData survivorJsonData;

    [Header("Debug")]
    public string playingAnimationName;
    public void SetData(SurvivorJsonData survivorJsonData)
    {
        this.survivorJsonData = survivorJsonData;
    }
    public void SetStayingFloor(FloorBase stayingFloorTarget)
    {
        this.stayingFloor = stayingFloorTarget.floorType;
        float floorY = stayingFloorTarget.GetEnterPosition().y;
        float randomX = UnityEngine.Random.Range(stayingFloorTarget.GetLimitPositionX().x, stayingFloorTarget.GetLimitPositionX().y);
        transform.position = new Vector3(randomX, floorY, GameDefine.GetSurvivorZ());
        Debug.Log($" {id} SetStayingFloorto {stayingFloorTarget.name}  {stayingFloorTarget.transform.position}");
    }
    void Update()
    {
    }
    public void SetFlip(bool isFlip)
    {
        sprite.flipX = isFlip;
    }
    public void OnPick(Vector3 pickPosition)
    {
        sprite.color = Color.white;
        transform.position = new Vector3(pickPosition.x, pickPosition.y, GameDefine.GetSurvivorZ());
        animationView.PlayAnimation("Idle");
    }
    public void SetCollider(bool isCollider)
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = isCollider;

    }
    public void OnDrop(Vector3 dropPos, FloorType floorType)
    {
        SetCollider(true);
        OnSetStayingFloor(floorType);
        sprite.color = Color.white;
        transform.localScale = Vector3.one;
        transform.position = dropPos;
    }
    public void PlayAnimation(string name, Action onComplete = null)
    {
        // string animationString = "Survivor_" + id + "_" + name;
        playingAnimationName = name;
        animationView?.PlayAnimation(name, onComplete);

    }
    public AnimationView GetAnimationView()
    {
        return animationView;
    }
    public virtual void OnAddLevel(int level)
    {
        this.level += level;
        onSaveLevel?.Invoke(id, level);
    }
    public virtual void OnSetStayingFloor(FloorType floorType)
    {
        //TODO 到main 有問題
        stayingFloor = floorType;
        onSaveStayingFloor?.Invoke(id, floorType);
    }
}
