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
    #region JSON data reference
    // Import data (in json format) to the game
    public TextAsset idiomDataJson;
    public TextAsset popupDataJson;
    public TextAsset mobDataJson;
    public TextAsset teammateDataJson;
    public TextAsset tileEffectDataJson;
    public TextAsset statusEffectDataJson;
    public TextAsset abilityDataJson;
    #endregion

    #region Data array reference
    // Create array for data <-shit way I know
    public static IdiomList idioms;
    public static PopupList popups;
    public static MobList mobs;
    public static TeammateList teammates;

    public static TileEffectList tileEffectsData;
    public static StatusEffectList statusEffectsData;
    public static AbilityList abilitiesData;

    public static Dictionary<int, TileEffect> tileEffectDictionary;
    public static Dictionary<int, StatusEffect> statusEffectDictionary;
    public static Dictionary<int, Ability> abilityDictionary;
    #endregion

    #region Factories
    private TileEffectFactory tileEffectFactory;
    private StatusEffectFactory statusEffectFactory;
    private AbilityFactory abilityFactory;
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{Time.time} ImportData.Awake (start)");

        tileEffectFactory = gameObject.GetComponent<TileEffectFactory>();
        statusEffectFactory = gameObject.GetComponent<StatusEffectFactory>();
        abilityFactory = gameObject.GetComponent<AbilityFactory>();

        InitalizingIdiomData();
        InitalizingPopupData();
        InitalizingMobData();
        InitalizingTeammateData();
        InitalizingTileEffectData();
        InitalizingStatusEffectData();
        InitalizingAbilityData();

        InitalizingTileEffectDict();
        InitalizingStatusEffectDict();
        InitalizingAbilityDict();

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
    #endregion

    public void InitalizingIdiomData()
    {
        Debug.Log($"{Time.time} ImportData.InitalizingIdiomData (start)");

        idioms = JsonUtility.FromJson<IdiomList>(idiomDataJson.text);
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
        Debug.Log($"{Time.time} Teammate data successfully loaded with {teammates.teammate.Length} teammates.");

        Debug.Log($"{Time.time} ImportData.InitalizingTeammateData (end)");
    }

    public void InitalizingTileEffectData()
    {
        Debug.Log($"{Time.time} ImportData.InitalizingTileEffectData (start)");

        tileEffectsData = JsonUtility.FromJson<TileEffectList>(tileEffectDataJson.text);
        Debug.Log($"{Time.time} Tile effect data successfully loaded with {tileEffectsData.tileEffectData.Length} tile effects data.");

        Debug.Log($"{Time.time} ImportData.InitalizingTileEffectData (end)");
    }

    public void InitalizingStatusEffectData()
    {
        Debug.Log($"{Time.time} ImportData.InitalizingStatusEffectData (start)");

        statusEffectsData = JsonUtility.FromJson<StatusEffectList>(statusEffectDataJson.text);
        Debug.Log($"{Time.time} Status effect data successfully loaded with {statusEffectsData.statusEffectData.Length} status effects data.");

        Debug.Log($"{Time.time} ImportData.InitalizingStatusEffectData (end)");
    }

    public void InitalizingAbilityData()
    {
        Debug.Log($"{Time.time} ImportData.InitalizingAbilityData (start)");

        abilitiesData = JsonUtility.FromJson<AbilityList>(abilityDataJson.text);
        Debug.Log($"{Time.time} Ability data successfully loaded with {abilitiesData.abilityData.Length} abilities data.");

        Debug.Log($"{Time.time} ImportData.InitalizingAbilityData (end)");
    }

    #region Convert data into actual object (for ability/status effect, which is not an actual game object)
    public void InitalizingTileEffectDict()
    {
        Debug.Log($"{Time.time} ImportData.InitalizingTileEffectDict (start)");

        tileEffectDictionary = new Dictionary<int, TileEffect>();
        if (tileEffectsData.tileEffectData.Length != 0)
        {
            for (int i = 0; i < tileEffectsData.tileEffectData.Length; i++)
            {
                tileEffectDictionary.Add(tileEffectsData.tileEffectData[i].tileEffectID,
                tileEffectFactory.CreateTileEffect
                (
                tileEffectsData.tileEffectData[i].tileEffectID,
                tileEffectsData.tileEffectData[i].tileEffectName, 
                tileEffectsData.tileEffectData[i].tileEffectDesc,
                tileEffectsData.tileEffectData[i].tileEffectType,
                tileEffectsData.tileEffectData[i].tileEffectIconPicName
                ));
            }
        }

        Debug.Log($"{Time.time} Tile effect dictioinary successfully loaded with {tileEffectDictionary.Count} tile effects.");

        Debug.Log($"{Time.time} ImportData.InitalizingTileEffectDict (end)");
    }
    public void InitalizingStatusEffectDict()
    {
        Debug.Log($"{Time.time} ImportData.InitalizingStatusEffectDict (start)");

        statusEffectDictionary = new Dictionary<int, StatusEffect>();
        if (statusEffectsData.statusEffectData.Length != 0)
        {
            for (int i = 0; i < statusEffectsData.statusEffectData.Length; i++)
            {
                statusEffectDictionary.Add(statusEffectsData.statusEffectData[i].effectID,
                statusEffectFactory.CreateStatusEffect
                (
                statusEffectsData.statusEffectData[i].effectID,
                statusEffectsData.statusEffectData[i].effectName, 
                statusEffectsData.statusEffectData[i].effectDesc,
                statusEffectsData.statusEffectData[i].effectType,
                statusEffectsData.statusEffectData[i].effectIconPicName,
                statusEffectsData.statusEffectData[i].sustainValue,
                statusEffectsData.statusEffectData[i].affectedStatsWithModifier,
                statusEffectsData.statusEffectData[i].affectedStatsModifierType,
                statusEffectsData.statusEffectData[i].affectedStatsModifierOrder,
                statusEffectsData.statusEffectData[i].affectedStatsModifierValue
                ));
            }
        }

        Debug.Log($"{Time.time} Status effect dictioinary successfully loaded with {statusEffectDictionary.Count} status effects.");

        Debug.Log($"{Time.time} ImportData.InitalizingStatusEffectDict (end)");
    }

    public void InitalizingAbilityDict()
    {
        Debug.Log($"{Time.time} ImportData.InitalizingAbilityDict (start)");

        abilityDictionary = new Dictionary<int, Ability>();
        if (abilitiesData.abilityData.Length != 0)
        {
            for (int i = 0; i < abilitiesData.abilityData.Length; i++)
            {
                abilityDictionary.Add(abilitiesData.abilityData[i].abilityID,
                abilityFactory.CreateAbility
                (
                abilitiesData.abilityData[i].abilityID,
                abilitiesData.abilityData[i].abilityName, 
                abilitiesData.abilityData[i].abilityDesc,
                abilitiesData.abilityData[i].abilityType,
                abilitiesData.abilityData[i].baseAbilityCD,
                abilitiesData.abilityData[i].baseAbilityCost,
                abilitiesData.abilityData[i].selfStatusEffectsID,
                abilitiesData.abilityData[i].selfStatusEffectsTurns,
                abilitiesData.abilityData[i].selfAffectedStats,
                abilitiesData.abilityData[i].selfAffectedStatsValue,
                abilitiesData.abilityData[i].enemyStatusEffectsID,
                abilitiesData.abilityData[i].enemyStatusEffectsTurns,
                abilitiesData.abilityData[i].enemyAffectedStats,
                abilitiesData.abilityData[i].enemyAffectedStatsValue
                ));
            }
        }

        Debug.Log($"{Time.time} Ability dictioinary successfully loaded with {abilityDictionary.Count} abilities.");

        Debug.Log($"{Time.time} ImportData.InitalizingAbilityDict (end)");
    }
    #endregion
}



