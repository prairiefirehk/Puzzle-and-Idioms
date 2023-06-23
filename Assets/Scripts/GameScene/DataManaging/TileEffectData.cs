using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TileEffectData
{
    // A bit special for this one. Although it is not an Ability object, we just turn this object's content into an Ability object through Factories and store in Dictionary
    // Basic info
    public int tileEffectID;
    public string tileEffectName;
    public string tileEffectDesc;
    public string tileEffectType;
    public string tileEffectIconPicName;
}