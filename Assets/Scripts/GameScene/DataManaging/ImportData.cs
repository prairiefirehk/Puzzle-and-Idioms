using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ImportData : MonoBehaviour
{
    // Import data (in json format) to the game
    public TextAsset idiomdataJson;
    public TextAsset popupDataJson;
    public TextAsset mobDataJson;
    public TextAsset teammateDataJson;

    // Create the idiom "library" object <-shit way
    public static IdiomList idioms;
    //public static PopupWindowList popupWindows;

    // Create array for data
    public static PopupList popups;
    public static MobList mobs;
    public static TeammateList teammates;

    void Awake()
    {
        Debug.Log("ImportData.Awake (start)");

        InitalizingIdiomData();
        InitalizingPopupData();
        InitalizingMobData();
        InitalizingTeammateData();

        Debug.Log("ImportData.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log("ImportData.OnEnable (start)");
        Debug.Log("ImportData.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log("ImportData.Start (start)");
        Debug.Log("ImportData.Start (end)");
    }

    void Update()
    {

    }
    
    void OnDisable()
    {
        Debug.Log($"ImportData.OnDisable (start)");
        Debug.Log($"ImportData.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"ImportData.OnDestroy (start)");
        Debug.Log($"ImportData.OnDestroy (end)");
    }

    public void InitalizingIdiomData()
    {
        Debug.Log("ImportData.InitalizingIdiomData (start)");

        idioms = JsonUtility.FromJson<IdiomList>(idiomdataJson.text);
        //Debug.Log(idioms.idiom[1].name);
        Debug.Log($"Idiom data successfully loaded with {idioms.idiom.Length} idioms.");

        Debug.Log("ImportData.InitalizingIdiomData (end)");
    }

    public void InitalizingPopupData()
    {
        Debug.Log("ImportData.InitalizingPopupData (start)");

        popups = JsonUtility.FromJson<PopupList>(popupDataJson.text);
        Debug.Log($"Popup data successfully loaded with {popups.popup.Length} popups.");

        Debug.Log("ImportData.InitalizingPopupData (end)");
    }

    public void InitalizingMobData()
    {
        Debug.Log("ImportData.InitalizingMobData (start)");

        mobs = JsonUtility.FromJson<MobList>(mobDataJson.text);
        //Debug.Log(mobs.mob[1].mobName);
        Debug.Log($"Mob data successfully loaded with {mobs.mob.Length} mobs.");
        
        Debug.Log("ImportData.InitalizingMobData (end)");
    }

    public void InitalizingTeammateData()
    {
        Debug.Log("ImportData.InitalizingTeammateData (start)");

        teammates = JsonUtility.FromJson<TeammateList>(teammateDataJson.text);
        Debug.Log($"Mob data successfully loaded with {teammates.teammate.Length} mobs.");

        Debug.Log("ImportData.InitalizingTeammateData (end)");
    }
}



