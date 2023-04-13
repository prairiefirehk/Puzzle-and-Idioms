using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity: Effectable
{
    #region Scripts
    public Board board;
    public RoundData roundData;

    #endregion
    #region Entity data
    [SerializeField] private int _level;
    public int level { get { return _level; } set { _level = value; } }

    [SerializeField] private EntityStat _maxHp;
    public EntityStat maxHp { get { return _maxHp; } set { _maxHp = value; } }

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

    [SerializeField] private EntityStat _evasionPoint;
    public EntityStat evasionPoint { get { return _evasionPoint;} set { _evasionPoint = value; } }

    [SerializeField] private EntityStat _criticalPoint;
    public EntityStat criticalPoint { get { return _criticalPoint;} set { _criticalPoint = value; } }

    [SerializeField] private EntityStat _dexterityPoint;
    public EntityStat dexterityPoint { get { return _dexterityPoint;} set { _dexterityPoint = value; } }

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

            if (currentHp.value > maxHp.value && (currentHp.value != maxHp.value))
            {
                currentHp.value = maxHp.value;
            }
        }
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
    public void Burning(int effectTurns, int effectLevel, float effectPercentage)
    {
        // OnInflict
        float damagePoint = effectPercentage * attackPoint.value * effectLevel;
        int remainingTurns = effectTurns;

        //StatusEffect burningEffect = new StatusEffect(EffectType.Burning, effectTurns, effectLevel, effectValue);
        //StatModifier burningStatModifier = new StatModifier(-10, StatModifierType.Flat, this);
        //target.attackPoint.AddModifier(burningStatModifier);

        // OnTurnStart
        //if (target.currentEffects[key: EffectType.Burning].effectTurns > 0)
        //if (effectTurns)
        //{
        //    target.currentHp.value -= 10f;
        //}

        // OnTurnEnd

        // OnRemoved
        /*
        if (target.currentEffects[EffectType.Burning].effectTurns == 0)
        {
            target.attackPoint.RemoveModifier(burningStatModifier);
        }
        */
    }

    public virtual void Attack(Entity target, float value)
    {
        Debug.Log($"Entities.Attack (start)");

        target.currentHp.value -= (value - target.defencePoint.value);

        Debug.Log($"Entities.Attack (end)");
    }
    
    public virtual void TakeDamage(float damagePoint)
    {
        Debug.Log($"Entities.TakeDamage (start)");

        float finalDamage = damagePoint;
        //currentHp.SetStatValue(newHp);


        currentHp.value -= finalDamage;

        Debug.Log($"Entities.TakeDamage (end)");
    }

    public virtual void Heal(float hpHealPoint)
    {
        Debug.Log($"Entities.Heal (start)");
        //float newHp = currentHp.GetStatValue() + hpHealPoint;
        float newHp = currentHp.value + hpHealPoint;

        //currentHp.SetStatValue(newHp);
        currentHp.value += hpHealPoint;

        Debug.Log($"Entities.Heal (end)");
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

