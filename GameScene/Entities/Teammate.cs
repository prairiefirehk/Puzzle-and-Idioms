using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BlueAndWhite.RoundManaging;
using BlueAndWhite.Objects;
using BlueAndWhite.Entities;
using BlueAndWhite.Abilities;
using TMPro;

namespace BlueAndWhite.Entities
{
    public class Teammate : Entity, IPointerDownHandler, IDropHandler
    {
        #region Scripts
        public RoundData roundData;
        public Player player;
        public Board board;
        #endregion

        #region Game object references
        public GameObject teammateBox;
        public Vector2 position;
        public Image teammatePic;
        public TMP_Text currentActiveSkillCDText;
        public TMP_Text outputValueText;
        #endregion

        #region Teammate data
        [SerializeField] private int _id;
        public int id { get { return _id; } set { _id = value; } }
        [SerializeField] private string _teammateName;
        public string teammateName { get { return _teammateName; } set { _teammateName = value; } }
        [SerializeField] private string _picName;
        public string picName { get { return _picName; } set { _picName = value; } }
        [SerializeField] private EntityStat _activeSkillID;
        public EntityStat activeSkillID { get { return _activeSkillID; } set { _activeSkillID = value; } }
        [SerializeField] private EntityStat _passiveSkillID;
        public EntityStat passiveSkillID { get { return _passiveSkillID; } set { _passiveSkillID = value; } }
        [SerializeField] private EntityStat _maxActiveSkillCD;
        public EntityStat maxActiveSkillCD { get { return _maxActiveSkillCD; } set { _maxActiveSkillCD = value; } }
        [SerializeField] private EntityStat _currentActiveSkillCD;
        public EntityStat currentActiveSkillCD { get { return _currentActiveSkillCD; } set { _currentActiveSkillCD = value; } }

        [SerializeField] private float _currentTotalAttackPoint;
        public float currentTotalAttackPoint { get { return _currentTotalAttackPoint; } set { _currentTotalAttackPoint = value; } }

        public float lerpSpeed;
        //public int weaponType;
        //public Image weaponTypeIcon;
        public float outputValue;
        #endregion

        #region Flow
        void Awake()
        {
            Debug.Log(message: $"{teammateName} Teammate.Awake (start)");

            // Not ideal place
            player = GameObject.Find("Round Manager").GetComponent<Player>();
            roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();
            board = GameObject.Find("Board").GetComponent<Board>();

            Debug.Log(message: $"{teammateName} Teammate.Awake (end)");
        }

        void OnEnable()
        {
            Debug.Log($"{teammateName} Teammate.OnEnable (start)");

            // Subscribe to the game events and listen
            Board.OnEndTurnEvent += OnNewTurn;

            Debug.Log($"{teammateName} Teammate.OnEnable (end)");
        }

        void Start()
        {
            Debug.Log($"{teammateName} Teammate.Start (start)");
            Debug.Log($"{teammateName} Teammate.Start (end)");
        }

        void Update()
        {
            if (currentActiveSkillCD.value == 0)
                {
                    // Temp
                    currentTotalAttackPoint += (attackPoint.value * 10f);
                    currentActiveSkillCD.value = maxActiveSkillCD.value;
                    //Board.OnNewTurnEvent?.Invoke();
                }
            
            currentActiveSkillCDText.text = currentActiveSkillCD.value.ToString();
            UpdateOutputValue(currentTotalAttackPoint, 0);
        }

        void OnDisable()
        {
            Debug.Log($"{teammateName} Teammate.OnDisable (start)");

            // Unsubscribe to the game events
            Board.OnEndTurnEvent -= OnNewTurn;

            Debug.Log($"{teammateName} Teammate.OnDisable (end)");
        }

        void OnDestroy() 
        {
            Debug.Log($"{teammateName} Teammate.OnDestroy (start)");
            Debug.Log($"{teammateName} Teammate.OnDestroy (end)");
        }
        #endregion

        #region Teammate functions
        public void UpdateOutputValue(float newValue, int decimalplace)
        {
            //Debug.Log($"{name} Teammate.UpdateOutputValue (start)");
            //Debug.Log($"newValue = {newValue}");
            lerpSpeed = 3f * Time.deltaTime;
            outputValue = Mathf.Lerp(outputValue, newValue, lerpSpeed);

            // Processing bar text
            outputValueText.text = outputValue.ToString("F" + decimalplace);
            
            //Debug.Log($"{name} Teammate.UpdateOutputValue (end)");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log($"{teammateName} Teammate.OnPointerDown (start)");

            Debug.Log("input clicked the teammate!");
            if (currentTotalAttackPoint > 0)
            {
                player.Attack(roundData.currentMob, currentTotalAttackPoint);
                currentTotalAttackPoint = 0;
                board.EndTurn();
            }
            
            Debug.Log($"{teammateName} Teammate.OnPointerDown (end)");
        }

        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log($"{teammateName} Teammate.OnDrop (start)");

            Tile dragTile = eventData.pointerDrag.GetComponent<Tile>();

            if (dragTile.CompareTag("NormalTile"))
            {
                Debug.Log($"^5.1B.1 {dragTile.name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text} drop to teammate {name} (msg from teammate)");
            }
            // That should not happen
            else if (dragTile.CompareTag("SpecialTile"))
            {
                Debug.Log($"^5.1B.2 {dragTile.name} (special tile) drop to teammate {name} (msg from teammate)");
            }
            else
            {
                Debug.Log("Who the fuck are you??");
            }
            
            player.SetDragTile(dragTile);

            if (dragTile.CompareTag("NormalTile"))
            {
                Debug.Log($"^5.1C.1 teammate {name} receive {dragTile.name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text}");
            }
            // That should not happen
            else if (dragTile.CompareTag("SpecialTile"))
            {
                Debug.Log($"^5.1C.2 teammate {name} receive {dragTile.name} (special tile)");
            }
            else
            {
                Debug.Log("^5.1C.3 Who the fuck are you receiving??");
            }
                
            player.Answer();

            Debug.Log($"{teammateName} Teammate.OnDrop (end)");
        }

        public void OnNewTurn()
        {
            Debug.Log($"{teammateName} Teammate.OnNewTurn (start)");

            currentActiveSkillCD.value -= 1;

            Debug.Log($"{teammateName} Teammate.OnNewTurn (end)");
        }
        #endregion
    }
}
