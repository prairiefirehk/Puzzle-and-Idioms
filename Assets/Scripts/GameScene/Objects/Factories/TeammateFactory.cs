using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TeammateFactory : MonoBehaviour, IFactory
{
    #region Scripts
    public RoundData roundData;
    #endregion

    #region Game object references
    public GameObject teammatePrefab;
    public GameObject teammateCells;
    public GameObject teammateSpawner;
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{Time.time} TeammateFactory.Awake (start)");

        // Not ideal place
        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();

        Debug.Log($"{Time.time} TeammateFactory.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} TeammateFactory.OnEnable (start)");
        Debug.Log($"{Time.time} TeammateFactory.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} TeammateFactory.Start (start)");
        Debug.Log($"{Time.time} TeammateFactory.Start (end)");
    }

    void Update()
    {
        
    }

    void OnDisable()
    {
        Debug.Log($"{Time.time} TeammateFactory.OnDisable (start)");
        Debug.Log($"{Time.time} TeammateFactory.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} TeammateFactory.OnDestroy (start)");
        Debug.Log($"{Time.time} TeammateFactory.OnDestroy (end)");
    }
    #endregion

    #region Factory functions
    public Teammate CreateTeammate(int teammateID, int teammateOrder)
    {
        Debug.Log($"{Time.time} TeammateFactory.CreateTeammate (start)");

        Teammate teammate = Instantiate(teammatePrefab, teammateSpawner.transform).GetComponent<Teammate>();

        // Teammate's basic info/ref
        teammate.teammateID = teammateID;
        teammate.teammateName = ImportData.teammates.teammate[teammateID].teammateName;
        teammate.picName = ImportData.teammates.teammate[teammateID].picName;
        //teammate.type = ImportData.teammates.teammate[teammateID].type;
        //teammate.faction = ImportData.teammates.teammate[teammateID].faction;

        // In Unity inspector name
        teammate.name = teammate.teammateName;

        // For interal use
        teammate.entityName = teammate.teammateName;

        // Teammate's basic stat
        teammate.level = ImportData.teammates.teammate[teammateID].level;
        
        teammate.baseHealthPoint = ImportData.teammates.teammate[teammateID].baseHealthPoint;
        teammate.baseAttackPoint = ImportData.teammates.teammate[teammateID].baseAttackPoint;
        teammate.baseDefencePoint = ImportData.teammates.teammate[teammateID].baseDefencePoint;
        teammate.baseDexterityPoint = ImportData.teammates.teammate[teammateID].baseDexterityPoint;
        teammate.basePerceptionPoint = ImportData.teammates.teammate[teammateID].basePerceptionPoint;
        teammate.baseConstitutionPoint = ImportData.teammates.teammate[teammateID].baseConstitutionPoint;

        // Initialize data
        teammate.maxHealthPoint = new EntityStat(teammate.baseHealthPoint);
        teammate.attackPoint = new EntityStat(teammate.baseAttackPoint);
        teammate.defencePoint = new EntityStat(teammate.baseDefencePoint);
        teammate.dexterityPoint = new EntityStat(teammate.baseDexterityPoint);
        teammate.perceptionPoint = new EntityStat(teammate.basePerceptionPoint);
        teammate.constitutionPoint = new EntityStat(teammate.baseConstitutionPoint);

        teammate.speedPoint = new EntityStat(teammate.dexterityPoint.GetStatValue());
        teammate.evasionPoint = new EntityStat(teammate.dexterityPoint.GetStatValue());
        teammate.criticalPoint = new EntityStat(teammate.perceptionPoint.GetStatValue());
        teammate.accuracyPoint = new EntityStat(teammate.perceptionPoint.GetStatValue());
        teammate.resistancePoint = new EntityStat(teammate.constitutionPoint.GetStatValue());

        teammate.currentMaxHealthPoint = new EntityStat(teammate.maxHealthPoint.GetStatValue());
        teammate.currentAttackPoint = new EntityStat(teammate.attackPoint.GetStatValue());
        teammate.currentDefencePoint = new EntityStat(teammate.defencePoint.GetStatValue());
        teammate.currentDexterityPoint = new EntityStat(teammate.dexterityPoint.GetStatValue());
        teammate.currentPerceptionPoint = new EntityStat(teammate.perceptionPoint.GetStatValue());
        teammate.currentConstitutionPoint = new EntityStat(teammate.constitutionPoint.GetStatValue());

        teammate.currentSpeedPoint = new EntityStat(teammate.speedPoint.GetStatValue());
        teammate.currentEvasionPoint = new EntityStat(teammate.evasionPoint.GetStatValue());
        teammate.currentCriticalPoint = new EntityStat(teammate.criticalPoint.GetStatValue());
        teammate.currentAccuracyPoint = new EntityStat(teammate.accuracyPoint.GetStatValue());
        teammate.currentResistancePoint = new EntityStat(teammate.resistancePoint.GetStatValue());

        teammate.currentAttackValue = teammate.GetAttackValue();
        teammate.currentDefenceValue = teammate.GetDefenseValue();
        teammate.currentSpeedValue = teammate.GetSpeedValue();
        teammate.currentMaxHealthValue = teammate.GetMaxHealthValue();
        teammate.currentHealthValue = teammate.currentMaxHealthValue;
        
        
        // Teammate's ability related
        teammate.activeAbilityID = ImportData.teammates.teammate[teammateID].activeAbilityID;
        teammate.activeAbilityLevel = ImportData.teammates.teammate[teammateID].activeAbilityLevel;

        teammate.activeAbility = ImportData.abilityDictionary[teammate.activeAbilityID];
        teammate.activeAbility.abilityLevel = teammate.activeAbilityLevel;
        //Debug.Log($"^ImportData.abilities.ability[teammate.activeAbilityID].abilityName =  {ImportData.abilitiesData.abilityData[teammate.activeAbilityID].abilityName}");

        teammate.maxActiveAbilityCD = teammate.activeAbility.baseAbilityCD;
        teammate.currentMaxActiveAbilityCD = new EntityStat(teammate.maxActiveAbilityCD);
        teammate.currentActiveAbilityCD = (int)teammate.currentMaxActiveAbilityCD.GetStatValue();

        teammate.maxActiveAbilityCost = teammate.activeAbility.baseAbilityCost;
        teammate.currentActiveAbilityCost = new EntityStat(teammate.maxActiveAbilityCost);

        teammate.activeAbility.InitalizingAbility(roundData.player, roundData.currentMob);

        teammate.currentActiveSkillCDText = teammate.transform.GetChild(2).GetComponent<TMP_Text>();
        teammate.currentActiveSkillCDText.text = teammate.currentActiveAbilityCD.ToString();
        //teammate.passiveAbilityID = new EntityStat(ImportData.teammates.teammate[teammateID].passiveAbilityID);
        //teammate.maxPassiveAbilityCD = new EntityStat(ImportData.teammates.teammate[teammateID].passiveAbilityCD);
        //teammate.maxPassiveAbilityCost = new EntityStat(ImportData.teammates.teammate[teammateID].maxPassiveAbilityCost);

        teammate.position = teammateCells.transform.GetChild(teammateOrder).position;
        teammate.transform.position = teammate.position;

        teammate.currentTotalAttackValue = 0;
        teammate.outputValue = teammate.currentTotalAttackValue;
        teammate.outputValueText = teammate.transform.GetChild(1).GetComponent<TMP_Text>();
        teammate.outputValueText.text = teammate.outputValue.ToString();


        teammate.teammatePic = teammate.GetComponent<Image>();
        Sprite teammateOrgImage = Resources.Load<Sprite>($"Prefabs/Teammate/{teammate.picName}");
        teammate.teammatePic.sprite = teammateOrgImage;

        teammate.frame = teammate.transform.GetChild(3).GetComponent<Image>();
        teammate.frame.gameObject.SetActive(false);


        Debug.Log($"{Time.time} TeammateFactory.CreateTeammate, return teammate(local var): {teammate.entityName}(teammate.name only) (end)"); 
        return teammate;
    }
    #endregion
}