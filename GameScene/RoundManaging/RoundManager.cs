﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BlueAndWhite.Objects;
using BlueAndWhite.Entities;
using BlueAndWhite.UIManaging;
using BlueAndWhite.Buttons;

namespace BlueAndWhite.RoundManaging
{
    public class RoundManager : MonoBehaviour
    {
        public RoundData roundData;
        public Board board;
        public Player player;
        public UIManage uiManage;

        //public bool isWin = false;
        //public bool isDead = false;
        //public bool isGameOver = false;

        // Pls create a object script for it
        //public GameObject popUpWindow;

        void Awake()
        {
            Debug.Log($"RoundManager.Awake (start)");

            //Debug.Log("I am running!");
            //disable multi touch
            Input.multiTouchEnabled = false;

            roundData = gameObject.GetComponent<RoundData>();
            board = GameObject.Find("Board").GetComponent<Board>();
            player = gameObject.GetComponent<Player>();
            uiManage = GameObject.Find("UI Manager").GetComponent<UIManage>();

            //should be contained in a reset function
            //isWin = false;
            //isGameOver = false;
            //isDead = false;

            Debug.Log($"RoundManager.Awake (end)");
        }

        void OnEnable()
        {
            Debug.Log($"RoundManager.OnEnable (start)");

            // Subscribe to the game events and listen
            RoundData.OnAllMobDefectedEvent -= GameOver;
            RoundData.OnAllMobDefectedEvent += GameOver;
            Player.OnDefeatedEvent -= GameOver;
            Player.OnDefeatedEvent += GameOver;

            Debug.Log($"RoundManager.OnEnable (end)");
        }
        
        void Start()
        {
            Debug.Log($"RoundManager.Start (start)");

            roundData.InitializeData();

            Debug.Log($"RoundManager.Start (end)");
        }
        void Update()
        {

        }

        void OnDisable()
        {
            Debug.Log($"RoundManager.OnDisable (start)");

            // Unsubscribe to the game events
            RoundData.OnAllMobDefectedEvent -= GameOver;
            Player.OnDefeatedEvent -= GameOver;

            Debug.Log($"RoundManager.OnDisable (end)");
        }

        void OnDestroy() 
        {
            Debug.Log($"RoundManager.OnDestroy (start)");
            Debug.Log($"RoundManager.OnDestroy (end)");
        }

        public void GameOver(string status)
        {
            Debug.Log($"RoundManager.GameOver (start)");

            switch (status)
            {
                case "dead":
                    Debug.Log("GG, you lose");
                    Player.OnDefeatedEvent -= GameOver;
                    uiManage.SpawnPopup("gameover", 4, () => SettingBtn.StopAdventure(), () => uiManage.currentPopup.DestroyPopup(uiManage.currentPopup));
                    
                    break;

                case "win":
                    Debug.Log("GG, you win");
                    RoundData.OnAllMobDefectedEvent -= GameOver;
                    uiManage.SpawnPopup("gameover", 5, () => SettingBtn.StopAdventure(), () => uiManage.currentPopup.DestroyPopup(uiManage.currentPopup));

                    break;
            }

            Debug.Log($"RoundManager.GameOver (end)");
        }
    }
}
