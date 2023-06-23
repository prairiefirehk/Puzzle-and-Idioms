using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TileEffectFactory : MonoBehaviour, IFactory
{
    #region Scripts
    #endregion

    #region Game object references
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{Time.time} TileEffectFactory.Awake (start)");
        Debug.Log($"{Time.time} TileEffectFactory.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} TileEffectFactory.OnEnable (start)");
        Debug.Log($"{Time.time} TileEffectFactory.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} TileEffectFactory.Start (start)");
        Debug.Log($"{Time.time} TileEffectFactory.Start (end)");
    }

    void Update()
    {
        
    }

    void OnDisable()
    {
        Debug.Log($"{Time.time} TileEffectFactory.OnDisable (start)");
        Debug.Log($"{Time.time} TileEffectFactory.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} TileEffectFactory.OnDestroy (start)");
        Debug.Log($"{Time.time} TileEffectFactory.OnDestroy (end)");
    }
    #endregion

    #region Factory functions
    public TileEffect CreateTileEffect(int id, string name, string desc, string effectType, string effectIconPicName)
    {
        Debug.Log($"{Time.time} TileEffectFactory.CreateTileEffect (start)");

        TileEffect tileEffect = new TileEffect();

        tileEffect.tileEffectID = id;
        tileEffect.tileEffectName = name;
        tileEffect.tileEffectDesc = desc;
        tileEffect.tileEffectType = effectType;
        tileEffect.tileEffectIconPicName = effectIconPicName;

        // For adding data into the lists
        

        Debug.Log($"{Time.time} TileEffectFactory.CreateTileEffect, return tileEffect(local var): {tileEffect.tileEffectName}(tileEffect.effectName only) (end)"); 
        return tileEffect;
    }

    #endregion
}