using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EffectBox : MonoBehaviour
{
    #region Game object reference
    public EffectBox effectBox;
    public Image effectBoxBackground;
    public Image effectIcon;
    public TMP_Text effectCDText;
    #endregion

    #region Effect box data
    public string iconPicName;
    public int effectCD;
    public Vector2 effectBoxPosition;
    public StatusEffect statusEffect;
    public EffectBoxState.State currentState = EffectBoxState.State.Idle;

    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{name} EffectBox.Awake (start)");

        effectBox = this.gameObject.GetComponent<EffectBox>();
        effectBoxBackground = effectBox.transform.GetChild(0).GetComponent<Image>();
        effectIcon = effectBox.transform.GetChild(1).GetComponent<Image>();
        effectCDText = effectBox.transform.GetChild(2).GetComponent<TMP_Text>();

        Debug.Log($"{name} EffectBox.Awake (end)");
    }


    void OnEnable()
    {
        Debug.Log($"{name} EffectBox.OnEnable (start)");
        Debug.Log($"{name} EffectBox.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{name} EffectBox.Start (start)");
        Debug.Log($"{name} EffectBox.Start (end)");
    }
    void Update()
    {

    }

    void OnDisable()
    {
        Debug.Log($"{name} EffectBox.OnDisable (start)");
        Debug.Log($"{name} EffectBox.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{name} EffectBox.OnDestroy (start)");
        Debug.Log($"{name} EffectBox.OnDestroy (end)");
    }
    #endregion

    #region Effect box functions
    public void InitializeEffectBox(StatusEffect statusEffect)
    {
        Debug.Log($"{name} EffectBox.InitializeEffectBox (start)");

        iconPicName = GetEffectBoxIconName(statusEffect.effectName);
        Sprite effectBoxIconOrgImage = Resources.Load<Sprite>($"Icons/{iconPicName}");
        effectIcon.sprite = effectBoxIconOrgImage;
        effectIcon.gameObject.SetActive(true);

        effectCD = statusEffect.effectRemainingTurns;
        effectCDText.text = effectCD.ToString();
        effectCDText.gameObject.SetActive(true);

        this.statusEffect = statusEffect;
        currentState = EffectBoxState.State.Occupied;

        Debug.Log($"{name} EffectBox.InitializeEffectBox (end)");
    }

    public void UpdateEffectBoxCDText()
    {
        Debug.Log($"{name} EffectBox.UpdateEffectBoxCDText (start)");

        if (currentState == EffectBoxState.State.Occupied)
        {
            effectCD = statusEffect.effectRemainingTurns;
            effectCDText.text = effectCD.ToString();
        }

        Debug.Log($"{name} EffectBox.UpdateEffectBoxCDText (end)");
    }

    public string GetEffectBoxIconName(StatusEffectName name)
    {
        Debug.Log($"{name} EffectBox.GetEffectBoxIconName (start)");

        switch(name)
        {
            case StatusEffectName.Burning:
                iconPicName = "Burn_effect_icon_red_60_hq";
                break;

            case StatusEffectName.Freezing:
                iconPicName = "Frozen_effect_icon_blue_60_hq";
                break;
                    
            case StatusEffectName.Stuning:
                iconPicName = "Stun_effect_icon_yellow_60_hq";
                break;
                    
            default:
                iconPicName = "Burn_effect_icon_red_60_hq";
                Debug.Log("Hey dude can't load effect box icon's name!");
                break;
        }
        Debug.Log($"{name} EffectBox.GetEffectBoxIconName, return picName(local var): {iconPicName} (end)");
        return iconPicName;
    }

    public void ResetEffectBoxItem()
    {
        Debug.Log($"{name} EffectBox.ResetEffectBoxItem (start)");

        statusEffect = null;

        iconPicName = "";
        effectIcon.sprite = null;
        effectIcon.gameObject.SetActive(value: false);

        effectCD = -1;
        effectCDText.gameObject.SetActive(false);

        currentState = EffectBoxState.State.Idle;

        Debug.Log($"{name} EffectBox.ResetEffectBoxItem (end)");
    }
    #endregion
}
