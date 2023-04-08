using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlueAndWhite.Objects;
using BlueAndWhite.RoundManaging;

namespace BlueAndWhite.DataManaging
{
    [System.Serializable]
    public class MobData
    {
        public int id;
        public string mobName;
        public string picName;
        public int level;
        public int maxHp;
        public int attackPoint;
        public int defencePoint;
        public int evasionPoint;
        public int criticalPoint;
        public string type;
        public string faction;
        public int maxAttackInterval;
        public int currentAttackInterval;
        public int expReward;
        public int coinReward;
        public int jadeReward;
        public Image mobPicture;
    }
}