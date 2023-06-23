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

    #region Scripts
    private RoundData roundData;
    #endregion

    #region Game object references
    public Dictionary<int, Popup> currentPopups = new Dictionary<int, Popup>();
    public GameObject popupPrefabTiny;
    public GameObject popupPrefabSmall;
    public GameObject popupPrefabMedium;
    public GameObject popupPrefabGameover;
    private PopupFactory popupFactory;
    #endregion

    #region UI Manager data
    public int popupCounts = -1;
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{Time.time} UIManage.Awake (start)");

        // Not ideal place
        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();
        popupFactory = gameObject.GetComponent<PopupFactory>();

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
    #endregion

    #region UI Manager functions
    // Change to Enum if possible

    // For battling msg
    public void SpawnPopup(string msg, string scheme)
    {
        Debug.Log($"{Time.time} UIManage.SpawnPopup (start)");

        // Temp
        popupCounts += 1;
        Popup popup = popupFactory.CreatePopup(popupPrefabTiny, this.gameObject, msg);

        switch (scheme)
        {
            case "normal":
                popup.bodyTextBox.GetComponent<Image>().color = new Color32(230, 250, 100, 255);
                //popup.bodyText.color = new Color32(230, 250, 100, 255);
                break;

            case "emergency":
                popup.bodyTextBox.GetComponent<Image>().color = new Color32(250, 80, 80, 255);
                //popup.bodyText.color = new Color32(230, 250, 100, 255);
                break;

            case "special":
                popup.bodyTextBox.GetComponent<Image>().color = new Color32(250, 250, 100, 255);
                //popup.bodyText.color = new Color32(230, 250, 100, 255);
                break;

        }
        
        currentPopups.Add(popup.popupID, popup);
        Debug.Log($"{Time.time} popup just spawned: {currentPopups[popup.popupID].name}");

        // Add time for displaying the popup
        roundData.currentTurnDuration += popup.popupDuration;

        // Need to be self-destroyed after 2s(default = 2s from popup class)
        this.Wait(popup.popupDuration, () => currentPopups[popup.popupID].DestroyPopup(popup.popupID));

        Debug.Log($"{Time.time} UIManage.SpawnPopup (end)");
    }

    // Other cases
    public void SpawnPopup(int caseRefID)
    {
        Debug.Log($"{Time.time} UIManage.SpawnPopup (start)");

        // Temp
        popupCounts += 1;
        Popup popup = popupFactory.CreatePopup(popupPrefabSmall, this.gameObject, caseRefID);
        //popup.popupID = popupCounts;
        currentPopups.Add(popup.popupID, popup);
        Debug.Log($"{Time.time} popup just spawned: {currentPopups[popup.popupID].name}");

        Debug.Log($"{Time.time} UIManage.SpawnPopup (end)");
    }
    public void SpawnPopup(int caseRefID, UnityAction buttonAction1)
    {
        Debug.Log($"{Time.time} UIManage.SpawnPopup (start)");

        // Temp
        popupCounts += 1;
        Popup popup = popupFactory.CreatePopup(popupPrefabMedium, this.gameObject, caseRefID, buttonAction1);
        //popup.popupID = popupCounts;
        currentPopups.Add(popup.popupID, popup);
        Debug.Log($"{Time.time} popup just spawned: {currentPopups[popup.popupID].name}");

        Debug.Log($"{Time.time} UIManage.SpawnPopup (end)");
    }
    public void SpawnPopup(string size, int caseRefID, UnityAction buttonAction1, UnityAction buttonAction2)
    {
        Debug.Log($"{Time.time} UIManage.SpawnPopup (start)");

        // Temp
        popupCounts += 1;
        GameObject popupPrefab = new GameObject();
        switch (size)
        {
            case "medium":
                popupPrefab = popupPrefabMedium;
                break;

            case "gameover":
                popupPrefab = popupPrefabGameover;
                break;
        }
        Popup popup = popupFactory.CreatePopup(popupPrefab, this.gameObject, caseRefID, buttonAction1, buttonAction2);
        //popup.popupID = popupCounts;
        currentPopups.Add(popup.popupID, popup);
        Debug.Log($"{Time.time} popup just spawned: {currentPopups[popup.popupID].name}");

        Debug.Log($"{Time.time} UIManage.SpawnPopup (end)");
    }

    public int GetPopupCounts()
    {
        Debug.Log($"{Time.time} UIManage.GetPopupCounts (start)");

        Debug.Log($"{Time.time} UIManage.GetPopupCounts, return popupCounts(local var): {popupCounts} (end)");
        return popupCounts;
    }
    #endregion
}
