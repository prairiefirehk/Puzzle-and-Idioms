using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AbilityFactory : MonoBehaviour, IFactory
{
    #region Scripts
    #endregion

    #region Game object references
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{Time.time} AbilityFactory.Awake (start)");
        Debug.Log($"{Time.time} AbilityFactory.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} AbilityFactory.OnEnable (start)");
        Debug.Log($"{Time.time} AbilityFactory.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} AbilityFactory.Start (start)");
        Debug.Log($"{Time.time} AbilityFactory.Start (end)");
    }

    void Update()
    {
        
    }

    void OnDisable()
    {
        Debug.Log($"{Time.time} AbilityFactory.OnDisable (start)");
        Debug.Log($"{Time.time} AbilityFactory.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} AbilityFactory.OnDestroy (start)");
        Debug.Log($"{Time.time} AbilityFactory.OnDestroy (end)");
    }
    #endregion

    #region Factory functions
    // level need to create afterwards?
    public Ability CreateAbility(int id, string name, string desc, string abilityType, int CD, int cost, 
                   int[] selfStatusEffectsID, int[] selfStatusEffectsTurns, string[] selfAffectedStats, int[] selfAffectedStatsValue,
                   int[] enemyStatusEffectsID, int[] enemyStatusEffectsTurns, string[] enemyAffectedStats, int[] enemyAffectedStatsValue)
    {
        Debug.Log($"{Time.time} AbilityFactory.CreateAbility (start)");

        Ability ability = new Ability();

        ability.abilityID = id;
        ability.abilityName = name;
        ability.abilityDesc = desc;
        ability.abilityType = abilityType;
        ability.baseAbilityCD = CD;
        ability.baseAbilityCost = cost;

        ability.selfStatusEffectsID = new List<int>();
        ability.selfStatusEffectsTurns = new List<int>();
        ability.selfAffectedStats = new List<string>();
        ability.selfAffectedStatsValue = new List<int>();

        ability.enemyStatusEffectsID = new List<int>();
        ability.enemyStatusEffectsTurns = new List<int>();
        ability.enemyAffectedStats = new List<string>();
        ability.enemyAffectedStatsValue = new List<int>();


        ability.selfStatusEffects = new List<StatusEffect>();
        ability.enemyStatusEffects = new List<StatusEffect>();

        // For adding data into the lists
        if (selfStatusEffectsID.Length != 0)
        {
            for (int i = 0; i < selfStatusEffectsID.Length; i++)
            {
                ability.selfStatusEffectsID.Add(selfStatusEffectsID[i]);
                Debug.Log($"$ability.selfStatusEffectsID[{i}]? {ability.selfStatusEffectsID[i]}");
            }
        }

        if (selfStatusEffectsTurns.Length != 0)
        {
            for (int i = 0; i < selfStatusEffectsTurns.Length; i++)
            {
                ability.selfStatusEffectsTurns.Add(selfStatusEffectsTurns[i]);
                Debug.Log($"$ability.selfStatusEffectsTurns[{i}]? {ability.selfStatusEffectsTurns[i]}");
            }
        }

        if (selfAffectedStats.Length != 0)
        {
            for (int i = 0; i < selfAffectedStats.Length; i++)
            {
                ability.selfAffectedStats.Add(selfAffectedStats[i]);
                Debug.Log($"$ability.selfAffectedStats[{i}]? {ability.selfAffectedStats[i]}");
            }
        }
        
        if (selfAffectedStatsValue.Length != 0)
        {
            for (int i = 0; i < selfAffectedStatsValue.Length; i++)
            {
                ability.selfAffectedStatsValue.Add(selfAffectedStatsValue[i]);
                Debug.Log($"$ability.selfAffectedStatsValue[{i}]? {ability.selfAffectedStatsValue[i]}");
            }
        }

        if (enemyStatusEffectsID.Length != 0)
        {
            for (int i = 0; i < enemyStatusEffectsID.Length; i++)
            {
                ability.enemyStatusEffectsID.Add(enemyStatusEffectsID[i]);
                Debug.Log($"$ability.enemyStatusEffectsID[{i}]? {ability.enemyStatusEffectsID[i]}");
            }
        }

        if (enemyStatusEffectsTurns.Length != 0)
        {
            for (int i = 0; i < enemyStatusEffectsTurns.Length; i++)
            {
                ability.enemyStatusEffectsTurns.Add(enemyStatusEffectsTurns[i]);
                Debug.Log($"$ability.enemyStatusEffectsTurns[{i}]? {ability.enemyStatusEffectsTurns[i]}");
            }
        }

        if (enemyAffectedStats.Length != 0)
        {
            for (int i = 0; i < enemyAffectedStats.Length; i++)
            {
                ability.enemyAffectedStats.Add(enemyAffectedStats[i]);
                Debug.Log($"$ability.enemyAffectedStats[{i}]? {ability.enemyAffectedStats[i]}");
            }
        }

        if (enemyAffectedStatsValue.Length != 0)
        {
            for (int i = 0; i < enemyAffectedStatsValue.Length; i++)
            {
                ability.enemyAffectedStatsValue.Add(enemyAffectedStatsValue[i]);
                Debug.Log($"$ability.enemyAffectedStatsValue[{i}]? {ability.enemyAffectedStatsValue[i]}");
            }
        }

        // For adding data into the self status effects list
        if (selfStatusEffectsID.Length != 0)
        {
            for (int i = 0; i < selfStatusEffectsID.Length; i++)
            {
                ability.selfStatusEffects.Add(ImportData.statusEffectDictionary[selfStatusEffectsID[i]]);
                Debug.Log($"$ability.selfStatusEffects[{i}].effectName = {ability.selfStatusEffects[i].effectName}");
            }
        }

        // For adding data into the enemy status effects list
        if (enemyStatusEffectsID.Length != 0)
        {
            for (int i = 0; i < enemyStatusEffectsID.Length; i++)
            {
                ability.enemyStatusEffects.Add(ImportData.statusEffectDictionary[enemyStatusEffectsID[i]]);
                Debug.Log($"$ability.enemyStatusEffects[{i}].effectName = {ability.enemyStatusEffects[i].effectName}");
            }
        }

        Debug.Log($"{Time.time} AbilityFactory.CreateAbility, return ability(local var): {ability.abilityName}(ability.abilityName only) (end)"); 
        return ability;
    }

    //public void 
    #endregion
}