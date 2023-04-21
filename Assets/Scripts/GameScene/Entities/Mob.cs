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

    [SerializeField] private EntityStat _currentMaxAttackInterval;
    public EntityStat currentMaxAttackInterval { get { return _currentMaxAttackInterval; } set { _currentMaxAttackInterval = value; } }

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
    //public static event Action OnDefeatedEvent;
    //public static event Action OnAttackIntervalZero;
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{mobName} Mob.Awake (start)");

        // Not ideal place
        player = GameObject.Find("Round Manager").GetComponent<Player>();
        board = GameObject.Find("Board").GetComponent<Board>();
        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();

        isDead = false;

        Debug.Log($"{mobName} Mob.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{mobName} Mob.OnEnable (start)");

        // Subscribe to the game events and listen
        //Board.OnEndTurnEvent += OnNewTurn;

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
        //Debug.Log($" currentMob {mobName} is {currentState}");

        if (currentState == EntityState.State.Alive)
        {
            //Debug.Log( $"{name}'s health: {currentHp}");
            if (currentHp.value <= 0)
            {
                // For visual
                currentHp.value = 0f;

                Debug.Log($"{name} just dead!");
                // Trigger here to send msg to event subscribers that mob is defeated
                currentState = EntityState.State.Dead;
                //OnDefeatedEvent?.Invoke();
                //isDead = true;
                //break;
            }

            if (currentHp.value > currentMaxHp.value && (currentHp.value != currentMaxHp.value))
            {
                currentHp.value = currentMaxHp.value;
            }
        }
    }

    void OnDisable()
    {
        Debug.Log($"{mobName} Mob.OnDisable (start)");

        // Unsubscribe to the game events
        //Board.OnEndTurnEvent -= OnNewTurn;

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
        Debug.Log($"Mob.CheckAttackInterval (start)");

        bool canProceed = false;
        if (currentAttackInterval.value <= 0)
        {
            currentAttackInterval.value = 0;
            canProceed = true;
        }
        
        Debug.Log($"Mob.CheckAttackInterval, return canProceed(local var): {canProceed} (end)");
        return canProceed;
    }

    public override void BeforeMoveStart()
    {
        Debug.Log($"Mob.BeforeMoveStart (override Entities.BeforeMoveStart) (start)");

        roundData.currentTurnState = TurnState.State.BeforeMobMoveStart;
        CheckAlive();
        //CheckStatusEffects();

        if (CheckAttackInterval() && (!isStun) && (currentState == EntityState.State.Alive) && (roundData.player.currentState == EntityState.State.Alive))
        {
            this.Wait(0f, MoveStart);
        }
        else
        {
            if (currentAttackInterval.value <= 0 || currentAttackInterval.value > currentMaxAttackInterval.value)
            {
                currentAttackInterval.value = currentMaxAttackInterval.value;
            }

            this.Wait(0f, MoveEnd);
        }

        Debug.Log($"Mob.BeforeMoveStart (override Entities.BeforeMoveStart) (end)");
    }

    public override void MoveStart()
    {
        Debug.Log($"Mob.MoveStart (override Entities.MoveStart) (start)");
        roundData.currentTurnState = TurnState.State.MobMoveStart;

        // Do some visual shit/popup/conversation

        this.Wait(0f, OnAction);

        Debug.Log($"Mob.MoveStart (override Entities.MoveStart) (end)");
    }

    public override void CheckAction()
    {
        roundData.currentTurnState = TurnState.State.WaitingMobAction;

        // Do some visual shit/popup/conversation
    }

    public override void OnAction()
    {
        Debug.Log($"Mob.OnAction (override Entities.OnAction) (start)");

        roundData.currentTurnState = TurnState.State.MobAction;

        // Do some visual shit/popup/conversation

        Attack(roundData.player, attackPoint.value);

        // Add extra one for later gerneral minus one in MoveEnd()
        currentAttackInterval.value = currentMaxAttackInterval.value + 1;
        this.Wait(0.5f, BeforeMoveEnd);

        Debug.Log($"Mob.OnAction (override Entities.OnAction) (end)");
    }

    public override void BeforeMoveEnd()
    {
        Debug.Log($"Mob.BeforeMoveEnd (override Entities.BeforeMoveEnd) (start)");

        roundData.currentTurnState = TurnState.State.BeforeMobMoveEnd;

        // Do some visual shit/popup/conversation

        this.Wait(0f, MoveEnd);

        Debug.Log($"Mob.BeforeMoveEnd (override Entities.BeforeMoveEnd) (end)");
    }

    public override void MoveEnd()
    {
        Debug.Log($"Mob.MoveEnd (override Entities.MoveEnd) (start)");

        roundData.currentTurnState = TurnState.State.MobMoveEnd;

        // Do some visual shit/popup/conversation


        currentAttackInterval.value -= 1;
        roundData.mobCDText.text = currentAttackInterval.value.ToString();

        moveEnded = true;
        if (roundData.player.moveEnded)
        {
            this.Wait(0f, roundData.TurnEnd);
        }
        else
        {
            this.Wait(0f, roundData.player.BeforeMoveStart);
        }

        Debug.Log($"Mob.MoveEnd (override Entities.MoveEnd) (end)");
    }

    // Temp
    public override void Attack(Entity target, float value)
    {
        Debug.Log($"Mob.Attack (override Entities.Attack) (start)");

        // Temp testing solution
        StatusEffect burn = new StatusEffect(StatusEffectName.Burning, 21, 1, 500, 100,
                                            target, "Mob: Burn!", 
                                            new List<string>{"currentMaxHp"}, new List<StatModifier>{new StatModifier(-200, StatModifierType.Flat, 1)});

        StatusEffect freeze = new StatusEffect(StatusEffectName.Freezing, 21, 1, 100, 50,
                                            target, "Mob: Freeze!", 
                                            new List<string>{"currentMaxHp"}, new List<float>{2000});

        StatusEffect stun = new StatusEffect(StatusEffectName.Stuning, 2, 1, 100, 0,
                                            target, "Mob: Stun!", 
                                            new List<string>{"currentMaxHp"}, new List<float>{5000});
        
        int randomAttackSkillNumber = UnityEngine.Random.Range(0, Enum.GetNames(typeof(StatusEffectName)).Length);
        //Debug.Log($"randomAttackSkillNumber = {randomAttackSkillNumber}");
        //Debug.Log($"Enum.GetNames(typeof(StatusEffectName)).Length = {Enum.GetNames(typeof(StatusEffectName)).Length}");
        switch(randomAttackSkillNumber)
        {
            case 0:
                burn.OnInflict();
                base.Attack(target, (value + burn.instantValue));
                break;

            case 1:
                freeze.OnInflict();
                base.Attack(target, (value + freeze.instantValue));
                break;

            case 2:
                stun.OnInflict();
                base.Attack(target, (value + stun.instantValue));
                break;
        }

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

        Debug.Log($"Mob.Attack (override Entities.Attack) (end)");
    }
    public void MobDefeated()
    {
        Debug.Log($"{mobName} Mob.MobDefeated (start)");

        Debug.Log($"{mobName} just dead!");
        // Trigger here to send msg to event subscribers that mob is defeated
        //OnDefeatedEvent?.Invoke();
        //isDead = true;


        Debug.Log($"{mobName} Mob.MobDefeated (end)");
    }
    
    public void DestroyMob(Mob mob)
    {
        Debug.Log($"{mobName} Mob.DestroyMob (start)");

        //Destroy(mob.gameObject);
        //mob.transform.SetParent(null);

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

