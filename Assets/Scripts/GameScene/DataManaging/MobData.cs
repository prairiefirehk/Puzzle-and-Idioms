using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MobData
{
    // Basic info
    public int mobID;
    public string mobName;
    public string mobPicName;
    public string type;
    public string faction;

    // Basic stats
    public int level;
    public int baseHealthPoint;
    public int baseAttackPoint;
    public int baseDefencePoint;
    public int baseDexterityPoint;
    public int basePerceptionPoint;
    public int baseConstitutionPoint;
    public int maxAttackInterval;

    // Ability related
    public int activeAbilityID;
    public int activeAbilityLevel;
    public int passiveAbilityID;
    public int passiveAbilityLevel;

    // Rewards
    public int expReward;
    public int coinReward;
    public int jadeReward;
}