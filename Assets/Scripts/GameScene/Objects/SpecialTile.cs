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
        player = GameObject.Find("Round Manager").GetComponent<Player>();

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

        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();
        board = GameObject.Find("Board").GetComponent<Board>();
        player = GameObject.Find("Round Manager").GetComponent<Player>();

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
        Board.OnEndTurnEvent += OnNewTurn;

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
        Board.OnEndTurnEvent -= OnNewTurn;

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
        player.SetDragTile(dragTile);
        //player.tile = dragTile;

        // Detect drag tile-> slot
        if (dragTile.inSlot == true)
        {
            Debug.Log($"^5.1A.2 {name} (special tile) drop to teammate {dragTile.interactTeammate.name} (msg from tile)");
        }
        else
        {

            Debug.Log($"^5.2A.2 {name} (special tile) drop to {dragTile.interactTile.name}");

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
                    // Which should not happen, spceial tile cannot be answer?
                    if (dragTile.interactTile.CompareTag("NormalTile"))
                    {
                        Debug.Log($"^5.3A.3 merge the correct answer {dragTile.name} (special tile) -> wrong answer {dragTile.interactTile.name} (normal tile) ({dragTile.interactTile.transform.GetChild(2).GetComponent<TMP_Text>().text})");
                    }
                    else if (dragTile.interactTile.CompareTag("SpecialTile"))
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
                            if (dragTile.interactTile.CompareTag("NormalTile"))
                            {
                                Debug.Log($"^5.3B.3 merging correctly {dragTile.name} (special tile) -> {dragTile.interactTile.name} (normal tile) {dragTile.interactTile.transform.GetChild(2).GetComponent<TMP_Text>().text}");
                            }
                            else if (dragTile.interactTile.CompareTag("SpecialTile"))
                            {
                                Debug.Log($"^5.3B.4 merging correctly {dragTile.name} (special tile) -> {dragTile.interactTile.name} (special tile)");
                            }

                            // Upgrade the tile level that merge into
                            TileUpgrade(dragTile.interactTile);

                            //Debug.Log("dragTile: " + dragTile.name);
                            dragTile.toBeDestroyed = true;

                            // Only setting tile has moved here but not checking afterwards, since they will getting destroyed
                            dragTile.isMoved = true;

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
                        if (dragTile.interactTile.CompareTag("NormalTile"))
                        {
                            Debug.Log($"^5.3E.3 merge the wrong answer {dragTile.name} (special tile) -> correct answer correct answer {dragTile.interactTile.name} (normal tile) {dragTile.interactTile.transform.GetChild(2).GetComponent<TMP_Text>().text}");
                        }
                        else if (dragTile.interactTile.CompareTag("SpecialTile"))
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

                        board.UpdateTileCell();
                        //board.DrawAnswer();
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
