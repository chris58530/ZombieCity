using UnityEngine;

public class SelectSkillPanelView : MonoBehaviour
{
    [SerializeField] private GameObject root;
    private void Awake()
    {
        ResetView();
    }

    public void ShowPanel()
    {
        root.SetActive(true);
    }
    public void HidePanel()
    {
        root.SetActive(false);
    }
    public void ResetView()
    {
        root.SetActive(false);
    }
}