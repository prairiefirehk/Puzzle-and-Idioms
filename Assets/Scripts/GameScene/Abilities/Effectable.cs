using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effectable : MonoBehaviour 
{
    //public Dictionary<StatusEffectType, StatusEffect> currentStatusEffects = new Dictionary<StatusEffectType,StatusEffect>();
    public List<StatusEffect> currentStatusEffects = new List<StatusEffect>();
    public bool HasStatusEffect = false;
    public bool isSkipTurn = false;
    public bool isStun = false;
    public bool isBurn = false;
    public bool isFrozen = false;

    #region Flow
    void Awake()
    {
        Debug.Log($"{name} Effectable.Awake (start)");
        Debug.Log($"{name} Effectable.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{name} Effectable.OnEnable (start)");
        Debug.Log($"{name} Effectable.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{name} Effectable.Start (start)");
        Debug.Log($"{name} Effectable.Start (end)");
    }

    void Update()
    {

    }
    
    void OnDisable()
    {
        Debug.Log($"{name} Effectable.OnDisable (start)");
        Debug.Log($"{name} Effectable.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{name} Effectable.OnDestroy (start)");
        Debug.Log($"{name} Effectable.OnDestroy (end)");
    }
    #endregion
    /*    
    public void ApplyEffect(StatusEffect effect)
    {
        // If there's new same status effect appling, just "renew" the turns of that status effect?
        // Entity can only have one same status effect
        if (HasStatusEffect(effect.effectType))
        {
            currentEffects[effect.effectType].effectRemainingTurns = effect.effectTurns;
        }

        currentEffects.Add(effect.effectType, effect);
    }

    public void RemoveEffect(StatusEffect effect)
    {
        currentEffects.Remove(effect.effectType);
    }

    public bool HasStatusEffect(StatusEffectType type)
    {
        foreach (KeyValuePair<StatusEffectType, StatusEffect> effect in currentEffects)
        {
            if (effect.Value.effectType == type)
            {
                return true;
            }
        }
        return false;
    }

    // OnTurnStart or OnTurnEnd?
    public void ProcessStatusEffects()
    {
        foreach (KeyValuePair<StatusEffectType, StatusEffect> effect in currentEffects) 
        {
            effect.Value.effectRemainingTurns--;
            switch (effect.Value.effectType) 
            {
                case StatusEffectType.Burning:
                    // Code
                    break;
                case StatusEffectType.Slow:
                    // Code 
                    break;
                case StatusEffectType.Stun:
                    // Code 
                    break;
            }

            if (effect.Value.effectRemainingTurns <= 0) 
            {
                currentEffects.Remove(effect.Value.effectType);
            }
        }
    }
    */

    public void CheckStatusEffects()
    {
        if (HasStatusEffect)
        {
            if (isBurn)
            {
                // Do the damage
            }

            if (isFrozen)
            {
                // Do the damage
            }

            // The last calculate, before counting the damage/effects
            if (isStun)
            {
                // Skip the turn
                isSkipTurn = true;
            }

            // ETC
        }
    }
}

