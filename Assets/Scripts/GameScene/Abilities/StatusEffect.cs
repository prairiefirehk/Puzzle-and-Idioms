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
    private StatusEffectName _effectName;
    public StatusEffectName effectName { get { return _effectName; } set { _effectName = value; } }
    
    private int _effectTurns;
    public int effectTurns { get { return _effectTurns; } set { _effectTurns = value; } }
    private int _effectRemainingTurns;
    public int effectRemainingTurns { get { return _effectRemainingTurns; } set { _effectRemainingTurns = value; } }
    private int _effectLevel;
    public int effectLevel { get { return _effectLevel; } set { _effectLevel = value; } }

    private float _instantValue;
    public float instantValue { get { return _instantValue; } set { _instantValue = value; } }
    private float _sustainValue;
    public float sustainValue { get { return _sustainValue; } set { _sustainValue = value; } }

    private Entity _effectTarget;
    public Entity effectTarget { get { return _effectTarget; } set { _effectTarget = value; } }
    private string _effectDesc;
    public string effectDesc { get { return _effectDesc; } set { _effectDesc = value; } }

    private string type;

    private List<string> _affectedStatWithModi;
    public List<string> affectedStatWithModi { get { return _affectedStatWithModi; } set { _affectedStatWithModi = value; } }

    private List<StatModifier> _affectedStatModifier;
    public List<StatModifier> affectedStatModifier { get { return _affectedStatModifier; } set { _affectedStatModifier = value; } }

    private List<string> _affectedStatsByDirectChange;
    public List<string> affectedStatsByDirectChange { get { return _affectedStatsByDirectChange; } set { _affectedStatsByDirectChange = value; } }

    private List<float> _affectedStatsDirectChangeValue;
    public List<float> affectedStatsDirectChangeValue { get { return _affectedStatsDirectChangeValue; } set { _affectedStatsDirectChangeValue = value; } }
    #endregion
    /*
    private List<int> _affectedStatModifierValue;
    public List<int> affectedStatModifierValue { get { return _affectedStatModifierValue; } set { _affectedStatModifierValue = value; } }
    private List<StatModifierType> _effectModifierCalcType;
    public List<StatModifierType> effectModifierCalcType { get { return _effectModifierCalcType; } set { _effectModifierCalcType = value; } }
    */

    // I know this is shit solution, ok?
    // For creating status effect that affecting stats with statsModifiers
    public StatusEffect(StatusEffectName effectName, int effectTurns, int effectLevel, float instantValue, float sustainValue, Entity effectTarget, string effectDesc, List<string> affectedStatsWithModi, List<StatModifier> affectedStatsModifier)
    {
        this.effectName = effectName;
        this.effectTurns = effectTurns;
        this.effectLevel = effectLevel;
        this.instantValue = instantValue;
        this.sustainValue = sustainValue;
        this.effectTarget = effectTarget;
        this.effectDesc = effectDesc;

        this.affectedStatWithModi = new List<string>();
        this.affectedStatModifier = new List<StatModifier>();

        this.type = "affectWithModifiers";
        // For adding the affected stats with stats modifier
        for(int i = 0; i < affectedStatsWithModi.Count; i++)
        {
            this.affectedStatWithModi.Add(affectedStatsWithModi[i]);
        }

        for(int i = 0; i < affectedStatsModifier.Count; i++)
        {
            this.affectedStatModifier.Add(affectedStatsModifier[i]);
        }

        this.effectRemainingTurns = effectTurns + 1;
    }

    // For creating status effect that affecting stats by direct change the value
    public StatusEffect(StatusEffectName effectName, int effectTurns, int effectLevel, float instantValue, float sustainValue, Entity effectTarget, string effectDesc, List<string> affectedStatsByDirectChange, List<float> affectedStatsDirectChangeValue)
    {
        this.effectName = effectName;
        this.effectTurns = effectTurns;
        this.effectLevel = effectLevel;
        this.instantValue = instantValue;
        this.sustainValue = sustainValue;
        this.effectTarget = effectTarget;
        this.effectDesc = effectDesc;

        this.affectedStatsByDirectChange = new List<string>();
        this.affectedStatsDirectChangeValue = new List<float>();

        this.type = "affectDirectly";
        // For adding the affected stats by direct change value
        for(int i = 0; i < affectedStatsByDirectChange.Count; i++)
        {
            this.affectedStatsByDirectChange.Add(affectedStatsByDirectChange[i]);
        }

        for(int i = 0; i < affectedStatsDirectChangeValue.Count; i++)
        {
            this.affectedStatsDirectChangeValue.Add(affectedStatsDirectChangeValue[i]);
        }

        this.effectRemainingTurns = effectTurns + 1;
    }

    // For creating status effect that affecting stats by both ways
    public StatusEffect(StatusEffectName effectName, int effectTurns, int effectLevel, float instantValue, float sustainValue, Entity effectTarget, string effectDesc, List<string> affectedStatsWithModi, List<StatModifier> affectedStatsModifier, List<string> affectedStatsByDirectChange, List<float> affectedStatsDirectChangeValue)
    {
        this.effectName = effectName;
        this.effectTurns = effectTurns;
        this.effectLevel = effectLevel;
        this.instantValue = instantValue;
        this.sustainValue = sustainValue;
        this.effectTarget = effectTarget;
        this.effectDesc = effectDesc;

        this.affectedStatWithModi = new List<string>();
        this.affectedStatModifier = new List<StatModifier>();
        this.affectedStatsByDirectChange = new List<string>();
        this.affectedStatsDirectChangeValue = new List<float>();

        this.type = "affectByBoth";
        // For adding the affected stats with stats modifier
        for(int i = 0; i < affectedStatsWithModi.Count; i++)
        {
            this.affectedStatWithModi.Add(affectedStatsWithModi[i]);
        }

        for(int i = 0; i < affectedStatsModifier.Count; i++)
        {
            this.affectedStatModifier.Add(affectedStatsModifier[i]);
        }

        // For adding the affected stats by direct change value
        for(int i = 0; i < affectedStatsByDirectChange.Count; i++)
        {
            this.affectedStatsByDirectChange.Add(affectedStatsByDirectChange[i]);
        }

        for(int i = 0; i < affectedStatsDirectChangeValue.Count; i++)
        {
            this.affectedStatsDirectChangeValue.Add(affectedStatsDirectChangeValue[i]);
        }

        this.effectRemainingTurns = effectTurns + 1;
    }


    // Within the caster's move, which giving status effect to the target
    // Called only once
    public virtual void OnInflict()
    {
        Debug.Log($"StatusEffect.OnInflict (start)");

        // Check if there is the same status effect from the target
        if (HasSameStatusEffect())
        {
            // If there is the same status effect, just remove it
            Debug.Log($"Found same status effect!");
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
        effectTarget.HasStatusEffect = true;
        
        // Apply new status effect flag to the target
        switch (effectName)
        {
            case StatusEffectName.Burning:
                effectTarget.isBurn = true;
                break;
            
            case StatusEffectName.Freezing:
                effectTarget.isFrozen = true;
                break;

            case StatusEffectName.Stuning:
                effectTarget.isStun = true;
                break;

            // And more...
        }

        if (type == "affectWithModifiers" || type == "affectByBoth")
        {
            // Apply new StatModifier to the target's stat
            for (int i = 0; i < affectedStatWithModi.Count; i++)
            {
                switch (affectedStatWithModi[i])
                {
                    case "currentMaxHp":
                        effectTarget.currentMaxHp.AddModifier(affectedStatModifier[i]);
                        break;

                    case "currentAttackPoint":
                        effectTarget.currentAttackPoint.AddModifier(affectedStatModifier[i]);
                        break;

                    case "currentDefencePoint":
                        effectTarget.currentDefencePoint.AddModifier(affectedStatModifier[i]);
                        break;

                    case "currentEvasionPoint":
                        effectTarget.currentEvasionPoint.AddModifier(affectedStatModifier[i]);
                        break;

                    case "currentCriticalPoint":
                        effectTarget.currentCriticalPoint.AddModifier(affectedStatModifier[i]);
                        break;

                    case "currentDexterityPoint":
                        effectTarget.currentDexterityPoint.AddModifier(affectedStatModifier[i]);
                        break;
                    
                    // And more...
                }
            }
        }

        // Apply new affected stat's value to the target
        else
        {
            for (int i = 0; i < affectedStatsByDirectChange.Count; i++)
            {
                switch (affectedStatsByDirectChange[i])
                {
                    case "currentMaxHp":
                        effectTarget.currentMaxHp.value = affectedStatsDirectChangeValue[i];
                        break;

                    case "currentAttackPoint":
                        effectTarget.currentAttackPoint.value = affectedStatsDirectChangeValue[i];
                        break;

                    case "currentDefencePoint":
                        effectTarget.currentDefencePoint.value = affectedStatsDirectChangeValue[i];
                        break;

                    case "currentEvasionPoint":
                        effectTarget.currentEvasionPoint.value = affectedStatsDirectChangeValue[i];
                        break;

                    case "currentCriticalPoint":
                        effectTarget.currentCriticalPoint.value = affectedStatsDirectChangeValue[i];
                        break;

                    case "currentDexterityPoint":
                        effectTarget.currentDexterityPoint.value = affectedStatsDirectChangeValue[i];
                        break;

                    // Testing
                    case "currentAttackInterval":
                        effectTarget.gameObject.GetComponent<Mob>().currentAttackInterval.value = affectedStatsDirectChangeValue[i];
                        break;

                    case "currentMaxAttackInterval":
                        effectTarget.gameObject.GetComponent<Mob>().currentMaxAttackInterval.value = affectedStatsDirectChangeValue[i];
                        break;

                    // And more...
                }
            }
        }

        // Apply the instant damage to the target
        //effectTarget.TakeDamage(instantValue);

        // Load the effect box icon and CD text in the arena
        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();

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

        Debug.Log($"StatusEffect.OnInflict (end)"); 
    }

    // On every new turn
    public virtual void OnTurnStart()
    {
        Debug.Log($"StatusEffect.OnTurnStart (start)"); 

        effectRemainingTurns -= 1;

        // Apply the sustain damage to the target
        effectTarget.TakeDamage(sustainValue);

        // Update the CD text in the effect box/arena
        if (effectTarget.HasStatusEffect)
        {
            UpdateEffectBox();
            effectBox.UpdateEffectBoxCDText();
        }

        Debug.Log($"StatusEffect.OnTurnStart (end)");
    }

    // On every turn ending
    public virtual void OnTurnEnd()
    {
        Debug.Log($"StatusEffect.OnTurnEnd (start)"); 
        
        // Update the CD text in the effect box/arena
        if (effectTarget.HasStatusEffect)
        {
            UpdateEffectBox();
            effectBox.UpdateEffectBoxCDText();
        }
        
        Debug.Log($"StatusEffect.OnTurnEnd (end)"); 
    }

    // On removing itself
    public virtual void OnRemove()
    {
        Debug.Log($"StatusEffect.OnRemove (start)"); 

        // Remove icon and CD text in effect box/arena
        effectBox.ResetEffectBoxItem();
        //effectBox = null;

        //

        // Remove the statModifier from the target
        if (type == "affectWithModifiers" || type == "affectByBoth")
        {
            for (int i = 0; i < affectedStatWithModi.Count; i++)
            {
                switch (this.affectedStatWithModi[i])
                {
                    case "currentMaxHp":
                        effectTarget.currentMaxHp.RemoveModifier(affectedStatModifier[i]);
                        break;

                    case "currentAttackPoint":
                        effectTarget.currentAttackPoint.RemoveModifier(affectedStatModifier[i]);
                        break;

                    case "currentDefencePoint":
                        effectTarget.currentDefencePoint.RemoveModifier(affectedStatModifier[i]);
                        break;

                    case "currentEvasionPoint":
                        effectTarget.currentEvasionPoint.RemoveModifier(affectedStatModifier[i]);
                        break;

                    case "currentCriticalPoint":
                        effectTarget.currentCriticalPoint.RemoveModifier(affectedStatModifier[i]);
                        break;

                    case "currentDexterityPoint":
                        effectTarget.currentDexterityPoint.RemoveModifier(affectedStatModifier[i]);
                        break;    

                    // And more...
                }
            }
        }
        else
        {
            for (int i = 0; i < affectedStatsByDirectChange.Count; i++)
            {
                switch (affectedStatsByDirectChange[i])
                {
                    case "currentMaxHp":
                        effectTarget.currentMaxHp.value = effectTarget.maxHp.value;
                        break;

                    case "currentAttackPoint":
                        effectTarget.currentAttackPoint.value = effectTarget.attackPoint.value;
                        break;

                    case "currentDefencePoint":
                        effectTarget.currentDefencePoint.value = effectTarget.defencePoint.value;
                        break;

                    case "currentEvasionPoint":
                        effectTarget.currentEvasionPoint.value = effectTarget.evasionPoint.value;
                        break;

                    case "currentCriticalPoint":
                        effectTarget.currentCriticalPoint.value = effectTarget.criticalPoint.value;
                        break;

                    case "currentDexterityPoint":
                        effectTarget.currentDexterityPoint.value = effectTarget.dexterityPoint.value;
                        break;

                    // Testing
                    case "currentAttackInterval":
                        effectTarget.gameObject.GetComponent<Mob>().currentAttackInterval.value = effectTarget.gameObject.GetComponent<Mob>().currentMaxAttackInterval.value;
                        break;

                    case "currentMaxAttackInterval":
                        effectTarget.gameObject.GetComponent<Mob>().currentMaxAttackInterval.value = effectTarget.gameObject.GetComponent<Mob>().currentMaxAttackInterval.value;
                        break;

                    // And more...
                }
            }
        }

        // Remove the status effect flag from the target
        switch (this.effectName)
        {
            case StatusEffectName.Burning:
                effectTarget.isBurn = false;
                break;
            
            case StatusEffectName.Freezing:
                effectTarget.isFrozen = false;
                break;

            case StatusEffectName.Stuning:
                effectTarget.isStun = false;
                effectTarget.isSkipTurn = false;
                break;

            // And more...
        }

        effectTarget.currentStatusEffects.Remove(this);
        if (effectTarget.currentStatusEffects.Count == 0)
        {
            effectTarget.HasStatusEffect = false;
        }

        Debug.Log($"StatusEffect.OnRemove (end)"); 
    }

    public bool HasSameStatusEffect()
    {
        Debug.Log($"StatusEffect.HasSameStatusEffect (start)"); 

        bool hasSameEffect = false;

        for (int i = (effectTarget.currentStatusEffects.Count - 1); i >= 0; i--)
        {
            if (effectTarget.currentStatusEffects[i].effectName == this.effectName)
            {
                hasSameEffect = true;
            }
        }

        Debug.Log($"StatusEffect.HasSameStatusEffect, return hasSameEffect(local val): {hasSameEffect} (end)"); 
        return hasSameEffect;
    }

    public int GetOrderOfStatusEffect()
    {
        Debug.Log($"StatusEffect.GetOrderOfStatusEffect (start)"); 

        int orderOfStatusEffect = -1;
        for (int i = (effectTarget.currentStatusEffects.Count - 1); i >= 0; i--)
        {
            if (effectTarget.currentStatusEffects[i].effectName == this.effectName)
            {
                orderOfStatusEffect = i;
            }
        }

        Debug.Log($"StatusEffect.GetOrderOfStatusEffect, return orderOfStatusEffect(local val): {orderOfStatusEffect} (end)"); 
        return orderOfStatusEffect;
    }

    public void UpdateEffectBox()
    {
        Debug.Log($"StatusEffect.UpdateEffectBox (end)"); 

        int orderOfStatusEffect = GetOrderOfStatusEffect();

        if (effectBox.transform.GetSiblingIndex() != orderOfStatusEffect)
        {
            effectBox.ResetEffectBoxItem();
        }

        if (effectRemainingTurns <= 0)
        {
            OnRemove();
        }
        else 
        {
            if (effectTarget == roundData.player)
            {
                effectBox = roundData.playerEffectBoxes.transform.GetChild(GetOrderOfStatusEffect()).GetComponent<EffectBox>();
            }
            else if (effectTarget == roundData.currentMob)
            {
                effectBox = roundData.mobEffectBoxes.transform.GetChild(GetOrderOfStatusEffect()).GetComponent<EffectBox>();
            }

            effectBox.InitializeEffectBox(this);
        }

        Debug.Log($"StatusEffect.UpdateEffectBox (end)"); 
    }
}
