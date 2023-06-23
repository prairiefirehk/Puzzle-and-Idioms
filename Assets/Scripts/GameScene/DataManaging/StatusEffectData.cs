using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StatusEffectData
{
    // A bit special for this one. Although it is not a status effect object, we just turn this object's content into a status effect object through Factories and store in Dictionary
    // Basic info
    public int effectID;
    public string effectName;
    public string effectDesc;
    public string effectType;
    public string effectIconPicName;

    // Basic stats
    public int sustainValue;

    // Affected stats
    public string[] affectedStatsWithModifier;
    public string[] affectedStatsModifierType;
    public int[] affectedStatsModifierOrder;
    public int[] affectedStatsModifierValue;
}