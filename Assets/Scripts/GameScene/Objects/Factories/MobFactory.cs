using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MobFactory : MonoBehaviour, IFactory
{
    #region Scripts
    public RoundData roundData;
    #endregion

    #region Game object references
    public GameObject mobPrefab;
    public GameObject mobSpawner;
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{Time.time} MobFactory.Awake (start)");

        // Not ideal place
        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();

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

        // Transfer data
        mob.mobID = ImportData.mobs.mob[mobID].mobID;
        Debug.Log($"{Time.time} mob id is = {mob.mobID}");
        mob.mobName = ImportData.mobs.mob[mobID].mobName;
        mob.mobPicName = ImportData.mobs.mob[mobID].mobPicName;
        mob.level = ImportData.mobs.mob[mobID].level;
        mob.type = ImportData.mobs.mob[mobID].type;
        mob.faction = ImportData.mobs.mob[mobID].faction;

        // In Unity inspector name
        mob.name = mob.mobName;

        // For interal use
        mob.entityName = mob.mobName;

        // Initialize base point
        mob.baseHealthPoint = ImportData.mobs.mob[mobID].baseHealthPoint;
        mob.baseAttackPoint = ImportData.mobs.mob[mobID].baseAttackPoint;
        mob.baseDefencePoint = ImportData.mobs.mob[mobID].baseDefencePoint;
        mob.baseDexterityPoint = ImportData.mobs.mob[mobID].baseDexterityPoint;
        mob.basePerceptionPoint = ImportData.mobs.mob[mobID].basePerceptionPoint;
        mob.baseConstitutionPoint = ImportData.mobs.mob[mobID].baseConstitutionPoint;
        mob.maxAttackInterval = ImportData.mobs.mob[mobID].maxAttackInterval;

        mob.maxHealthPoint = new EntityStat(mob.baseHealthPoint);
        mob.attackPoint = new EntityStat(mob.baseAttackPoint);
        mob.defencePoint = new EntityStat(mob.baseDefencePoint);
        mob.dexterityPoint = new EntityStat(mob.baseDexterityPoint);
        mob.perceptionPoint = new EntityStat(mob.basePerceptionPoint);
        mob.constitutionPoint = new EntityStat(mob.baseConstitutionPoint);

        mob.speedPoint = new EntityStat(mob.dexterityPoint.GetStatValue());
        mob.evasionPoint = new EntityStat(mob.dexterityPoint.GetStatValue());
        mob.criticalPoint = new EntityStat(mob.perceptionPoint.GetStatValue());
        mob.accuracyPoint = new EntityStat(mob.perceptionPoint.GetStatValue());
        mob.resistancePoint = new EntityStat(mob.constitutionPoint.GetStatValue());

        // Initialize data
        mob.currentMaxHealthPoint = new EntityStat(mob.maxHealthPoint.GetStatValue());
        mob.currentAttackPoint = new EntityStat(mob.attackPoint.GetStatValue());
        mob.currentDefencePoint = new EntityStat(mob.defencePoint.GetStatValue());
        mob.currentDexterityPoint = new EntityStat(mob.dexterityPoint.GetStatValue());
        mob.currentPerceptionPoint = new EntityStat(mob.perceptionPoint.GetStatValue());
        mob.currentConstitutionPoint = new EntityStat(mob.constitutionPoint.GetStatValue());

        mob.currentSpeedPoint = new EntityStat(mob.speedPoint.GetStatValue());
        mob.currentEvasionPoint = new EntityStat(mob.evasionPoint.GetStatValue());
        mob.currentCriticalPoint = new EntityStat(mob.criticalPoint.GetStatValue());
        mob.currentAccuracyPoint = new EntityStat(mob.accuracyPoint.GetStatValue());
        mob.currentResistancePoint = new EntityStat(mob.resistancePoint.GetStatValue());

        mob.currentAttackValue = mob.GetAttackValue();
        mob.currentDefenceValue = mob.GetDefenseValue();
        mob.currentSpeedValue = mob.GetSpeedValue();
        mob.currentMaxHealthValue = mob.GetMaxHealthValue();
        mob.currentHealthValue = mob.currentMaxHealthValue;

        // Mob's ability related
        mob.activeAbilityID = ImportData.mobs.mob[mobID].activeAbilityID;
        mob.activeAbilityLevel = ImportData.mobs.mob[mobID].activeAbilityLevel;

        mob.activeAbility = ImportData.abilityDictionary[mob.activeAbilityID];
        mob.activeAbility.abilityLevel = mob.activeAbilityLevel;
        //Debug.Log($"^ImportData.abilities.ability[teammate.activeAbilityID].abilityName =  {ImportData.abilitiesData.abilityData[teammate.activeAbilityID].abilityName}");

        mob.maxActiveAbilityCD = mob.activeAbility.baseAbilityCD;
        mob.currentMaxActiveAbilityCD = new EntityStat(mob.maxActiveAbilityCD);
        mob.currentActiveAbilityCD = (int)mob.currentMaxActiveAbilityCD.GetStatValue();

        mob.maxActiveAbilityCost = mob.activeAbility.baseAbilityCost;
        mob.currentActiveAbilityCost = new EntityStat(mob.maxActiveAbilityCost);

        mob.activeAbility.InitalizingAbility(mob, roundData.player);

        mob.expReward = ImportData.mobs.mob[mobID].expReward;
        mob.coinReward = ImportData.mobs.mob[mobID].coinReward;
        mob.jadeReward = ImportData.mobs.mob[mobID].jadeReward;

        mob.mobPicture = mob.transform.GetChild(0).GetComponent<Image>();
        Sprite mobOrgImage = Resources.Load<Sprite>($"Prefabs/Mobs/{mob.mobPicName}");
        mob.mobPicture.sprite = mobOrgImage;
        mob.mobPicture.rectTransform.sizeDelta = new Vector2(mobOrgImage.rect.width, mobOrgImage.rect.height);
        mob.mobPicture.rectTransform.sizeDelta = new Vector2(600, 600);

        mob.currentState = EntityState.State.Alive;

        // For testing only, need to be draw
        mob.currentMaxAttackInterval = new EntityStat(mob.maxAttackInterval);
        mob.currentAttackInterval = (int)mob.currentMaxAttackInterval.GetStatValue();

        Debug.Log($"{Time.time} MobFactory.CreateMob, return mob(local var): {mob.entityName}(mob.name only) (start)");
        return mob;
    }
    #endregion
}