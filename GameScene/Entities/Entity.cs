using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlueAndWhite.Objects;
using BlueAndWhite.Abilities;

namespace BlueAndWhite.Entities
{
    public abstract class Entity: Effectable
    {
        #region Entity data
        [SerializeField] private int _level;
        public int level { get { return _level; } set { _level = value; } }
        public int SetLevel(int inputLevel)
        {
            level = inputLevel;
            return level;
        }


        //[SerializeField] private EntityStat _maxHp;
        public EntityStat maxHp; //{ get { return _maxHp; } set { _maxHp = value; } }

        //[SerializeField] private EntityStat _currentHp;
        public EntityStat currentHp; //{ get { return _currentHp; } set { _currentHp = value; } }

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

        [SerializeField] private string _type;
        public string type { get { return _type; } set { _type = value; } }

        [SerializeField] private string _faction;
        public string faction { get { return _faction; } set { _faction = value; } }

        private bool _isDead = false;
        public bool isDead { get { return _isDead;} set { _isDead = value; } }
        #endregion

        #region Flow
        void Awake()
        {
            Debug.Log($"{name} Entities.Awake (start)");
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
            Debug.Log("Entities.Attack (start)");

            target.currentHp.value -= (value - target.defencePoint.value);

            Debug.Log("Entities.Attack (end)");
        }
        
        public virtual void TakeDamage(float damagePoint)
        {
            Debug.Log("Entities.TakeDamage (start)");

            currentHp.value -= damagePoint;

            Debug.Log("Entities.TakeDamage (end)");
        }

        public virtual void Heal(float hpHealPoint)
        {
            Debug.Log("Entities.Heal (start)");

            currentHp.value += hpHealPoint;

            Debug.Log("Entities.Heal (end)");
        }
        #endregion

        #region Effect functions
        #endregion
    }
}
