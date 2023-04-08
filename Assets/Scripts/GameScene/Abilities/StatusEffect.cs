using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using BlueAndWhite.Entities;
using BlueAndWhite.Objects;

namespace BlueAndWhite.Abilities
{
    [System.Serializable]
    public class StatusEffect 
        {
            private StatusEffectType _effectType;
            public StatusEffectType effectType { get { return _effectType; } set { _effectType = value; } }
            
            private int _effectTurns;
            public int effectTurns { get { return _effectTurns; } set { _effectTurns = value; } }
            private int _effectRemainingTurns;
            public int effectRemainingTurns { get { return _effectRemainingTurns; } set { _effectRemainingTurns = value; } }
            private int _effectLevel;
            public int effectLevel { get { return _effectLevel; } set { _effectLevel = value; } }
            private float _effectValue;
            public float effectValue { get { return _effectValue; } set { _effectValue = value; } }

            private Entity _effectTarget;
            public Entity effectTarget { get { return _effectTarget; } set { _effectTarget = value; } }
            private string _affectedStat;
            public string affectedStat { get { return _affectedStat; } set { _affectedStat = value; } }


            public StatusEffect(StatusEffectType effectType, int effectTurns, int effectLevel, float effectValue, Entity effectTarget, string affectedStat)
            {
                this.effectType = effectType;
                this.effectTurns = effectTurns;
                this.effectLevel = effectLevel;
                this.effectValue = effectValue;
                this.effectTarget = effectTarget;
                this.affectedStat = affectedStat;
            }


            // Within the turn that entity action to the target which giving status effect? Or before starting new turn?
            // Called only once at start
            public void OnInflict()
            {
                // What if there's more than one?
                effectTarget[affectedStat].AddModifier(new StatModifier(effectValue, StatModifierType.Flat, this));
            }

            // On every new turn
            public void OnTurnStart()
            {
                // 
                if (effectTarget.currentEffects[key: effectType].effectTurns > 0)
                {
                    // Do something with effectValue
                    //target.affectedStat.value -= 10f;
                    
                }
            }

            // On every turn ending
            public void OnTurnEnd()
            {
                // 
                if (effectTarget.currentEffects[key: effectType].effectTurns > 0)
                {
                    // Do something with effectValue
                    //target.affectedStat.value -= 10f;
                    
                }
            }

            // On removing itself
            public void OnRemove()
            {
                // 
                if (effectTarget.currentEffects[key: effectType].effectTurns > 0)
                {
                    // Do something with effectValue
                    //target.affectedStat.value -= 10f;
                    
                }
            }
        }
}