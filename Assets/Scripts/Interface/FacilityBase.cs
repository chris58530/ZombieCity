using UnityEngine;

public class FacilityBase : MonoBehaviour
{
   public bool isUsing;
   public AnimationView animationView;
   public GameObject mask;
   public string animationName;
   public void Init(bool isLock, string animationName)
   {
      SetLock(isLock);
      if (isLock) return;
      SetAnimation(animationName);
   }
   public void SetLock(bool isLock)
   {
      mask.SetActive(isLock);
   }
   public void SetAnimation(string animationName)
   {
      this.animationName = animationName;
      animationView.PlayAnimation(animationName);
   }


   public void SetStartTime(int time)
   {
   }

   public void SetEfficientTime(int time)
   {
   }
}
