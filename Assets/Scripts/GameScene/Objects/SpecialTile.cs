using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BlueAndWhite.RoundManaging;
using BlueAndWhite.Entities;
using TMPro;

namespace BlueAndWhite.Objects
{
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
}
