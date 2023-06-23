using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Affectable : MonoBehaviour 
{
    //[SerializeField] public Dictionary<StatusEffectName, StatusEffect> currentStatusEffects = new Dictionary<StatusEffectName, StatusEffect>();
    public List<StatusEffect> currentStatusEffects = new List<StatusEffect>();
    public bool hasStatusEffect = false;
    public bool isSkipTurn = false;

    #region Flow
    void Awake()
    {
        Debug.Log($"{Time.time} {name} Affectable.Awake (start)");
        Debug.Log($"{Time.time} {name} Affectable.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} {name} Affectable.OnEnable (start)");
        Debug.Log($"{Time.time} {name} Affectable.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} {name} Affectable.Start (start)");
        Debug.Log($"{Time.time} {name} Affectable.Start (end)");
    }

    void Update()
    {

    }
    
    void OnDisable()
    {
        Debug.Log($"{Time.time} {name} Affectable.OnDisable (start)");
        Debug.Log($"{Time.time} {name} Affectable.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} {name} Affectable.OnDestroy (start)");
        Debug.Log($"{Time.time} {name} Affectable.OnDestroy (end)");
    }
    #endregion

    public void CheckStatusEffects()
    {
        Debug.Log($"{name} Affectable.CheckStatusEffects (start)");

        Debug.Log($"{name} Affectable.CheckStatusEffects (end)");
    }
}

