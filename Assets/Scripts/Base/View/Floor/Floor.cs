using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] private GameObject mask;
    public void SetMask(bool active)
    {
        mask.SetActive(active);
    }

}
