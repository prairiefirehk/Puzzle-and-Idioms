using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlueAndWhite.Objects;
using BlueAndWhite.Entities;
using BlueAndWhite.DataManaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;

namespace BlueAndWhite.RoundManaging
{
    public class RoundData : MonoBehaviour
    {
        private Board board;
        private Player _player;
        public Player player { get { return _player; } set { _player = value; } }
        public RoundManager roundManager;

        //// Round data ////
        // Current game round location
        private string _currentLocation;
        public string currentLocation { get { return _currentLocation; } set { _currentLocation = value; } }

        private int _currentRound = 1;
        public int currentRound { get { return _currentRound; } set { _currentRound = value; } }

        private int _currentTurn = 1;
        public int currentTurn { get { return _currentTurn; } set { _currentTurn = value; } }

        // Won't show in UI before gg
        private float _timeUsed = 0;
        public float timeUsed { get { return _timeUsed; } set { _timeUsed = value; } }

        // Won't show in UI before gg
        private int _coinGained = 0;
        public int coinGained { get { return _coinGained; } set { _coinGained = value; } }

        // Won't show in UI before gg
        private int _expGained = 0;
        public int expGained { get { return _expGained; } set { _expGained = value; } }

        // Won't show in UI before gg
        private int _jadeGained = 0;
        public int jadeGained { get { return _jadeGained; } set { _jadeGained = value; } }

        // For reference
        [SerializeField] private Mob _currentMob;
        public Mob currentMob { get { return _currentMob; } set { _currentMob = value; } }

        [SerializeField] private Idiom _currentIdiom;
        public Idiom currentIdiom { get { return _currentIdiom; } set { _currentIdiom = value; } }
        [SerializeField] private int _currentIdiomID;
        public int currentIdiomID { get { return _currentIdiomID; } set { _currentIdiomID = value; } }

        //private int _maxAnswerTiles = 25;
        //public int maxAnswerTiles { get { return _maxAnswerTiles; } set { _maxAnswerTiles = value; } }

        private string _currentAnswerWord;
        public string currentAnswerWord { get { return _currentAnswerWord; } set { _currentAnswerWord = value; } }

        private int _currentAnswerWordOrder;
        public int currentAnswerWordOrder { get { return _currentAnswerWordOrder; } set { _currentAnswerWordOrder = value; } }

        private int _currentAnswerTilePosition;
        public int currentAnswerTilePosition { get { return _currentAnswerTilePosition; } set { _currentAnswerTilePosition = value; } }

        private int _remainingMobsNumber;
        public int remainingMobsNumber { get { return _remainingMobsNumber; } set { _remainingMobsNumber = value; } }

        [SerializeField] private List<int> _roundMobs;
        public List<int> roundMobs { get { return _roundMobs; } set { _roundMobs = value; } }
        [SerializeField] private List<Teammate> _roundTeammates;
        public List<Teammate> roundTeammates { get { return _roundTeammates; } set { _roundTeammates = value; } }
        private int _roundTeammateNumber = 5;
        public int roundTeammateNumber { get { return _roundTeammateNumber; } set { _roundTeammateNumber = value; } }
        private int _roundMobsNumber = 3;
        public int roundMobsNumber { get { return _roundMobsNumber; } set { _roundMobsNumber = value; } }

        private int _roundIdiomSize = 4;
        public int roundIdiomSize { get { return _roundIdiomSize; } set { _roundIdiomSize = value; } }

        // Parent of question tile
        public GameObject questionTiles;
        public GameObject answerTiles;

        // Chosen missing word tile
        public GameObject missingWordTile;

        // Storing tiles data in the board for the round
        public List<string> tilesWord;
        public List<int> tilesWordPosition;
        public List<int> tilesWordIdiomsID;

        // Storing idioms answer data for the **whole game**
        public List<string> askedAnswerWords;
        public List<int> askedAnswerPositions;
        public List<int> askedAnswerIdiomsIDs;

        // Load idiom word
        public TMP_Text firstWordText;
        public TMP_Text secondWordText;
        public TMP_Text thirdWordText;
        public TMP_Text fourthWordText;

        // Load other elements
        public TMP_Text mobCDText;
        public TMP_Text boardScoreText;

        // Temp
        public TMP_Text currentIdiomIDText;

        // Factory
        private MobFactory mobFactory;
        private TeammateFactory teammateFactory;

        // Get components from mob info area
        public TMP_Text mobLevelText;
        public TMP_Text mobNameText;

        // Get components from mob pic area
        public TMP_Text currentComboText;
        public Image currenMobPic;

        // Get components from player info area
        public Bar mobHealthBars;
        public Bar healthBars;

        // Get components(hidden until gameover) from the UI
        public TMP_Text currentRoundText;
        public TMP_Text currentTurnText;

        // Events
        public static event Action OnNewRoundEvent;
        public static event Action<string> OnAllMobDefectedEvent;

        void Awake()
        {
            Debug.Log($"RoundData.Awake (start)");

            Debug.Log("answers counts in board: " + tilesWord.Count);
            Debug.Log("answerIdiomsID counts in board: " + tilesWordIdiomsID.Count);

            board = GameObject.Find("Board").GetComponent<Board>();
            player = gameObject.GetComponent<Player>();
            mobFactory = gameObject.GetComponent<MobFactory>();
            roundManager = gameObject.GetComponent<RoundManager>();
            teammateFactory = gameObject.GetComponent<TeammateFactory>();
            //roundMobs = new List<int>();
            //roundTeammates = new List<Teammate>();

            remainingMobsNumber = roundMobsNumber;
            SpawnTeammates();
            DrawMobs();
            SpawnMobs();

            Debug.Log($"RoundData.Awake (end)");
        }

        void OnEnable()
        {
            Debug.Log($"RoundData.OnEnable (start)");

            // Subscribe to the game events and listen
            Mob.OnDefeatedEvent += OnNewRound;
            Board.OnEndTurnEvent += OnNewTurn;

            Debug.Log($"RoundData.OnEnable (end)");
        }
        
        void Start()
        {
            Debug.Log($"RoundData.Start (start)");
            Debug.Log($"RoundData.Start (end)");
        }

        void Update()
        {
            
            StartTimer();
            healthBars.ResizeBarValue(player.currentHp.value, 0);
            //Debug.Log($"player's HP = {player.currentHp.value}");
            mobHealthBars.ResizeBarValue(currentMob.currentHp.value, 0);
            //Debug.Log($"mob's HP = {currentMob.currentHp.value}");

            boardScoreText.text = board.tileLevelSpawnScore.ToString();
            currentTurnText.text = currentTurn.ToString();

            mobCDText.text = currentMob.currentAttackInterval.value.ToString();
            /*
            currentMob.currentAttackInterval -= Time.deltaTime;
            if (currentMob.currentAttackInterval <= 0)
            {
                currentMob.Attack(player, currentMob.attackPoint);
                healthBox.ResizeValue(player.currentHp, 0);
                currentMob.currentAttackInterval = currentMob.maxAttackInterval + 1;
            }

            mobHealthBox.ResizeValue(currentMob.currentHp, 0);
            healthBox.ResizeValue(player.currentHp, 0);
            timeleftBox.ResizeValue(currentMob.currentAttackInterval, 2);

            mobHealthBars.Resize(currentMob.currentHp);
            healthBars.Resize(player.currentHp);
            timeleftBars.Resize(currentMob.currentAttackInterval);

            mobHealthBox.UpdateValue(0);
            healthBox.UpdateValue(0);
            timeleftBox.UpdateValue(2);
            */
        }
        
        void OnDisable()
        {
            Debug.Log($"RoundData.OnDisable (start)");

            // Unsubscribe to the game events
            Mob.OnDefeatedEvent -= OnNewRound;
            Board.OnEndTurnEvent -= OnNewTurn;

            Debug.Log($"RoundData.OnDisable (end)");
        }

        void OnDestroy() 
        {
            Debug.Log($"RoundData.OnDestroy (start)");
            Debug.Log($"RoundData.OnDestroy (end)");
        }

        // Should handle all display round data shit
        
        public void SpawnMobs()
        {
            Debug.Log($"RoundData.SpawnMobs (start)");

            currentMob = mobFactory.CreateMob(roundMobs[currentRound - 1]);
            mobHealthBars.InitializeBar(mobHealthBars, currentMob.currentHp.value, currentMob.maxHp.value);
            mobLevelText.text = currentMob.level.ToString();
            mobNameText.text = currentMob.mobName;
            currenMobPic = currentMob.mobPicture;
            mobCDText.text = currentMob.currentAttackInterval.ToString();

            Debug.Log($"{currentMob.name} just spawn!");

            Debug.Log($"RoundData.SpawnMobs (end)");
        }

        // Should change to drawing mob from specific mob list data, but now just draw mob first than spawn accordingly
        public void DrawMobs()
        {
            Debug.Log($"RoundData.DrawMobs (start)");

            for (int i = 0; i < roundMobsNumber; i++)
            {
                int randomMobID = UnityEngine.Random.Range(0, ImportData.mobs.mob.Length);
                roundMobs.Add(randomMobID);
            }

            Debug.Log($"RoundData.DrawMobs (end)");
        }

        public void SpawnTeammates()
        {
            Debug.Log($"RoundData.SpawnTeammates (start)");

            for(int i = 0; i < roundTeammateNumber; i++)
            {
                // Temp
                Teammate teammate = teammateFactory.CreateTeammate(i, i);
                roundTeammates.Add(teammate);
            }

            Debug.Log($"RoundData.SpawnTeammates (end)");
        }

        public void CurrentMobDefeated()
        {
            Debug.Log($"RoundData.CurrentMobDefeated (start)");

            Debug.Log($"remainingMobsNumber(before) = {remainingMobsNumber}");
            remainingMobsNumber -= 1;
            Debug.Log($"remainingMobsNumber(after) = {remainingMobsNumber}");
            //roundMobs.Remove(roundMobs[remainingMobsNumber]);

            StoreGainedReward();
            // Incert delay here    
            currentMob.DestroyMob(currentMob);
            
            if (remainingMobsNumber == 0)
            {
                // Trigger here to send msg to event subscribers that player win
                OnAllMobDefectedEvent?.Invoke("win");
            }

            Debug.Log($"RoundData.CurrentMobDefeated (end)");
        }

        public void InitializeData()
        {
            Debug.Log($"RoundData.InitializeData (start)");

            // Set the original data, however mob data had been initialized?
            //currentMob.currentHp = currentMob.maxHp;
            player.currentHp.SetStatValue(player.maxHp.GetStatValue());
            //currentMob.currentAttackInterval = currentMob.maxAttackInterval;
            currentRoundText.text = currentRound.ToString() + "/" + roundMobsNumber;
            //mobHealthBars.InitializeBar(mobHealthBars, currentMob.currentHp, currentMob.maxHp);
            healthBars.InitializeBar(healthBars, player.currentHp.value, player.maxHp.value);

            Debug.Log($"RoundData.InitializeData (end)");
        }

        public void StartTimer()
        {
            //Debug.Log($"RoundData.StartTimer (start)");

            timeUsed = timeUsed + Time.deltaTime;

            //Debug.Log($"RoundData.StartTimer (end)");
        }
        
        public void StoreGainedReward()
        {
            Debug.Log($"RoundData.StoreGainedReward (start)");

            expGained += currentMob.expReward;
            coinGained += currentMob.coinReward;
            jadeGained += currentMob.jadeReward;

            Debug.Log($"RoundData.StoreGainedReward (end)");
        }

        public void OnNewRound()
        {
            Debug.Log($"RoundData.OnNewRound (start)");

            CurrentMobDefeated();
            if (remainingMobsNumber > 0)
            {
                currentRound += 1;
                currentRoundText.text = currentRound.ToString() + "/" + roundMobsNumber;
                SpawnMobs();
            }

            // Trigger here to send msg to other object that new round
            OnNewRoundEvent?.Invoke();

            Debug.Log($"RoundData.OnNewRound (end)");
        }

        public void OnNewTurn()
        {
            Debug.Log($"RoundData.OnNewTurn (start)");

            currentTurn += 1;
            currentTurnText.text = currentTurn.ToString();

            Debug.Log($"RoundData.OnNewTurn (end)");
        }
    }
}
