using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SpecialTile : Tile
{
    #region Game object references
    public Image tileIcon;
    #endregion

    #region Tile data
    public TileEffect tileEffect;
    public string tileIconName;
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{name} SpecialTile.Awake (start)");

        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();
        board = GameObject.Find("Board").GetComponent<Board>();

        //Debug.Log("I am running!");
        canvasGroup = GetComponent<CanvasGroup>();
        //mainCamera = Camera.main;

        tile = this.gameObject;
        tileIcon = tile.transform.GetChild(2).GetComponent<Image>();
        tileLevelText = tile.transform.GetChild(3).GetComponent<TMP_Text>();
        tileEffectAIcon = tile.transform.GetChild(4).transform.GetChild(0).GetComponent<Image>();
        tileEffectBIcon = tile.transform.GetChild(4).transform.GetChild(1).GetComponent<Image>();
        tileEffectATurnsText = tile.transform.GetChild(5).GetComponent<TMP_Text>();
        tileEffectBTurnsText = tile.transform.GetChild(6).GetComponent<TMP_Text>();

        tileLevelText.gameObject.SetActive(true);
        tileEffectAIcon.gameObject.SetActive(false);
        tileEffectBIcon.gameObject.SetActive(false);
        tileEffectATurnsText.gameObject.SetActive(false);
        tileEffectBTurnsText.gameObject.SetActive(false);

        tileRect = this.GetComponent<RectTransform>();
        tileLocation = this.transform.position;
        //tileCellPosition = this.transform.GetSiblingIndex();

        //tileLevel = Convert.ToInt32(tileLevelText.text);
        //tileEffectATurn = Convert.ToInt32(tileEffectATurnText.text);
        //tileEffectBTurn = Convert.ToInt32(tileEffectBTurnText.text);

        // Initialise the data of the tile
        //spawnRate = board.specialTileSpawnRate;
        tileLevel = 0;
        //tileSpawnedTurns = 當前;
        tileExistedTurns = 1;
        tileSpawnedTurns = 0;

        // For testing
        //tileEffectATurn = 20;
        //tileEffectBTurn = 20;

        tileLevelText.text = tileLevel.ToString();
        //tileEffectATurnText.text = tileEffectATurn.ToString();
        //tileEffectBTurnText.text = tileEffectBTurn.ToString();

        // Make sure tilePositionRecords is empty

        //score = (int)player.attackPoint;
        baseValueModifier = valueModifierArr[0];

        // Set false for answer cheat here

        Debug.Log($"{name} SpecialTile.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{name} SpecialTile.OnEnable (start)");
        
        // Subscribe to the game events and listen
        //Board.OnEndTurnEvent += OnNewTurn;

        Debug.Log($"{name} SpecialTile.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{name} SpecialTile.Start (start)");
        Debug.Log($"{name} SpecialTile.Start (end)");
    }

    void Update()
    {
        
    }
    
    void OnDisable()
    {
        Debug.Log($"{name} SpecialTile.OnDisable (start)");
        
        // Unubscribe to the game events and listen
        //Board.OnEndTurnEvent -= OnNewTurn;

        Debug.Log($"{name} SpecialTile.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{name} SpecialTile.OnDestroy (start)");
        Debug.Log($"{name} SpecialTile.OnDestroy (end)");
    }
    #endregion

    #region Tile functions
    // For tile merge, receive after onpointerup (AS A DROP RECEIVER) //
    public override void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"{name} SpecialTile.OnDrop (override Entities.OnDrop) (start)");

        Debug.Log("^5 input drop the tile");
        currentState = TileState.State.Processing;
        tileStatusProcessed += 1;

        // They are the same thing
        Tile dragTile = eventData.pointerDrag.GetComponent<Tile>();

        // ****************************************
        roundData.player.SetDragTile(dragTile);
        //player.tile = dragTile;

        // Detect drag tile-> slot
        if (dragTile.inSlot == true)
        {
            
        }
        else
        {
            // Detect drag tile -> other tile
            if (dragTile.inTile == true)
            {
                if (CompareTag("NormalTile"))
                {
                    Debug.Log($"^5.1B.3 {name} received {dragTile.name} (normal tile) {dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text}");
                }
                else if (CompareTag("SpecialTile"))
                {
                    Debug.Log($"^5.1B.4 {name} received {dragTile.name} (special tile)");
                }

                // DragTile itself is answer
                if (dragTile.isAnswer == true)
                {
                    if (dragTile.CompareTag("NormalTile"))
                    {
                        Debug.Log($"^5.2A.3 merge the correct answer {dragTile.name} (normal tile) ({dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text}) -> wrong answer {name} (special tile)");
                    }
                    // Which should not happen
                    else if (dragTile.CompareTag("SpecialTile"))
                    {
                        Debug.Log($"^5.2A.4 merge the correct answer {dragTile.name} (special tile) -> wrong answer {name} (special tile)");
                    }

                    // Some punishment here
                    roundData.player.TakeDamage(roundData.player.currentMaxHp.value * 0.15f);

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
                else // Dragtile is not answer
                {
                    // The tile is answer
                    if (isAnswer == true)
                    {
                        if (dragTile.CompareTag("NormalTile"))
                        {
                            Debug.Log($"^5.2B.3 merge the wrong answer {dragTile.name} (normal tile) ({dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text}) -> correct answer {name} (special tile)");
                        }
                        else if (dragTile.CompareTag("SpecialTile"))
                        {
                            Debug.Log($"^5.2B.4 merge the wrong answer {dragTile.name} (special tile) -> correct answer {name} (special tile)");
                        }
                        // Some punishment here
                        roundData.player.TakeDamage(roundData.player.currentMaxHp.value * 0.15f);

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

                    // There should be one new function as MergeTile ***************************************************
                    // the tile is not answer
                    else 
                    {
                        // Check tileLevel
                        if (dragTile.tileLevel == tileLevel)
                        {
                            if (dragTile.CompareTag("NormalTile"))
                            {
                                Debug.Log($"^5.2C.3 merging correctly {dragTile.name} (normal tile) ({dragTile.transform.GetChild(2).GetComponent<TMP_Text>().text}) -> {name} (special tile)");
                            }
                            else if (dragTile.CompareTag("SpecialTile"))
                            {
                                Debug.Log($"^5.2C.4 merging correctly {dragTile.name} (special tile) -> {name} (special tile)");
                            }

                            // Upgrade the tile level that merge into
                            TileUpgrade(dragTile.interactTile);

                            //Debug.Log("dragTile: " + dragTile.name);
                            dragTile.toBeDestroyed = true;

                            // Only setting tile has moved here but not checking afterwards, since they will getting destroyed
                            dragTile.isMoved = true;
                            roundData.player.isActioned = true;

                            //board.tileCell[dragTile.tileCellPosition] = -1;
                            board.UpdateTileCell();
                        }
                        else
                        {
                            Debug.Log("^5.3B You merge two different tile level tiles together!");
                        }
                    }
                }
            }
        }

        Debug.Log($"{name} SpecialTile.OnDrop (override Entities.OnDrop) (end)");
    }

    public void OnNewTurn()
    {
        Debug.Log($"{name} SpecialTile.OnNewTurn (start)");

        tileExistedTurns += 1;

        tileEffectARemainingTurns -= 1;
        tileEffectATurnsText.text = tileEffectARemainingTurns.ToString();
        
        tileEffectBRemainingTurns -= 1;
        tileEffectBTurnsText.text = tileEffectBRemainingTurns.ToString();

        Debug.Log($"{name} SpecialTile.OnNewTurn (end)");
    }
    #endregion
}
