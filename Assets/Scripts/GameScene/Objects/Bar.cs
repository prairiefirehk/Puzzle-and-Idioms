using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Bar : MonoBehaviour
{
    #region Game object reference
    public Bar bars;
    public Image bar;
    public Image valueBar;
    public TMP_Text barValueText;
    #endregion

    #region Bar data
    public float barWidth;
    public float barHeight;
    public Vector2 barPosition;
    public float valueBarWidth;
    public float valueBarHeight;
    public Vector2 valueBarPosition;

    public float value;
    public float maxValue;

    public float lerpSpeed;
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{name} Bar.Awake (start)");
        Debug.Log($"{name} Bar.Awake (end)");
    }


    void OnEnable()
    {
        Debug.Log($"{name} Bar.OnEnable (start)");
        Debug.Log($"{name} Bar.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{name} Bar.Start (start)");
        Debug.Log($"{name} Bar.Start (end)");
    }
    void Update()
    {
        if (value < 0)
        {
            value = 0;
        }
        if (value > maxValue)
        {
            value = maxValue;
        }
    }

    void OnDisable()
    {
        Debug.Log($"{name} Bar.OnDisable (start)");
        Debug.Log($"{name} Bar.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{name} Bar.OnDestroy (start)");
        Debug.Log($"{name} Bar.OnDestroy (end)");
    }
    #endregion

    #region Bar functions
    public void InitializeBar(Bar barParent, float barValue, float barMaxValue)
    {
        Debug.Log($"{name} Bar.InitializeBar (start)");

        bars = barParent;
        bar = bars.transform.GetChild(0).GetComponent<Image>();
        barPosition = bar.transform.localPosition;
        valueBar = bars.transform.GetChild(1).GetComponent<Image>();
        valueBarPosition = valueBar.transform.localPosition;
        barValueText = bars.transform.GetChild(2).GetComponent<TMP_Text>();

        value = barValue;
        maxValue = barMaxValue;

        barWidth = bar.GetComponent<RectTransform>().rect.width;
        barHeight = bar.GetComponent<RectTransform>().rect.height;
        valueBarWidth = valueBar.GetComponent<RectTransform>().rect.width;
        valueBarHeight = valueBar.GetComponent<RectTransform>().rect.height;
        barValueText.text = value + "/" + maxValue;

        // Default 100%
        valueBarWidth = barWidth;

        Debug.Log($"{name} Bar.InitializeBar (end)");
    }
    public void ResizeBarValue(float newValue, float maxValue, int decimalplace)
    {
        //Debug.Log($"{name} Bar.ResizeBarValue (start)");

        //Debug.Log($"{name} old value = {value} and newValue is {newValue}");
        lerpSpeed = 3f * Time.deltaTime;
        value = Mathf.Lerp(value, newValue, lerpSpeed);

        // Processing bar
        valueBarWidth = value / maxValue * barWidth;
        valueBar.GetComponent<RectTransform>().sizeDelta = new Vector2(valueBarWidth, valueBarHeight);
        valueBar.transform.localPosition = new Vector2(barPosition.x - (barWidth - valueBarWidth) / 2, barPosition.y);

        // Processing bar text
        barValueText.text = value.ToString("F" + decimalplace) + "/" + maxValue;

        //Debug.Log($"{name} Bar.ResizeBarValue (end)");
    }
    #endregion
}
