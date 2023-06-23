using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AbilityData
{
    // A bit special for this one. Although it is not an Ability object, we just turn this object's content into an Ability object through Factories and store in Dictionary
    // Basic info
    public int abilityID;
    public string abilityName;
    public string abilityDesc;
    public string abilityType;
    public string abilityIconPicName;

    // Basic stats
    public int baseAbilityCD;
    public int baseAbilityCost;

    // Self status effects list
    public int[] selfStatusEffectsID;
    public int[] selfStatusEffectsTurns;
    public string[] selfAffectedStats;
    public int[] selfAffectedStatsValue;

    // Enemy status effects list
    public int[] enemyStatusEffectsID;
    public int[] enemyStatusEffectsTurns;
    public string[] enemyAffectedStats;
    public int[] enemyAffectedStatsValue;
}