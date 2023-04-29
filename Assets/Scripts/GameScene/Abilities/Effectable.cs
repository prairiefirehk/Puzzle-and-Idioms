using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effectable : MonoBehaviour 
{
    //[SerializeField] public Dictionary<StatusEffectName, StatusEffect> currentStatusEffects = new Dictionary<StatusEffectName, StatusEffect>();
    public List<StatusEffect> currentStatusEffects = new List<StatusEffect>();
    public bool hasStatusEffect = false;
    public bool isSkipTurn = false;
    public bool isStun = false;
    public bool isBurn = false;
    public bool isFrozen = false;

    #region Flow
    void Awake()
    {
        Debug.Log($"{Time.time} {name} Effectable.Awake (start)");
        Debug.Log($"{Time.time} {name} Effectable.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} {name} Effectable.OnEnable (start)");
        Debug.Log($"{Time.time} {name} Effectable.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} {name} Effectable.Start (start)");
        Debug.Log($"{Time.time} {name} Effectable.Start (end)");
    }

    void Update()
    {

    }
    
    void OnDisable()
    {
        Debug.Log($"{Time.time} {name} Effectable.OnDisable (start)");
        Debug.Log($"{Time.time} {name} Effectable.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} {name} Effectable.OnDestroy (start)");
        Debug.Log($"{Time.time} {name} Effectable.OnDestroy (end)");
    }
    #endregion

    public void CheckStatusEffects()
    {
        Debug.Log($"{name} Effectable.CheckStatusEffects (start)");

        if (hasStatusEffect)
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

        Debug.Log($"{name} Effectable.CheckStatusEffects (end)");
    }
}

