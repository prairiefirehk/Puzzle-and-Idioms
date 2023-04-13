using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//[System.Serializable]
public class Mob: Entity
{
    #region Scripts
    public Player player;
    public MobData mobdata;
    #endregion

    #region Mob data
    [SerializeField] private int _id;
    public int id { get { return _id; } set { _id = value; } }

    [SerializeField] private string _mobName;
    public string mobName { get { return _mobName; } set { _mobName = value; } }

    [SerializeField] private string _picName;
    public string picName { get { return _picName; } set { _picName = value; } }

    [SerializeField] private EntityStat _maxAttackInterval;
    public EntityStat maxAttackInterval { get { return _maxAttackInterval; } set { _maxAttackInterval = value; } }

    [SerializeField] private EntityStat _currentAttackInterval;
    public EntityStat currentAttackInterval { get { return _currentAttackInterval; } set { _currentAttackInterval = value; } }

    [SerializeField] private int _expReward;
    public int expReward { get { return _expReward; } set { _expReward = value; } }

    [SerializeField] private int _coinReward;
    public int coinReward { get { return _coinReward; } set { _coinReward = value; } }

    [SerializeField] private int _jadeReward;
    public int jadeReward { get { return _jadeReward; } set { _jadeReward = value; } }

    [SerializeField] private Image _mobPicture;
    public Image mobPicture { get { return _mobPicture;} set { _mobPicture = value; } }
    #endregion

    #region Events
    // Add an event for mob defeated
    public static event Action OnDefeatedEvent;
    //public static event Action OnAttackIntervalZero;
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{mobName} Mob.Awake (start)");

        // Not ideal place
        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();
        board = GameObject.Find("Board").GetComponent<Board>();

        isDead = false;

        Debug.Log($"{mobName} Mob.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{mobName} Mob.OnEnable (start)");

        // Subscribe to the game events and listen
        Board.OnEndTurnEvent += OnNewTurn;

        Debug.Log($"{mobName} Mob.OnEnable (end)");
    }

    void Start()
    {
        Debug.Log($"{mobName} Mob.Start (start)");
        
        // Move here didn't cause error
        Debug.Log($"{mobName} Mob.Start (end)");
    }
    
    
    void Update()
    {

    }

    void OnDisable()
    {
        Debug.Log($"{mobName} Mob.OnDisable (start)");

        // Unsubscribe to the game events
        Board.OnEndTurnEvent -= OnNewTurn;

        Debug.Log($"{mobName} Mob.OnDisable (end)");
    }

    void OnDestroy()
    {
        Debug.Log($"{mobName} Mob.OnDestroy (start)");
        Debug.Log($"{mobName} Mob.OnDestroy (end)");
    }
    #endregion

    #region Mob functions
    public bool CheckAttackInterval()
    {
        bool canProceed = false;
        if (currentAttackInterval.value == 0)
        {
            canProceed = true;
        }
        
        return canProceed;
    }

    public override void BeforeMoveStart()
    {
        roundData.currentTurnState = TurnState.State.BeforeMobMoveStart;
        CheckStatusEffects();

        if (CheckAttackInterval() && (!isStun))
        {
            MoveStart();
        }
        else
        {
            MoveEnd();
        }
    }

    public override void MoveStart()
    {
        roundData.currentTurnState = TurnState.State.MobMoveStart;

        // Do some visual shit/popup/conversation



    }

    public override void CheckAction()
    {
        roundData.currentTurnState = TurnState.State.WaitingMobAction;

        // Do some visual shit/popup/conversation
    }

    public override void Action()
    {
        roundData.currentTurnState = TurnState.State.MobAction;

        // Do some visual shit/popup/conversation

        Attack(player, attackPoint.value);

        // Add extra one for later gerneral minus one in MoveEnd()
        currentAttackInterval.value = maxAttackInterval.value + 1;
        BeforeMoveEnd();
    }

    public override void BeforeMoveEnd()
    {
        roundData.currentTurnState = TurnState.State.BeforeMobMoveEnd;

        // Do some visual shit/popup/conversation

        MoveEnd();
    }

    public override void MoveEnd()
    {
        roundData.currentTurnState = TurnState.State.MobMoveEnd;
        board.DisableBoard();

        // Do some visual shit/popup/conversation


        currentAttackInterval.value -= 1;
        if (HasStatusEffect) 
        {
            for (int i = 0; i < currentStatusEffects.Count; i++)
            {
                //currentStatusEffects[i].remainingTurn -= 1;

            }
        }

        moveEnded = true;
        if (roundData.player.moveEnded)
        {
            roundData.TurnEnd();
        }
        else
        {
            roundData.player.BeforeMoveStart();
        }
    }

    // Temp
    public override void Attack(Entity target, float value)
    {
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

        int randomTileEffect = UnityEngine.Random.Range(0, Enum.GetNames(typeof(Tile.TileEffect)).Length);
        int randomTileEffectTurns = UnityEngine.Random.Range(0, 99);
        tile.GetTileEffect(tile, (Tile.TileEffect)randomTileEffect, randomTileEffectTurns);

    }
    public void MobDefeated()
    {
        Debug.Log($"{mobName} Mob.MobDefeated (start)");

        Debug.Log($"{mobName} just dead!");
        // Trigger here to send msg to event subscribers that mob is defeated
        OnDefeatedEvent?.Invoke();
        isDead = true;


        Debug.Log($"{mobName} Mob.MobDefeated (end)");
    }
    
    public void DestroyMob(Mob mob)
    {
        Debug.Log($"{mobName} Mob.DestroyMob (start)");

        Destroy(mob.gameObject);
        mob.transform.SetParent(null);

        Debug.Log($"{mobName} Mob.DestroyMob (end)");
    }

    public void OnNewTurn()
    {
        Debug.Log($"{mobName} Mob.OnNewTurn (start)");

        currentAttackInterval.value -= 1;

        Debug.Log($"{mobName} Mob.OnNewTurn (end)");
    }
    #endregion
}

