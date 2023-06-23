using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity: Affectable
{
    #region Scripts
    public Board board;
    public RoundData roundData;
    public Player player;
    public UIManage uiManage;
    public RoundManager roundManager;
    #endregion

    #region Entity data
    [SerializeField] private string _entityName;
    public string entityName { get { return _entityName;} set { _entityName = value; } }

    [SerializeField] private EntityState.State _currentState;
    public EntityState.State currentState { get { return _currentState;} set { _currentState = value; } }
    [SerializeField] private int _level;
    public int level { get { return _level; } set { _level = value; } }
    [SerializeField] private List<Ability> _ownedAbilities = new List<Ability>();
    public List<Ability> ownedAbilities { get { return ownedAbilities;} set { ownedAbilities = value; } }

    // Basic main stats
    // Base of some stats(raw number from data) will convert into EntityStat -> xxxPoint, then convert into actual value, eg 20pts of HP-> 2000 actual HP
    // HP
    [SerializeField] private float _baseHealthPoint;
    public float baseHealthPoint { get { return _baseHealthPoint; } set { _baseHealthPoint = value; } }
    [SerializeField] private EntityStat _maxHealthPoint;
    public EntityStat maxHealthPoint { get { return _maxHealthPoint; } set { _maxHealthPoint = value; } }
    [SerializeField] private EntityStat _currentMaxHealthPoint;
    public EntityStat currentMaxHealthPoint { get { return _currentMaxHealthPoint; } set { _currentMaxHealthPoint = value; } }
    [SerializeField] private float _currentMaxHealthValue;
    public float currentMaxHealthValue { get { return _currentMaxHealthValue; } set { _currentMaxHealthValue = value; } }
    [SerializeField] private float _currentHealthValue;
    public float currentHealthValue { get { return _currentHealthValue; } set { _currentHealthValue = value; } }
    // ATK
    [SerializeField] private float _baseAttackPoint;
    public float baseAttackPoint { get { return _baseAttackPoint; } set { _baseAttackPoint = value; } }
    [SerializeField] private EntityStat _attackPoint;
    public EntityStat attackPoint { get { return _attackPoint; } set { _attackPoint = value; } }
    [SerializeField] private EntityStat _currentAttackPoint;
    public EntityStat currentAttackPoint { get { return _currentAttackPoint; } set { _currentAttackPoint = value; } }
    [SerializeField] private float _currentAttackValue;
    public float currentAttackValue { get { return _currentAttackValue; } set { _currentAttackValue = value; } }
    // DEF
    [SerializeField] private float _baseDefencePoint;
    public float baseDefencePoint { get { return _baseDefencePoint; } set { _baseDefencePoint = value; } }
    [SerializeField] private EntityStat _defencePoint;
    public EntityStat defencePoint { get { return _defencePoint; } set { _defencePoint = value; } }
    [SerializeField] private EntityStat _currentDefencePoint;
    public EntityStat currentDefencePoint { get { return _currentDefencePoint; } set { _currentDefencePoint = value; } }
    [SerializeField] private float _currentDefenceValue;
    public float currentDefenceValue { get { return _currentDefenceValue; } set { _currentDefenceValue = value; } }
    
    // Assist stats
    // DEX
    [SerializeField] private float _baseDexterityPoint;
    public float baseDexterityPoint { get { return _baseDexterityPoint;} set { _baseDexterityPoint = value; } }
    [SerializeField] private EntityStat _dexterityPoint;
    public EntityStat dexterityPoint { get { return _dexterityPoint;} set { _dexterityPoint = value; } }
    [SerializeField] private EntityStat _currentDexterityPoint;
    public EntityStat currentDexterityPoint { get { return _currentDexterityPoint; } set { _currentDexterityPoint = value; } }
    // PCT
    [SerializeField] private float _basePerceptionPoint;
    public float basePerceptionPoint { get { return _basePerceptionPoint;} set { _basePerceptionPoint = value; } }
    [SerializeField] private EntityStat _perceptionPoint;
    public EntityStat perceptionPoint { get { return _perceptionPoint; } set { _perceptionPoint = value; } }
    [SerializeField] private EntityStat _currentPerceptionPoint;
    public EntityStat currentPerceptionPoint { get { return _currentPerceptionPoint; } set { _currentPerceptionPoint = value; } }
    // CON
    [SerializeField] private float _baseConstitutionPoint;
    public float baseConstitutionPoint { get { return _baseConstitutionPoint;} set { _baseConstitutionPoint = value; } }
    [SerializeField] private EntityStat _constitutionPoint;
    public EntityStat constitutionPoint { get { return _constitutionPoint;} set { _constitutionPoint = value; } }
    [SerializeField] private EntityStat _currentConstitutionPoint;
    public EntityStat currentConstitutionPoint { get { return _currentConstitutionPoint; } set { _currentConstitutionPoint = value; } }
    
    // Sub-stats, no base stats since they are influenced/equal to related assist stats, so no need to state in data. Scale into % (every point = 0.2%)
    // ACC
    [SerializeField] private EntityStat _accuracyPoint;
    public EntityStat accuracyPoint { get { return _accuracyPoint;} set { _accuracyPoint = value; } }
    [SerializeField] private EntityStat _currentAccuracyPoint;
    public EntityStat currentAccuracyPoint { get { return _currentAccuracyPoint; } set { _currentAccuracyPoint = value; } }
    // CRT
    [SerializeField] private EntityStat _criticalPoint;
    public EntityStat criticalPoint { get { return _criticalPoint;} set { _criticalPoint = value; } }
    [SerializeField] private EntityStat _currentCriticalPoint;
    public EntityStat currentCriticalPoint { get { return _currentCriticalPoint; } set { _currentCriticalPoint = value; } }
    // EVS
    [SerializeField] private EntityStat _evasionPoint;
    public EntityStat evasionPoint { get { return _evasionPoint;} set { _evasionPoint = value; } }
    [SerializeField] private EntityStat _currentEvasionPoint;
    public EntityStat currentEvasionPoint { get { return _currentEvasionPoint; } set { _currentEvasionPoint = value; } }
    // RST
    [SerializeField] private EntityStat _resistancePoint;
    public EntityStat resistancePoint { get { return _resistancePoint;} set { _resistancePoint = value; } }
    [SerializeField] private EntityStat _currentResistancePoint;
    public EntityStat currentResistancePoint { get { return _currentResistancePoint; } set { _currentResistancePoint = value; } }
    // SPD, not scale into %
    [SerializeField] private EntityStat _speedPoint;
    public EntityStat speedPoint { get { return _speedPoint;} set { _speedPoint = value; } }
    [SerializeField] private EntityStat _currentSpeedPoint;
    public EntityStat currentSpeedPoint { get { return _currentSpeedPoint; } set { _currentSpeedPoint = value; } }
    [SerializeField] private float _currentSpeedValue;
    public float currentSpeedValue { get { return _currentSpeedValue; } set { _currentSpeedValue = value; } }

    // Still considering
    [SerializeField] private EntityStat _movementPoint;
    public EntityStat movementPoint { get { return _movementPoint;} set { _movementPoint = value; } }
    [SerializeField] private EntityStat _currentMovementPoint;
    public EntityStat currentMovementPoint { get { return _currentMovementPoint; } set { _currentMovementPoint = value; } }

    [SerializeField] private string _type;
    public string type { get { return _type; } set { _type = value; } }
    [SerializeField] private string _faction;
    public string faction { get { return _faction; } set { _faction = value; } }

    // Ability related
    [SerializeField] private Ability _activeAbility;
    public Ability activeAbility { get { return _activeAbility; } set { _activeAbility = value; } }
    [SerializeField] private int _activeAbilityID;
    public int activeAbilityID { get { return _activeAbilityID; } set { _activeAbilityID = value; } }
    [SerializeField] private int _activeAbilityLevel;
    public int activeAbilityLevel { get { return _activeAbilityLevel; } set { _activeAbilityLevel = value; } }

    [SerializeField] private int _maxActiveAbilityCD;
    public int maxActiveAbilityCD { get { return _maxActiveAbilityCD; } set { _maxActiveAbilityCD = value; } }
    [SerializeField] private EntityStat _currentMaxActiveAbilityCD;
    public EntityStat currentMaxActiveAbilityCD { get { return _currentMaxActiveAbilityCD; } set { _currentMaxActiveAbilityCD = value; } }
    [SerializeField] private int _currentActiveAbilityCD;
    public int currentActiveAbilityCD { get { return _currentActiveAbilityCD; } set { _currentActiveAbilityCD = value; } }

    [SerializeField] private int _maxActiveAbilityCost;
    public int maxActiveAbilityCost { get { return _maxActiveAbilityCost; } set { _maxActiveAbilityCost = value; } }
    [SerializeField] private EntityStat _currentActiveAbilityCost;
    public EntityStat currentActiveAbilityCost { get { return _currentActiveAbilityCost; } set { _currentActiveAbilityCost = value; } }

    //[SerializeField] private Ability _passiveAbility;
    //public Ability passiveAbility { get { return _passiveAbility; } set { _passiveAbility = value; } }
    //[SerializeField] private int _passiveAbilityID;
    //public int passiveAbilityID { get { return _passiveAbilityID; } set { _passiveAbilityID = value; } }
    //[SerializeField] private int _passiveAbilityLevel;
    //public int passiveAbilityLevel { get { return _passiveAbilityLevel; } set { _passiveAbilityLevel = value; } }

    //[SerializeField] private EntityStat _maxPassiveAbilityCD;
    //public EntityStat maxPassiveAbilityCD { get { return _maxPassiveAbilityCD; } set { _maxPassiveAbilityCD = value; } }
    //[SerializeField] private int _currentPassiveAbilityCD;
    //public int currentPassiveAbilityCD { get { return _currentPassiveAbilityCD; } set { _currentPassiveAbilityCD = value; } }

    //[SerializeField] private int _maxPassiveAbilityCost;
    //public int maxPassiveAbilityCost { get { return _maxPassiveAbilityCost; } set { _maxPassiveAbilityCost = value; } }
    //[SerializeField] private EntityStat _currentPassiveAbilityCost;
    //public EntityStat currentPassiveAbilityCost { get { return _currentPassiveAbilityCost; } set { _currentPassiveAbilityCost = value; } }



    // Booleans
    public bool isActioned = false;
    private bool _MoveEnded = false;
    public bool moveEnded { get { return _MoveEnded; } set { _MoveEnded = value; } }
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{Time.time} {entityName} Entities.Awake (start)");

        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();
        board = GameObject.Find("Board").GetComponent<Board>();
        player = GameObject.Find("Round Manager").GetComponent<Player>();
        uiManage = GameObject.Find("UI Manager").GetComponent<UIManage>();
        roundManager = GameObject.Find("Round Manager").GetComponent<RoundManager>();

        Debug.Log($"{Time.time} {entityName} Entities.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} {entityName} Entities.OnEnable (start)");
        Debug.Log($"{Time.time} {entityName} Entities.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} {entityName} Entities.Start (start)");
        Debug.Log($"{Time.time} {entityName} Entities.Start (end)");
    }

    void Update()
    {

    }
    
    void OnDisable()
    {
        Debug.Log($"{Time.time} {entityName} Entities.OnDisable (start)");
        Debug.Log($"{Time.time} {entityName} Entities.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} {entityName} Entities.OnDestroy (start)");
        Debug.Log($"{Time.time} {entityName} Entities.OnDestroy (end)");
    }
    #endregion

    #region Entity functions
    
    // Getting entity stats, convert points into actual value
    public virtual float GetMaxHealthPoint()
    {
        //Debug.Log($"{Time.time} {name} Entities.GetMaxHealthPoint (start)");
        //Debug.Log($"{Time.time} {name} Entities.GetMaxHealthPoint, return currentMaxHealthPoint.value: {currentMaxHealthPoint.value} (end)");
        return currentMaxHealthPoint.GetStatValue();
    }
    public virtual float GetAttackPoint()
    {
        //Debug.Log($"{Time.time} {name} Entities.GetAttackPoint (start)");
        //Debug.Log($"{Time.time} {name} Entities.GetAttackPoint, return currentAttackPoint.value: {currentAttackPoint.value} (end)");
        return currentAttackPoint.GetStatValue();
    }
    public virtual float GetDefencePoint()
    {
        //Debug.Log($"{Time.time} {name} Entities.GetDefencePoint (start)");
        //Debug.Log($"{Time.time} {name} Entities.GetDefencePoint, return currentDefencePoint.value: {currentDefencePoint.value} (end)");
        return currentDefencePoint.GetStatValue();
    }
    public virtual float GetDexterityPoint()
    {
        //Debug.Log($"{Time.time} {name} Entities.GetDexterityPoint (start)");
        //Debug.Log($"{Time.time} {name} Entities.GetDexterityPoint, return currentDexterityPoint.value: {currentDexterityPoint.value} (end)");
        return currentDexterityPoint.GetStatValue();
    }
    public virtual float GetPerceptionPoint()
    {
        //Debug.Log($"{Time.time} {name} Entities.GetPerceptionPoint (start)");
        //Debug.Log($"{Time.time} {name} Entities.GetPerceptionPoint, return currentPerceptionPoint.value: {currentPerceptionPoint.value} (end)");
        return currentPerceptionPoint.GetStatValue();
    }
    public virtual float GetConstitutionPoint()
    {
        //Debug.Log($"{Time.time} {name} Entities.GetConstitutionPoint (start)");
        //Debug.Log($"{Time.time} {name} Entities.GetConstitutionPoint, return currentConstitutionPoint.value: {currentConstitutionPoint.value} (end)");
        return currentConstitutionPoint.GetStatValue();
    }

    public virtual float GetSpeedPoint()
    {
        //Debug.Log($"{Time.time} {name} Entities.GetSpeedPoint (start)");
        //Debug.Log($"{Time.time} {name} Entities.GetSpeedPoint, return currentDexterityPoint.value: {currentDexterityPoint.value} (end)");
        return currentDexterityPoint.GetStatValue();
    }
    public virtual float GetEvasionPoint()
    {
        //Debug.Log($"{Time.time} {name} Entities.GetEvasionPoint (start)");
        //Debug.Log($"{Time.time} {name} Entities.GetEvasionPoint, return currentDexterityPoint.value: {currentDexterityPoint.value} (end)");
        return currentDexterityPoint.GetStatValue();
    }
    public virtual float GetCriticalPoint()
    {
        //Debug.Log($"{Time.time} {name} Entities.GetCriticalPoint (start)");
        //Debug.Log($"{Time.time} {name} Entities.GetCriticalPoint, return currentPerceptionPoint.value: {currentPerceptionPoint.value} (end)");
        return currentPerceptionPoint.GetStatValue();
    }
    public virtual float GetAccuracyPoint()
    {
        //Debug.Log($"{Time.time} {name} Entities.GetAccuracyPoint (start)");
        //Debug.Log($"{Time.time} {name} Entities.GetAccuracyPoint, return currentPerceptionPoint.value: {currentPerceptionPoint.value} (end)");
        return currentPerceptionPoint.GetStatValue();
    }
    public virtual float GetResistancePoint()
    {
        //Debug.Log($"{Time.time} {name} Entities.GetResistancePoint (start)");
        //Debug.Log($"{Time.time} {name} Entities.GetResistancePoint, return currentConstitutionPoint.value: {currentConstitutionPoint.value} (end)");
        return currentConstitutionPoint.GetStatValue();
    }

    public virtual float GetMaxHealthValue()
    {
        //Debug.Log($"{Time.time} {name} Entities.GetMaxHealthValue (start)");
        //Debug.Log($"{Time.time} {name} Entities.GetMaxHealthValue, return currentMaxHealthPoint.value * 100: {currentMaxHealthPoint.value * 100} (end)");
        return (float)Math.Round(currentMaxHealthPoint.GetStatValue() * 100, 0);
    }
    public virtual float GetAttackValue()
    {
        //Debug.Log($"{Time.time} {name} Entities.GetAttackValue (start)");
        //Debug.Log($"{Time.time} {name} Entities.GetAttackValue, return currentAttackPoint.value * 50: {currentAttackPoint.value * 50} (end)");
        return (float)Math.Round(currentAttackPoint.GetStatValue() * 50, 2);
    }
    public virtual float GetDefenseValue()
    {
        //Debug.Log($"{Time.time} {name} Entities.GetDefenseValue (start)");
        //Debug.Log($"{Time.time} {name} Entities.GetDefenseValue, return currentDefencePoint.value * 50: {currentDefencePoint.value * 50} (end)");
        return (float)Math.Round(currentDefencePoint.GetStatValue() * 50, 2);
    }
    public virtual float GetSpeedValue()
    {
        //Debug.Log($"{Time.time} {name} Entities.GetSpeedValue (start)");
        //Debug.Log($"{Time.time} {name} Entities.GetSpeedValue, return currentSpeedPoint.value * 5: {currentSpeedPoint.value * 5} (end)");
        return (float)Math.Round(currentSpeedPoint.GetStatValue() * 5, 2);
    }

    public virtual void UpdateEntityStats()
    {
        // Need to test what'll happen if there's stat modifier inside EntityStat
        currentSpeedPoint.SetStatValue(GetSpeedPoint());
        currentEvasionPoint.SetStatValue(GetEvasionPoint());
        currentCriticalPoint.SetStatValue(GetCriticalPoint());
        currentAccuracyPoint.SetStatValue(GetAccuracyPoint());
        currentResistancePoint.SetStatValue(GetResistancePoint());

        currentMaxHealthValue = GetMaxHealthValue();
        currentAttackValue = GetAttackValue();
        currentDefenceValue = GetDefenseValue();
        currentSpeedValue = GetSpeedValue();
    }

    public virtual void DisplayCurrentStatusEffects()
    {
        //Debug.Log($"$gogogogo");
        if (currentStatusEffects.Count > 0)
        Debug.Log($"${entityName}'s current status effects: ");
        {
            for (int i = 0; i < currentStatusEffects.Count; i++)
            {
                Debug.Log($"$No. {i}: {currentStatusEffects[i].effectName}");
                Debug.Log($"$No. {i}'s order from function:{currentStatusEffects[i].effectName} order: {currentStatusEffects[i].GetOrderOfStatusEffect()}");
            }
        }
    }

    public virtual void Attack(Entity target, float value)
    {
        Debug.Log($"{Time.time} Entities.Attack (start)");

        if (roundManager.currentGameState == GameState.State.IsBattling)
        {
            string popupMessage = $"{entityName}攻擊 !";
            uiManage.SpawnPopup(popupMessage, "normal");

            target.TakeDamage(value);
            //this.Wait(3f, () => {Debug.Log($"Wait... (Attack)");});
        }
        Debug.Log($"{Time.time} Entities.Attack (end)");
    }
    
    public virtual void TakeDamage(float damageValue)
    {
        Debug.Log($"{Time.time} Entities.TakeDamage (start)");

        currentHealthValue -= damageValue;
        //this.Wait(3f, () => {Debug.Log($"Wait... (TakeDamage)");});

        Debug.Log($"{Time.time} Entities.TakeDamage (end)");
    }

    public virtual void Heal(float hpHealValue)
    {
        Debug.Log($"{Time.time} Entities.Heal (start)");

        currentHealthValue += hpHealValue;
        //this.Wait(3f, () => {Debug.Log($"Wait... (Heal)");});

        Debug.Log($"{Time.time} Entities.Heal (end)");
    }

    public virtual void CheckAlive()
    {
        Debug.Log($"{Time.time} Entity.CheckAlive (start)");

        if (currentHealthValue <= 0)
        {
            currentState = EntityState.State.Dead;
        }
        
        Debug.Log($"{Time.time} Entity.CheckAlive (end)");
    }

    // Turn based functions
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
