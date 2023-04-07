using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlueAndWhite.Entities;
using BlueAndWhite.Objects;

namespace BlueAndWhite.Abilities
{
    [System.Serializable]
    public class Effectable : MonoBehaviour 
    {
        public Dictionary<StatusEffectType, StatusEffect> currentEffects = new Dictionary<StatusEffectType,StatusEffect>();
        
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

        
        /*
        public void Burning(Entity target, int effectTurns, int effectLevel, float effectValue)
        {
            // OnInflict
            StatusEffect burningEffect = new StatusEffect(EffectType.Burning, effectTurns, effectLevel, effectValue);
            StatModifier burningStatModifier = new StatModifier(-10, StatModifierType.Flat, this);
            target.attackPoint.AddModifier(burningStatModifier);

            // OnTurnStart
            if (target.currentEffects[key: EffectType.Burning].effectTurns > 0)
            {
                target.currentHp.value -= 10f;
            }

            // OnTurnEnd

            // OnRemoved
            if (target.currentEffects[EffectType.Burning].effectTurns == 0)
            {
                target.attackPoint.RemoveModifier(burningStatModifier);
            }
        }
        */
    }
}
