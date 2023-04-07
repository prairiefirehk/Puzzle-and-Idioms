using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using BlueAndWhite.Entities;
using BlueAndWhite.Objects;

namespace BlueAndWhite.Abilities
{
    [System.Serializable]
    public class EntityStat
        {
            protected float baseValue;
            // Default setting lowest value?
            protected float lastBaseValue = float.MinValue;
            // Last calculation value
            protected float _value;
            public virtual float value
            {
                get 
                {
                    if (isDirty || baseValue != lastBaseValue)
                    {
                        lastBaseValue = baseValue;
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
            protected bool isDirty = true;

            // By setting this as readonly, statModifiers cannot be assigning null/creating another new list
            // This list varible will not able to be pointing other varibles
            // But still able to add/remove/modify elements
            protected readonly List<StatModifier> _statModifiers;
            // Readonly cannot prevent to let outside changing the list and we cannot check the modifiers from outside,
            // So now creating a public reference for outside, which still prohibit changes the actual list? 
            // And same reason to set this as readonly so that this var will not pointing to others after init
            public readonly ReadOnlyCollection<StatModifier> statModifiers;

            public EntityStat()
            {
                // By create this list, no more new list will be able to create
                _statModifiers = new List<StatModifier>();
                // Now the public readonly collection will have a reference to the original stat modifier list
                // This readonly collection will always reflect changes in the original list
                statModifiers = _statModifiers.AsReadOnly();
            }
            public EntityStat(float inputBaseValue) : this()
            {
                baseValue = inputBaseValue;
            }

            public virtual void AddModifier(StatModifier statModifier)
            {
                isDirty = true;
                _statModifiers.Add(statModifier);
                _statModifiers.Sort(CompareModifierOrder);
            }

            public virtual bool RemoveModifier(StatModifier statModifier)
            {
                if (_statModifiers.Remove(statModifier))
                {
                    isDirty = true;
                    return true;
                }
                return false;
            }

            public virtual bool RemoveAllModifierFromSource(object source)
            {
                bool didRemove = false;

                for (int i = _statModifiers.Count; i >= 0; i--)
                {
                    if (_statModifiers[i].source == source)
                    {
                        isDirty = true;
                        didRemove = true;
                        _statModifiers.RemoveAt(i);
                    }
                }
                return didRemove;
            }

            protected virtual int CompareModifierOrder(StatModifier statA, StatModifier statB)
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

            protected virtual float CalculateFinalValue()
            {
                float finalValue = baseValue;
                float sumPercentAdd = 0;

                // Looping the list and apply all stat modifiers to the value
                for (int i = 0; i < _statModifiers.Count; i++)
                {
                    StatModifier mod = _statModifiers[i];

                    if (mod.type == StatModifierType.Flat)
                    {
                        finalValue += mod.value;
                    }
                    else if (mod.type == StatModifierType.PercentAdd)
                    {
                        // SumPercentAdd is the temp value for caluating all same stack percent type modifier
                        sumPercentAdd += mod.value;

                        // Add until next modifier is not stack percent type modifier
                        if (i + 1 >= _statModifiers.Count || _statModifiers[i + 1].type != StatModifierType.PercentAdd)
                        {
                            finalValue *= 1 + sumPercentAdd;
                            sumPercentAdd = 0;
                        }
                    }
                    else if (mod.type == StatModifierType.PercentMultiple)
                    {
                        finalValue *= 1 + mod.value;
                    }
                    else if (mod.type == StatModifierType.Equal)
                    {
                        finalValue = mod.value;
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