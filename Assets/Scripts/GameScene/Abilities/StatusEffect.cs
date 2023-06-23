using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class StatusEffect
{
    #region Scripts
    public RoundData roundData;
    #endregion

    #region Game object references
    public EffectBox effectBox;
    #endregion

    #region Status effect data
    private int _effectID;
    public int effectID { get { return _effectID; } set { _effectID = value; } }
    private string _effectName;
    public string effectName { get { return _effectName; } set { _effectName = value; } }
    private string _effectDesc;
    public string effectDesc { get { return _effectDesc; } set { _effectDesc = value; } }
    private string _effectType;
    public string effectType { get { return _effectType; } set { _effectType = value; } }
    private string _effectIconPicName;
    public string effectIconPicName { get { return _effectIconPicName; } set { _effectIconPicName = value; } }
    
    // Damage within turns
    //private float _instantValue;
    //public float instantValue { get { return _instantValue; } set { _instantValue = value; } }
    private float _sustainValue;
    public float sustainValue { get { return _sustainValue; } set { _sustainValue = value; } }

    private Entity _effectTarget;
    public Entity effectTarget { get { return _effectTarget; } set { _effectTarget = value; } }
    private int _effectTurns;
    public int effectTurns { get { return _effectTurns; } set { _effectTurns = value; } }
    private int _effectRemainingTurns;
    public int effectRemainingTurns { get { return _effectRemainingTurns; } set { _effectRemainingTurns = value; } }
    private int _effectLevel;
    public int effectLevel { get { return _effectLevel; } set { _effectLevel = value; } }

    private List<string> _affectedStatsWithModifier;
    public List<string> affectedStatsWithModifier { get { return _affectedStatsWithModifier; } set { _affectedStatsWithModifier = value; } }
    private List<string> _affectedStatsModifierType;
    public List<string> affectedStatsModifierType { get { return _affectedStatsModifierType; } set { _affectedStatsModifierType = value; } }
    private List<int> _affectedStatsModifierOrder;
    public List<int> affectedStatsModifierOrder { get { return _affectedStatsModifierOrder; } set { _affectedStatsModifierOrder = value; } }
    private List<int> _affectedStatsModifierValue;
    public List<int> affectedStatsModifierValue { get { return _affectedStatsModifierValue; } set { _affectedStatsModifierValue = value; } }
    private List<StatModifier> _affectedStatsModifier;
    public List<StatModifier> affectedStatsModifier { get { return _affectedStatsModifier; } set { _affectedStatsModifier = value; } }

    /*
    private List<string> _affectedStatsDirectChange;
    public List<string> affectedStatsDirectChange { get { return _affectedStatsDirectChange; } set { _affectedStatsDirectChange = value; } }
    private List<int> _affectedStatsDirectValue;
    public List<int> affectedStatsDirectValue { get { return _affectedStatsDirectValue; } set { _affectedStatsDirectValue = value; } }

    // Boolean for checking types of changing stats it contains
    private Boolean _hasModifiedStats = false;
    public Boolean hasModifiedStats { get { return _hasModifiedStats; } set { _hasModifiedStats = value; } }
    private Boolean _hasDirectChangedStats = false;
    public Boolean hasDirectChangedStats { get { return _hasDirectChangedStats; } set { _hasDirectChangedStats = value; } }
    */

    #endregion
    /*
    private List<int> _affectedStatModifierValue;
    public List<int> affectedStatModifierValue { get { return _affectedStatModifierValue; } set { _affectedStatModifierValue = value; } }
    private List<StatModifierType> _effectModifierCalcType;
    public List<StatModifierType> effectModifierCalcType { get { return _effectModifierCalcType; } set { _effectModifierCalcType = value; } }
    */

    #region Status effect functions
    // Within the caster's move, which giving status effect to the target
    // Called only once
    public virtual void OnInflict()
    {
        Debug.Log($"{Time.time} {effectName} StatusEffect.OnInflict (start)");

        // Check if there is the same status effect from the target
        if (HasSameStatusEffect())
        {
            // If there is the same status effect, just remove it**************************************B(5 turns, [1])A(1 turn, [0]) -> + A(5 turns) -> A(5 turns, [1]B(4 turns, [0]))
            Debug.Log($"{Time.time} Found same status effect!");
            effectTarget.currentStatusEffects[GetOrderOfStatusEffect()].OnRemove();
        }

        // Check if there are more than 5 status effects from the target
        if (effectTarget.currentStatusEffects.Count >= 5)
        {
            // Temp solution, just remove the oldest effect
            effectTarget.currentStatusEffects[0].OnRemove();
        }

        // Add the new status effect into the list
        effectTarget.currentStatusEffects.Add(this);
        effectTarget.hasStatusEffect = true;

        // Get the remaining turns for the status effect
        effectRemainingTurns = effectTurns;

        // Apply new StatModifier to the target's stat
        //if (hasModifiedStats)
        {
            for (int i = 0; i < affectedStatsWithModifier.Count; i++)
            {
                switch (affectedStatsWithModifier[i])
                {
                    case "currentMaxHealthPoint":
                        Debug.Log($"$effectTarget.currentMaxHealthPoint.value before = {effectTarget.currentMaxHealthPoint.GetStatValue()}");
                        effectTarget.currentMaxHealthPoint.AddModifier(affectedStatsModifier[i]);
                        Debug.Log($"$effectTarget.currentMaxHealthPoint.value after = {effectTarget.currentMaxHealthPoint.GetStatValue()}");
                        break;

                    case "currentAttackPoint":
                        Debug.Log($"$effectTarget.currentAttackPoint.value before = {effectTarget.currentAttackPoint.GetStatValue()}");
                        effectTarget.currentAttackPoint.AddModifier(affectedStatsModifier[i]);
                        Debug.Log($"$effectTarget.currentAttackPoint.value after = {effectTarget.currentAttackPoint.GetStatValue()}");
                        break;

                    case "currentDefencePoint":
                        Debug.Log($"$effectTarget.currentDefencePoint.value before = {effectTarget.currentDefencePoint.GetStatValue()}");
                        effectTarget.currentDefencePoint.AddModifier(affectedStatsModifier[i]);
                        Debug.Log($"$effectTarget.currentDefencePoint.value after = {effectTarget.currentDefencePoint.GetStatValue()}");
                        break;

                    case "currentPerceptionPoint":
                        Debug.Log($"$effectTarget.currentPerceptionPoint.value before = {effectTarget.currentPerceptionPoint.GetStatValue()}");
                        effectTarget.currentPerceptionPoint.AddModifier(affectedStatsModifier[i]);
                        Debug.Log($"$effectTarget.currentPerceptionPoint.value after = {effectTarget.currentPerceptionPoint.GetStatValue()}");
                        break;

                    case "currentDexterityPoint":
                        Debug.Log($"$effectTarget.currentDexterityPoint.value before = {effectTarget.currentDexterityPoint.GetStatValue()}");
                        effectTarget.currentDexterityPoint.AddModifier(affectedStatsModifier[i]);
                        Debug.Log($"$effectTarget.currentDexterityPoint.value after = {effectTarget.currentDexterityPoint.GetStatValue()}");
                        break;

                    case "currentConstitutionPoint":
                        Debug.Log($"$effectTarget.currentConstitutionPoint.value before = {effectTarget.currentConstitutionPoint.GetStatValue()}");
                        effectTarget.currentConstitutionPoint.AddModifier(affectedStatsModifier[i]);
                        Debug.Log($"$effectTarget.currentConstitutionPoint.value after = {effectTarget.currentConstitutionPoint.GetStatValue()}");
                        break;

                    case "currentMaxAttackInterval":
                        effectTarget.GetComponent<Mob>().currentMaxAttackInterval.AddModifier(affectedStatsModifier[i]);
                        break;

                    case "currentMaxActiveAbilityCD":
                        foreach (KeyValuePair<TeammateType, Teammate> teammate in effectTarget.GetComponent<Player>().teammates)
                        {
                            Debug.Log($"$teammate({teammate.Value.teammateName}).Value.currentMaxActiveAbilityCD.value before = {teammate.Value.currentMaxActiveAbilityCD.GetStatValue()}");
                            teammate.Value.currentMaxActiveAbilityCD.AddModifier(affectedStatsModifier[i]);
                            Debug.Log($"$teammate({teammate.Value.teammateName}).Value.currentMaxActiveAbilityCD.value after = {teammate.Value.currentMaxActiveAbilityCD.GetStatValue()}");
                        }
                        break;
                        
                    // And more...
                }
            }
        }
        
        /*
        if (hasDirectChangedStats)
        {
            for (int i = 0; i < affectedStatsWithModifier.Count; i++)
            {
                switch (affectedStatsWithModifier[i])
                {
                    case "currentAttackInterval":
                        effectTarget.GetComponent<Mob>().currentAttackInterval += affectedStatsDirectValue[i];
                        break;

                    case "currentActiveAbilityCD":
                        effectTarget.GetComponent<Teammate>().currentActiveAbilityCD += affectedStatsDirectValue[i];
                        break;
                    
                    // And more...
                }
            }
        }
        */
        
        
        // Apply the instant damage to the target
        //effectTarget.TakeDamage(instantValue);

        // Load the effect box icon and CD text in the arena
        /*
        if (effectTarget == roundData.player)
        {
            effectBox = roundData.playerEffectBoxes.transform.GetChild(GetOrderOfStatusEffect()).GetComponent<EffectBox>();
            effectBox.InitializeEffectBox(this);
        }
        else if (effectTarget == roundData.currentMob)
        {
            effectBox = roundData.mobEffectBoxes.transform.GetChild(GetOrderOfStatusEffect()).GetComponent<EffectBox>();
            effectBox.InitializeEffectBox(this);
        }
        else
        {
            Debug.Log($"Dude wtf is effectTarget?"); 
        }
        */
        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();
        effectBox = roundData.effectBoxFactory.CreateEffectBox(effectTarget, this, GetOrderOfStatusEffect());

        Debug.Log($"{Time.time} {effectName} StatusEffect.OnInflict (end)");
    }

    // On every new turn
    public virtual void OnBeforeTurnStart()
    {
        Debug.Log($"{Time.time} StatusEffect.OnBeforeTurnStart (start)"); 

        effectRemainingTurns -= 1;

        // Apply the sustain damage to the target
        effectTarget.TakeDamage(sustainValue);

        Debug.Log($"{Time.time} StatusEffect.OnBeforeTurnStart (end)");
    }

    // On every turn ending
    public virtual void OnTurnEnd()
    {
        Debug.Log($"{Time.time} StatusEffect.OnTurnEnd (start)"); 
        
        // Update the CD text in the effect box/arena
        if (effectTarget.hasStatusEffect)
        {
            //effectBox.UpdateEffectBox();
        }

        if (effectRemainingTurns <= 0)
        {
            OnRemove();
        }
        
        
        Debug.Log($"{Time.time} StatusEffect.OnTurnEnd (end)"); 
    }

    // On removing itself
    public virtual void OnRemove()
    {
        Debug.Log($"{Time.time} StatusEffect.OnRemove (start)"); 

        // Remove icon and CD text in effect box/arena
        effectBox.DestroyEffectBox();

        // Remove the statModifier from the target
        //if (hasModifiedStats)
        {
            for (int i = 0; i < affectedStatsWithModifier.Count; i++)
            {
                switch (this.affectedStatsWithModifier[i])
                {
                    case "currentMaxHealthPoint":
                        effectTarget.currentMaxHealthPoint.RemoveModifier(affectedStatsModifier[i]);
                        break;

                    case "currentAttackPoint":
                        effectTarget.currentAttackPoint.RemoveModifier(affectedStatsModifier[i]);
                        break;

                    case "currentDefencePoint":
                        effectTarget.currentDefencePoint.RemoveModifier(affectedStatsModifier[i]);
                        break;

                    case "currentPerceptionPoint":
                        effectTarget.currentPerceptionPoint.RemoveModifier(affectedStatsModifier[i]);
                        break;

                    case "currentDexterityPoint":
                        effectTarget.currentDexterityPoint.RemoveModifier(affectedStatsModifier[i]);
                        break;

                    case "currentConstitutionPoint":
                        effectTarget.currentConstitutionPoint.RemoveModifier(affectedStatsModifier[i]);
                        break;

                    case "currentMaxAttackInterval":
                        effectTarget.GetComponent<Mob>().currentMaxAttackInterval.RemoveModifier(affectedStatsModifier[i]);
                        break;

                    case "currentMaxActiveAbilityCD":
                        foreach (KeyValuePair<TeammateType, Teammate> teammate in effectTarget.GetComponent<Player>().teammates)
                        {
                            Debug.Log($"$teammate({teammate.Value.teammateName}).Value.currentMaxActiveAbilityCD.value before = {teammate.Value.currentMaxActiveAbilityCD.GetStatValue()}");
                            teammate.Value.currentMaxActiveAbilityCD.RemoveModifier(affectedStatsModifier[i]);
                            Debug.Log($"$teammate({teammate.Value.teammateName}).Value.currentMaxActiveAbilityCD.value after = {teammate.Value.currentMaxActiveAbilityCD.GetStatValue()}");
                        }
                        break;

                    // And more...
                }
            }
        }

        //effectTarget.currentStatusEffects.Remove(this);
        effectTarget.currentStatusEffects.Remove(this);
        if (effectTarget.currentStatusEffects.Count == 0)
        {
            effectTarget.hasStatusEffect = false;
        }

        Debug.Log($"{Time.time} StatusEffect.OnRemove (end)"); 
    }

    public bool HasSameStatusEffect()
    {
        Debug.Log($"{Time.time} StatusEffect.HasSameStatusEffect (start)"); 

        bool hasSameEffect = false;

        //Debug.Log($"$effectTarget.name {effectTarget.name}");

        if (effectTarget != null)
        {
            if (effectTarget.currentStatusEffects != null)
            {
                foreach (StatusEffect statusEffect in effectTarget.currentStatusEffects)
                {
                    if (statusEffect.effectName == this.effectName)
                    {
                        hasSameEffect = true;
                    }
                }
            }
        }
        
        Debug.Log($"{Time.time} StatusEffect.HasSameStatusEffect, return hasSameEffect(local val): {hasSameEffect} (end)"); 
        return hasSameEffect;
    }

    public int GetOrderOfStatusEffect()
    {
        Debug.Log($"{Time.time} StatusEffect.GetOrderOfStatusEffect (start)"); 

        int orderOfStatusEffect = -1;
        for (int i = 0; i <= (effectTarget.currentStatusEffects.Count - 1); i++)
        {
            if (effectTarget.currentStatusEffects[i].effectName == this.effectName)
            {
                orderOfStatusEffect = i;
            }
        }

        Debug.Log($"{Time.time} StatusEffect.GetOrderOfStatusEffect, return orderOfStatusEffect(local val): {orderOfStatusEffect} (end)"); 
        return orderOfStatusEffect;
    }
    #endregion
}
