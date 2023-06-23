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
        Debug.Log($"{Time.time} {name} NormalTile.Awake (start)");

        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();
        board = GameObject.Find("Board").GetComponent<Board>();
        canvasGroup = GetComponent<CanvasGroup>();

        tile = this.gameObject;
        tileText = tile.transform.GetChild(2).GetComponent<TMP_Text>();
        tileLevelText = tile.transform.GetChild(3).GetComponent<TMP_Text>();
        tileEffectIcon = tile.transform.GetChild(4).transform.GetChild(0).GetComponent<Image>();
        tileEffectTurnsText = tile.transform.GetChild(4).transform.GetChild(1).GetComponent<TMP_Text>();

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

        currentvalueModifier = roundData.player.attackPoint.GetStatValue() * baseValueModifier;

        tileLevelText.gameObject.SetActive(true);
        tileEffectParent.gameObject.SetActive(false);
        // Set false for answer cheat here

        Debug.Log($"{Time.time} {name} NormalTile.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} {name} NormalTile.OnEnable (start)");
        
        // Subscribe to the game events and listen
        //Board.OnEndTurnEvent += OnNewTurn;

        Debug.Log($"{Time.time} {name} NormalTile.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} {name} NormalTile.Start (start)");
        Debug.Log($"{Time.time} {name} NormalTile.Start (end)");
    }

    void Update()
    {
        
    }
    
    void OnDisable()
    {
        Debug.Log($"{Time.time} {name} NormalTile.OnDisable (start)");
        Debug.Log($"{Time.time} {name} NormalTile.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} {name} NormalTile.OnDestroy (start)");
        Debug.Log($"{Time.time} {name} NormalTile.OnDestroy (end)");
    }
    #endregion

    #region Tile functions
    // To let normal tile not merge, merge is for special tile only
    // For tile merge, receive after onpointerup (AS A DROP RECEIVER) //
    public override void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"{Time.time} {name} NormalTile.OnDrop (override Entities.OnDrop) (start)");

        Debug.Log($"{Time.time} ^5 input drop the tile");

        Tile dragTile = eventData.pointerDrag.GetComponent<Tile>();
        currentState = TileState.State.Processing;
        tileStatusProcessed += 1;

        // ****************************************
        roundData.player.SetDragTile(dragTile);
        //player.tile = dragTile;

        // Detect drag tile -> slot
        if (dragTile.inSlot == true)
        {
            // Do nothing
        }
        else
        {
            // Detect drag tile -> other tile
            if (dragTile.inTile == true)
            {
                if (CompareTag("NormalTile"))
                {
                    Debug.Log($"{Time.time} ^5.1B.1 {name} received {dragTile.name} (normal tile) {transform.GetChild(2).GetComponent<TMP_Text>().text}");
                }
                else if (CompareTag("SpecialTile"))
                {
                    Debug.Log($"{Time.time} ^5.1B.2 {name} received {dragTile.name} (special tile)");
                }

                // DragTile itself is answer
                if (dragTile.isAnswer == true)
                {
                    if (dragTile.CompareTag("NormalTile"))
                    {
                        Debug.Log($"{Time.time} ^5.2A.1 merge the correct answer {dragTile.name} (normal tile) ({dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text}) -> wrong answer {name} (normal tile) ({transform.GetChild(2).GetComponent<TMP_Text>().text})");
                    }
                    // Which should not happen
                    else if (dragTile.CompareTag("SpecialTile"))
                    {
                        Debug.Log($"{Time.time} ^5.2A.2 merge the correct answer {dragTile.name} (special tile) -> wrong answer {name} (normal tile) ({transform.GetChild(2).GetComponent<TMP_Text>().text})");
                    }

                    // Some punishment here
                    roundData.player.TakeDamage((float)0.15 * roundData.player.currentMaxHealthValue);
                    roundData.currentPowerScore -= GetOutPutPower() * 2;

                    // Reset new round
                    //Debug.Log("dragTile.interactTile: " + dragTile.interactTile.name);
                    toBeDestroyed = true;
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
                    // The tile is answer
                    if (isAnswer == true)
                    {
                        if (dragTile.CompareTag("NormalTile"))
                        {
                            Debug.Log($"{Time.time} ^5.2B.1 merge the wrong answer {dragTile.name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text} -> correct answer {name} (normal tile) {transform.GetChild(2).GetComponent<TMP_Text>().text}");
                        }
                        else if (dragTile.CompareTag("SpecialTile"))
                        {
                            Debug.Log($"{Time.time} ^5.2B.2 merge the wrong answer {dragTile.name} (special tile) -> correct answer {name} (normal tile)");
                        }

                        // Some punishment here
                        roundData.player.TakeDamage((float)0.15 * roundData.player.currentMaxHealthValue);
                        roundData.currentPowerScore -= GetOutPutPower() * 2;

                        // Reset new round
                        //Debug.Log("dragTile.interactTile: " + dragTile.interactTile.name);
                        toBeDestroyed = true;
                        //Debug.Log("dragTile: " + dragTile.name);
                        dragTile.toBeDestroyed = true;

                        // Only setting tile has moved here but not checking afterwards, since they will getting destroyed
                        dragTile.isMoved = true;
                        roundData.player.isActioned = true;

                        board.UpdateTileCell();
                        //board.DrawAnswer();
                    }

                    // Correct way to destroy non answer tiles together
                    // The tile is not answer
                    else 
                    {
                        if (dragTile.CompareTag("NormalTile"))
                        {
                            Debug.Log($"{Time.time} ^5.2C.1 merging correctly {dragTile.name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text} -> {name} (normal tile) {transform.GetChild(2).GetComponent<TMP_Text>().text}");
                        }
                        else if (dragTile.CompareTag("SpecialTile"))
                        {
                            Debug.Log($"{Time.time} ^5.2C.2 merging correctly {dragTile.name} (special tile) -> {name} (normal tile)");
                        }

                        //Debug.Log("dragTile: " + dragTile.name);
                        toBeDestroyed = true;
                        dragTile.toBeDestroyed = true;

                        // Only setting tile has moved here but not checking afterwards, since they will getting destroyed
                        dragTile.isMoved = true;
                        roundData.player.isActioned = true;

                        // Contribute the score to board
                        roundData.currentPowerScore += GetOutPutPower();

                        //board.tileCell[dragTile.tileCellPosition] = -1;
                        board.UpdateTileCell();
                    }
                }
            }
        }

        Debug.Log($"{Time.time} {name} NormalTile.OnDrop (override Entities.OnDrop) (end)");
    }
    public override void OnBeforeTurnStart()
    {
        base.OnBeforeTurnStart();
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();

        if (currentTileEffect != null)
        {
            currentTileEffect.tileEffectRemainingTurns -= 1;
            tileEffectTurnsText.text = currentTileEffect.tileEffectRemainingTurns.ToString();
        }
    }
    #endregion
}
