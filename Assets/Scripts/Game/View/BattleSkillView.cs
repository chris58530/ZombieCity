using UnityEngine;

public class BattleSkillView : MonoBehaviour
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
