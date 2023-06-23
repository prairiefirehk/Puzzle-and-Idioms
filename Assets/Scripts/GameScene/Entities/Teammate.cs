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
    // Temp, for ability
    public Image frame;
    #endregion

    #region Teammate data
    [SerializeField] private int _teammateID;
    public int teammateID { get { return _teammateID; } set { _teammateID = value; } }
    [SerializeField] private string _teammateName;
    public string teammateName { get { return _teammateName; } set { _teammateName = value; } }
    [SerializeField] private string _picName;
    public string picName { get { return _picName; } set { _picName = value; } }
    [SerializeField] private float _currentTotalAttackValue;
    public float currentTotalAttackValue { get { return _currentTotalAttackValue; } set { _currentTotalAttackValue = value; } }

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
        roundManager = GameObject.Find("Round Manager").GetComponent<RoundManager>();
        uiManage = GameObject.Find("UI Manager").GetComponent<UIManage>();

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
        if (currentActiveAbilityCD <= 0 && (roundManager.currentGameState == GameState.State.IsFlying || roundManager.currentGameState == GameState.State.IsBattling))
        {
            frame.gameObject.SetActive(true);
            // Temp
            //currentTotalAttackValue += (attackPoint.value * 10f);
            if (currentMaxActiveAbilityCD.GetStatValue() <= 1)
            {
                currentMaxActiveAbilityCD.SetStatValue(1);
            }

            currentActiveAbilityCD = 0;
        }
        else
        {
            frame.gameObject.SetActive(false);
        }

        if (currentActiveAbilityCD > currentMaxActiveAbilityCD.GetStatValue())
        {
            currentActiveAbilityCD = (int)currentMaxActiveAbilityCD.GetStatValue();
        }
        
        currentActiveSkillCDText.text = currentActiveAbilityCD.ToString();
        UpdateOutputValue(currentTotalAttackValue, 0);
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
        if (currentTotalAttackValue > 0)
        {
            player.Attack(roundData.currentMob, currentTotalAttackValue);
            currentTotalAttackValue = 0;
            player.isActioned = true;
        }
        else if (currentActiveAbilityCD <= 0 && roundData.currentPowerScore >= activeAbility.currentAbilityCost)
        {
            activeAbility.OnTrigger(this);
            currentActiveAbilityCD = (int)currentMaxActiveAbilityCD.GetStatValue();
            player.isActioned = true;
        }
        else
        {
            Debug.Log($"Nothing happened, no ability or attack was released.");
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
            Debug.Log($"{Time.time} ^5.1A.1 teammate {entityName} receive {dragTile.name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text}");
        }

        else if (dragTile.CompareTag("SpecialTile"))
        {
            Debug.Log($"{Time.time} ^5.1A.2 teammate {entityName} receive {dragTile.name} (special tile)");
        }
        else
        {
            Debug.Log($"{Time.time} ^5.1A.3 Who the fuck are you receiving??");
        }

        player.Answer(this);

        Debug.Log($"{Time.time} {teammateName} Teammate.OnDrop (end)");
    }

    public void AnsweredCorrectly(Tile tile)
    {
        Debug.Log($"{Time.time} Teammate.AnswerCorrectly (start)");

        Debug.Log($"{Time.time} ^5.5A teammate get the correct answer!");

        // Temp
        //Heal(maxHp.value * tile.interactTeammate.defencePoint);
        if (player.currentHealthValue < player.currentMaxHealthValue)//(currentHp.GetStatValue() < maxHp.GetStatValue())
        {
            player.Heal(player.currentMaxHealthValue * 0.15f);
            //Debug.Log($"$healed hp = {maxHp.value * 0.15f}");
        }

        currentActiveAbilityCD -= 1;
        
        if (roundManager.currentGameState == GameState.State.IsFlying)
        {
            roundData.currentDistance += (int)roundData.player.currentSpeedValue;

            float controlNum = (roundData.roundMobNumber - roundData.currentSpawnedMobNumber) / ((roundData.roundDistance - roundData.currentDistance) / roundData.player.currentSpeedValue);
            //Debug.Log($"&controlNum = {controlNum}");
            float randomNum = UnityEngine.Random.Range(0, 100) / 100f;
            //Debug.Log($"&randomNum = {randomNum}");

            if (randomNum < controlNum)
            {
                roundData.flyToBattle = true;
                //roundManager.currentGameState = GameState.State.IsBattling;
            }
        }
        else if (roundManager.currentGameState == GameState.State.IsBattling)
        {
            // Consider to use tile.currentvalueModifier later
            currentTotalAttackValue += (roundData.player.GetAttackPoint() * GetAttackValue() * (tile.tileLevel + 1));
        }
        
        // Stop the wrong count combo
        roundData.wrongCountCombo = 0;

        // Contribute the score to board
        roundData.currentPowerScore += tile.GetOutPutPower() * 3;

        // Destroy the tile
        tile.toBeDestroyed = true;
        board.UpdateTileCell();
        tile.DestroyTile(tile);

        // Force next move/ round
        player.isActioned = true;


        Debug.Log($"{Time.time} Teammate.AnswerCorrectly (end)");
    }

    public void AnsweredWrongly(Tile tile)
    {
        Debug.Log($"{Time.time} Teammate.AnswerWrongly (start)");

        Debug.Log($"{Time.time} ^5.5B teammate get the wrong answer!");

        if (roundManager.currentGameState == GameState.State.IsFlying)
        {
            // Some punishment here
            roundData.currentDistance -= (int)roundData.player.currentSpeedValue / 2;

        }
        else if (roundManager.currentGameState == GameState.State.IsBattling)
        {
            // ?
        }

        // Some punishment here
        player.TakeDamage(player.currentMaxHealthValue * 0.15f);
        roundData.currentPowerScore -= tile.GetOutPutPower() * 2;

        // Record the count of wrong submittion
        roundData.wrongCountCombo += 1;
        roundData.totalWrongCount += 1;

        // Destroy the tile
        tile.toBeDestroyed = true;
        board.UpdateTileCell();
        tile.DestroyTile(tile);

        if (roundData.wrongCountCombo >= 5)
        {
            // Do some visual shit/popup/conversation
            string popupMessage = $"{roundData.player.entityName}答錯太多次啦! 正確答案係: {roundData.currentAnswerWord}";
            uiManage.SpawnPopup(popupMessage, "emergency");

            board.answerTile.transform.GetChild(0).GetComponent<Image>().color = new Color32(160, 60, 60, 255);
            board.answerTile.transform.GetChild(2).GetComponent<TMP_Text>().color = new Color32(255, 255, 255, 255);

            roundData.wrongCountCombo = 0;

            board.answerTile.toBeDestroyed = true;
            board.UpdateTileCell();

            // Destroy the tile
            //board.answerTile.DestroyTile(board.answerTile);

            this.Wait(roundData.currentTurnDuration, () => player.isActioned = true);
        }
        else
        {
            // Force next move/ round
            player.isActioned = true;
        }

        Debug.Log($"{Time.time} Teammate.AnswerWrongly (end)");
    }

    public void AnsweredSpecialTile(Tile tile)
    {
        Debug.Log($"{Time.time} Teammate.AnsweredSpecialTile (start)");

        Debug.Log($"{Time.time} ^5.5C teammate get the special tile!");

        if (roundManager.currentGameState == GameState.State.IsFlying)
        {
            // ?
        }
        else if (roundManager.currentGameState == GameState.State.IsBattling)
        {
            // ?
        }

        // Force next move/ round
        player.isActioned = true;

        // Destroy the tile
        tile.toBeDestroyed = true;
        board.UpdateTileCell();
        tile.DestroyTile(tile);
    }
    #endregion
}

