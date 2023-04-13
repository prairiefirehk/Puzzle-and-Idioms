using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TeammateData
{
    public int id;
    public string teammateName;
    public string picName;
    public int level;
    public int maxHp;
    public int attackPoint;
    public int defencePoint;
    public int evasionPoint;
    public int criticalPoint;
    public string type;
    public string faction;
    public int maxActiveSkillCD;
    public int activeSkillID;
    public int passiveSkillID;
}
