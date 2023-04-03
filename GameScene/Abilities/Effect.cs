using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlueAndWhite.Entities;
using BlueAndWhite.Objects;

namespace BlueAndWhite.Abilities
{
    [System.Serializable]
    public class Effect 
        {
            private Effectable.EffectName _effectName;
            public Effectable.EffectName effectName { get { return _effectName; } set { _effectName = value; } }
            private int _effectTurns;
            public int effectTurns { get { return _effectTurns; } set { _effectTurns = value; } }
            private int _effectRemainingTurns;
            public int effectRemainingTurns { get { return _effectRemainingTurns; } set { _effectRemainingTurns = value; } }
            private int _effectLevel;
            public int effectLevel { get { return _effectLevel; } set { _effectLevel = value; } }
            private float _effectValue;
            public float effectValue { get { return _effectValue; } set { _effectValue = value; } }

            public Effect(Effectable.EffectName effectName, int effectTurns, int effectLevel, float effectValue)
            {
                this.effectName = effectName;
                this.effectTurns = effectTurns;
                this.effectLevel = effectLevel;
                this.effectValue = effectValue;
            }

            // HOW
            public void ApplyEffect(Effectable.EffectName effectName, int effectTurns, int effectLevel, Entity target, EntityStat stat, float effectValue)
            {
                // Should move to other place? You can't create new effect in a effect?
                //Effect effect = new Effect(effectName, effectTurns, effectLevel, effectValue);
                //target.stat.AddModifier(new StatModifier(effectValue, StatModifierType.Flat, this));

            }

            public void RemoveEffect(Entity target, EntityStat stat)
            {
                //target.stat.RemoveAllModifierFromSource(this);
            }
        }
}