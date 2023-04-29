using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        Debug.Log($"{Time.time} MobFactory.Awake (start)");
        Debug.Log($"{Time.time} MobFactory.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} MobFactory.OnEnable (start)");
        Debug.Log($"{Time.time} MobFactory.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} MobFactory.Start (start)");
        Debug.Log($"{Time.time} MobFactory.Start (end)");
    }

    void Update()
    {
        
    }
    
    void OnDisable()
    {
        Debug.Log($"{Time.time} MobFactory.OnDisable (start)");
        Debug.Log($"{Time.time} MobFactory.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} MobFactory.OnDestroy (start)");
        Debug.Log($"{Time.time} MobFactory.OnDestroy (end)");
    }
    #endregion
    
    #region Factory functions
    public Mob CreateMob(int mobID)
    {
        Debug.Log($"{Time.time} MobFactory.CreateMob (start)");

        Mob mob = Instantiate(mobPrefab, mobSpawner.transform).GetComponent<Mob>();

        mob.id = ImportData.mobs.mob[mobID].id;
        Debug.Log($"{Time.time} mob id is = {mob.id}");
        mob.mobName = ImportData.mobs.mob[mobID].mobName;
        mob.picName = ImportData.mobs.mob[mobID].picName;
        mob.level = ImportData.mobs.mob[mobID].level;
        mob.maxHp = new EntityStat(ImportData.mobs.mob[mobID].maxHp);
        mob.attackPoint = new EntityStat(ImportData.mobs.mob[mobID].attackPoint);
        mob.defencePoint = new EntityStat(ImportData.mobs.mob[mobID].defencePoint);
        mob.dexterityPoint = new EntityStat(1f);
        //mob.evasionPoint = new EntityStat(ImportData.mobs.mob[mobID].evasionPoint);
        //mob.criticalPoint = new EntityStat(ImportData.mobs.mob[mobID].criticalPoint);
        //mob.type = ImportData.mobs.mob[mobID].type;
        //mob.faction = ImportData.mobArray[mobID]["faction"].ToString();
        mob.maxAttackInterval = new EntityStat(ImportData.mobs.mob[mobID].maxAttackInterval);
        mob.expReward = ImportData.mobs.mob[mobID].expReward;
        mob.coinReward = ImportData.mobs.mob[mobID].coinReward;
        mob.jadeReward = ImportData.mobs.mob[mobID].jadeReward;

        mob.mobPicture = mob.transform.GetChild(0).GetComponent<Image>();
        Sprite mobOrgImage = Resources.Load<Sprite>($"Prefabs/Mobs/{mob.picName}");
        mob.mobPicture.sprite = mobOrgImage;
        mob.mobPicture.rectTransform.sizeDelta = new Vector2(mobOrgImage.rect.width, mobOrgImage.rect.height);
        mob.mobPicture.rectTransform.sizeDelta = new Vector2(600, 600);

        mob.currentState = EntityState.State.Alive;

        // In Unity inspector name
        mob.name = mob.mobName;

        // Initialize data
        //Debug.Log("mob current hp: " + mob.currentHp);
        mob.currentMaxHp = new EntityStat(mob.maxHp.value);
        mob.currentHp = new EntityStat(mob.currentMaxHp.value);
        //Debug.Log("mob current hp: " + mob.currentHp);
        mob.currentAttackPoint = new EntityStat(mob.attackPoint.value);
        mob.currentDefencePoint = new EntityStat(mob.defencePoint.value);
        mob.currentDexterityPoint = new EntityStat(mob.dexterityPoint.value);
        // For testing only, need to be draw
        mob.currentMaxAttackInterval = new EntityStat(mob.maxAttackInterval.value);
        mob.currentAttackInterval = new EntityStat(mob.currentMaxAttackInterval.value);

        Debug.Log($"{Time.time} MobFactory.CreateMob, return mob(local var): {mob.name}(mob.name only) (start)");
        return mob;
    }
    #endregion
}