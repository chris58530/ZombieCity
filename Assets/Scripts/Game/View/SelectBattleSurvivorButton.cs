using UnityEngine;

public class SelectBattleSurvivorButton : MonoBehaviour
{
    [SerializeField] private GameObject isSelectImage;
    [SerializeField] private GameObject nonSelectImage;
    [SerializeField] private int playerId;

    public void SetSelected(bool isSelected)
    {
        isSelectImage.SetActive(isSelected);
        nonSelectImage.SetActive(!isSelected);
    }

    public int GetPlayerId()
    {
        return playerId;
    }
}
