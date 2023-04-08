using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BlueAndWhite.RoundManaging;
using BlueAndWhite.Entities;
using BlueAndWhite.DataManaging;
using BlueAndWhite.Abilities;

namespace BlueAndWhite.Objects
{
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
            Debug.Log($"TeammateFactory.Awake (start)");
            Debug.Log($"TeammateFactory.Awake (end)");
        }

        void OnEnable()
        {
            Debug.Log($"TeammateFactory.OnEnable (start)");
            Debug.Log($"TeammateFactory.OnEnable (end)");
        }
        
        void Start()
        {
            Debug.Log($"TeammateFactory.Start (start)");
            Debug.Log($"TeammateFactory.Start (end)");
        }

        void Update()
        {
            
        }

        void OnDisable()
        {
            Debug.Log($"TeammateFactory.OnDisable (start)");
            Debug.Log($"TeammateFactory.OnDisable (end)");
        }

        void OnDestroy() 
        {
            Debug.Log($"TeammateFactory.OnDestroy (start)");
            Debug.Log($"TeammateFactory.OnDestroy (end)");
        }
        #endregion

        #region Factory functions
        public Teammate CreateTeammate(int teammateID, int teammateOrder)
        {
            Debug.Log($"TeammateFactory.CreateTeammate (start)");

            Teammate teammate = Instantiate(teammatePrefab, teammateSpawner.transform).GetComponent<Teammate>();

            teammate.id = teammateID;
            teammate.teammateName = ImportData.teammates.teammate[teammateID].teammateName;
            teammate.picName = ImportData.teammates.teammate[teammateID].picName;
            teammate.level = ImportData.teammates.teammate[teammateID].level;
            teammate.maxHp = new EntityStat(ImportData.teammates.teammate[teammateID].maxHp);
            Debug.Log($"{teammate.name}'s maxHp is: {teammate.maxHp.value} ({ImportData.teammates.teammate[teammateID].maxHp})");
            teammate.attackPoint.value = ImportData.teammates.teammate[teammateID].attackPoint;
            teammate.defencePoint.value = ImportData.teammates.teammate[teammateID].defencePoint;
            //teammate.evasionPoint = ImportData.teammates.teammate[teammateID].evasionPoint;
            //teammate.criticalPoint = ImportData.teammates.teammate[teammateID].criticalPoint;
            //teammate.type = ImportData.teammates.teammate[teammateID].type;
            //teammate.faction = ImportData.teammates.teammate[teammateID].faction;
            teammate.activeSkillID.value = ImportData.teammates.teammate[teammateID].activeSkillID;
            teammate.passiveSkillID.value = ImportData.teammates.teammate[teammateID].passiveSkillID;
            teammate.maxActiveSkillCD.value = ImportData.teammates.teammate[teammateID].maxActiveSkillCD;
            Debug.Log($"{teammate.name}'s cd is {teammate.maxActiveSkillCD}");

            teammate.position = teammateCells.transform.GetChild(teammateOrder).position;
            teammate.transform.position = teammate.position;


            teammate.currentTotalAttackPoint = 0;
            teammate.outputValue = teammate.currentTotalAttackPoint;
            teammate.outputValueText = teammate.transform.GetChild(1).GetComponent<TMP_Text>();
            teammate.outputValueText.text = teammate.outputValue.ToString();

            teammate.currentActiveSkillCD.value = teammate.maxActiveSkillCD.value;
            teammate.currentActiveSkillCDText = teammate.transform.GetChild(2).GetComponent<TMP_Text>();
            teammate.currentActiveSkillCDText.text = teammate.currentActiveSkillCD.value.ToString();

            teammate.teammatePic = teammate.GetComponent<Image>();
            Sprite teammateOrgImage = Resources.Load<Sprite>($"Prefabs/Teammate/{teammate.picName}");
            teammate.teammatePic.sprite = teammateOrgImage;

            teammate.name = teammate.teammateName;

            Debug.Log($"TeammateFactory.CreateTeammate, return teammate(local var): {teammate.name}(teammate.name only) (end)"); 
            return teammate;
        }
        #endregion
    }
}