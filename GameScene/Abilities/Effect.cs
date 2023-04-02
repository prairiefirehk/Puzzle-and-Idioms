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

            public Effect(Effectable.EffectName effectName, int effectTurns, int effectLevel)
            {
                this.effectName = effectName;
                this.effectTurns = effectTurns;
                this.effectLevel = effectLevel;
            }
        }
}