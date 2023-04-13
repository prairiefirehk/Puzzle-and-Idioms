using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Tile : Effectable, IPointerDownHandler, IBeginDragHandler, IDragHandler, IPointerUpHandler, IEndDragHandler, IDropHandler
{
    #region Scripts
    public Board board;
    public RoundData roundData;
    #endregion

    #region GameObjects
    // Blank tile without anythiung shown in the canvas, need to be initialize later
    public GameObject tile;
    public TMP_Text tileLevelText;
    public TMP_Text tileEffectATurnsText;
    public TMP_Text tileEffectBTurnsText;
    public Image tileEffectAIcon;
    public Image tileEffectBIcon;

    // Controlling the tile transparent
    public CanvasGroup canvasGroup;
    // Controlling the tile rect transform shit
    public RectTransform tileRect;
    #endregion

    #region Tile info
    // Spawning chance, >0 ~ <100?
    public int spawnRate;
    // "normal", "special", "teammate"
    public string tileType;
    // (x,y)
    public Vector2 tileLocation;
    // 0-24 in the answer board
    public int tileCellPosition;
    // For storing tile cell position
    public int tileCellPositionAtStart;
    // 0-?
    public int tileLevel;
    // Name of the element
    public string tileElementType;
    // (r, b, g, a), 0-255
    public Color32 tileColor;
    // Effect A
    public TileEffect tileEffectA;
    // Effect A icon name
    public string tileEffectAIconName;
    // Effective turns for effect A
    public int tileEffectATurns;
    // Remaining turns for effect A
    public int tileEffectARemainingTurns;
    public bool hasTileEffectA;
    // Effect B
    public TileEffect tileEffectB;
    // Effect B icon name
    public string tileEffectBIconName;
    // Effective turns for effect B
    public int tileEffectBTurns;
    // Remaining turns for effect B
    public int tileEffectBRemainingTurns;
    public bool hasTileEffectB;

    public TileState.State currentState = TileState.State.IsInitalizing;
    public int tileStatusProcessed = 0;
    #endregion

    #region Tile records
    // Which turn the tile spawned, 0-?
    public int tileSpawnedTurns;
    // Total existed turn for the tile, 1-?
    public int tileExistedTurns;
    // Total moved turn for the tile, 0-?
    public int tileMovedTurns;
    // Stores this tile moving record
    public int[] tilePositionRecords;
    // Output value
    public int outputValue;
    // Base score **modifier**
    public float baseValueModifier;
    // Adjusted current score
    public float currentvalueModifier;
    // For managing score tile bonus based on bonus, testing only
    public float[] valueModifierArr = { 0, 0.05f, 0.1125f, 0.2531f, 0.5695f, 1.2814f, 2.8833f, 6.4873f, 14.5965f, 32.8420f, 73.8946f, 166.2628f, 374.0914f, 841.7056f, 1893.8376f, 4261.1346f, 9587.5530f};
    // Board score related
    public int outputScore;
    public int[] tileLevelScoreArr = { 0, 2000, 5000, 9000, 14000, 20000, 27000, 34000, 43000, 53000};
    #endregion

    #region Tile booleans
    // For detecting answer
    public bool isAnswer;

    // Checking if the tile moved
    public bool isMoved;
    // For those methods of the tile, change to state and will be deleted
    public bool isSelect;
    public bool isDrag;
    public bool toBeDestroyed;
    #endregion

    #region Tile interaction
    // May need to change/delete
    public bool inSlot;
    public Teammate interactTeammate;
    public Vector2 interactSlotLocation;
    public bool inTile;
    public Tile interactTile;
    public Vector2 interactTileLocation;
    #endregion

    #region Flow
    void Awake() 
    {
        Debug.Log($"{name} Tile.Awake (start)");

        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();
        board = GameObject.Find("Board").GetComponent<Board>();
        currentvalueModifier = baseValueModifier;

        Debug.Log($"{name} Tile.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{name} Tile.OnEnable (start)");

        // Subscribe to the game events and listen
        //Board.OnNewTurnEvent += OnNewTurn;

        Debug.Log($"{name} Tile.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{name} Tile.Start (start)");
        Debug.Log($"{name} Tile.Start (end)");
    }

    void Update()
    {
        
    }
    
    void OnDisable()
    {
        Debug.Log($"{name} Tile.OnDisable (start)");

        // Unubscribe to the game events and listen
        //Board.OnNewTurnEvent -= OnNewTurn;

        Debug.Log($"{name} Tile.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{name} Tile.OnDestroy (start)");
        Debug.Log($"{name} Tile.OnDestroy (end)");
    }
    #endregion

    #region Enum of tile effect
    public enum TileEffect
    {
        healPlayer,
        damagePlayer,
        tileLevelIncrease,
        tileLevelDecrease,
        multipleAttackPoint,
        divideAttackPoint,
        tileSpawnScoreIncrease,
        tileSpawnScoreDecrease,
        coinGainedIncrease,
        coinGainedDecrease,
        expGainedIncrease,
        expGainedDecrease,
        jadeGainedIncrease,
        jadeGainedDecrease
    }
    #endregion
    
    #region Tile events
    //public static event Action OnEndTurnEvent;
    #endregion

    #region Tile functions
    // Temp
    public string GetTileIconName(int tileFunctionNumber)
    {
        Debug.Log($"{name} Tile.GetTileIconName (start)");

        string picName = "";
        switch(tileFunctionNumber)
        {
            case 1:
                picName = "Health_increase_icon_red_50_hq";
                break;

            case 2:
                picName = "Health_decrease_icon_red_50_hq";
                break;
                    
            case 3:
                picName = "TileLevel_increase_icon_yellow_50_hq";
                break;
                    
            case 4:
                picName = "TileLevel_decrease_icon_yellow_50_hq";
                break;
                    
            case 5:
                picName = "Multiple_attackPoint_icon_black_50_hq";
                break;
                    
            case 6:
                picName = "Divide_attackPoint_icon_black_50_hq";
                break;
                    
            case 7:
                picName = "TileSpawnScore_increase_icon_green_50_hq";
                break;
                    
            case 8:
                picName = "TileSpawnScore_decrease_icon_green_50_hq";
                break;
                    
            case 9:
                picName = "CoinGained_increase_icon_yellow_50_hq";
                break;
                    
            case 10:
                picName = "CoinGained_decrease_icon_yellow_50_hq";
                break;
                    
            case 11:
                picName = "ExpGained_increase_icon_purple_50_hq";
                break;
                    
            case 12:
                picName = "ExpGained_decrease_icon_purple_50_hq";
                break;
                    
            case 13:
                picName = "JadeGained_increase_icon_green_50_hq";
                break;
                    
            case 14:
                picName = "JadeGained_decrease_icon_green_50_hq";
                break;
                    
            default:
                picName = "Health_increase_icon_red_50_hq";
                Debug.Log("Hey dude can't load special tile's name!");
                break;
        }
        Debug.Log($"{name} Tile.GetTileIconName, return picName(local var): {picName} (end)");
        return picName;
    }

    public void GetTileEffect(Tile tile, TileEffect tileEffectOne, int tileEffectOneTurns)
    {
        Debug.Log($"{name} Tile.GetTileEffect (start)");


        tile.tileEffectA = tileEffectOne;
        tile.tileEffectATurns = tileEffectOneTurns;
        tile.tileEffectARemainingTurns = tile.tileEffectATurns;
        tile.tileEffectAIconName = tile.GetTileIconName((int)tileEffectOne);
        Sprite tileEffectAOrgImage = Resources.Load<Sprite>($"Prefabs/Tiles/{tile.tileEffectAIconName}");
        tile.tileEffectAIcon.sprite = tileEffectAOrgImage;


        // Make sure the effect icon/text object is active
        tile.tileEffectAIcon.gameObject.SetActive(true);
        tile.tileEffectATurnsText.gameObject.SetActive(true);

        tile.tileEffectATurnsText.text = tile.tileEffectARemainingTurns.ToString();

        Debug.Log($"{name} Tile.GetTileEffect (end)");
    }

    public void GetTileEffect(Tile tile, TileEffect tileEffectOne, int tileEffectOneTurns, TileEffect tileEffectTwo, int tileEffectTwoTurns)
    {
        Debug.Log($"{name} Tile.GetTileEffect (start)");

        tile.tileEffectA = tileEffectOne;
        tile.tileEffectATurns = tileEffectOneTurns;
        tile.tileEffectARemainingTurns = tile.tileEffectATurns;
        tile.tileEffectATurnsText.text = tileEffectARemainingTurns.ToString();
        tile.tileEffectAIconName = tile.GetTileIconName((int)tileEffectOne);
        Sprite tileEffectAOrgImage = Resources.Load<Sprite>($"Prefabs/Tiles/{tile.tileEffectAIconName}");
        tile.tileEffectAIcon.sprite = tileEffectAOrgImage;

        // Make sure the effect icon/text object is active
        tile.tileEffectAIcon.gameObject.SetActive(true);
        tile.tileEffectATurnsText.gameObject.SetActive(true);

        tile.tileEffectATurnsText.text = tile.tileEffectARemainingTurns.ToString();

        tile.tileEffectB = tileEffectTwo;
        tile.tileEffectBTurns = tileEffectTwoTurns;
        tile.tileEffectBRemainingTurns = tile.tileEffectBTurns;
        tile.tileEffectBTurnsText.text = tileEffectBRemainingTurns.ToString();
        tile.tileEffectBIconName = tile.GetTileIconName((int)tileEffectOne);
        Sprite tileEffectBOrgImage = Resources.Load<Sprite>($"Prefabs/Tiles/{tile.tileEffectBIconName}");
        tile.tileEffectBIcon.sprite = tileEffectBOrgImage;

        // Make sure the effect icon/text object is active
        tile.tileEffectBIcon.gameObject.SetActive(true);
        tile.tileEffectBTurnsText.gameObject.SetActive(true);

        tile.tileEffectBTurnsText.text = tile.tileEffectBRemainingTurns.ToString();

        Debug.Log($"{name} Tile.GetTileEffect (end)");
    }

    public void SetTileLocation()
    {
        Debug.Log($"{name} Tile.SetTileLocation (start)");

        tile.transform.position = tileLocation;

        Debug.Log($"{name} Tile.SetTileLocation (end)");
    }

    public void BeingSelected()
    {
        Debug.Log($"{name} Tile.BeingSelected (start)");

        isSelect = true;
        Debug.Log("^1.1 input selected the tile");

        Debug.Log($"{name} Tile.BeingSelected (end)");
    }

    public void StartDrag()
    {
        Debug.Log($"{name} Tile.StartDrag (start)");

        isDrag = true;
        // ****************************************
        roundData.player.SetDragTile(this);

        // Getting the tile count in the answer tile spawner
        transform.SetSiblingIndex(board.transform.GetChild(6).childCount);

        tile.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
        canvasGroup.alpha = 0.75f;
        canvasGroup.blocksRaycasts = false;
        Debug.Log("^2.1 start drag the tile");

        Debug.Log($"{name} Tile.StartDrag (end)");
    }

    public void UnSelected()
    {
        Debug.Log($"{name} Tile.UnSelected (start)");
        
        isSelect = false;
        roundData.player.SetDragTile(null);

        Debug.Log("^4.1 input unselected the tile");

        Debug.Log($"{name} Tile.UnSelected (end)");
    }

    public void TileUpgrade(Tile tilePrefab)
    {
        Debug.Log($"{name} Tile.TileUpgrade (start)");

        Debug.Log($"{tilePrefab.name}'s level upgraded");
        if (tilePrefab.tileLevelText.isActiveAndEnabled == false)
        {
            tilePrefab.tileLevelText.gameObject.SetActive(true);
        }
        tilePrefab.tileLevel++;
        tilePrefab.currentvalueModifier = tilePrefab.valueModifierArr[tilePrefab.tileLevel];
        tilePrefab.tileLevelText.text = tilePrefab.tileLevel.ToString();
        Debug.Log("^5.3C tile tileLevel up");

        Debug.Log($"{name} Tile.TileUpgrade (end)");
    }

    public void ResetTile(Tile tilePrefab)
    {
        Debug.Log($"{name} Tile.ResetTile (start)");

        Debug.Log("^6.7 ResetTile start");

        // Reset stat
        if (tilePrefab.interactTile != null || tilePrefab.interactTeammate != null)
        {
            //tile.interactTile.position = new Vector2(0, 0);
            tilePrefab.interactTile = null;

            //tile.interactSlot.position = new Vector2(0, 0);
            tilePrefab.interactTeammate = null;
        }

        // If tile get merge flag, destroy the tile
        /*
        if (tilePrefab.toBeDestroyed == true)
        {
            tilePrefab.toBeDestroyed = false;
            //Debug.Log("Tile being merged (" + tilePrefab.tileCellPositionAtStart.ToString() + ") is : " + );
            DestroyTile(tilePrefab);

            Debug.Log("6.1A tile destroyed");
        }
        */

        tilePrefab.inTile = false;
        tilePrefab.inSlot = false;
        tilePrefab.currentState = TileState.State.Idle;
        tilePrefab.isMoved = false;
        tileStatusProcessed = 0;

        Debug.Log("^6.8 ResetTile end");

        Debug.Log($"{name} Tile.ResetTile (end)");
    }

    public void EndDrag(Tile tilePrefab)
    {
        Debug.Log($"{name} Tile.EndDrag (start)");

        Debug.Log("^6.1 EndDrag start");
        isDrag = false;
        // Before apply move tile function, only return to it's own position
        transform.SetSiblingIndex(tileCellPosition);
        //ResetTile(this);
        // Should rewrite it as when one turn passes, do such steps...
        UpdatePosition(this);
        //board.RenameTiles();
        board.CheckAndDestroyTiles();


        tile.GetComponent<BoxCollider2D>().size = new Vector2(232, 247);
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        roundData.player.SetDragTile(null);

        // Only moved tile will count as turn move
        if (tilePrefab.isMoved == true)
        {
            //OnEndTurnEvent?.Invoke();
            //roundData.TurnEnd();
            Debug.Log("ismoved! end turn!");
        }
        
        //board.EndTurn();
        //OnEndTurnEvent?.Invoke();
        // Final reset
        ResetTile(this);
        Debug.Log("^6.9 EndDragDrag end");

        Debug.Log($"{name} Tile.EndDrag (end)");
    }

    public void UpdatePosition(Tile tilePrefab)
    {
        Debug.Log($"{name} Tile.UpdatePosition (start)");

        Debug.Log("^6.2 Update position");
        // For displaying map for tile cell
        board.DisplayTileCell();
        //Debug.Log("board.answerCell before: \n" + tileCellMap);

        // Check if the tile changed its position
        // If tile position had been drop to another tile cell, which the tile cell is blank cell
        if (board.tileCell[tilePrefab.tileCellPosition] == 0)
        {
            Debug.Log("^6.3A Changed position!");

            tilePrefab.isMoved = true;
            roundData.player.isActioned = true;
            // Change tile location
            tilePrefab.gameObject.transform.position = board.transform.GetChild(5).gameObject.transform.GetChild(tileCellPosition).position;
            // Indicate tile cell has new tile
            if (tilePrefab.tileType == "normal")
            {
                board.tileCell[tilePrefab.tileCellPosition] = 1;
            }
            else if (tilePrefab.tileType == "special")
            {
                board.tileCell[tilePrefab.tileCellPosition] = 2;
            }
            
            board.tileCell[tilePrefab.tileCellPositionAtStart] = 0;
            // Set tile new position
            tilePrefab.tileCellPositionAtStart = tileCellPosition;
        }
        // If that tile cell is occupied, return its starting position
        else 
        {
            Debug.Log("^6.3B Didn't change position!");

            // Don't set it
            //tilePrefab.isMoved = false;
            tilePrefab.gameObject.transform.position = board.transform.GetChild(5).gameObject.transform.GetChild(tileCellPositionAtStart).position;
        }

        // For displaying map for tile cell
        board.DisplayTileCell();
        //Debug.Log("board.answerCell after: \n" + tileCellMap);

        Debug.Log($"{name} Tile.UpdatePosition (end)");
    }

    public void DestroyTile(Tile tilePrefab)
    {
        Debug.Log($"{name} Tile.DestroyTile (start)");

        Debug.Log("^6.5 Destroy tile ");
        Debug.Log($"Tile being destroyed ({tilePrefab.tileCellPositionAtStart}) is : {tilePrefab.name}");
        // Mark the cell is blank now
        board.tileCell[tilePrefab.tileCellPositionAtStart] = 0;

        // Contribute the score to board
        board.tileLevelSpawnScore += outputScore;
        
        //Debug.Log("Tile destroyed: " + tilePrefab.gameObject.transform.GetSiblingIndex().ToString());
        // Remove item in answerTile list
        board.tilesInBoard.Remove(tilePrefab);
        if (tilePrefab.CompareTag("NormalTile"))
        {
            // Remove item in roundData
            string removeWord = tilePrefab.transform.GetComponent<NormalTile>().tileText.text.ToString();
            int removeID = (int)tilePrefab.transform.GetComponent<NormalTile>().tileIdiomID;

            if (tilePrefab.isAnswer)
            {
                tilePrefab.isAnswer = false;
            }

            roundData.tilesWord.Remove(removeWord);
            roundData.tilesWordIdiomsID.Remove(removeID);

            
            // For debug
            /*
            string answerString = "";
            foreach (string ans in roundData.answers)
            {
                Debug.Log(ans);
                answerString = String.Join(", ", roundData.answers);
            }
            Debug.Log("answers counts in board: " + roundData.answers.Count + " and answers are: " + answerString);

            string answerIdiomIDString = "";
            foreach (int ID in roundData.answerIdiomsID)
            {
                Debug.Log(ID);
                answerIdiomIDString = String.Join(", ", roundData.answerIdiomsID);
            }
            Debug.Log("answerIdiomsID counts in board: " + roundData.answerIdiomsID.Count + " and answers id are: " + answerIdiomIDString);
            */
        }

        Destroy(tilePrefab.gameObject);
        tilePrefab.transform.SetParent(null);

        Debug.Log($"{name} Tile.DestroyTile (end)");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{name} Tile.OnTriggerEnter2D (start)");

        if (isDrag && isSelect)
        {
            if (collision.CompareTag("Teammate"))
            {
                Debug.Log($"^3.1A enter teammate {collision.name} ----------------------------");
                inSlot = true;
                interactTeammate = collision.gameObject.GetComponent<Teammate>();
                

                // For effect
                interactSlotLocation = collision.transform.position;
            }
            else if (collision.CompareTag("NormalTile") || collision.CompareTag("SpecialTile"))
            {
                Debug.Log($"^3.1B enter tile {collision.name} ----------------------------");
                inTile = true;
                interactTile = collision.gameObject.GetComponent<Tile>();
                interactTile.interactTile = this;
                interactTileLocation = collision.transform.position;
                interactTile.interactTileLocation = this.tileLocation;

                // For detact each other?
                //interactTile.interactTile = this;
            }

            if (collision.CompareTag("Cell"))
            {
                tileCellPosition = collision.gameObject.transform.GetSiblingIndex();

                if (this.CompareTag("NormalTile"))
                {
                    Debug.Log($"^3.1C enter cell---------------------------- {name} {this.transform.GetChild(2).GetComponent<TMP_Text>().text} from position {tileCellPositionAtStart} -> {tileCellPosition}");
                }
                else if (this.CompareTag("SpecialTile"))
                {
                    Debug.Log($"^3.1C enter cell---------------------------- {name} from position {tileCellPositionAtStart} -> {tileCellPosition}");
                }
            }
        }

        Debug.Log($"{name} Tile.OnTriggerEnter2D (end)");
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log($"{name} Tile.OnTriggerExit2D (start)");

        if(isDrag && isSelect)
        {
            if (collision.CompareTag("Teammate"))
            {
                Debug.Log($"^3.2A exit teammate {collision.name} *****************************");
                inSlot = false;
                //interactSlot.position = new Vector2(0, 0);
                interactTeammate = null;
            }
            else if (collision.CompareTag("NormalTile") || collision.CompareTag("SpecialTile"))
            {
                Debug.Log($"^3.2B exit tile {collision.name} *****************************");
                inTile = false;
                //interactTile.interactTile.position = new Vector2(0, 0);
                interactTile.interactTile = null;

                //interactTile.position = new Vector2(0, 0);
                interactTile = null;
            }

            if (collision.CompareTag("Cell"))
            {
                if (this.CompareTag("NormalTile"))
                {
                    Debug.Log($"^3.2C exit cell***************************** {name} {this.transform.GetChild(2).GetComponent<TMP_Text>().text} from position {tileCellPositionAtStart} -> {tileCellPosition}");
                }
                else if (this.CompareTag("SpecialTile"))
                {
                    Debug.Log($"^3.2C exit cell***************************** {name} from position {tileCellPositionAtStart} -> {tileCellPosition}");
                }

                tileCellPosition = tileCellPositionAtStart;
            }
        }

        Debug.Log($"{name} Tile.OnTriggerExit2D (end)");
    }
    #endregion

    #region Tile pointerEvents (in order)
    // Click //
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"{name} Tile.OnPointerDown (start)");

        Debug.Log("^1 input click the tile");
        currentState = TileState.State.Clicked;
        tileStatusProcessed += 1;
        BeingSelected();

        Debug.Log($"{name} Tile.OnPointerDown (end)");
    }

    // Start drag, called only once, if just click won't be triggered //
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"{name} Tile.OnBeginDrag (start)");

        Debug.Log("^2 begin drag the tile");
        currentState = TileState.State.StartDrag;
        roundData.currentTurnState = TurnState.State.PlayerAction;

        tileStatusProcessed += 1;
        StartDrag();

        Debug.Log($"{name} Tile.OnBeginDrag (end)");
    }

    // Dragging //
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log($"{name} Tile.OnDrag (start)");

        //Debug.Log("^3 dragging the tile");
        //isDrag = true;
        currentState = TileState.State.Dragging;
        transform.position = Input.mousePosition;

        Debug.Log($"{name} Tile.OnDrag (end)");
    }

    // Drop, then dropper handler will receive after this //
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"{name} Tile.OnPointerUp (start)");

        Debug.Log("^4 input release the tile");
        currentState = TileState.State.Released;
        roundData.currentTurnState = TurnState.State.WaitingMobAction;

        tileStatusProcessed += 1;
        UnSelected();

        // Sometime player only just clicked the tile, by this will just end the whole dragging process.
        // To prevent this, just check the status processed(if the tile didn't go through OnBeginDrag, tileStatusProcessed will less than 3)
        if (tileStatusProcessed < 3)
        {
            // Forcing tile to reset
            ResetTile(this);
            tileStatusProcessed = 0;
        }

        Debug.Log($"{name} Tile.OnPointerUp (end)");
    }

    // For tile merge, receive after onpointerup (AS A DROP RECEIVER) //
    public virtual void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"{name} Tile.OnDrop (start)");
        /*
        Debug.Log("^5 input drop the tile");
        currentState = TileState.State.Processing;
        tileStatusProcessed += 1;

        // They are the same thing
        Tile dragTile = eventData.pointerDrag.GetComponent<Tile>();

        // ****************************************
        player.SetDragTile(dragTile);
        //player.tile = dragTile;

        // Detect drag tile-> slot
        if (dragTile.inSlot == true)
        {
            // Do nothing, teammate drop handler will deal with it
            if (dragTile.CompareTag("NormalTile"))
            {
                Debug.Log($"^5.1A.1 {name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text} drop to teammate {dragTile.interactTeammate.name} (msg from tile)");
            }
            else if (dragTile.CompareTag("SpecialTile"))
            {
                Debug.Log($"^5.1A.2 {name} (special tile) drop to teammate {dragTile.interactTeammate.name} (msg from tile)");
            }
            
        }
        else
        {
            if (dragTile.CompareTag("NormalTile"))
            {
                Debug.Log($"^5.2A.1 {name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text} drop to {dragTile.interactTile.name}");
            }
            else if (dragTile.CompareTag("SpecialTile"))
            {
                Debug.Log($"^5.2A.2 {name} (special tile) drop to {dragTile.interactTile.name}");
            }

            // Detect drag tile -> other tile
            if (dragTile.inTile == true)
            {
                if (dragTile.interactTile.CompareTag("NormalTile"))
                {
                    Debug.Log($"^5.2B.1 {name} received {dragTile.interactTile.name} (normal tile) {dragTile.interactTile.transform.GetChild(2).GetComponent<TMP_Text>().text}");
                }
                else if (dragTile.interactTile.CompareTag("SpecialTile"))
                {
                    Debug.Log($"^5.2B.2 {name} received {dragTile.interactTile.name} (special tile)");
                }

                // DragTile itself is answer
                if (dragTile.isAnswer == true)
                {
                    if (dragTile.CompareTag("NormalTile") && dragTile.interactTile.CompareTag("NormalTile"))
                    {
                        Debug.Log($"^5.3A.1 merge the correct answer {dragTile.name} (normal tile) ({dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text}) -> wrong answer {dragTile.interactTile.name} (normal tile) ({dragTile.interactTile.transform.GetChild(2).GetComponent<TMP_Text>().text})");
                    }
                    else if (dragTile.CompareTag("NormalTile") && dragTile.interactTile.CompareTag("SpecialTile"))
                    {
                        Debug.Log($"^5.3A.2 merge the correct answer {dragTile.name} (normal tile) ({dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text}) -> wrong answer {dragTile.interactTile.name} (special tile)");
                    }
                    // Which should not happen, spceial tile cannot be answer?
                    else if (dragTile.CompareTag("SpecialTile") && dragTile.interactTile.CompareTag("NormalTile"))
                    {
                        Debug.Log($"^5.3A.3 merge the correct answer {dragTile.name} (special tile) -> wrong answer {dragTile.interactTile.name} (normal tile) ({dragTile.interactTile.transform.GetChild(2).GetComponent<TMP_Text>().text})");
                    }
                    else if (dragTile.CompareTag("SpecialTile") && dragTile.interactTile.CompareTag("SpecialTile"))
                    {
                        Debug.Log($"^5.3A.4 merge the correct answer {dragTile.name} (special tile) -> wrong answer {dragTile.interactTile.name} (special tile)");
                    }
                    // Some punishment here
                    player.TakeDamage(player.maxHp.value * 0.15f);

                    // Reset new round
                    //Debug.Log("dragTile.interactTile: " + dragTile.interactTile.name);
                    dragTile.interactTile.toBeDestroyed = true;
                    //board.tileCell[dragTile.interactTile.tileCellPosition] = -1;
                    //Debug.Log("dragTile: " + dragTile.name);
                    dragTile.toBeDestroyed = true;

                    // Only setting tile has moved here but not checking afterwards, since they will getting destroyed
                    dragTile.isMoved = true;
                    player.isActioned = true;

                    //board.tileCell[dragTile.tileCellPosition] = -1;
                    board.UpdateTileCell();
                    //board.DrawAnswer();
                }
                else
                {
                    // Other tile is not answer
                    if (dragTile.interactTile.isAnswer == false)
                    {
                        // Split them as new function as MergeTile ***************************************************
                        // Check tileLevel
                        if (dragTile.tileLevel == dragTile.interactTile.tileLevel)
                        {
                            if (dragTile.CompareTag("NormalTile") && dragTile.interactTile.CompareTag("NormalTile"))
                            {
                                Debug.Log($"^5.3B.1 merging correctly {dragTile.name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text} -> {dragTile.interactTile.name} (normal tile) {dragTile.interactTile.transform.GetChild(2).GetComponent<TMP_Text>().text}");
                            }
                            else if (dragTile.CompareTag("NormalTile") && dragTile.interactTile.CompareTag("SpecialTile"))
                            {
                                Debug.Log($"^5.3B.2 merging correctly {dragTile.name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text} -> {dragTile.interactTile.name} (special tile)");
                            }
                            else if (dragTile.CompareTag("SpecialTile") && dragTile.interactTile.CompareTag("NormalTile"))
                            {
                                Debug.Log($"^5.3B.3 merging correctly {dragTile.name} (special tile) -> {dragTile.interactTile.name} (normal tile) {dragTile.interactTile.transform.GetChild(2).GetComponent<TMP_Text>().text}");
                            }
                            else if (dragTile.CompareTag("SpecialTile") && dragTile.interactTile.CompareTag("SpecialTile"))
                            {
                                Debug.Log($"^5.3B.4 merging correctly {dragTile.name} (special tile) -> {dragTile.interactTile.name} (special tile)");
                            }

                            // Upgrade the tile level that merge into
                            TileUpgrade(dragTile.interactTile);

                            //Debug.Log("dragTile: " + dragTile.name);
                            dragTile.toBeDestroyed = true;

                            // Only setting tile has moved here but not checking afterwards, since they will getting destroyed
                            dragTile.isMoved = true;
                            player.isActioned = true;;

                            //board.tileCell[dragTile.tileCellPosition] = -1;
                            board.UpdateTileCell();
                        }
                        else
                        {
                            Debug.Log("^5.3D You merge the wrong tileLevel of answer tile!");
                        }
                    }
                    // Other tile is answer
                    else
                    {
                        if (dragTile.CompareTag("NormalTile") && dragTile.interactTile.CompareTag("NormalTile"))
                        {
                            Debug.Log($"^5.3E.1 merge the wrong answer {dragTile.name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text} -> correct answer {dragTile.interactTile.name} (normal tile) {dragTile.interactTile.transform.GetChild(2).GetComponent<TMP_Text>().text}");
                        }
                        else if (dragTile.CompareTag("NormalTile") && dragTile.interactTile.CompareTag("SpecialTile"))
                        {
                            Debug.Log($"^5.3E.2 merge the wrong answer {dragTile.name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text} -> correct answer {dragTile.interactTile.name} (special tile)");
                        }
                        else if (dragTile.CompareTag("SpecialTile") && dragTile.interactTile.CompareTag("NormalTile"))
                        {
                            Debug.Log($"^5.3E.3 merge the wrong answer {dragTile.name} (special tile) -> correct answer correct answer {dragTile.interactTile.name} (normal tile) {dragTile.interactTile.transform.GetChild(2).GetComponent<TMP_Text>().text}");
                        }
                        else if (dragTile.CompareTag("SpecialTile") && dragTile.interactTile.CompareTag("SpecialTile"))
                        {
                            Debug.Log($"^5.3E.4 merge the wrong answer {dragTile.name} (special tile) -> correct answer correct answer {dragTile.interactTile.name} (special tile)");
                        }
                        // Some punishment here
                        player.TakeDamage(player.maxHp.value * 0.15f);

                        // Reset new round
                        //Debug.Log("dragTile.interactTile: " + dragTile.interactTile.name);
                        dragTile.interactTile.toBeDestroyed = true;
                        //Debug.Log("dragTile: " + dragTile.name);
                        dragTile.toBeDestroyed = true;

                        // Only setting tile has moved here but not checking afterwards, since they will getting destroyed
                        dragTile.isMoved = true;
                        player.isActioned = true;

                        board.UpdateTileCell();
                        //board.DrawAnswer();
                    }
                }
            }
        }

        Debug.Log($"{name} Tile.OnDrop (end)");
        */
    }

    // End drag, called only once, for reset. If just click won't be triggered //
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"{name} Tile.OnEndDrag (start)");

        Debug.Log("^6 end drag the tile");
        currentState = TileState.State.EndDrag;
        tileStatusProcessed += 1;

        EndDrag(this);

        Debug.Log($"{name} Tile.OnEndDrag (end)");
    }
    #endregion
}