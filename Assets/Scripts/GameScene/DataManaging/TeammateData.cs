using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TeammateData
{
    // Basic info
    public int teammateID;
    public string teammateName;
    public string picName;
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

    // Ability related
    public int activeAbilityID;
    public int activeAbilityLevel;
    public int maxActiveAbilityCD;
    public int maxActiveAbilityCost;
    //public int passiveAbilityID;
    //public int passiveAbilityLevel;
    //public int maxPassiveAbilityCD;
    //public int maxPassiveAbilityCost;
}
