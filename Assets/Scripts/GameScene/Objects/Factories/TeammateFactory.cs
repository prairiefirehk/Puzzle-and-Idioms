using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TeammateFactory : MonoBehaviour, IFactory
{
    #region Scripts
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

        
        teammate.id = teammateID;
        teammate.teammateName = ImportData.teammates.teammate[teammateID].teammateName;
        teammate.picName = ImportData.teammates.teammate[teammateID].picName;
        teammate.level = ImportData.teammates.teammate[teammateID].level;
        //Debug.Log($"{Time.time} {teammate.name}'s level is: {teammate.level} ({ImportData.teammates.teammate[teammateID].level})");
        teammate.currentMaxHp = new EntityStat(ImportData.teammates.teammate[teammateID].maxHp);
        //Debug.Log($"{Time.time} {teammate.name}'s maxHp is: {teammate.currentMaxHp.value} ({ImportData.teammates.teammate[teammateID].maxHp})");
        teammate.attackPoint = new EntityStat(ImportData.teammates.teammate[teammateID].attackPoint);
        teammate.defencePoint = new EntityStat(ImportData.teammates.teammate[teammateID].defencePoint);
        //teammate.evasionPoint = ImportData.teammates.teammate[teammateID].evasionPoint;
        //teammate.criticalPoint = ImportData.teammates.teammate[teammateID].criticalPoint;
        //teammate.type = ImportData.teammates.teammate[teammateID].type;
        //teammate.faction = ImportData.teammates.teammate[teammateID].faction;
        teammate.activeSkillID = new EntityStat(ImportData.teammates.teammate[teammateID].activeSkillID);
        teammate.passiveSkillID = new EntityStat(ImportData.teammates.teammate[teammateID].passiveSkillID);
        teammate.maxActiveSkillCD= new EntityStat(ImportData.teammates.teammate[teammateID].maxActiveSkillCD);
        //Debug.Log($"{Time.time} {teammate.name}'s cd is {teammate.maxActiveSkillCD.value}");

        teammate.position = teammateCells.transform.GetChild(teammateOrder).position;
        teammate.transform.position = teammate.position;


        teammate.currentTotalAttackPoint = 0;
        teammate.outputValue = teammate.currentTotalAttackPoint;
        teammate.outputValueText = teammate.transform.GetChild(1).GetComponent<TMP_Text>();
        teammate.outputValueText.text = teammate.outputValue.ToString();

        teammate.currentActiveSkillCD = new EntityStat(teammate.maxActiveSkillCD.value);
        teammate.currentActiveSkillCDText = teammate.transform.GetChild(2).GetComponent<TMP_Text>();
        teammate.currentActiveSkillCDText.text = teammate.currentActiveSkillCD.value.ToString();

        teammate.teammatePic = teammate.GetComponent<Image>();
        Sprite teammateOrgImage = Resources.Load<Sprite>($"Prefabs/Teammate/{teammate.picName}");
        teammate.teammatePic.sprite = teammateOrgImage;

        teammate.name = teammate.teammateName;

        Debug.Log($"{Time.time} TeammateFactory.CreateTeammate, return teammate(local var): {teammate.name}(teammate.name only) (end)"); 
        return teammate;
    }
    #endregion
}