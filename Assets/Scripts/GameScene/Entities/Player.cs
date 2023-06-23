using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : Entity
{
    #region Scripts
    public Tile tile;
    //public Teammate teammate;
    #endregion

    #region Teammates references
    [SerializeField] public Dictionary<TeammateType, Teammate> teammates = new Dictionary<TeammateType, Teammate>();
    public Teammate teammateLeader;
    public Teammate teammateTwo;
    public Teammate teammateThree;
    public Teammate teammateFour;
    public Teammate teammateFriend;
    #endregion

    #region Reserved data
    public float currentAttackFever;
    public float maxAttackFever;
    public float currentDefenceFever;
    public float maxDefenceFever;

    // Should I keep it here?
    public int currentCombo;
    public int highestComob;
    public bool isWaitingForReset;
    #endregion

    #region Events
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{Time.time} Player.Awake (start)");

        // Not ideal place
        board = GameObject.Find("Board").GetComponent<Board>();
        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();
        uiManage = GameObject.Find("UI Manager").GetComponent<UIManage>();
        roundManager = GameObject.Find("Round Manager").GetComponent<RoundManager>();

        // Temp solution before creating/transfering + importing player data

        Debug.Log($"{Time.time} Player.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} Player.OnEnable (start)");
        Debug.Log($"{Time.time} Player.OnEnable (end)");
    }

    void Start()
    {
        Debug.Log($"{Time.time} Player.Start (start)");

        // Move here will cause error
        //roundData = gameObject.GetComponent<RoundData>();
        //board = GameObject.Find("Board").GetComponent<Board>();

        Debug.Log($"{Time.time} Player.Start (end)");
    }

    void Update()
    {
        
        //Debug.Log($"{Time.time} player's baseHealthPoint = {baseHealthPoint}");
        //Debug.Log($"{Time.time} player's maxHealthPoint = {maxHealthPoint.value}");
        //Debug.Log($"{Time.time} player's currentMaxHealthPoint = {currentMaxHealthPoint.value}");
        //Debug.Log($"{Time.time} player's currentMaxHealthValue = {currentMaxHealthValue}");
        //Debug.Log($"{Time.time} player's currentHealthValue = {currentHealthValue}");
        //Debug.Log($"{Time.time} player's currentDexterityPoint(Get) = {currentDexterityPoint.GetStatValue()}");
        //Debug.Log($"{Time.time} player's currentDexterityPoint = {currentDexterityPoint.value}");
        DisplayCurrentStatusEffects();

        if (currentState == EntityState.State.Alive)
        {
            UpdateEntityStats();
            //Debug.Log( $"{name}'s health: {currentHp}");
            if (currentHealthValue <= 0)
            {
                // For visual
                currentHealthValue = 0f;

                Debug.Log($"{Time.time} {entityName} just dead!");
                // Trigger here to send msg to event subscribers that mob is defeated
                currentState = EntityState.State.Dead;
                //OnDefeatedEvent?.Invoke();
                //isDead = true;
                //break;
            }

            // Prevent current health bar expend outside screen, force fixed max health value
            if (currentHealthValue > currentMaxHealthValue && (currentHealthValue != currentMaxHealthValue))
            {
                currentHealthValue = currentMaxHealthValue;
            }
            
        }

        if (isActioned)
        {
            roundData.currentTurnState = TurnState.State.BeforePlayerMoveEnd;
            isActioned = false;
            //this.Wait(roundData.currentTurnDuration, BeforeMoveEnd);
            BeforeMoveEnd();
        }
    }

    void OnDisable()
    {
        Debug.Log($"{Time.time} Player.OnDisable (start)");
        Debug.Log($"{Time.time} Player.OnDisable (end)");
    }

    void OnDestroy()
    {
        Debug.Log($"{Time.time} Player.OnDestroy (start)");
        Debug.Log($"{Time.time} Player.OnDestroy (end)");
    }
    #endregion

    #region Player functions

    // Temp solution before creating/transfering + importing player data
    public void InitPlayer()
    {
        Debug.Log($"{Time.time} Player.InitPlayer (start)");

        // Temp solution, I know it's shit
        teammateLeader = roundData.SpawnTeammates(0, 0);
        teammates.Add(TeammateType.TeammateLeader, teammateLeader);

        teammateTwo = roundData.SpawnTeammates(1, 1);
        teammates.Add(TeammateType.TeammateTwo, teammateTwo);

        teammateThree = roundData.SpawnTeammates(2, 2);
        teammates.Add(TeammateType.TeammateThree, teammateThree);

        teammateFour = teammateTwo = roundData.SpawnTeammates(3, 3);
        teammates.Add(TeammateType.TeammateFour, teammateFour);

        teammateFriend = roundData.SpawnTeammates(4, 4);
        teammates.Add(TeammateType.TeammateFriend, teammateFriend);

        // Getting stat points from the teammates
        maxHealthPoint = new EntityStat(InitMaxHealthPoint());
        attackPoint = new EntityStat(1f); // Later can just apply all attack from teammate to multiple with this
        defencePoint = new EntityStat(InitDefencePoint());
        dexterityPoint = new EntityStat(InitDexterityPoint());
        perceptionPoint = new EntityStat(InitPerceptionPoint());
        constitutionPoint = new EntityStat(InitConstitutionPoint());

        speedPoint = new EntityStat(dexterityPoint.GetStatValue());
        evasionPoint = new EntityStat(dexterityPoint.GetStatValue());
        criticalPoint = new EntityStat(perceptionPoint.GetStatValue());
        accuracyPoint = new EntityStat(perceptionPoint.GetStatValue());
        resistancePoint = new EntityStat(constitutionPoint.GetStatValue());

        // First time initialize
        currentMaxHealthPoint = new EntityStat(maxHealthPoint.GetStatValue());
        currentAttackPoint = new EntityStat(attackPoint.GetStatValue());
        currentDefencePoint = new EntityStat(defencePoint.GetStatValue());
        currentDexterityPoint = new EntityStat(dexterityPoint.GetStatValue());
        currentPerceptionPoint = new EntityStat(perceptionPoint.GetStatValue());
        currentConstitutionPoint = new EntityStat(constitutionPoint.GetStatValue());

        currentSpeedPoint = new EntityStat(speedPoint.GetStatValue());
        currentEvasionPoint = new EntityStat(evasionPoint.GetStatValue());
        currentCriticalPoint = new EntityStat(criticalPoint.GetStatValue());
        currentAccuracyPoint = new EntityStat(accuracyPoint.GetStatValue());
        currentResistancePoint = new EntityStat(resistancePoint.GetStatValue());

        currentAttackValue = GetAttackValue();
        currentDefenceValue = GetDefenseValue();
        currentSpeedValue = GetSpeedValue();
        currentMaxHealthValue = GetMaxHealthValue();
        currentHealthValue = currentMaxHealthValue;

        currentState = EntityState.State.Alive;

        this.entityName = "你";

        Debug.Log($"{Time.time} Player.InitPlayer (end)");
    }
    
    // Getting entity stats, convert points into actual value
    public float InitMaxHealthPoint()
    {
        Debug.Log($"{Time.time} {entityName} Player.InitMaxHealthPoint (start)");

        float playerMaxHealthPoint = 0;
        //Debug.Log($"{roundData.roundTeammates == null}");
        foreach (KeyValuePair<TeammateType, Teammate> teammate in teammates)
        {
            playerMaxHealthPoint += teammate.Value.currentMaxHealthPoint.GetStatValue();
        }

        Debug.Log($"{Time.time} {entityName} Player.InitMaxHealthPoint, return playerMaxHealthPoint (local var): {playerMaxHealthPoint} (end)");
        return playerMaxHealthPoint;
    }
    // Player's attack point = 1 * (modifier), no need to be combined
    /*
    public override float GetAttackPoint()
    {
        Debug.Log($"{Time.time} {name} Player.GetAttackPoint (start)");

        float playerAttackPoint = 0;
        //Debug.Log($"{roundData.roundTeammates == null}");
        foreach (KeyValuePair<TeammateType, Teammate> teammate in teammates)
        {
            playerAttackPoint += teammate.Value.currentAttackPoint.value;
        }

        Debug.Log($"{Time.time} {name} Player.GetAttackPoint, return playerAttackPoint (local var): {playerAttackPoint} (end)");
        return playerAttackPoint;
    }
    */
    public float InitDefencePoint()
    {
        Debug.Log($"{Time.time} {entityName} Player.InitDefencePoint (start)");

        float playerDefencePoint = 0;
        //Debug.Log($"{roundData.roundTeammates == null}");
        foreach (KeyValuePair<TeammateType, Teammate> teammate in teammates)
        {
            playerDefencePoint += teammate.Value.currentDefencePoint.GetStatValue();
        }

        Debug.Log($"{Time.time} {entityName} Player.InitDefencePoint, return playerDefencePoint (local var): {playerDefencePoint} (end)");
        return playerDefencePoint;
    }
    public float InitDexterityPoint()
    {
        Debug.Log($"{Time.time} {entityName} Player.InitDexterityPoint (start)");

        float playerDexterityPoint = 0;
        //Debug.Log($"{roundData.roundTeammates == null}");
        foreach (KeyValuePair<TeammateType, Teammate> teammate in teammates)
        {
            playerDexterityPoint += teammate.Value.currentDexterityPoint.GetStatValue();
        }

        Debug.Log($"{Time.time} {entityName} Player.InitDexterityPoint, return playerDexterityPoint (local var): {playerDexterityPoint} (end)");
        return playerDexterityPoint;
    }
    public float InitPerceptionPoint()
    {
        Debug.Log($"{Time.time} {entityName} Player.InitPerceptionPoint (start)");

        float playerPerceptionPoint = 0;
        //Debug.Log($"{roundData.roundTeammates == null}");
        foreach (KeyValuePair<TeammateType, Teammate> teammate in teammates)
        {
            playerPerceptionPoint += teammate.Value.currentPerceptionPoint.GetStatValue();
        }

        Debug.Log($"{Time.time} {entityName} Player.InitPerceptionPoint, return playerPerceptionPoint (local var): {playerPerceptionPoint} (end)");
        return playerPerceptionPoint;
    }
    public float InitConstitutionPoint()
    {
        Debug.Log($"{Time.time} {entityName} Player.InitConstitutionPoint (start)");

        float playerConstitutionPoint = 0;
        //Debug.Log($"{roundData.roundTeammates == null}");
        foreach (KeyValuePair<TeammateType, Teammate> teammate in teammates)
        {
            playerConstitutionPoint += teammate.Value.currentConstitutionPoint.GetStatValue();
        }

        Debug.Log($"{Time.time} {entityName} Player.InitConstitutionPoint, return playerConstitutionPoint (local var): {playerConstitutionPoint} (end)");
        return playerConstitutionPoint;
    }

    /*
    public float InitSpeedPoint()
    {
        Debug.Log($"{Time.time} {entityName} Player.InitSpeedPoint (start)");

        float playerSpeedPoint = 0;
        List<float> teammateSpeedPoints = new List<float>();

        foreach (KeyValuePair<TeammateType, Teammate> teammate in teammates)
        {
            teammateSpeedPoints.Add(teammate.Value.currentSpeedPoint.GetStatValue());
        }

        playerSpeedPoint = teammateSpeedPoints.Max();

        Debug.Log($"{Time.time} {entityName} Player.InitSpeedPoint, return playerSpeedPoint (local var): {playerSpeedPoint} (end)");
        return playerSpeedPoint;
    }
    */

    // Turn based functions
    public override void BeforeMoveStart()
    {
        Debug.Log($"{Time.time} Player.BeforeMoveStart (override Entities.BeforeMoveStart) (start)");

        // Change turn state and data
        roundData.currentTurnState = TurnState.State.BeforePlayerMoveStart;
        roundData.currentTurnDuration = roundData.baseTurnDuration;

        CheckAlive();
        //CheckStatusEffects();

        if (roundManager.currentGameState == GameState.State.IsFlying)
        {
            this.Wait(roundData.currentTurnDuration, MoveStart);
        }
        else if (/*(!isStun) &&*/ (currentState == EntityState.State.Alive) && (roundData.currentMob.currentState == EntityState.State.Alive))
        {
            this.Wait(roundData.currentTurnDuration, MoveStart);
        }
        else
        {
            this.Wait(roundData.currentTurnDuration,MoveEnd);
        }

        Debug.Log($"{Time.time} Player.BeforeMoveStart (override Entities.BeforeMoveStart) (end)");
    }

    public override void MoveStart()
    {
        Debug.Log($"{Time.time} Player.MoveStart (override Entities.MoveStart) (start)");

        // Change turn state and data
        roundData.currentTurnState = TurnState.State.PlayerMoveStart;
        roundData.currentTurnDuration = roundData.baseTurnDuration;

        // Do some visual shit/popup/conversation
        string popupMessage = $"輪到{entityName}開始行動啦!";
        uiManage.SpawnPopup(popupMessage, "normal");
        board.EnableBoard();

        Debug.Log($"{Time.time} Player.MoveStart (override Entities.MoveStart) (end)");
    }

    public override void CheckAction()
    {
        // If player is dragging/click teammate -> roundData.currentTurnState = TurnState.State.PlayerAction
        // Else roundData.currentTurnState = TurnState.State.WaitingPlayerAction;

        // Change turn state and data
        roundData.currentTurnState = TurnState.State.WaitingPlayerAction;
        roundData.currentTurnDuration = roundData.baseTurnDuration;
        

        // Do some visual shit/popup/conversation
    }

    public override void OnAction()
    {
        // Change turn state and data
        roundData.currentTurnState = TurnState.State.PlayerAction;
        roundData.currentTurnDuration = roundData.baseTurnDuration;

        // Do some visual shit/popup/conversation

    }

    public override void BeforeMoveEnd()
    {
        Debug.Log($"{Time.time} Player.BeforeMoveEnd (override Entities.BeforeMoveEnd) (start)");

        // Change turn state and data
        roundData.currentTurnState = TurnState.State.BeforePlayerMoveEnd;
        roundData.currentTurnDuration = roundData.baseTurnDuration;

        // Do some visual shit/popup/conversation
        board.DisableBoard();

        this.Wait(roundData.currentTurnDuration, MoveEnd); //0.5

        Debug.Log($"{Time.time} Player.BeforeMoveEnd (override Entities.BeforeMoveEnd) (start)");
    }

    public override void MoveEnd()
    {
        Debug.Log($"{Time.time} Player.MoveEnd (override Entities.MoveEnd) (start)");

        // Change turn state and data
        roundData.currentTurnState = TurnState.State.PlayerMoveEnd;
        roundData.currentTurnDuration = roundData.baseTurnDuration;
        
        /*
        if (hasStatusEffect) 
        {
            for (int i = 0; i < currentStatusEffects.Count; i++)
            {
                //currentStatusEffects[i].remainingTurn -= 1;
            }
        }
        */

        // Do some visual shit/popup/conversation
        string popupMessage = $"{entityName}嘅行動完結咗";
        uiManage.SpawnPopup(popupMessage, "normal");

        moveEnded = true;

        if (roundManager.currentGameState == GameState.State.IsFlying)
        {
            this.Wait(roundData.currentTurnDuration, roundData.TurnEnd);
        }
        else if (roundData.currentMob.moveEnded)
        {
            this.Wait(roundData.currentTurnDuration, roundData.TurnEnd);
        }
        else
        {
            this.Wait(roundData.currentTurnDuration, roundData.currentMob.BeforeMoveStart);
        }

        Debug.Log($"{Time.time} Player.MoveEnd (override Entities.MoveEnd) (end)");
    }

    // Get the player max hp
    public float GetPlayerMaxHp()
    {
        Debug.Log($"{Time.time} Player.GetPlayerMaxHp (start)");

        float playerMaxHp = 0;
        //Debug.Log($"{roundData.roundTeammates == null}");
        foreach (KeyValuePair<TeammateType, Teammate> teammate in teammates)
        {
            playerMaxHp += teammate.Value.currentMaxHealthValue;
        }

        Debug.Log($"{Time.time} Player.GetPlayerMaxHp, return playerMaxHp(local var): {playerMaxHp} (end)");
        return playerMaxHp;
    }

    // Get the tile which is dragging
    public void SetDragTile(Tile dragTile)
    {
        Debug.Log($"{Time.time} Player.SetDragTile (start)");

        tile = dragTile;

        Debug.Log($"{Time.time} Player.SetDragTile (end)");
    }

    public void Answer(Teammate teammate)
    {
        Debug.Log($"{Time.time} Player.Answer (start)");

        if (tile.CompareTag("NormalTile"))
        {
            if (tile.isAnswer == true)
            {
                teammate.AnsweredCorrectly(tile);
            }
            else
            {
                teammate.AnsweredWrongly(tile);
            }
        }
        else if (tile.CompareTag("SpecialTile"))
        {
            tile.GetComponent<SpecialTile>().tileMainEffect.OnDropEffect();
            teammate.AnsweredSpecialTile(tile);
        }
        

        Debug.Log($"{Time.time} Player.Answer (end)");
    }
    #endregion
}

