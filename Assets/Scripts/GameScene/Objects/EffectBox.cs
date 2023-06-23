using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EffectBox : MonoBehaviour
{
    #region Scripts
    public RoundData roundData;
    #endregion
    #region Game object reference
    //public GameObject effectPrefab;
    public Image effectBackground;
    public Image effectIcon;
    public TMP_Text effectCDText;
    #endregion

    #region Effect box data
    public string iconPicName;
    public int effectCD;
    public Vector2 effectBoxPosition;
    public GameObject effectBoxCellsParent;
    public StatusEffect statusEffect;
    public EffectBoxState.State currentState = EffectBoxState.State.Initalizing;

    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{Time.time} {name} EffectBox.Awake (start)");

        // Not ideal place
        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();

        Debug.Log($"{Time.time} {name} EffectBox.Awake (end)");
    }


    void OnEnable()
    {
        Debug.Log($"{Time.time} {name} EffectBox.OnEnable (start)");
        Debug.Log($"{Time.time} {name} EffectBox.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} {name} EffectBox.Start (start)");
        Debug.Log($"{Time.time} {name} EffectBox.Start (end)");
    }
    void Update()
    {
        UpdateEffectBox();
    }

    void OnDisable()
    {
        Debug.Log($"{Time.time} {name} EffectBox.OnDisable (start)");
        Debug.Log($"{Time.time} {name} EffectBox.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} {name} EffectBox.OnDestroy (start)");
        Debug.Log($"{Time.time} {name} EffectBox.OnDestroy (end)");
    }
    #endregion

    #region Effect box functions

    public void UpdateEffectBox()
    {
        Debug.Log($"{Time.time} {name} EffectBox.UpdateEffectBoxCDText (start)");

        effectCD = statusEffect.effectRemainingTurns;
        effectCDText.text = effectCD.ToString();

        if (effectCD != 0)
        {
            transform.position = effectBoxCellsParent.transform.GetChild(statusEffect.GetOrderOfStatusEffect()).position;
            effectBoxPosition = transform.position;
        }

        Debug.Log($"$Effect box {name} is now at {effectBoxCellsParent.name}'s {statusEffect.GetOrderOfStatusEffect()} box, effectCD is {effectCD}");
        Debug.Log($"{Time.time} {name} EffectBox.UpdateEffectBoxCDText (end)");
    }
    
    public void DestroyEffectBox()
    {
        Debug.Log($"{Time.time} {name} EffectBox.ResetEffectBoxItem (start)");

        Destroy(gameObject);
        gameObject.transform.SetParent(null);

        Debug.Log($"{Time.time} {name} EffectBox.ResetEffectBoxItem (end)");
    }
    #endregion
}
