using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SelectSkillPanelView : MonoBehaviour, IView
{
    [SerializeField] private GameObject root;

    public Action<SkillType> onSelectSkill;

    private void Awake()
    {
        ResetView();
    }

    public void ShowPanel()
    {
        //塞數值
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
    /// <summary>
    ///  Sprite Event
    /// </summary>
    public void OnClickAddSkill(int skillType)
    {
        onSelectSkill?.Invoke((SkillType)skillType);
        HidePanel();
    }



}