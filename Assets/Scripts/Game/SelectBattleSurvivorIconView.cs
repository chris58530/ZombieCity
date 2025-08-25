using UnityEngine;
using UnityEngine.UI;

public class SelectBattleSurvivorIconView : MonoBehaviour
{
    [SerializeField] private SurvivorDataSetting survivorDataSetting;
    [SerializeField] private Image[] survivorImages;

    public void UpdateIcon(int[] selectedSurvivorIds)
    {
        for (int i = 0; i < survivorImages.Length; i++)
        {
            if (i < selectedSurvivorIds.Length)
            {
                var survivorData = survivorDataSetting.GetSurvivorData(selectedSurvivorIds[i]);
                survivorImages[i] = survivorData?.survivorInfo.icon;
            }
            else
            {
                survivorImages[i] = null;
            }
        }
    }
}
