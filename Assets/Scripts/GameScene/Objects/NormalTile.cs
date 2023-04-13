using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class NormalTile : Tile
{
    #region Game object references
    public TMP_Text tileText;
    #endregion

    #region Tile data
    // The extracted idiom object
    public Idiom tileIdiom;
    public string tileIdiomName;
    // 0-1999 for now, the idiom id
    public int tileIdiomID;
    // 0-3, the idiom word order
    public int tileWordOrder;
    // The word
    public string tileWord;
    // 0-14657
    public int tileWordID;
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{name} NormalTile.Awake (start)");

        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();
        board = GameObject.Find("Board").GetComponent<Board>();
        canvasGroup = GetComponent<CanvasGroup>();

        tile = this.gameObject;
        tileText = tile.transform.GetChild(2).GetComponent<TMP_Text>();
        tileLevelText = tile.transform.GetChild(3).GetComponent<TMP_Text>();
        tileEffectAIcon = tile.transform.GetChild(4).transform.GetChild(0).GetComponent<Image>();
        tileEffectBIcon = tile.transform.GetChild(4).transform.GetChild(1).GetComponent<Image>();
        tileEffectATurnsText = tile.transform.GetChild(5).GetComponent<TMP_Text>();
        tileEffectBTurnsText = tile.transform.GetChild(6).GetComponent<TMP_Text>();

        tileRect = this.GetComponent<RectTransform>();
        tileLocation = this.transform.position;
        //tileCellPosition = this.transform.GetSiblingIndex();

        //tileLevel = Convert.ToInt32(tileLevelText.text);
        //tileEffectATurn = Convert.ToInt32(tileEffectATurnText.text);
        //tileEffectBTurn = Convert.ToInt32(tileEffectBTurnText.text);

        // Initialise the data of the tile, Instantiate data from factory ***WILL BE COVERED*** here!
        //spawnRate = board.normalTileSpawnRate;
        //tileLevel = 0;
        //tileSpawnedTurns = 當前;
        //tileExistedTurns = 1;
        tileSpawnedTurns = 0;

        // For testing
        //tileEffectATurn = 20;
        //tileEffectBTurn = 20;

        tileLevelText.text = tileLevel.ToString();
        //tileEffectATurnText.text = tileEffectATurn.ToString();
        //tileEffectBTurnText.text = tileEffectBTurn.ToString();

        // Make sure tilePositionRecords is empty
        baseValueModifier = valueModifierArr[0];
        //Debug.Log("I set the base score first: " + baseValueModifier);

        currentvalueModifier = roundData.player.attackPoint.value * baseValueModifier;

        tileLevelText.gameObject.SetActive(true);
        tileEffectAIcon.gameObject.SetActive(false);
        tileEffectBIcon.gameObject.SetActive(false);
        tileEffectATurnsText.gameObject.SetActive(false);
        tileEffectBTurnsText.gameObject.SetActive(false);
        // Set false for answer cheat here

        Debug.Log($"{name} NormalTile.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{name} NormalTile.OnEnable (start)");
        
        // Subscribe to the game events and listen
        //Board.OnEndTurnEvent += OnNewTurn;

        Debug.Log($"{name} NormalTile.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{name} NormalTile.Start (start)");
        Debug.Log($"{name} NormalTile.Start (end)");
    }

    void Update()
    {
        
    }
    
    void OnDisable()
    {
        Debug.Log($"{name} NormalTile.OnDisable (start)");
        
        // Unubscribe to the game events and listen
        //Board.OnEndTurnEvent -= OnNewTurn;

        Debug.Log($"{name} NormalTile.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{name} NormalTile.OnDestroy (start)");
        Debug.Log($"{name} NormalTile.OnDestroy (end)");
    }
    #endregion

    #region Tile functions
    // To let normal tile not merge, merge is for special tile only
    public override void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"{name} NormalTile.OnDrop (override Entities.OnDrop) (start)");

        Debug.Log("^5 input drop the tile");

        Tile dragTile = eventData.pointerDrag.GetComponent<Tile>();

        // ****************************************
        roundData.player.SetDragTile(dragTile);
        //player.tile = dragTile;

        // Detect drag tile-> slot
        if (dragTile.inSlot == true)
        {
            Debug.Log($"^5.1A.1 {name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text} drop to teammate {dragTile.interactTeammate.name} (msg from tile)");
        }
        else
        {
            Debug.Log($"^5.2A.1 {name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text} drop to {dragTile.interactTile.name}");

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
                    // Some punishment here
                    roundData.player.TakeDamage((float)0.15 * roundData.player.maxHp.value);

                    // Reset new round
                    //Debug.Log("dragTile.interactTile: " + dragTile.interactTile.name);
                    dragTile.interactTile.toBeDestroyed = true;
                    //board.tileCell[dragTile.interactTile.tileCellPosition] = -1;
                    //Debug.Log("dragTile: " + dragTile.name);
                    dragTile.toBeDestroyed = true;

                    // Only setting tile has moved here but not checking afterwards, since they will getting destroyed
                    dragTile.isMoved = true;
                    roundData.player.isActioned = true;

                    //board.tileCell[dragTile.tileCellPosition] = -1;
                    board.UpdateTileCell();
                    //board.DrawAnswer();
                }
                else
                {
                    // Other tile is answer
                    if (dragTile.interactTile.isAnswer == true)
                    {
                        if (dragTile.interactTile.CompareTag("NormalTile"))
                        {
                            Debug.Log($"^5.3E.1 merge the wrong answer {dragTile.name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text} -> correct answer {dragTile.interactTile.name} (normal tile) {dragTile.interactTile.transform.GetChild(2).GetComponent<TMP_Text>().text}");
                        }
                        else if (dragTile.interactTile.CompareTag("SpecialTile"))
                        {
                            Debug.Log($"^5.3E.2 merge the wrong answer {dragTile.name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text} -> correct answer {dragTile.interactTile.name} (special tile)");
                        }

                        // Some punishment here
                        roundData.player.TakeDamage((float)0.15 * roundData.player.maxHp.value);

                        // Reset new round
                        //Debug.Log("dragTile.interactTile: " + dragTile.interactTile.name);
                        dragTile.interactTile.toBeDestroyed = true;
                        //Debug.Log("dragTile: " + dragTile.name);
                        dragTile.toBeDestroyed = true;

                        // Only setting tile has moved here but not checking afterwards, since they will getting destroyed
                        dragTile.isMoved = true;
                        roundData.player.isActioned = true;

                        board.UpdateTileCell();
                        //board.DrawAnswer();
                    }
                    // Correct way to destroy non answer tiles together
                    else
                    {
                        if (dragTile.interactTile.CompareTag("NormalTile"))
                        {
                            Debug.Log($"^5.3B.1 merging correctly {dragTile.name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text} -> {dragTile.interactTile.name} (normal tile) {dragTile.interactTile.transform.GetChild(2).GetComponent<TMP_Text>().text}");
                        }
                        else if (dragTile.interactTile.CompareTag("SpecialTile"))
                        {
                            Debug.Log($"^5.3B.2 merging correctly {dragTile.name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text} -> {dragTile.interactTile.name} (special tile)");
                        }

                        //Debug.Log("dragTile: " + dragTile.name);
                        dragTile.toBeDestroyed = true;
                        dragTile.interactTile.toBeDestroyed = true;

                        // Only setting tile has moved here but not checking afterwards, since they will getting destroyed
                        dragTile.isMoved = true;
                        roundData.player.isActioned = true;

                        //board.tileCell[dragTile.tileCellPosition] = -1;
                        board.UpdateTileCell();
                    }
                }
            }
        }

        Debug.Log($"{name} NormalTile.OnDrop (override Entities.OnDrop) (end)");
    }

    public void OnNewTurn()
    {
        Debug.Log($"{name} NormalTile.OnNewTurn (start)");

        tileExistedTurns += 1;

        tileEffectARemainingTurns -= 1;
        tileEffectATurnsText.text = tileEffectARemainingTurns.ToString();
        
        tileEffectBRemainingTurns -= 1;
        tileEffectBTurnsText.text = tileEffectBRemainingTurns.ToString();

        Debug.Log($"{name} NormalTile.OnNewTurn (end)");
    }
    #endregion
}
