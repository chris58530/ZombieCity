using UnityEngine;

public class BattleBackGroundView : MonoBehaviour
{
    [SerializeField] private GameObject root;

    private void Awake()
    {
        ResetView();
    }
    public void ResetView()
    {
        root.SetActive(false);
    }
}
