using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        Debug.Log($"{Time.time} UIManage.Awake (start)");

        // Not ideal place
        //gameData = GameObject.Find("Round Manager").GetComponent<RoundData>();
        //popupFactory = gameObject.GetComponent<PopupFactory>();

        Debug.Log($"{Time.time} UIManage.Awake (end)");
    }
    
    void OnEnable()
    {
        Debug.Log($"{Time.time} UIManage.OnEnable (start)");
        Debug.Log($"{Time.time} UIManage.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} UIManage.Start (start)");

        gameData = GameObject.Find("Round Manager").GetComponent<RoundData>();
        popupFactory = gameObject.GetComponent<PopupFactory>();

        Debug.Log($"{Time.time} UIManage.Start (end)");
    }
    void Update()
    {

    }

    void OnDisable()
    {
        Debug.Log($"{Time.time} UIManage.OnDisable (start)");
        Debug.Log($"{Time.time} UIManage.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} UIManage.OnDestroy (start)");
        Debug.Log($"{Time.time} UIManage.OnDestroy (end)");
    }

    // Change to Enum if possible
    public void SpawnPopup(string size, int caseRefID)
    {
        Debug.Log($"{Time.time} UIManage.SpawnPopup (start)");

        currentPopup = popupFactory.CreatePopup(popupPrefabSmall, this.gameObject, caseRefID);
        Debug.Log($"{Time.time} popup just spawned: {currentPopup.name}");

        Debug.Log($"{Time.time} UIManage.SpawnPopup (end)");
    }
    public void SpawnPopup(string size, int caseRefID, UnityAction buttonAction1)
    {
        Debug.Log($"{Time.time} UIManage.SpawnPopup (start)");

        currentPopup = popupFactory.CreatePopup(popupPrefabMedium, this.gameObject, caseRefID, buttonAction1);
        Debug.Log($"{Time.time} popup just spawned: {currentPopup.name}");

        Debug.Log($"{Time.time} UIManage.SpawnPopup (end)");
    }
    public void SpawnPopup(string size, int caseRefID, UnityAction buttonAction1, UnityAction buttonAction2)
    {
        Debug.Log($"{Time.time} UIManage.SpawnPopup (start)");

        switch (size)
        {
            case "medium":
                currentPopup = popupFactory.CreatePopup(popupPrefabMedium, this.gameObject, caseRefID, buttonAction1, buttonAction2);
                break;

            case "gameover":
                currentPopup = popupFactory.CreatePopup(popupPrefabGameover, this.gameObject, caseRefID, buttonAction1, buttonAction2);
                break;
        }
        Debug.Log($"{Time.time} popup just spawned: {currentPopup.name}");

        Debug.Log($"{Time.time} UIManage.SpawnPopup (end)");
    }
}
