using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum TeammateType
{
    // Normally, I would set enum as 3-digit system as to leave space for future expension.
    // But this time, I want it to relate the order of dictionary from player's teammates
    TeammateLeader = 0,
    TeammateTwo = 1,
    TeammateThree = 2,
    TeammateFour = 3,
    TeammateFriend = 4
}

public class Teammate : Entity, IPointerDownHandler, IDropHandler
{
    #region Scripts
    #endregion

    #region Game object references
    //public GameObject teammateBox;
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
        Debug.Log(message: $"{Time.time} {teammateName} Teammate.Awake (start)");

        // Not ideal place
        player = GameObject.Find("Round Manager").GetComponent<Player>();
        board = GameObject.Find("Board").GetComponent<Board>();
        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();

        Debug.Log(message: $"{Time.time} {teammateName} Teammate.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} {teammateName} Teammate.OnEnable (start)");

        // Subscribe to the game events and listen
        //Board.OnEndTurnEvent += OnNewTurn;

        Debug.Log($"{Time.time} {teammateName} Teammate.OnEnable (end)");
    }

    void Start()
    {
        Debug.Log($"{Time.time} {teammateName} Teammate.Start (start)");
        Debug.Log($"{Time.time} {teammateName} Teammate.Start (end)");
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
        Debug.Log($"{Time.time} {teammateName} Teammate.OnDisable (start)");

        // Unsubscribe to the game events
        //Board.OnEndTurnEvent -= OnNewTurn;

        Debug.Log($"{Time.time} {teammateName} Teammate.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} {teammateName} Teammate.OnDestroy (start)");
        Debug.Log($"{Time.time} {teammateName} Teammate.OnDestroy (end)");
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
        Debug.Log($"{Time.time} {teammateName} Teammate.OnPointerDown (start)");

        //Debug.Log($"{Time.time} input clicked the teammate!");
        if (currentTotalAttackPoint > 0)
        {
            player.Attack(roundData.currentMob, currentTotalAttackPoint);
            currentTotalAttackPoint = 0;
            player.isActioned = true;
        }
        
        Debug.Log($"{Time.time} {teammateName} Teammate.OnPointerDown (end)");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"{Time.time} {teammateName} Teammate.OnDrop (start)");

        
        //Debug.Log($"who is null? roundData: {roundData == null}");
        //Debug.Log($"player: {player == null}");
        //Debug.Log($"tile: {player.tile == null}");
        Tile dragTile = eventData.pointerDrag.GetComponent<Tile>();

        //roundData.player.SetDragTile(dragTile);

        if (dragTile.CompareTag("NormalTile"))
        {
            Debug.Log($"{Time.time} ^5.1A.1 teammate {name} receive {dragTile.name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text}");
        }
        // Temp, that should not happen
        else if (dragTile.CompareTag("SpecialTile"))
        {
            Debug.Log($"{Time.time} ^5.1A.2 teammate {name} receive {dragTile.name} (special tile)");
        }
        else
        {
            Debug.Log($"{Time.time} ^5.1A.3 Who the fuck are you receiving??");
        }
            
        player.Answer(this);

        Debug.Log($"{Time.time} {teammateName} Teammate.OnDrop (end)");
    }

    public void AnswerCorrectly(Tile tile)
    {
        Debug.Log($"{Time.time} Teammate.AnswerCorrectly (start)");

        Debug.Log($"{Time.time} ^5.5A teammate get the correct answer!");

        //Heal(maxHp.value * tile.interactTeammate.defencePoint);
        if (player.currentHp.value < player.currentMaxHp.value)//(currentHp.GetStatValue() < maxHp.GetStatValue())
        {
            player.Heal(player.currentMaxHp.value * 0.15f);
            //Debug.Log($"$healed hp = {maxHp.value * 0.15f}");
        }

        // Consider to use tile.currentvalueModifier later
        currentTotalAttackPoint += (attackPoint.value * (tile.tileLevel + 1));
        //isWaitingForReset = true;
        tile.toBeDestroyed = true;
        board.UpdateTileCell();

        // Contribute the score to board
        roundData.powerScore += tile.GetOutPutPower() * 3;

        // Destroy the tile
        tile.DestroyTile(tile);

        //// Next round preparation ////
        player.isActioned = true;


        Debug.Log($"{Time.time} Teammate.AnswerCorrectly (end)");
    }

    public void AnswerWrongly(Tile tile)
    {
        Debug.Log($"{Time.time} Teammate.AnswerWrongly (start)");

        Debug.Log($"{Time.time} ^5.5B teammate get the wrong answer!");

        // Some punishment here
        player.TakeDamage(player.currentMaxHp.value * 0.15f);
        tile.toBeDestroyed = true;
        board.UpdateTileCell();
        roundData.powerScore -= tile.GetOutPutPower() * 2;

        // Destroy the tile
        tile.DestroyTile(tile);

        //// Next round preparation ////
        player.isActioned = true;

        Debug.Log($"{Time.time} Teammate.AnswerWrongly (end)");
    }
    #endregion
}

