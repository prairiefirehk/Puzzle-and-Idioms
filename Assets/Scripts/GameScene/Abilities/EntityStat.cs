using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

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
        Debug.Log($"{Time.time} EntityStat.AddModifier (start)");

        isDirty = true;
        _statModifiers.Add(statModifier);
        _statModifiers.Sort(CompareModifierOrder);

        Debug.Log($"{Time.time} EntityStat.AddModifier (end)");
    }

    public virtual bool RemoveModifier(StatModifier statModifier)
    {
        Debug.Log(message: $"{Time.time} EntityStat.RemoveModifier (start)");

        bool isRemoved = false;
        if (_statModifiers.Remove(statModifier))
        {
            isDirty = true;
            isRemoved = true;
        }

        Debug.Log($"{Time.time} EntityStat.RemoveModifier, return isRemoved(local var): {isRemoved} (end)");
        return isRemoved;
    }

    public virtual bool RemoveAllModifierFromSource(object source)
    {
        Debug.Log(message: $"{Time.time} EntityStat.RemoveAllModifierFromSource (start)");

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

        Debug.Log($"{Time.time} EntityStat.RemoveAllModifierFromSource, return didRemove(local var): {didRemove} (end)");
        return didRemove;
    }

    protected virtual int CompareModifierOrder(StatModifier statA, StatModifier statB)
    {
        Debug.Log(message: $"{Time.time} EntityStat.CompareModifierOrder (start)");

        int controlNum;
        if (statA.order < statB.order)
        {
            controlNum = -1;
        }
        else if (statA.order > statB.order)
        {
            controlNum = 1;
        }
        else // Which should be equal, right?
        {
            controlNum = 0;
        }

        Debug.Log($"{Time.time} EntityStat.CompareModifierOrder, return controlNum(local var): {controlNum} (end)");
        return controlNum;
    }

    protected virtual float CalculateFinalValue()
    {
        Debug.Log(message: $"{Time.time} EntityStat.CalculateFinalValue (start)");

        float finalValue = baseValue;
        float sumPercentStack = 0;

        // Looping the list and apply all stat modifiers to the value
        for (int i = 0; i < _statModifiers.Count; i++)
        {
            StatModifier mod = _statModifiers[i];

            if (mod.type == StatModifierType.Flat)
            {
                finalValue += mod.value;
            }
            else if (mod.type == StatModifierType.PercentStack)
            {
                // SumPercentStack is the temp value for caluating all same stack percent type modifier
                sumPercentStack += mod.value;

                // Add until next modifier is not stack percent type modifier
                if (i + 1 >= _statModifiers.Count || _statModifiers[i + 1].type != StatModifierType.PercentStack)
                {
                    finalValue = (100 + sumPercentStack)/100 * finalValue;
                    sumPercentStack = 0;
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

        Debug.Log($"{Time.time} EntityStat.CalculateFinalValue, return finalValue with Math.Round(local var): {(float)Mathf.Round(finalValue)} and without Math.Round: {finalValue} (end)");
        return finalValue;
    }

    public float GetStatValue()
    {
        //Debug.Log(message: $"{Time.time} EntityStat.GetStatValue (start)");

        //Debug.Log(message: $"{Time.time} EntityStat.GetStatValue, return value(local var): {value} (end)");
        return value;
    }

    
    public void SetStatValue(float inputValue)
    {
        //Debug.Log(message: $"{Time.time} EntityStat.SetStatValue (start)");

        this.value = inputValue;

        //Debug.Log(message: $"{Time.time} EntityStat.SetStatValue (end)");
    }
    
}
