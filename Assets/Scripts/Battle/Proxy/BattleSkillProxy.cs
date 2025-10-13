using UnityEngine;

public class BattleSkillProxy : IProxy
{
    public void RequestSkillUpgrade(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.Add:
                Debug.Log("Request to add new bullet type.");
                listener.BroadCast(BattleSkillEvent.ON_SELECT_ADD);
                break;
            case SkillType.Penetrate:
                Debug.Log("Request to upgrade to piercing bullets.");
                listener.BroadCast(BattleSkillEvent.ON_SELECT_PENETRATE);
                break;
            case SkillType.FireRate:
                Debug.Log("Request to increase fire rate.");
                listener.BroadCast(BattleSkillEvent.ON_SELECT_FIRE_RATE);
                break;
            case SkillType.Damage:
                Debug.Log("Request to increase damage.");
                listener.BroadCast(BattleSkillEvent.ON_SELECT_DAMAGE);
                break;
            case SkillType.Lightning:
                Debug.Log("Request to unlock lightning skill.");
                listener.BroadCast(BattleSkillEvent.ON_SELECT_LIGHTNING);
                break;
            case SkillType.Poison:
                Debug.Log("Request to unlock poison skill.");
                listener.BroadCast(BattleSkillEvent.ON_SELECT_POISON);
                break;
            default:
                Debug.LogWarning("Unknown skill type requested.");
                break;
        }
    }
}
