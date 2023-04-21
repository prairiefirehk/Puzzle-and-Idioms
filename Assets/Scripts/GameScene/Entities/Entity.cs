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

    public object this[string propertyName]
    {
        get
        {
            var propertyInfo = this.GetType().GetProperty(propertyName);

            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(this);
            }

            var fieldInfo = this.GetType().GetField(propertyName);

            if (fieldInfo != null)
            {
                return fieldInfo.GetValue(this);
            }

            Debug.LogError($"Property '{propertyName}' not found.");
            throw new ArgumentException();
        }
        set
        {
            var propertyInfo = this.GetType().GetProperty(propertyName);

            if (propertyInfo != null)
            {
                propertyInfo.SetValue(this, value);
                return;
            }

            var fieldInfo = this.GetType().GetField(propertyName);

            if (fieldInfo != null)
            {
                fieldInfo.SetValue(this, value);
                return;
            }

            Debug.LogError($"Property '{propertyName}' not found.");
            throw new ArgumentException();
        }
    }

    #endregion


    #region Flow
    void Awake()
    {
        Debug.Log($"{name} Entities.Awake (start)");

        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();
        board = GameObject.Find("Board").GetComponent<Board>();
        player = GameObject.Find("Round Manager").GetComponent<Player>();

        Debug.Log($"{name} Entities.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{name} Entities.OnEnable (start)");
        Debug.Log($"{name} Entities.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{name} Entities.Start (start)");
        Debug.Log($"{name} Entities.Start (end)");
    }

    void Update()
    {

    }
    
    void OnDisable()
    {
        Debug.Log($"{name} Entities.OnDisable (start)");
        Debug.Log($"{name} Entities.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{name} Entities.OnDestroy (start)");
        Debug.Log($"{name} Entities.OnDestroy (end)");
    }
    #endregion

    #region Entity functions
    public virtual void Attack(Entity target, float value)
    {
        Debug.Log($"Entities.Attack (start)");

        target.TakeDamage(value);
        //this.Wait(3f, () => {Debug.Log($"Wait... (Attack)");});

        Debug.Log($"Entities.Attack (end)");
    }
    
    public virtual void TakeDamage(float damagePoint)
    {
        Debug.Log($"Entities.TakeDamage (start)");

        currentHp.value -= damagePoint;
        //this.Wait(3f, () => {Debug.Log($"Wait... (TakeDamage)");});

        Debug.Log($"Entities.TakeDamage (end)");
    }

    public virtual void Heal(float hpHealPoint)
    {
        Debug.Log($"Entities.Heal (start)");

        currentHp.value += hpHealPoint;
        //this.Wait(3f, () => {Debug.Log($"Wait... (Heal)");});

        Debug.Log($"Entities.Heal (end)");
    }

    public virtual void CheckAlive()
    {
        Debug.Log($"Entity.CheckAlive (start)");

        if (currentHp.value <= 0)
        {
            currentState = EntityState.State.Dead;
        }
        
        Debug.Log($"Entity.CheckAlive (end)");
    }

    public virtual void BeforeMoveStart()
    {
        Debug.Log($"Entities.BeforeMoveStart (start)");
        Debug.Log($"Entities.BeforeMoveStart (end)");
    }

    public virtual void MoveStart()
    {
        Debug.Log($"Entities.MoveStart (start)");
        Debug.Log($"Entities.MoveStart (end)");
    }

    public virtual void CheckAction()
    {
        Debug.Log($"Entities.CheckAction (start)");
        Debug.Log($"Entities.CheckAction (end)");
    }

    public virtual void OnAction()
    {
        Debug.Log($"Entities.OnAction (start)");
        Debug.Log($"Entities.OnAction (end)");
    }

    public virtual void BeforeMoveEnd()
    {
        Debug.Log($"Entities.BeforeMoveEnd (start)");
        Debug.Log($"Entities.BeforeMoveEnd (end)");
    }

    public virtual void MoveEnd()
    {
        Debug.Log($"Entities.MoveEnd (start)");
        Debug.Log($"Entities.MoveEnd (end)");
    }


    #endregion

    #region Effect functions
    #endregion
}

