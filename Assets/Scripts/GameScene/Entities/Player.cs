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
    public bool isActioned = false;
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
        Debug.Log($"{Time.time} $player's maxHp = {maxHp.value}");
        Debug.Log($"{Time.time} $player's currentHp = {currentHp.value}");
        Debug.Log($"{Time.time} $player's currentMaxHp = {currentMaxHp.value}");

        if (currentState == EntityState.State.Alive)
        {
            //Debug.Log( $"{name}'s health: {currentHp}");
            if (currentHp.value <= 0)
            {
                // For visual
                currentHp.value = 0f;

                Debug.Log($"{Time.time} {name} just dead!");
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

        
        if (isActioned)
        {
            roundData.currentTurnState = TurnState.State.BeforePlayerMoveEnd;
            isActioned = false;
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

        maxHp = new EntityStat(GetPlayerMaxHp());
        currentMaxHp = new EntityStat(maxHp.value);
        currentHp = new EntityStat(currentMaxHp.value);
        attackPoint = new EntityStat(0f);
        defencePoint = new EntityStat(125f);
        dexterityPoint = new EntityStat(10f);

        currentAttackPoint = new EntityStat(0f);
        currentDefencePoint = new EntityStat(defencePoint.value);
        currentDexterityPoint = new EntityStat(dexterityPoint.value);

        currentState = EntityState.State.Alive;

        Debug.Log($"{Time.time} Player.InitPlayer (end)");
    }
    public override void BeforeMoveStart()
    {
        Debug.Log($"{Time.time} Player.BeforeMoveStart (override Entities.BeforeMoveStart) (start)");

        roundData.currentTurnState = TurnState.State.BeforePlayerMoveStart;
        CheckAlive();
        //CheckStatusEffects();

        if ((!isStun) && (currentState == EntityState.State.Alive) && (roundData.currentMob.currentState == EntityState.State.Alive))
        {
            this.Wait(0.5f, MoveStart);
        }
        else
        {
            this.Wait(0.5f,MoveEnd);
        }

        Debug.Log($"{Time.time} Player.BeforeMoveStart (override Entities.BeforeMoveStart) (end)");
    }

    public override void MoveStart()
    {
        Debug.Log($"{Time.time} Player.MoveStart (override Entities.MoveStart) (start)");

        roundData.currentTurnState = TurnState.State.PlayerMoveStart;

        // Do some visual shit/popup/conversation
        board.EnableBoard();

        Debug.Log($"{Time.time} Player.MoveStart (override Entities.MoveStart) (end)");
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
        Debug.Log($"{Time.time} Player.BeforeMoveEnd (override Entities.BeforeMoveEnd) (start)");

        roundData.currentTurnState = TurnState.State.BeforePlayerMoveEnd;

        // Do some visual shit/popup/conversation
        board.DisableBoard();

        this.Wait(0.5f, MoveEnd); //0.5

        Debug.Log($"{Time.time} Player.BeforeMoveEnd (override Entities.BeforeMoveEnd) (start)");
    }

    public override void MoveEnd()
    {
        Debug.Log($"{Time.time} Player.MoveEnd (override Entities.MoveEnd) (start)");

        roundData.currentTurnState = TurnState.State.PlayerMoveEnd;
        

        if (hasStatusEffect) 
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
            this.Wait(0.5f, roundData.TurnEnd);
        }
        else
        {
            this.Wait(0.5f, roundData.currentMob.BeforeMoveStart);
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
            playerMaxHp += teammate.Value.currentMaxHp.value;
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

        if (tile.isAnswer == true)
        {
            teammate.AnswerCorrectly(tile);
        }
        else
        {
            teammate.AnswerWrongly(tile);
        }

        Debug.Log($"{Time.time} Player.Answer (end)");
    }

    public override void Attack(Entity target, float value)
    {
        Debug.Log($"{Time.time} Player.Attack (override Entities.Attack) (start)");

        // Temp testing solution
        StatusEffect burn = new StatusEffect(StatusEffectName.Burning, 21, 1, 200, 100,
                                            target, "Player: Burn!", 
                                            new List<string>{"currentMaxHp"}, new List<StatModifier>{new StatModifier(-200, StatModifierType.Flat, 1)});

        StatusEffect freeze = new StatusEffect(StatusEffectName.Freezing, 21, 1, 100, 50,
                                            target, "Player: Freeze!", 
                                            new List<string>{"currentMaxAttackInterval"}, new List<float>{1});

        StatusEffect stun = new StatusEffect(StatusEffectName.Stuning, 5, 1, 100, 0,
                                            target, "Player: Stun!", 
                                            new List<string>{"currentMaxHp"}, new List<float>{1000});
        
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

        Debug.Log($"{Time.time} Player.Attack (override Entities.Attack) (end)");
    }
    #endregion
}

