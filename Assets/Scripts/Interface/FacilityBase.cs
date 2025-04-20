using System;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

public class FacilityBase : MonoBehaviour
{
   public int order; // 排序用 每一個數字只能有一個
   public bool isUsing;
   public AnimationView animationView;
   public GameObject mask;
   public string animationName;
   public int usingSurvivor;
   public float pickUpTime = 1; //設施使用中 需要按住多久時間才能讓設施回Idel並且讓倖存者pickup
   [SerializeField] private GameObject eventTrigger;
   private FacilityData facilityData; //要存在Json裡面的資料
   public Action<int, FacilityBase> onSurvivorEndWork;

   public void Init(FacilityData data)
   {
      facilityData = data;
      this.usingSurvivor = data.usingSurvivor;
      animationName = data.animationString;
      isUsing = data.isUsing;
      mask.SetActive(false);
      animationView.PlayAnimation(animationName);
      eventTrigger.SetActive(isUsing);
   }
   public void SetLock(bool isLock)
   {
      mask.SetActive(isLock);
   }
   public void SetAnimation(string animationName)
   {
      this.animationName = animationName;
      if (animationName != "Idle")
         eventTrigger.SetActive(true);
      else
         eventTrigger.SetActive(false);
      animationView.PlayAnimation(animationName);
   }
   public void SetStartTime(int time)
   {
   }
   Tween countTimeTween;
   //event trigger
   public void OnPointDown()
   {
      countTimeTween = DOVirtual.DelayedCall(pickUpTime, () =>
      {
         SetAnimation("Idle");
         eventTrigger.SetActive(false);
         onSurvivorEndWork?.Invoke(usingSurvivor, this);
      });
   }
   public void OnPointUp()
   {
      countTimeTween?.Kill();

   }
   public void SetEfficientTime(int time)
   {
   }
   public FacilityData GetData()
   {
      facilityData.animationString = animationName;
      facilityData.isUsing = isUsing;
      facilityData.usingSurvivor = usingSurvivor;
      facilityData.startTime = 0;
      facilityData.efficientTime = 0;
      facilityData.order = order;
      return facilityData;
   }
}
