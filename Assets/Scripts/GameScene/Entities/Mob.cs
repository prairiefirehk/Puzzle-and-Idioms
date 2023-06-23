using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class Mob: Entity
{
    #region Scripts
    #endregion

    #region Game object references
    [SerializeField] private Image _mobPicture;
    public Image mobPicture { get { return _mobPicture;} set { _mobPicture = value; } }
    #endregion

    #region Mob data
    [SerializeField] private int _mobID;
    public int mobID { get { return _mobID; } set { _mobID = value; } }

    [SerializeField] private string _mobName;
    public string mobName { get { return _mobName; } set { _mobName = value; } }

    [SerializeField] private string _mobPicName;
    public string mobPicName { get { return _mobPicName; } set { _mobPicName = value; } }

    // CD turn for mob
    [SerializeField] private int _maxAttackInterval;
    public int maxAttackInterval { get { return _maxAttackInterval; } set { _maxAttackInterval = value; } }
    [SerializeField] private EntityStat _currentMaxAttackInterval;
    public EntityStat currentMaxAttackInterval { get { return _currentMaxAttackInterval; } set { _currentMaxAttackInterval = value; } }
    [SerializeField] private int _currentAttackInterval;
    public int currentAttackInterval { get { return _currentAttackInterval; } set { _currentAttackInterval = value; } }


    // Rewards by defeating this mob
    [SerializeField] private int _expReward;
    public int expReward { get { return _expReward; } set { _expReward = value; } }
    [SerializeField] private int _coinReward;
    public int coinReward { get { return _coinReward; } set { _coinReward = value; } }
    [SerializeField] private int _jadeReward;
    public int jadeReward { get { return _jadeReward; } set { _jadeReward = value; } }
    #endregion

    #region Events
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{Time.time} {mobName} Mob.Awake (start)");

        // Not ideal place
        player = GameObject.Find("Round Manager").GetComponent<Player>();
        board = GameObject.Find("Board").GetComponent<Board>();
        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();
        uiManage = GameObject.Find("UI Manager").GetComponent<UIManage>();
        roundManager = GameObject.Find("Round Manager").GetComponent<RoundManager>();

        Debug.Log($"{Time.time} {mobName} Mob.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} {mobName} Mob.OnEnable (start)");

        // Subscribe to the game events and listen
        //Board.OnEndTurnEvent += OnNewTurn;

        Debug.Log($"{Time.time} {mobName} Mob.OnEnable (end)");
    }

    void Start()
    {
        Debug.Log($"{Time.time} {mobName} Mob.Start (start)");
        
        // Move here didn't cause error
        Debug.Log($"{Time.time} {mobName} Mob.Start (end)");
    }
    
    
    void Update()
    {
        //Debug.Log($" currentMob {mobName} is {currentState}");
        DisplayCurrentStatusEffects();

        if (currentState == EntityState.State.Alive)
        {
            UpdateEntityStats();
            //Debug.Log( $"{name}'s health: {currentHp}");
            if (currentHealthValue <= 0)
            {
                // For visual
                currentHealthValue = 0f;

                Debug.Log($"{entityName} just dead!");
                // Trigger here to send msg to event subscribers that mob is defeated
                currentState = EntityState.State.Dead;
                //OnDefeatedEvent?.Invoke();
                //isDead = true;
                //break;
            }

            if (currentHealthValue > currentMaxHealthValue && (currentHealthValue != currentMaxHealthValue))
            {
                currentHealthValue = currentMaxHealthValue;
            }
        }
    }

    void OnDisable()
    {
        Debug.Log($"{Time.time} {mobName} Mob.OnDisable (start)");

        // Unsubscribe to the game events
        //Board.OnEndTurnEvent -= OnNewTurn;

        Debug.Log($"{Time.time} {mobName} Mob.OnDisable (end)");
    }

    void OnDestroy()
    {
        Debug.Log($"{Time.time} {mobName} Mob.OnDestroy (start)");
        Debug.Log($"{Time.time} {mobName} Mob.OnDestroy (end)");
    }
    #endregion

    #region Mob functions
    public bool CheckAttackInterval()
    {
        Debug.Log($"{Time.time} Mob.CheckAttackInterval (start)");

        bool canProceed = false;
        if (currentAttackInterval <= 0)
        {
            currentAttackInterval = 0;
            canProceed = true;
        }
        
        Debug.Log($"{Time.time} Mob.CheckAttackInterval, return canProceed(local var): {canProceed} (end)");
        return canProceed;
    }

    public override void BeforeMoveStart()
    {
        Debug.Log($"{Time.time} Mob.BeforeMoveStart (override Entities.BeforeMoveStart) (start)");

        // Change turn state and data
        roundData.currentTurnState = TurnState.State.BeforeMobMoveStart;
        roundData.currentTurnDuration = roundData.baseTurnDuration;

        CheckAlive();
        //CheckStatusEffects();

        if (CheckAttackInterval() /* && (!isStun) */ && (currentState == EntityState.State.Alive) && (roundData.player.currentState == EntityState.State.Alive))
        {
            this.Wait(roundData.currentTurnDuration, MoveStart);
        }
        else
        {
            if (currentMaxAttackInterval.GetStatValue() <= 0 || currentAttackInterval > currentMaxAttackInterval.GetStatValue())
            {
                currentAttackInterval = (int)currentMaxAttackInterval.GetStatValue();
            }

            this.Wait(roundData.currentTurnDuration, MoveEnd);
        }

        Debug.Log($"{Time.time} Mob.BeforeMoveStart (override Entities.BeforeMoveStart) (end)");
    }

    public override void MoveStart()
    {
        Debug.Log($"{Time.time} Mob.MoveStart (override Entities.MoveStart) (start)");

        // Change turn state and data
        roundData.currentTurnState = TurnState.State.MobMoveStart;
        roundData.currentTurnDuration = roundData.baseTurnDuration;

        // Do some visual shit/popup/conversation
        string popupMessage = $"輪到{entityName}開始行動啦!";
        uiManage.SpawnPopup(popupMessage, "normal");

        this.Wait(roundData.currentTurnDuration, OnAction);

        Debug.Log($"{Time.time} Mob.MoveStart (override Entities.MoveStart) (end)");
    }

    public override void CheckAction()
    {
        // Change turn state and data
        roundData.currentTurnState = TurnState.State.WaitingMobAction;
        roundData.currentTurnDuration = roundData.baseTurnDuration;

        // Do some visual shit/popup/conversation
    }

    public override void OnAction()
    {
        Debug.Log($"{Time.time} Mob.OnAction (override Entities.OnAction) (start)");

        // Change turn state and data
        roundData.currentTurnState = TurnState.State.MobAction;
        roundData.currentTurnDuration = roundData.baseTurnDuration;

        // Do some visual shit/popup/conversation

        // Temp way for decide the mob to use the skill or not
        int randomAttackMethod = UnityEngine.Random.Range(0, 100);

        if (randomAttackMethod < 60)
        {
            Debug.Log($"roundData.player = null? {roundData.player == null}");
            Attack(roundData.player, currentAttackValue);
        }
        else
        {
            activeAbility.OnTrigger(this);
        }
        
        isActioned = true;
        // Add extra one for later gerneral minus one in MoveEnd()
        currentAttackInterval = (int)currentMaxAttackInterval.GetStatValue();

        this.Wait(roundData.currentTurnDuration, BeforeMoveEnd); //0.5

        Debug.Log($"{Time.time} Mob.OnAction (override Entities.OnAction) (end)");
    }

    public override void BeforeMoveEnd()
    {
        Debug.Log($"{Time.time} Mob.BeforeMoveEnd (override Entities.BeforeMoveEnd) (start)");

        // Change turn state and data
        roundData.currentTurnState = TurnState.State.BeforeMobMoveEnd;
        roundData.currentTurnDuration = roundData.baseTurnDuration;

        // Do some visual shit/popup/conversation

        this.Wait(roundData.currentTurnDuration, MoveEnd); //0.5

        Debug.Log($"{Time.time} Mob.BeforeMoveEnd (override Entities.BeforeMoveEnd) (end)");
    }

    public override void MoveEnd()
    {
        Debug.Log($"{Time.time} Mob.MoveEnd (override Entities.MoveEnd) (start)");

        // Change turn state and data
        roundData.currentTurnState = TurnState.State.MobMoveEnd;
        roundData.currentTurnDuration = roundData.baseTurnDuration;

        // Do some visual shit/popup/conversation
        string popupMessage;
        if (currentState == EntityState.State.Alive)
        {
            if (isActioned)
            {
                popupMessage = $"{entityName}嘅行動完結咗";
            }
            else 
            {
                popupMessage = $"{entityName}冇任何行動";
            }

            uiManage.SpawnPopup(popupMessage, "normal");
        }

        moveEnded = true;

        if (roundData.player.moveEnded)
        {
            this.Wait(roundData.currentTurnDuration, roundData.TurnEnd);
        }
        else
        {
            this.Wait(roundData.currentTurnDuration, roundData.player.BeforeMoveStart);
        }

        Debug.Log($"{Time.time} Mob.MoveEnd (override Entities.MoveEnd) (end)");
    }

    // Temp
    public override void Attack(Entity target, float value)
    {
        Debug.Log($"{Time.time} Mob.Attack (override Entities.Attack) (start)");

        base.Attack(target, value);

        int randomTileNumber = UnityEngine.Random.Range(0, board.tilesInBoard.Count);

        /*
        // Only give it to normal tile?
        if (board.tilesInBoard[randomTileNumber].tileType == "special")
        {
            do 
            {
                randomTileNumber = UnityEngine.Random.Range(0, board.tilesInBoard.Count);
            } 
            while (board.tilesInBoard[randomTileNumber].tileType == "special");
        }
        
        // Check null?
        if (GameObject.Find($"Tile {randomTileNumber}").GetComponent<Tile>() == null)
        {
            do 
            {
                randomTileNumber = UnityEngine.Random.Range(0, board.tilesInBoard.Count);
            } 
            while (GameObject.Find($"Tile {randomTileNumber}").GetComponent<Tile>() == null);
        }
        */

        Tile tile = GameObject.Find("Answer tiles spawner").transform.GetChild(randomTileNumber).GetComponent<Tile>();

        int randomTileEffectID = UnityEngine.Random.Range(0, ImportData.tileEffectDictionary.Count);
        int randomTileEffectTurns = UnityEngine.Random.Range(0, 99);
        tile.GetTileEffect(this, randomTileEffectID, randomTileEffectTurns);

        Debug.Log($"{Time.time} Mob.Attack (override Entities.Attack) (end)");
    }
    #endregion
}

