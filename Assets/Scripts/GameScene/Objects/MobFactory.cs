using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlueAndWhite.RoundManaging;
using BlueAndWhite.DataManaging;
using BlueAndWhite.Entities;

namespace BlueAndWhite.Objects
{
    //[System.Serializable]
    public class MobFactory : MonoBehaviour, IFactory
    {
        #region Scripts
        #endregion

        #region Game object references
        public GameObject mobPrefab;
        public GameObject mobSpawner;
        #endregion

        #region Flow
        void Awake()
        {
            Debug.Log($"MobFactory.Awake (start)");
            Debug.Log($"MobFactory.Awake (end)");
        }

        void OnEnable()
        {
            Debug.Log($"MobFactory.OnEnable (start)");
            Debug.Log($"MobFactory.OnEnable (end)");
        }
        
        void Start()
        {
            Debug.Log($"MobFactory.Start (start)");
            Debug.Log($"MobFactory.Start (end)");
        }

        void Update()
        {
            
        }
        
        void OnDisable()
        {
            Debug.Log($"MobFactory.OnDisable (start)");
            Debug.Log($"MobFactory.OnDisable (end)");
        }

        void OnDestroy() 
        {
            Debug.Log($"MobFactory.OnDestroy (start)");
            Debug.Log($"MobFactory.OnDestroy (end)");
        }
        #endregion
        
        #region Factory functions
        public Mob CreateMob(int mobID)
        {
            Debug.Log($"MobFactory.CreateMob (start)");

            Mob mob = Instantiate(mobPrefab, mobSpawner.transform).GetComponent<Mob>();

            mob.id = ImportData.mobs.mob[mobID].id;
            mob.mobName = ImportData.mobs.mob[mobID].mobName;
            mob.picName = ImportData.mobs.mob[mobID].picName;
            mob.level = ImportData.mobs.mob[mobID].level;
            mob.maxHp.value = ImportData.mobs.mob[mobID].maxHp;
            mob.attackPoint.value = ImportData.mobs.mob[mobID].attackPoint;
            mob.defencePoint.value = ImportData.mobs.mob[mobID].defencePoint;
            //mob.evasionPoint = (int)ImportData.mobs.mob[mobID].evasionPoint;
            //mob.criticalPoint = (int)ImportData.mobs.mob[mobID].criticalPoint;
            //mob.type = ImportData.mobs.mob[mobID].type;
            //mob.faction = ImportData.mobArray[mobID]["faction"].ToString();
            mob.maxAttackInterval.value = ImportData.mobs.mob[mobID].maxAttackInterval;
            mob.expReward = ImportData.mobs.mob[mobID].expReward;
            mob.coinReward = ImportData.mobs.mob[mobID].coinReward;
            mob.jadeReward = ImportData.mobs.mob[mobID].jadeReward;

            mob.mobPicture = mob.transform.GetChild(0).GetComponent<Image>();
            Sprite mobOrgImage = Resources.Load<Sprite>($"Prefabs/Mobs/{mob.picName}");
            mob.mobPicture.sprite = mobOrgImage;
            mob.mobPicture.rectTransform.sizeDelta = new Vector2(mobOrgImage.rect.width, mobOrgImage.rect.height);
            mob.mobPicture.rectTransform.sizeDelta = new Vector2(600, 600);

            // In Unity inspector name
            mob.name = mob.mobName;

            // Initialize data
            //Debug.Log("mob current hp: " + mob.currentHp);
            mob.currentHp.value = mob.maxHp.value;
            //Debug.Log("mob current hp: " + mob.currentHp);
            mob.currentAttackPoint.value = mob.attackPoint.value;
            mob.currentDefencePoint.value = mob.defencePoint.value;
            // For testing only, need to be draw
            mob.currentAttackInterval.value = mob.maxAttackInterval.value;

            Debug.Log($"MobFactory.CreateMob, return mob(local var): {mob.name}(mob.name only) (start)");
            return mob;
        }
        #endregion
    }
}