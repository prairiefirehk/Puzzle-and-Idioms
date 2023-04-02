using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlueAndWhite.Entities;
using BlueAndWhite.Objects;

namespace BlueAndWhite.Abilities
{
    public enum StatModifierType
    {
        Flat,

        // Stackable, eg. 10%+10% = 20% (original will become 120%)
        PercentAdd, 

        // Will multiply no matter what, will calculate after percentAdd, eg. 50% (original will become 120*1.5 = 180%)
        PercentMultiple, 
    }

    [System.Serializable]
    public class StatModifier
        {
            public readonly float value;
            public readonly StatModifierType type;
            public readonly int order;

            // For exterior reference who provide the modifier
            public readonly object source;

            public StatModifier(float inputValue, StatModifierType inputType, int inputOrder, object inputSource)
            {
                value = inputValue;
                type = inputType;
                order = inputOrder;
                source = inputSource;
            }

            // Serve as devault method if there's no order/source for the modifier.
            // Which the order will be according to the order of enum of stat modifier type
            public StatModifier(float inputValue, StatModifierType inputType) : this (inputValue, inputType, (int)inputType, null) {}
            public StatModifier(float inputValue, StatModifierType inputType, object inputSource) : this (inputValue, inputType, (int)inputType, inputSource) {}
            public StatModifier(float inputValue, StatModifierType inputType, int inputOrder) : this (inputValue, inputType, inputOrder, null) {}
        }
}