using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundData : MonoBehaviour
{
    #region Scripts
    private Board board;
    private Player _player;
    public Player player { get { return _player; } set { _player = value; } }
    public RoundManager roundManager;
    public UIManage uiManage;
    #endregion

    #region Game object references
    // Load idiom word
    public TMP_Text firstWordText;
    public TMP_Text secondWordText;
    public TMP_Text thirdWordText;
    public TMP_Text fourthWordText;
    
    // Common components from info area
    public TMP_Text currentTimeText;
    public GameObject generalInfo;
    public GameObject mobInfo;

    // Get components from general info
    public Bar progressBars;

    // Get components from mob info
    public TMP_Text currentWaveText;
    public TMP_Text mobLevelText;
    public TMP_Text mobNameText;
    public Bar mobHealthBars;

    // Common components from arena area
    public TMP_Text currentComboText;
    public GameObject mobEffectBoxes;
    public GameObject playerEffectBoxes;
    public TMP_Text powerScoreText;
    public GameObject generalArenaInfo;
    public GameObject mobArenaInfo;

    // Get components from arena general elements
    public Image currentProgressBox;
    public TMP_Text currentProgressText;

    // Get components from arena mob elements
    public Image mobCDBox;
    public TMP_Text mobCDText;
    public Image currenMobPic;

    // Get components from player info (board)
    public Bar healthBars;

    // Temp, for reference
    public TMP_Text currentIdiomIDText;
    public TMP_Text currentGameStateText;
    public TMP_Text currentTurnStateText;
    public TMP_Text currentMobIDText;
    public TMP_Text currentTurnText;
    #endregion

    #region Round data
    // Current game round location
    private string _homeLocation;
    public string homeLocation { get { return _homeLocation; } set { _homeLocation = value; } }
    private string _destinationLocation;
    public string destinationLocation { get { return _destinationLocation; } set { _destinationLocation = value; } }
    private int _roundDistance = 200;
    public int roundDistance { get { return _roundDistance; } set { _roundDistance = value; } }
    private int _currentDistance = 0;
    public int currentDistance { get { return _currentDistance; } set { _currentDistance = value; } }

    private int _currentWave = 1;
    public int currentWave { get { return _currentWave; } set { _currentWave = value; } }

    private int _currentTurn = 0;
    public int currentTurn { get { return _currentTurn; } set { _currentTurn = value; } }

    private TurnState.State _currentTurnState = TurnState.State.BeforeTurnStart;
    public TurnState.State currentTurnState { get { return _currentTurnState; } set { _currentTurnState = value; } }

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

    // For caluating wrong answers submited
    [SerializeField] private int _wrongCountCombo = 0;
    public int wrongCountCombo { get { return _wrongCountCombo; } set { _wrongCountCombo = value; } }

    [SerializeField] private int _totalWrongCount = 0;
    public int totalWrongCount { get { return _totalWrongCount; } set { _totalWrongCount = value; } }

    [SerializeField] private List<int> _roundMobs;
    public List<int> roundMobs { get { return _roundMobs; } set { _roundMobs = value; } }
    //[SerializeField] private List<Teammate> _roundTeammates;
    //public List<Teammate> roundTeammates { get { return _roundTeammates; } set { _roundTeammates = value; } }
    //private int _roundTeammateNumber = 5;
    //public int roundTeammateNumber { get { return _roundTeammateNumber; } set { _roundTeammateNumber = value; } }
    private int _roundMobNumber;
    public int roundMobNumber { get { return _roundMobNumber; } set { _roundMobNumber = value; } }
    private int _currentSpawnedMobNumber;
    public int currentSpawnedMobNumber { get { return _currentSpawnedMobNumber; } set { _currentSpawnedMobNumber = value; } }


    private int _roundIdiomSize = 4;
    public int roundIdiomSize { get { return _roundIdiomSize; } set { _roundIdiomSize = value; } }

    private int _currentPowerScore = 0;
    public int currentPowerScore { get { return _currentPowerScore; } set { _currentPowerScore = value; } }

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

    // Storing turn state duration
    public float baseTurnDuration = 0.5f;
    public float currentTurnDuration;
    
    // Booleans
    private bool _requestNewWave = false;
    public bool requestNewWave { get { return _requestNewWave;} set { _requestNewWave = value; } }
    private bool _flyToBattle = false;
    public bool flyToBattle { get { return _flyToBattle;} set { _flyToBattle = value; } }
    private bool _battleToFly = false;
    public bool battleToFly { get { return _battleToFly;} set { _battleToFly = value; } }
    #endregion

    #region Factories
    public MobFactory mobFactory;
    public TeammateFactory teammateFactory;
    public EffectBoxFactory effectBoxFactory;
    #endregion

    #region Flows
    void Awake()
    {
        Debug.Log($"{Time.time} RoundData.Awake (start)");

        Debug.Log($"{Time.time} answers counts in board: {tilesWord.Count}");
        Debug.Log($"{Time.time} answerIdiomsID counts in board: {tilesWordIdiomsID.Count}");

        board = GameObject.Find("Board").GetComponent<Board>();
        player = gameObject.GetComponent<Player>();
        roundManager = gameObject.GetComponent<RoundManager>();
        uiManage = GameObject.Find("UI Manager").GetComponent<UIManage>();
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

        FirstTimeInit();

        Debug.Log($"{Time.time} RoundData.Start (end)");
    }

    void Update()
    {
        //Debug.Log($"{Time.time} RoundData.Update (start)");

        UpdateTimer();
        healthBars.ResizeBarValue(player.currentHealthValue, player.currentMaxHealthValue, 0);
        //Debug.Log($"player's HP = {player.currentHp.value}");
        //mobHealthBars.ResizeBarValue(currentMob.currentHealthValue, currentMob.currentMaxHealthValue, 0);
        //Debug.Log($"mob's HP = {currentMob.currentHp.value}");
        //progressBars.ResizeBarValue(currentDistance, roundDistance, 0);

        currentGameStateText.text = roundManager.currentGameState.ToString();
        currentTurnStateText.text = currentTurnState.ToString();
        currentTurnText.text = currentTurn.ToString();
                
        if (currentPowerScore < 0)
        {
            currentPowerScore = 0;
        }
        powerScoreText.text = currentPowerScore.ToString();

        if (roundManager.currentGameState == GameState.State.IsBattling)
        {
            mobHealthBars.ResizeBarValue(currentMob.currentHealthValue, currentMob.currentMaxHealthValue, 0);
            currentMobIDText.text = currentMob.mobID.ToString();
            mobCDText.text = currentMob.currentAttackInterval.ToString();
        }

        if (roundManager.currentGameState == GameState.State.IsFlying)
        {
            // Prevent current progress bar expend outside screen, force fixed max health value
            if (currentDistance > roundDistance && (currentDistance != roundDistance))
            {
                currentDistance = roundDistance;
            }

            if (currentDistance <= 0)
            {
                // For visual
                currentDistance = 0;
            }

            progressBars.ResizeBarValue(currentDistance, roundDistance, 0);
            currentProgressText.text = currentDistance.ToString();

            // Pack into update progress box function
            currentProgressBox.gameObject.transform.position = new Vector3(progressBars.transform.GetChild(1).transform.position.x + progressBars.transform.GetChild(1).transform.GetComponent<RectTransform>().rect.width/2, currentProgressBox.gameObject.transform.position.y, 0);
            
        }

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
    #endregion

    #region Round data functions
    // Should handle all display round data shit
    public void SpawnMobs()
    {
        Debug.Log($"{Time.time} RoundData.SpawnMobs (start)");

        currentMob = mobFactory.CreateMob(roundMobs[currentWave - 1]);
        mobHealthBars.InitializeBar(mobHealthBars, currentMob.currentHealthValue, currentMob.currentMaxHealthValue);
        mobLevelText.text = currentMob.level.ToString();
        mobNameText.text = currentMob.mobName;
        currenMobPic = currentMob.mobPicture;
        mobCDText.text = currentMob.currentAttackInterval.ToString();

        currentSpawnedMobNumber += 1;

        Debug.Log($"{Time.time} {currentMob.entityName} just spawn!");

        Debug.Log($"{Time.time} RoundData.SpawnMobs (end)");
    }

    // Should change to drawing mob from specific mob list data, but now just draw mob first than spawn accordingly
    public void DrawMobs()
    {
        Debug.Log($"{Time.time} RoundData.DrawMobs (start)");

        for (int i = 0; i < roundMobNumber; i++)
        {
            int randomMobID = UnityEngine.Random.Range(0, ImportData.mobs.mob.Length);
            roundMobs.Add(randomMobID);
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
        player.currentHealthValue = player.currentMaxHealthValue;
        currentWaveText.text = currentWave.ToString() + "/" + roundMobNumber;
        //mobHealthBars.InitializeBar(mobHealthBars, currentMob.currentHp, currentMob.maxHp);
        healthBars.InitializeBar(healthBars, player.currentHealthValue, player.currentMaxHealthValue);

        Debug.Log($"{Time.time} RoundData.InitializeData (end)");
    }

    public void UpdateTimer()
    {
        //Debug.Log($"{Time.time} RoundData.StartTimer (start)");

        timeUsed = timeUsed + Time.deltaTime;

        int mins = (int)(timeUsed/60);
        string minsText = mins.ToString("00");
        int seconds = (int)(timeUsed-(mins*60));
        string secondsText = seconds.ToString("00");

        currentTimeText.text = $"{minsText}:{secondsText}";

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

    public Entity CompareSpeedPoint(Entity player, Entity mob)
    {
        Debug.Log($"{Time.time} RoundData.CompareSpeedPoint (start)");

        Entity entityMoveFirst;
        string popupMessage;
        //Debug.Log($"{Time.time} player dex = {player.GetDexterityPoint()}, mob dex = {currentMob.GetDexterityPoint()}");
        if (player.currentSpeedValue > mob.currentSpeedValue)
        {
            entityMoveFirst = player;
            popupMessage = $"{player.entityName}先手! ({player.currentSpeedValue} > {currentMob.currentSpeedValue})";
        }
        else 
        {
            entityMoveFirst = mob;
            popupMessage = $"{mob.entityName}先手! ({currentMob.currentSpeedValue} > {player.currentSpeedValue})";
        }

        uiManage.SpawnPopup(popupMessage, "normal");

        Debug.Log($"{Time.time} RoundData.CompareSpeedPoint, return entityMoveFirst(local var): {entityMoveFirst} (end)");
        return entityMoveFirst;
    }

    public void FirstTimeInit()
    {
        Debug.Log($"{Time.time} RoundData.FirstTimeInit (start)");

        // First time while enter the game loop, a.k.a init
        currentTurnState = TurnState.State.FirstTimeInit;
        currentTurnDuration = baseTurnDuration;

        if (roundDistance < 1000)
        {
            roundMobNumber = 1;
        }
        else
        {
            roundMobNumber = (int)(roundDistance / 1000);
        }

        progressBars.InitializeBar(progressBars, currentDistance, roundDistance);

        board.SpawnTiles(board.CheckBlankCell());
        board.DrawAnswer();
        player.InitPlayer();
        roundManager.currentGameState = GameState.State.IsFlying;

        generalInfo.gameObject.SetActive(true);
        mobInfo.gameObject.SetActive(false);

        generalArenaInfo.gameObject.SetActive(true);
        mobArenaInfo.gameObject.SetActive(false);

        this.Wait(currentTurnDuration, BeforeTurnStart);

        Debug.Log($"{Time.time} RoundData.FirstTimeInit (end)");
        
    }

    public void BeforeTurnStart()
    {
        Debug.Log($"{Time.time} RoundData.BeforeTurnStart (start)");

        // Change turn state and data
        currentTurnState = TurnState.State.BeforeTurnStart;
        currentTurnDuration = baseTurnDuration;

        currentTurn += 1;
        currentTurnText.text = currentTurn.ToString();

        // Tile do their before turn start shit
        for (int i = 0; i < board.answerTilesSpawner.transform.childCount; i++)
        {
            Tile tile = board.answerTilesSpawner.transform.GetChild(i).GetComponent<Tile>();
            tile.OnBeforeTurnStart();
        }

        if (flyToBattle)
        {
            // Init mob 
            generalInfo.gameObject.SetActive(false);
            mobInfo.gameObject.SetActive(true);

            generalArenaInfo.gameObject.SetActive(false);
            mobArenaInfo.gameObject.SetActive(true);
            DrawMobs();
            SpawnMobs();

            // Update/register ability target
            foreach (KeyValuePair<TeammateType, Teammate> teammate in player.teammates)
            {
                teammate.Value.activeAbility.UpdateAbilityTarget(player, currentMob);
            }

            currentMob.activeAbility.UpdateAbilityTarget(currentMob, player);
            roundManager.currentGameState = GameState.State.IsBattling;
            flyToBattle = false;
        }

        if (battleToFly)
        {
            generalInfo.gameObject.SetActive(true);
            mobInfo.gameObject.SetActive(false);

            generalArenaInfo.gameObject.SetActive(true);
            mobArenaInfo.gameObject.SetActive(false);

            roundManager.currentGameState = GameState.State.IsFlying;
            battleToFly = false;
        }

        // Check status effects and do the damage/other shit to the status effect owner
        player.CheckStatusEffects();

        if (player.currentStatusEffects.Count > 0)
        {
            for (int i = (player.currentStatusEffects.Count - 1); i >= 0; i--)
            {
                player.currentStatusEffects[i].OnBeforeTurnStart();
            }
        }

        // Update effect box
        Transform effectBoxParent = GameObject.Find("Effect boxes spawner").transform;
        for (int i = 0; i < effectBoxParent.childCount; i++)
        {
            if (effectBoxParent.GetChild(i).transform.childCount > 0)
            {
                foreach (Transform effectBoxGameObject in effectBoxParent.GetChild(i))
                {
                    EffectBox effectBox = effectBoxGameObject.GetComponent<EffectBox>();
                    //effectBox.UpdateEffectBox();
                }
            }
        }

        if (roundManager.currentGameState == GameState.State.IsFlying)
        {
            this.Wait(currentTurnDuration, player.BeforeMoveStart);
        }
        else if (roundManager.currentGameState == GameState.State.IsBattling)
        {
            currentMob.CheckStatusEffects();

            if (currentMob.currentStatusEffects.Count > 0)
            {
                for (int i = (currentMob.currentStatusEffects.Count - 1); i >= 0; i--)
                {
                    currentMob.currentStatusEffects[i].OnBeforeTurnStart();
                }
            }

            this.Wait(currentTurnDuration, CompareSpeedPoint(player, currentMob).BeforeMoveStart);
        }

        Debug.Log($"{Time.time} RoundData.BeforeTurnStart (end)");
    }

    public void TurnEnd()
    {
        Debug.Log($"{Time.time} RoundData.TurnEnd (start)");

        // Change turn state and data
        currentTurnState = TurnState.State.TurnEnd;
        currentTurnDuration = baseTurnDuration;

        // Reset shit
        player.moveEnded = false;
        if (roundManager.currentGameState == GameState.State.IsBattling)
        {
            currentMob.moveEnded = false;
        }
        
        // Reset/update status effects
        if (player.currentStatusEffects.Count > 0)
        {
            for (int i = (player.currentStatusEffects.Count - 1); i >= 0; i--)
            {
                Debug.Log($"{Time.time} player's status effect no.{i}: {player.currentStatusEffects[i].effectName}");
                player.currentStatusEffects[i].OnTurnEnd();
            }
        }

        if (roundManager.currentGameState == GameState.State.IsBattling)
        {
            if (currentMob.currentStatusEffects.Count > 0)
            {
                for (int i = (currentMob.currentStatusEffects.Count - 1); i >= 0; i--)
                {
                    Debug.Log($"{Time.time} mob's status effect no.{i}: {currentMob.currentStatusEffects[i].effectName}");
                    currentMob.currentStatusEffects[i].OnTurnEnd();
                }
            }
        }

        Debug.Log($"{Time.time} ^6.6A End turn");

        // Teammate do their turn end shit
        /*
        foreach (KeyValuePair<TeammateType, Teammate> teammate in player.teammates)
        {
            teammate.Value.currentActiveAbilityCD -= 1;
        }\
        */

        // Tile do their turn end shit
        for (int i = 0; i < board.answerTilesSpawner.transform.childCount; i++)
        {
            Tile tile = board.answerTilesSpawner.transform.GetChild(i).GetComponent<Tile>();
            if (tile.currentTileEffect != null)
            {
                tile.currentTileEffect.OnTurnEnd(tile);
            }

            tile.OnTurnEnd();
        }

        // Check the tiles in board again(for cleaning some effect which can't be destroyed for some reason)
        board.CheckAndDestroyTiles();

        board.DisplayTileCell(); // For debug use
        board.RenameTiles();
        board.CheckAnswerTile();
        board.DisplayTileCell(); // Double check, just in case

        // Check gameover
        // 1. Arrived to the destination
        if (currentDistance >= roundDistance)
        {
            roundManager.currentGameState = GameState.State.PlayerWin;
            roundManager.GameOver(roundManager.currentGameState);
        }
        
        // 2. Mob defeated
        if (roundManager.currentGameState == GameState.State.IsBattling)
        {
            if (currentMob.currentState == EntityState.State.Dead)
            {
                string popupMessage = $"{currentMob.entityName}已經被撃退咗!";
                uiManage.SpawnPopup(popupMessage, "emergency");

                StoreGainedReward();
                // Reset all status effect from preveus mob
                for (int i = mobEffectBoxes.transform.childCount - 1; i >= 0; i--)
                {
                    mobEffectBoxes.transform.GetChild(i).GetComponent<EffectBox>().DestroyEffectBox();
                }

                Destroy(currentMob.gameObject);
                currentMob.transform.SetParent(null);

                //roundManager.currentGameState = GameState.State.IsFlying;
                battleToFly = true;

                this.Wait(currentTurnDuration, BeforeTurnStart);
            }
        }
        
        // 3. Player defeated
        if (player.currentState == EntityState.State.Dead) 
        {
            roundManager.currentGameState = GameState.State.PlayerLose;
            roundManager.GameOver(roundManager.currentGameState);
        }

        // 4. New turn
        else
        {
            if (roundManager.currentGameState == GameState.State.IsBattling)
            {
                currentMob.currentAttackInterval -= 1;
                mobCDText.text = currentMob.currentAttackInterval.ToString();
            }
            
            this.Wait(currentTurnDuration, BeforeTurnStart);
        }

        Debug.Log($"{Time.time} RoundData.TurnEnd (end)");
    }
    #endregion
}
