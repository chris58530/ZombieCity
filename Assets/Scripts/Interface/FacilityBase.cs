using UnityEngine;

public class FacilityBase : MonoBehaviour
{
   public bool isUsing;
   public AnimationView animationView;
   public GameObject mask;
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
      if (animationName == null) return;
      animationView.PlayAnimation(animationName);
   }
}
