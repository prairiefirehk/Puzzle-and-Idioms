using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatModifierType
{
    // Setting enum as 3-digit system as to leave space for future expension
    // Direct add/minus
    Flat = 100,

    // Stackable, eg. 10%+10% = 20% (original will become 120%)
    PercentStack = 200,

    // Will multiply no matter what, will calculate after PercentStack, eg. 50% (original will become 120*1.5 = 180%)
    PercentMultiple = 300, 

    // Directly set the number equal to that number (eg. = 20)
    Equal = 400
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
