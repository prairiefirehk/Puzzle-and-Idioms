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
        Debug.Log($"{Time.time} ImportData.Awake (start)");

        InitalizingIdiomData();
        InitalizingPopupData();
        InitalizingMobData();
        InitalizingTeammateData();

        Debug.Log($"{Time.time} ImportData.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} ImportData.OnEnable (start)");
        Debug.Log($"{Time.time} ImportData.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} ImportData.Start (start)");
        Debug.Log($"{Time.time}ImportData.Start (end)");
    }

    void Update()
    {

    }
    
    void OnDisable()
    {
        Debug.Log($"{Time.time} ImportData.OnDisable (start)");
        Debug.Log($"{Time.time} ImportData.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} ImportData.OnDestroy (start)");
        Debug.Log($"{Time.time} ImportData.OnDestroy (end)");
    }

    public void InitalizingIdiomData()
    {
        Debug.Log($"{Time.time} ImportData.InitalizingIdiomData (start)");

        idioms = JsonUtility.FromJson<IdiomList>(idiomdataJson.text);
        //Debug.Log(idioms.idiom[1].name);
        Debug.Log($"{Time.time} Idiom data successfully loaded with {idioms.idiom.Length} idioms.");

        Debug.Log($"{Time.time} ImportData.InitalizingIdiomData (end)");
    }

    public void InitalizingPopupData()
    {
        Debug.Log($"{Time.time} ImportData.InitalizingPopupData (start)");

        popups = JsonUtility.FromJson<PopupList>(popupDataJson.text);
        Debug.Log($"{Time.time} Popup data successfully loaded with {popups.popup.Length} popups.");

        Debug.Log($"{Time.time} ImportData.InitalizingPopupData (end)");
    }

    public void InitalizingMobData()
    {
        Debug.Log($"{Time.time} ImportData.InitalizingMobData (start)");

        mobs = JsonUtility.FromJson<MobList>(mobDataJson.text);
        //Debug.Log(mobs.mob[1].mobName);
        Debug.Log($"{Time.time} Mob data successfully loaded with {mobs.mob.Length} mobs.");
        
        Debug.Log($"{Time.time} ImportData.InitalizingMobData (end)");
    }

    public void InitalizingTeammateData()
    {
        Debug.Log($"{Time.time} ImportData.InitalizingTeammateData (start)");

        teammates = JsonUtility.FromJson<TeammateList>(teammateDataJson.text);
        Debug.Log($"{Time.time} Mob data successfully loaded with {teammates.teammate.Length} mobs.");

        Debug.Log($"{Time.time} ImportData.InitalizingTeammateData (end)");
    }
}



