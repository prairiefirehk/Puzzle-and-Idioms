using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using BlueAndWhite.Objects;
using BlueAndWhite.DataManaging;
using BlueAndWhite.RoundManaging;
using BlueAndWhite.Abilities;

namespace BlueAndWhite.Entities
{
    //[System.Serializable]
    public class Mob: Entity
    {
        #region Scripts
        public Board board;
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
            //player = GameObject.Find("Round Manager").GetComponent<Player>();

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
            board = GameObject.Find("Board").GetComponent<Board>();
            player = GameObject.Find("Round Manager").GetComponent<Player>();


            Debug.Log($"{mobName} Mob.Start (end)");
        }
        
        
        void Update()
        {
            if (isDead == false)
            {
                //Debug.Log( $"{name}'s health: {currentHp}");
                if (currentHp.value <= 0f)
                {
                    // For visual
                    currentHp.value = 0f;

                    Debug.Log($"{mobName} just dead!");
                    // Trigger here to send msg to event subscribers that mob is defeated
                    OnDefeatedEvent?.Invoke();
                    isDead = true;
                    //break;
                }

                if (currentHp.value > maxHp.value && (currentHp.value != maxHp.value))
                {
                    currentHp.value = maxHp.value;
                }

                if (currentAttackInterval.value == 0)
                {
                    Attack(player, attackPoint.value);

                    // Temp skill
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

                    currentAttackInterval = maxAttackInterval;
                    //Board.OnNewTurnEvent?.Invoke();
                }
            }
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
        // Temp
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
}
