using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    #region Scripts
    public Tile tile;
    //public Teammate teammate;
    #endregion

    #region Teammates references
    public List<Teammate> team;
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
    public bool isActioned = false;
    #endregion

    #region Events
    //public static event Action<GameState.State> OnDefeatedEvent;
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"Player.Awake (start)");

        // Not ideal place
        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();
        board = GameObject.Find("Board").GetComponent<Board>();

        // Temp solution before creating/transfering + importing player data

        Debug.Log($"Player.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"Player.OnEnable (start)");
        Debug.Log($"Player.OnEnable (end)");
    }

    void Start()
    {
        Debug.Log($"Player.Start (start)");

        // Move here will cause error
        //roundData = gameObject.GetComponent<RoundData>();
        //board = GameObject.Find("Board").GetComponent<Board>();

        Debug.Log($"Player.Start (end)");
    }

    void Update()
    {
        //Debug.Log($"$player's currentHp = {currentHp.value}");
        //Debug.Log($"$player's maxHp = {maxHp.value}");

        /*
        if (currentHp.value < 0)//(currentHp.GetStatValue() < 0)
        {
            //currentHp.SetStatValue(0f);
            currentHp.value = 0f;
        }

        if (currentHp.value > maxHp.value && (currentHp.value != maxHp.value))//(currentHp.GetStatValue() > maxHp.GetStatValue() && (currentHp.GetStatValue() != maxHp.GetStatValue()))
        {
            //currentHp.SetStatValue(maxHp.GetStatValue());
            currentHp.value = maxHp.value;
        }
        
        if (!isDead)
        {
            if (currentHp.value <= 0)//(currentHp.GetStatValue() <= 0)
            {
                Debug.Log("player just dead!");
                // Trigger here to send msg to event subscribers that player is defeated
                OnDefeatedEvent?.Invoke(GameState.State.PlayerLose);
                isDead = true;
            }
        }
        */
        if (isActioned)
        {
            roundData.currentTurnState = TurnState.State.BeforePlayerMoveEnd;
            isActioned = false;
            BeforeMoveEnd();
        }
    }

    void OnDisable()
    {
        Debug.Log($"Player.OnDisable (start)");
        Debug.Log($"Player.OnDisable (end)");
    }

    void OnDestroy()
    {
        Debug.Log($"Player.OnDestroy (start)");
        Debug.Log($"Player.OnDestroy (end)");
    }
    #endregion

    #region Player functions

    // Temp solution before creating/transfering + importing player data
    public void InitPlayer()
    {
        Debug.Log($"Player.InitPlayer (start)");

        maxHp = new EntityStat(GetPlayerMaxHp());
        currentHp = new EntityStat(maxHp.GetStatValue());
        defencePoint = new EntityStat(125f);
        dexterityPoint = new EntityStat(10f);

        currentState = EntityState.State.Alive;

        Debug.Log($"Player.InitPlayer (end)");
    }
    public override void BeforeMoveStart()
    {
        Debug.Log($"Player.BeforeMoveStart (override Entities.BeforeMoveStart) (start)");

        Debug.Log($"^RoundData is null? {roundData == null}");

        // How come null
        roundData.currentTurnState = TurnState.State.BeforePlayerMoveStart;
        CheckStatusEffects();

        if (!isStun)
        {
            MoveStart();
        }
        else
        {
            MoveEnd();
        }

        Debug.Log($"Player.BeforeMoveStart (override Entities.BeforeMoveStart) (end)");
    }

    public override void MoveStart()
    {
        Debug.Log($"Player.MoveStart (override Entities.MoveStart) (start)");

        roundData.currentTurnState = TurnState.State.PlayerMoveStart;

        // Do some visual shit/popup/conversation
        board.EnableBoard();

        Debug.Log($"Player.MoveStart (override Entities.MoveStart) (end)");
    }

    public override void CheckAction()
    {
        // If player is dragging/click teammate -> roundData.currentTurnState = TurnState.State.PlayerAction
        // Else roundData.currentTurnState = TurnState.State.WaitingPlayerAction;

        roundData.currentTurnState = TurnState.State.WaitingPlayerAction;
        
        

        // Do some visual shit/popup/conversation
    }

    public override void OnAction()
    {
        roundData.currentTurnState = TurnState.State.PlayerAction;

        // Do some visual shit/popup/conversation

        

    }

    public override void BeforeMoveEnd()
    {
        Debug.Log($"Player.BeforeMoveEnd (override Entities.BeforeMoveEnd) (start)");

        roundData.currentTurnState = TurnState.State.BeforePlayerMoveEnd;

        // Do some visual shit/popup/conversation
        board.DisableBoard();

        MoveEnd();

        Debug.Log($"Player.BeforeMoveEnd (override Entities.BeforeMoveEnd) (start)");
    }

    public override void MoveEnd()
    {
        Debug.Log($"Player.MoveEnd (override Entities.MoveEnd) (start)");

        roundData.currentTurnState = TurnState.State.PlayerMoveEnd;
        

        if (HasStatusEffect) 
        {
            for (int i = 0; i < currentStatusEffects.Count; i++)
            {
                //currentStatusEffects[i].remainingTurn -= 1;

            }
        }

        // Do some visual shit/popup/conversation

        moveEnded = true;
        if (roundData.currentMob.moveEnded)
        {
            roundData.TurnEnd();
        }
        else
        {
            roundData.currentMob.BeforeMoveStart();
        }

        Debug.Log($"Player.MoveEnd (override Entities.MoveEnd) (end)");
    }

    // Get the player max hp
    public float GetPlayerMaxHp()
    {
        Debug.Log($"Player.GetPlayerMaxHp (start)");

        float playerMaxHp = 0;
        //Debug.Log($"{roundData.roundTeammates == null}");
        for (int i = 0; i < roundData.roundTeammates.Count; i++)
        {
            //playerMaxHp += roundData.roundTeammates[i].maxHp.GetStatValue();
            playerMaxHp += roundData.roundTeammates[i].maxHp.value;
            //Debug.Log(playerMaxHp);
        }

        Debug.Log($"Player.GetPlayerMaxHp, return playerMaxHp(local var): {playerMaxHp} (end)");
        return playerMaxHp;
    }

    // Get the tile which is dragging
    public void SetDragTile(Tile dragTile)
    {
        Debug.Log($"Player.SetDragTile (start)");

        tile = dragTile;

        Debug.Log($"Player.SetDragTile (end)");
    }

    public void Answer()
    {
        Debug.Log($"Player.Answer (start)");

        if (tile.isAnswer == true)
        {
            AnswerCorrectly();
        }
        else
        {
            AnswerWrongly();
        }

        Debug.Log($"Player.Answer (end)");
    }

    public void AnswerCorrectly()
    {
        Debug.Log($"Player.AnswerCorrectly (start)");

        Debug.Log("^5.5A teammate get the correct answer!");

        Debug.Log(tile.currentvalueModifier);
        Debug.Log(tile.interactTeammate.name);

        //Heal(maxHp.value * tile.interactTeammate.defencePoint);
        if (currentHp.value < maxHp.value)//(currentHp.GetStatValue() < maxHp.GetStatValue())
        {
            Heal(maxHp.value * 0.15f);
            Debug.Log($"$healed hp = {maxHp.value * 0.15f}");
        }

        tile.interactTeammate.currentTotalAttackPoint += (tile.interactTeammate.attackPoint.value * (1 + tile.currentvalueModifier));
        //isWaitingForReset = true;
        tile.toBeDestroyed = true;
        board.UpdateTileCell();

        // Destroy the tile
        tile.DestroyTile(tile);

        //// Next round preparation ////
        isActioned = true;
        //roundData.TurnEnd();

        // Before drawing the tile to the board, rename the tile according to their current position
        //board.RenameTiles();

        // Draw new answer tile and refill blank cell
        //board.SpawnTiles(board.CheckBlankCell());
        //board.DrawAnswer();

        //tile.StopDrag();

        Debug.Log($"Player.AnswerCorrectly (end)");
    }

    public void AnswerWrongly()
    {
        Debug.Log($"Player.AnswerWrongly (start)");

        Debug.Log("^5.5B teammate get the wrong answer!");

        // Some punishment here
        TakeDamage(maxHp.value * 0.15f);
        tile.toBeDestroyed = true;
        board.UpdateTileCell();

        // Destroy the tile
        tile.DestroyTile(tile);

        //// Next round preparation ////
        isActioned = true;
        //roundData.TurnEnd();
        // Before drawing the tile to the board, rename the tile according to their current position
        //board.RenameTiles();

        // Draw new answer tile and refill blank cell
        //board.SpawnTiles(board.CheckBlankCell());
        //board.DrawAnswer();

        Debug.Log($"Player.AnswerWrongly (end)");
    }
    #endregion
}

