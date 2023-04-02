using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlueAndWhite.Entities;
using BlueAndWhite.Objects;

namespace BlueAndWhite.Abilities
{
    [System.Serializable]
    public class EntityStat
        {
            public float baseValue;

            // Last calculation value
            public float _value;
            public float value
            {
                get 
                {
                    if (isDirty)
                    {
                        _value = CalculateFinalValue();
                        isDirty = false;
                    }
                    return _value;
                }
                set
                {
                    _value = value;
                }

            }
            // Indicate if there's need to recalculate the value or not
            private bool isDirty = true;

            // By setting this as readonly, statModifiers cannot be assigning null/creating another new list. 
            // But still add/remove/modify elements.
            private readonly List<StatModifier> statModifiers;

            public EntityStat(float inputBaseValue)
            {
                baseValue = inputBaseValue;
                // By create this list, no more new list will be able to create
                statModifiers = new List<StatModifier>();
            }

            public void AddModifier(StatModifier statModifier)
            {
                isDirty = true;
                statModifiers.Add(statModifier);
                statModifiers.Sort(CompareModifierOrder);
            }

            public void RemoveModifier(StatModifier statModifier)
            {
                isDirty = true;
                statModifiers.Remove(statModifier);
            }

            public bool RemoveAllModifierFromSource(object source)
            {
                bool didRemove = false;

                for (int i = statModifiers.Count; i >= 0; i--)
                {
                    if (statModifiers[i].source == source)
                    {
                        isDirty = true;
                        didRemove = true;
                        statModifiers.RemoveAt(i);
                    }
                }
                return didRemove;
            }

            private int CompareModifierOrder(StatModifier statA, StatModifier statB)
            {
                if (statA.order < statB.order)
                {
                    return -1;
                }
                else if (statA.order > statB.order)
                {
                    return 1;
                }
                else // Which should be equal, right?
                {
                    return 0;
                }
            }

            private float CalculateFinalValue()
            {
                float finalValue = baseValue;
                float sumPercentAdd = 0;

                // Looping the list and apply all stat modifiers to the value
                for (int i = 0; i < statModifiers.Count; i++)
                {
                    StatModifier mod = statModifiers[i];

                    if (mod.type == StatModifierType.Flat)
                    {
                        finalValue += mod.value;
                    }
                    else if (mod.type == StatModifierType.PercentAdd)
                    {
                        // SumPercentAdd is the temp value for caluating all same stack percent type modifier
                        sumPercentAdd += mod.value;

                        // Add until next modifier is not stack percent type modifier
                        if (i + 1 >= statModifiers.Count || statModifiers[i + 1].type != StatModifierType.PercentAdd)
                        {
                            finalValue *= 1 + sumPercentAdd;
                            sumPercentAdd = 0;
                        }
                    }
                    else if (mod.type == StatModifierType.PercentMultiple)
                    {
                        finalValue *= 1 + mod.value;
                    }
                    
                }
                return (float)System.Math.Round(finalValue, 0);
            }

            public float GetStatValue()
            {
                return value;
            }

            
            public void SetStatValue(float inputValue)
            {
                this.value = inputValue;
            }
            
        }
}