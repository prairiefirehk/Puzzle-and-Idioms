using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using BlueAndWhite.Objects;
using BlueAndWhite.Buttons;
using BlueAndWhite.DataManaging;
using BlueAndWhite.RoundManaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlueAndWhite.UIManaging
{
    public class UIManage : MonoBehaviour
    {

        // Popup window data
        private RoundData gameData;

        // Current popup
        public Popup currentPopup;

        //// Popup prefabs////
        public GameObject popupPrefabSmall;
        public GameObject popupPrefabMedium;
        public GameObject popupPrefabGameover;

        // Test
        private PopupFactory popupFactory;

        void Awake()
        {
            Debug.Log($"UIManage.Awake (start)");

            // Not ideal place
            //gameData = GameObject.Find("Round Manager").GetComponent<RoundData>();
            //popupFactory = gameObject.GetComponent<PopupFactory>();

            Debug.Log($"UIManage.Awake (end)");
        }
        
        void OnEnable()
        {
            Debug.Log($"UIManage.OnEnable (start)");
            Debug.Log($"UIManage.OnEnable (end)");
        }
        
        void Start()
        {
            Debug.Log($"UIManage.Start (start)");

            gameData = GameObject.Find("Round Manager").GetComponent<RoundData>();
            popupFactory = gameObject.GetComponent<PopupFactory>();

            Debug.Log($"UIManage.Start (end)");
        }
        void Update()
        {

        }

        void OnDisable()
        {
            Debug.Log($"UIManage.OnDisable (start)");
            Debug.Log($"UIManage.OnDisable (end)");
        }

        void OnDestroy() 
        {
            Debug.Log($"UIManage.OnDestroy (start)");
            Debug.Log($"UIManage.OnDestroy (end)");
        }

        // Change to Enum if possible
        public void SpawnPopup(string size, int caseRefID)
        {
            Debug.Log($"UIManage.SpawnPopup (start)");

            currentPopup = popupFactory.CreatePopup(popupPrefabSmall, this.gameObject, caseRefID);
            Debug.Log($"popup just spawned: {currentPopup.name}");

            Debug.Log($"UIManage.SpawnPopup (end)");
        }
        public void SpawnPopup(string size, int caseRefID, UnityAction buttonAction1)
        {
            Debug.Log($"UIManage.SpawnPopup (start)");

            currentPopup = popupFactory.CreatePopup(popupPrefabMedium, this.gameObject, caseRefID, buttonAction1);
            Debug.Log($"popup just spawned: {currentPopup.name}");

            Debug.Log($"UIManage.SpawnPopup (end)");
        }
        public void SpawnPopup(string size, int caseRefID, UnityAction buttonAction1, UnityAction buttonAction2)
        {
            Debug.Log($"UIManage.SpawnPopup (start)");

            switch (size)
            {
                case "medium":
                    currentPopup = popupFactory.CreatePopup(popupPrefabMedium, this.gameObject, caseRefID, buttonAction1, buttonAction2);
                    break;

                case "gameover":
                    currentPopup = popupFactory.CreatePopup(popupPrefabGameover, this.gameObject, caseRefID, buttonAction1, buttonAction2);
                    break;
            }
            Debug.Log($"popup just spawned: {currentPopup.name}");

            Debug.Log($"UIManage.SpawnPopup (end)");
        }
    }
}
