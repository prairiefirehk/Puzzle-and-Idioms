using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;

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

    private int _currentWave = 1;
    public int currentWave { get { return _currentWave; } set { _currentWave = value; } }

    private int _currentTurn = 0;
    public int currentTurn { get { return _currentTurn; } set { _currentTurn = value; } }

    private TurnState.State _currentTurnState = TurnState.State.BeforeTurnStart;
    public TurnState.State currentTurnState { get { return _currentTurnState; } set { _currentTurnState = value; } }

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

    [SerializeField] private List<int> _waveMobs;
    public List<int> waveMobs { get { return _waveMobs; } set { _waveMobs = value; } }
    //[SerializeField] private List<Teammate> _roundTeammates;
    //public List<Teammate> roundTeammates { get { return _roundTeammates; } set { _roundTeammates = value; } }
    //private int _roundTeammateNumber = 5;
    //public int roundTeammateNumber { get { return _roundTeammateNumber; } set { _roundTeammateNumber = value; } }
    private int _wavesNumber = 3;
    public int wavesNumber { get { return _wavesNumber; } set { _wavesNumber = value; } }

    private int _roundIdiomSize = 4;
    public int roundIdiomSize { get { return _roundIdiomSize; } set { _roundIdiomSize = value; } }

    private int _powerScore = 0;
    public int powerScore { get { return _powerScore; } set { _powerScore = value; } }

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
    public TMP_Text powerScoreText;

    // Temp
    public TMP_Text currentIdiomIDText;
    public TMP_Text currentTurnStateText;
    public TMP_Text currentMobIDText;
    public TMP_Text currentTurnText;

    // Factory
    #region Factories
    public MobFactory mobFactory;
    public TeammateFactory teammateFactory;
    public EffectBoxFactory effectBoxFactory;
    #endregion

    // Get components from mob info area
    public TMP_Text currentWaveText;
    public TMP_Text mobLevelText;
    public TMP_Text mobNameText;

    // Get components from arena
    public TMP_Text currentComboText;
    public Image currenMobPic;
    public GameObject mobEffectBoxes;
    public GameObject playerEffectBoxes;

    // Get components from player info area
    public Bar mobHealthBars;
    public Bar healthBars;

    // Booleans
    private bool _requestNewWave = false;
    public bool requestNewWave { get { return _requestNewWave;} set { _requestNewWave = value; } }

    // Events
    //public static event Action OnNewWaveEvent;
    //public static event Action<GameState.State> OnAllMobDefectedEvent;

    void Awake()
    {
        Debug.Log($"{Time.time} RoundData.Awake (start)");

        Debug.Log($"{Time.time} answers counts in board: {tilesWord.Count}");
        Debug.Log($"{Time.time} answerIdiomsID counts in board: {tilesWordIdiomsID.Count}");

        board = GameObject.Find("Board").GetComponent<Board>();
        player = gameObject.GetComponent<Player>();
        roundManager = gameObject.GetComponent<RoundManager>();
        mobFactory = gameObject.GetComponent<MobFactory>();
        teammateFactory = gameObject.GetComponent<TeammateFactory>();
        effectBoxFactory = gameObject.GetComponent<EffectBoxFactory>();

        Debug.Log($"{Time.time} RoundData.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} RoundData.OnEnable (start)");
        Debug.Log($"{Time.time} RoundData.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} RoundData.Start (start)");

        BeforeTurnStart();

        Debug.Log($"{Time.time} RoundData.Start (end)");
    }

    void Update()
    {
        //Debug.Log($"{Time.time} RoundData.Update (start)");

        StartTimer();
        healthBars.ResizeBarValue(player.currentHp.value, player.currentMaxHp.value, 0);
        //Debug.Log($"player's HP = {player.currentHp.value}");
        mobHealthBars.ResizeBarValue(currentMob.currentHp.value, currentMob.currentMaxHp.value, 0);
        //Debug.Log($"mob's HP = {currentMob.currentHp.value}");

        currentTurnStateText.text = currentTurnState.ToString();
        currentMobIDText.text = currentMob.id.ToString();
        currentTurnText.text = currentTurn.ToString();

        mobCDText.text = currentMob.currentAttackInterval.value.ToString();
        
        if (powerScore < 0)
        {
            powerScore = 0;
        }
        powerScoreText.text = powerScore.ToString();

        //Debug.Log($"{Time.time} RoundData.Update (end)");
    }
    
    void OnDisable()
    {
        Debug.Log($"{Time.time} RoundData.OnDisable (start)");

        // Unsubscribe to the game events
        //Mob.OnDefeatedEvent -= OnNewWave;
        //Board.OnEndTurnEvent -= OnNewTurn;

        Debug.Log($"{Time.time} RoundData.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} RoundData.OnDestroy (start)");
        Debug.Log($"{Time.time} RoundData.OnDestroy (end)");
    }

    // Should handle all display round data shit
    
    public void SpawnMobs()
    {
        Debug.Log($"{Time.time} RoundData.SpawnMobs (start)");

        currentMob = mobFactory.CreateMob(waveMobs[currentWave - 1]);
        mobHealthBars.InitializeBar(mobHealthBars, currentMob.currentHp.value, currentMob.currentMaxHp.value);
        mobLevelText.text = currentMob.level.ToString();
        mobNameText.text = currentMob.mobName;
        currenMobPic = currentMob.mobPicture;
        mobCDText.text = currentMob.currentAttackInterval.value.ToString();

        Debug.Log($"{Time.time} {currentMob.name} just spawn!");

        Debug.Log($"{Time.time} RoundData.SpawnMobs (end)");
    }

    // Should change to drawing mob from specific mob list data, but now just draw mob first than spawn accordingly
    public void DrawMobs()
    {
        Debug.Log($"{Time.time} RoundData.DrawMobs (start)");

        for (int i = 0; i < wavesNumber; i++)
        {
            int randomMobID = UnityEngine.Random.Range(0, ImportData.mobs.mob.Length);
            waveMobs.Add(randomMobID);
        }

        Debug.Log($"{Time.time} RoundData.DrawMobs (end)");
    }

    public Teammate SpawnTeammates(int teammateID, int teammateOrder)
    {
        Debug.Log($"{Time.time} RoundData.SpawnTeammates (start)");

        Teammate teammate = teammateFactory.CreateTeammate(teammateID, teammateOrder);
        /*
        for(int i = 0; i < roundTeammateNumber; i++)
        {
            // Temp
            Teammate teammate = teammateFactory.CreateTeammate(i, i);
            roundTeammates.Add(teammate);
        }
        */

        Debug.Log($"{Time.time} RoundData.SpawnTeammates (end)");
        return teammate;
    }

    public void InitializeData()
    {
        Debug.Log($"{Time.time} RoundData.InitializeData (start)");

        // Set the original data, however mob data had been initialized?
        //currentMob.currentHp = currentMob.maxHp;
        player.currentHp.SetStatValue(player.currentMaxHp.GetStatValue());
        currentWaveText.text = currentWave.ToString() + "/" + wavesNumber;
        //mobHealthBars.InitializeBar(mobHealthBars, currentMob.currentHp, currentMob.maxHp);
        healthBars.InitializeBar(healthBars, player.currentHp.value, player.currentMaxHp.value);

        Debug.Log($"{Time.time} RoundData.InitializeData (end)");
    }

    public void StartTimer()
    {
        //Debug.Log($"{Time.time} RoundData.StartTimer (start)");

        timeUsed = timeUsed + Time.deltaTime;

        //Debug.Log($"{Time.time} RoundData.StartTimer (end)");
    }
    
    public void StoreGainedReward()
    {
        Debug.Log($"{Time.time} RoundData.StoreGainedReward (start)");

        expGained += currentMob.expReward;
        coinGained += currentMob.coinReward;
        jadeGained += currentMob.jadeReward;

        Debug.Log($"{Time.time} RoundData.StoreGainedReward (end)");
    }

    public Entity CompareDexPoint(Entity player, Entity mob)
    {
        Debug.Log($"{Time.time} RoundData.CompareDexPoint (start)");

        Entity entityMoveFirst;
        Debug.Log($"{Time.time} player dex = {player.dexterityPoint.value}, mob dex = {currentMob.dexterityPoint.value}");
        if (player.dexterityPoint.value > mob.dexterityPoint.value)
        {
            entityMoveFirst = player;
        }
        else 
        {
            entityMoveFirst = mob;
        }

        Debug.Log($"{Time.time} RoundData.CompareDexPoint, return entityMoveFirst(local var): {entityMoveFirst} (end)");
        return entityMoveFirst;
    }

    public void BeforeTurnStart()
    {
        Debug.Log($"{Time.time} RoundData.BeforeTurnStart (start)");

        currentTurnState = TurnState.State.BeforeTurnStart;

        // First time while enter the game loop, a.k.a init
        if (roundManager.currentGameState == GameState.State.IsInitalizing)
        {
            roundManager.currentGameState = GameState.State.IsBattling;
            board.SpawnTiles(board.CheckBlankCell());
            board.DrawAnswer();
            //SpawnTeammates();
            player.InitPlayer();
            DrawMobs();
            SpawnMobs();
        }

        if (requestNewWave)
        {
            NewWave();
            requestNewWave = false;
        }

        currentTurn += 1;
        currentTurnText.text = currentTurn.ToString();

        // Check status effects and do the damage/other shit to the status effect owner
        player.CheckStatusEffects();
        if (player.currentStatusEffects.Count > 0)
        {
            for (int i = (player.currentStatusEffects.Count - 1); i >= 0; i--)
            {
                player.currentStatusEffects[i].OnTurnStart();
            }
        }

        currentMob.CheckStatusEffects();
        if (currentMob.currentStatusEffects.Count > 0)
        {
            for (int i = (currentMob.currentStatusEffects.Count - 1); i >= 0; i--)
            {
                currentMob.currentStatusEffects[i].OnTurnStart();
            }
        }

        this.Wait(0.5f, CompareDexPoint(player, currentMob).BeforeMoveStart);

        Debug.Log($"{Time.time} RoundData.BeforeTurnStart (end)");
    }

    public void TurnEnd()
    {
        Debug.Log($"{Time.time} RoundData.TurnEnd (start)");

        currentTurnState = TurnState.State.TurnEnd;

        // Reset shit
        player.moveEnded = false;
        currentMob.moveEnded = false;

        if (player.currentStatusEffects.Count > 0)
        {
            for (int i = (player.currentStatusEffects.Count - 1); i >= 0; i--)
            {
                Debug.Log($"{Time.time} player's status effect no.{i}: {player.currentStatusEffects[i].effectName}");
                player.currentStatusEffects[i].OnTurnEnd();
            }
        }

        if (currentMob.currentStatusEffects.Count > 0)
        {
            for (int i = (currentMob.currentStatusEffects.Count - 1); i >= 0; i--)
            {
                Debug.Log($"{Time.time} mob's status effect no.{i}: {currentMob.currentStatusEffects[i].effectName}");
                currentMob.currentStatusEffects[i].OnTurnEnd();
            }
        }

        Debug.Log($"{Time.time} ^6.6A End turn");
        board.DisplayTileCell(); // For debug use
        board.RenameTiles();
        board.CheckAnswerTile();
        board.DisplayTileCell(); // Double check, just in case

        // Teammate do their turn end shit
        foreach (KeyValuePair<TeammateType, Teammate> teammate in player.teammates)
        {
            teammate.Value.currentActiveSkillCD.value -= 1;
        }

        // Tile do their turn end shit
        for (int i = 0; i < board.answerTilesSpawner.transform.childCount; i++)
        {
            Tile tile = board.answerTilesSpawner.transform.GetChild(i).GetComponent<Tile>();
            tile.OnTurnEnd();
        }

        // Check gameover
        // Mob defeated
        if (currentMob.currentState == EntityState.State.Dead)
        {
            //Debug.Log($"currentWave is {currentWave}");
            // No more new wave of mobs
            if (currentWave == wavesNumber)
            {
                roundManager.currentGameState = GameState.State.PlayerWin;
                roundManager.GameOver(roundManager.currentGameState);
            }

            // Mob defeated, but still new turn
            else
            {
                Destroy(currentMob.gameObject);
                currentMob.transform.SetParent(null);

                // New Wave flag for new turn
                requestNewWave = true;

                this.Wait(0.5f, BeforeTurnStart);
            }
        }

        // Player defeated
        else if (player.currentState == EntityState.State.Dead) 
        {
            roundManager.currentGameState = GameState.State.PlayerLose;
            roundManager.GameOver(roundManager.currentGameState);
        }

        // New turn
        else
        {
            currentMob.currentAttackInterval.value -= 1;
            mobCDText.text = currentMob.currentAttackInterval.value.ToString();
            
            this.Wait(0.5f, BeforeTurnStart);
        }

        Debug.Log($"{Time.time} RoundData.TurnEnd (end)");
    }

    public void NewWave()
    {
        Debug.Log($"{Time.time} RoundData.NewWave (start)");

        StoreGainedReward();

        // Reset all status effect from preveus mob
        for (int i = mobEffectBoxes.transform.childCount - 1; i >= 0; i--)
        {
            mobEffectBoxes.transform.GetChild(i).GetComponent<EffectBox>().DestroyEffectBox();
        }

        currentWave += 1;
        currentWaveText.text = currentWave.ToString() + "/" + wavesNumber;
        SpawnMobs();

        Debug.Log($"{Time.time} RoundData.NewWave (end)");
    }
}
