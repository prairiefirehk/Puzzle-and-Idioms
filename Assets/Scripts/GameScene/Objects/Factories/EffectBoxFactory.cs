using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EffectBoxFactory : MonoBehaviour, IFactory
{
    #region Scripts
    public RoundData roundData;
    #endregion

    #region Game object references
    public GameObject effectBoxPrefab;
    public GameObject effectBoxCells;
    public GameObject effectBoxSpawner;
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{Time.time} EffectBoxFactory.Awake (start)");

        // Not ideal place
        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();

        Debug.Log($"{Time.time} EffectBoxFactory.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} EffectBoxFactory.OnEnable (start)");
        Debug.Log($"{Time.time} EffectBoxFactory.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} EffectBoxFactory.Start (start)");
        Debug.Log($"{Time.time} EffectBoxFactory.Start (end)");
    }

    void Update()
    {
        
    }

    void OnDisable()
    {
        Debug.Log($"{Time.time} EffectBoxFactory.OnDisable (start)");
        Debug.Log($"{Time.time} EffectBoxFactory.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} EffectBoxFactory.OnDestroy (start)");
        Debug.Log($"{Time.time} EffectBoxFactory.OnDestroy (end)");
    }
    #endregion

    #region Factory functions
    public EffectBox CreateEffectBox(Entity target, StatusEffect statusEffect, int statusEffectOrder)
    {
        Debug.Log($"{Time.time} EffectBoxFactory.CreateEffectBox (start)");


        Transform effectBoxSpawnParent;
        if (target == roundData.currentMob)
        {
            effectBoxSpawnParent = roundData.mobEffectBoxes.transform;
        }
        else
        {
            effectBoxSpawnParent = roundData.playerEffectBoxes.transform;
        }

        EffectBox effectBox = Instantiate(effectBoxPrefab, effectBoxSpawnParent).GetComponent<EffectBox>();

        effectBox.statusEffect = statusEffect;
        effectBox.name = statusEffect.effectName.ToString();

        if (target == roundData.currentMob)
        {
            effectBox.effectBoxCellsParent = effectBoxCells.transform.GetChild(0).gameObject;
            //Debug.Log($"{Time.time} {effectBox.effectBoxCellsParent.name}'s local position is {effectBox.effectBoxCellsParent.transform.GetChild(statusEffectOrder).position}");
            effectBox.effectBoxPosition = effectBox.effectBoxCellsParent.transform.GetChild(statusEffectOrder).position;
            effectBox.effectBackground.transform.localPosition = new Vector2(35, 0);
            effectBox.effectIcon.transform.localPosition = new Vector2(35, 0);
            effectBox.effectCDText.transform.localPosition = new Vector2(-35, -10);
            effectBox.effectCDText.alignment = TextAlignmentOptions.Right;
        }
        else
        {
            effectBox.effectBoxCellsParent = effectBoxCells.transform.GetChild(1).gameObject;
            effectBox.effectBoxPosition = effectBox.effectBoxCellsParent.transform.GetChild(statusEffectOrder).position;
            effectBox.effectBackground.transform.localPosition = new Vector2(-35, 0);
            effectBox.effectIcon.transform.localPosition = new Vector2(-35, 0);
            effectBox.effectCDText.transform.localPosition = new Vector2(35, -10);
            effectBox.effectCDText.alignment = TextAlignmentOptions.Left;
        }
        effectBox.transform.position = effectBox.effectBoxPosition;

        Debug.Log($"$statusEffect.effectIconPicName = {statusEffect.effectIconPicName}");
        effectBox.iconPicName = statusEffect.effectIconPicName;
        Debug.Log($"$effectBox.iconPicName = {effectBox.iconPicName}");
        Sprite effectBoxIconOrgImage = Resources.Load<Sprite>($"Icons/{effectBox.iconPicName}");
        effectBox.effectIcon.sprite = effectBoxIconOrgImage;
        //effectBox.effectIcon.gameObject.SetActive(true);

        effectBox.effectCD = statusEffect.effectRemainingTurns;
        effectBox.effectCDText.text = effectBox.effectCD.ToString();
        //effectBox.effectCDText.gameObject.SetActive(true);

        
        effectBox.currentState = EffectBoxState.State.Occupied;
        Debug.Log($"$Effect box {effectBox.name} spawned at {effectBox.effectBoxCellsParent.name}'s {effectBox.statusEffect.GetOrderOfStatusEffect()} box");
        Debug.Log($"{Time.time} EffectBoxFactory.CreateEffectBox, return effectBox(local var): {effectBox.name}(effectBox.name only) (end)"); 
        return effectBox;
    }
    #endregion
}