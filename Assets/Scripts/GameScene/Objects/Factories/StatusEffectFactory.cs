using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatusEffectFactory : MonoBehaviour, IFactory
{
    #region Scripts
    #endregion

    #region Game object references
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{Time.time} StatusEffectFactory.Awake (start)");
        Debug.Log($"{Time.time} StatusEffectFactory.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} StatusEffectFactory.OnEnable (start)");
        Debug.Log($"{Time.time} StatusEffectFactory.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} StatusEffectFactory.Start (start)");
        Debug.Log($"{Time.time} StatusEffectFactory.Start (end)");
    }

    void Update()
    {
        
    }

    void OnDisable()
    {
        Debug.Log($"{Time.time} StatusEffectFactory.OnDisable (start)");
        Debug.Log($"{Time.time} StatusEffectFactory.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} StatusEffectFactory.OnDestroy (start)");
        Debug.Log($"{Time.time} StatusEffectFactory.OnDestroy (end)");
    }
    #endregion

    #region Factory functions
    // turns, level, target, modifier need to create afterwards?
    public StatusEffect CreateStatusEffect(int ID, string name, string desc, string type, string iconPicName, 
                        float susValue, string[] statsWithModifier, string[] statsModifierType, int[] statsModifierOrder, int[] statsModifierValue)
    {
        Debug.Log($"{Time.time} StatusEffectFactory.CreateStatusEffect (start)");

        StatusEffect statusEffect = new StatusEffect();

        statusEffect.effectID = ID;
        statusEffect.effectName = name;
        statusEffect.effectDesc = desc;
        statusEffect.effectType = type;
        statusEffect.effectIconPicName = iconPicName;
        //this.instantValue = insValue;
        statusEffect.sustainValue = susValue;

        //statusEffect.effectTurns = turns;
        //statusEffect.effectLevel = level;
        //statusEffect.effectTarget = target;

        statusEffect.affectedStatsWithModifier = new List<string>();
        statusEffect.affectedStatsModifierType = new List<string>();
        statusEffect.affectedStatsModifierOrder = new List<int>();
        statusEffect.affectedStatsModifierValue = new List<int>();
        statusEffect.affectedStatsModifier = new List<StatModifier>();

        // For adding data into the list
        if (statsWithModifier.Length != 0)
        {
            for (int i = 0; i < statsWithModifier.Length; i++)
            {
                statusEffect.affectedStatsWithModifier.Add(statsWithModifier[i]);
                Debug.Log($"$statusEffect.affectedStatsWithModifier[{i}]? {statusEffect.affectedStatsWithModifier[i]}");
            }
        }

        if (statsModifierOrder.Length != 0)
        {
            for (int i = 0; i < statsModifierOrder.Length; i++)
            {
                statusEffect.affectedStatsModifierOrder.Add(statsModifierOrder[i]);
                Debug.Log($"$statusEffect.affectedStatsModifierOrder[{i}]? {statusEffect.affectedStatsModifierOrder[i]}");
            }
        }

        if (statsModifierValue.Length != 0)
        {
            for (int i = 0; i < statsModifierValue.Length; i++)
            {
                statusEffect.affectedStatsModifierValue.Add(statsModifierValue[i]);
                Debug.Log($"$statusEffect.affectedStatsModifierValue[{i}]? {statusEffect.affectedStatsModifierValue[i]}");
            }
        }

        for (int i = 0; i < statsModifierType.Length; i++)
        {
            StatModifierType statModType = new StatModifierType();

            switch (statsModifierType[i])
            {
                case "Flat":
                    statModType = StatModifierType.Flat;
                    break;

                case "PercentStack":
                    statModType = StatModifierType.PercentStack;
                    break;
                
                case "PercentMultiple":
                    statModType = StatModifierType.PercentMultiple;
                    break;

                case "Equal":
                    statModType = StatModifierType.Equal;
                    break;
            }

            statusEffect.affectedStatsModifier.Add(new StatModifier(statusEffect.affectedStatsModifierValue[i], statModType,statusEffect.affectedStatsModifierOrder[i]));
            Debug.Log($"$statusEffect.affectedStatsModifier[{i}].type? {statusEffect.affectedStatsModifier[i].type}");
        } 

        //statusEffect.effectRemainingTurns = turns + 1;
    

        Debug.Log($"{Time.time} StatusEffectFactory.CreateStatusEffect, return statusEffect(local var): {statusEffect.effectName}(statusEffect.effectName only) (end)"); 
        return statusEffect;
    }

    //public void 
    #endregion
}