using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity: Effectable
{
    #region Scripts
    public Board board;
    public RoundData roundData;
    public Player player;
    #endregion

    #region Entity data
    [SerializeField] private int _level;
    public int level { get { return _level; } set { _level = value; } }
    [SerializeField] private EntityStat _maxHp;
    public EntityStat maxHp { get { return _maxHp; } set { _maxHp = value; } }
    [SerializeField] private EntityStat _currentMaxHp;
    public EntityStat currentMaxHp { get { return _currentMaxHp; } set { _currentMaxHp = value; } }

    [SerializeField] private EntityStat _currentHp;
    public EntityStat currentHp { get { return _currentHp; } set { _currentHp = value; } }

    [SerializeField] private EntityStat _attackPoint;
    public EntityStat attackPoint { get { return _attackPoint; } set { _attackPoint = value; } }

    [SerializeField] private EntityStat _currentAttackPoint;
    public EntityStat currentAttackPoint { get { return _currentAttackPoint; } set { _currentAttackPoint = value; } }

    [SerializeField] private EntityStat _defencePoint;
    public EntityStat defencePoint { get { return _defencePoint; } set { _defencePoint = value; } }

    [SerializeField] private EntityStat _currentDefencePoint;
    public EntityStat currentDefencePoint { get { return _currentDefencePoint; } set { _currentDefencePoint = value; } }


    // Not implement yet
    [SerializeField] private EntityStat _evasionPoint;
    public EntityStat evasionPoint { get { return _evasionPoint;} set { _evasionPoint = value; } }

    [SerializeField] private EntityStat _currentEvasionPoint;
    public EntityStat currentEvasionPoint { get { return _currentEvasionPoint; } set { _currentEvasionPoint = value; } }

    [SerializeField] private EntityStat _criticalPoint;
    public EntityStat criticalPoint { get { return _criticalPoint;} set { _criticalPoint = value; } }

    [SerializeField] private EntityStat _currentCriticalPoint;
    public EntityStat currentCriticalPoint { get { return _currentCriticalPoint; } set { _currentCriticalPoint = value; } }

    [SerializeField] private EntityStat _dexterityPoint;
    public EntityStat dexterityPoint { get { return _dexterityPoint;} set { _dexterityPoint = value; } }

    [SerializeField] private EntityStat _currentDexterityPoint;
    public EntityStat currentDexterityPoint { get { return _currentDexterityPoint; } set { _currentDexterityPoint = value; } }


    // Still considering
    [SerializeField] private EntityStat _movementPoint;
    public EntityStat movementPoint { get { return _movementPoint;} set { _movementPoint = value; } }

    [SerializeField] private EntityState.State _currentState;
    public EntityState.State currentState { get { return _currentState;} set { _currentState = value; } }

    [SerializeField] private string _type;
    public string type { get { return _type; } set { _type = value; } }

    [SerializeField] private string _faction;
    public string faction { get { return _faction; } set { _faction = value; } }

    private bool _MoveEnded = false;
    public bool moveEnded { get { return _MoveEnded; } set { _MoveEnded = value; } }

    // *****************************************************************************************************************Replace with state!
    private bool _isDead = false;
    public bool isDead { get { return _isDead;} set { _isDead = value; } }
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{Time.time} {name} Entities.Awake (start)");

        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();
        board = GameObject.Find("Board").GetComponent<Board>();
        player = GameObject.Find("Round Manager").GetComponent<Player>();

        Debug.Log($"{Time.time} {name} Entities.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} {name} Entities.OnEnable (start)");
        Debug.Log($"{Time.time} {name} Entities.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} {name} Entities.Start (start)");
        Debug.Log($"{Time.time} {name} Entities.Start (end)");
    }

    void Update()
    {

    }
    
    void OnDisable()
    {
        Debug.Log($"{Time.time} {name} Entities.OnDisable (start)");
        Debug.Log($"{Time.time} {name} Entities.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} {name} Entities.OnDestroy (start)");
        Debug.Log($"{Time.time} {name} Entities.OnDestroy (end)");
    }
    #endregion

    #region Entity functions
    public virtual void Attack(Entity target, float value)
    {
        Debug.Log($"{Time.time} Entities.Attack (start)");

        target.TakeDamage(value);
        //this.Wait(3f, () => {Debug.Log($"Wait... (Attack)");});

        Debug.Log($"{Time.time} Entities.Attack (end)");
    }
    
    public virtual void TakeDamage(float damagePoint)
    {
        Debug.Log($"{Time.time} Entities.TakeDamage (start)");

        currentHp.value -= damagePoint;
        //this.Wait(3f, () => {Debug.Log($"Wait... (TakeDamage)");});

        Debug.Log($"{Time.time} Entities.TakeDamage (end)");
    }

    public virtual void Heal(float hpHealPoint)
    {
        Debug.Log($"{Time.time} Entities.Heal (start)");

        currentHp.value += hpHealPoint;
        //this.Wait(3f, () => {Debug.Log($"Wait... (Heal)");});

        Debug.Log($"{Time.time} Entities.Heal (end)");
    }

    public virtual void CheckAlive()
    {
        Debug.Log($"{Time.time} Entity.CheckAlive (start)");

        if (currentHp.value <= 0)
        {
            currentState = EntityState.State.Dead;
        }
        
        Debug.Log($"{Time.time} Entity.CheckAlive (end)");
    }

    public virtual void BeforeMoveStart()
    {
        Debug.Log($"{Time.time} Entities.BeforeMoveStart (start)");
        Debug.Log($"{Time.time} Entities.BeforeMoveStart (end)");
    }

    public virtual void MoveStart()
    {
        Debug.Log($"{Time.time} Entities.MoveStart (start)");
        Debug.Log($"{Time.time} Entities.MoveStart (end)");
    }

    public virtual void CheckAction()
    {
        Debug.Log($"{Time.time} Entities.CheckAction (start)");
        Debug.Log($"{Time.time} Entities.CheckAction (end)");
    }

    public virtual void OnAction()
    {
        Debug.Log($"{Time.time} Entities.OnAction (start)");
        Debug.Log($"{Time.time} Entities.OnAction (end)");
    }

    public virtual void BeforeMoveEnd()
    {
        Debug.Log($"{Time.time} Entities.BeforeMoveEnd (start)");
        Debug.Log($"{Time.time} Entities.BeforeMoveEnd (end)");
    }

    public virtual void MoveEnd()
    {
        Debug.Log($"{Time.time} Entities.MoveEnd (start)");
        Debug.Log($"{Time.time} Entities.MoveEnd (end)");
    }
    #endregion
}
