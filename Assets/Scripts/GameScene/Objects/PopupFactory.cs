using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

//[System.Serializable]
public class PopupFactory : MonoBehaviour, IFactory
{
    #region Scripts
    public RoundData roundData;
    #endregion

    #region Game object references
    public Button confirmBtnPrefab;
    public Button dismissBtnPrefab;
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"PopupFactory.Awake (start)");

        // Not ideal place
        roundData = gameObject.GetComponent<RoundData>();

        Debug.Log($"PopupFactory.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"PopupFactory.OnEnable (start)");
        Debug.Log($"PopupFactory.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"PopupFactory.Start (start)");
        Debug.Log($"PopupFactory.Start (end)");
    }

    void Update()
    {
        
    }
    
    void OnDisable()
    {
        Debug.Log($"PopupFactory.OnDisable (start)");
        Debug.Log($"PopupFactory.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"PopupFactory.OnDestroy (start)");
        Debug.Log($"PopupFactory.OnDestroy (end)");
    }
    #endregion

    #region Factory functions
    public Popup CreatePopup(GameObject popupPrefab, GameObject parent, int caseRefID)
    {
        Debug.Log($"PopupFactory.CreatePopup (start)");

        Popup popup = Instantiate(popupPrefab, parent.transform).GetComponent<Popup>();

        popup.title = ImportData.popups.popup[caseRefID].title.ToString();
        popup.titleText.text = popup.title;

        popup.body = ImportData.popups.popup[caseRefID].body.ToString();
        popup.bodyText.text = popup.body;

        // In Unity inspector name
        popup.name = popup.title;

        Debug.Log($"PopupFactory.CreatePopup, return popup(local var): {popup.name}(popup.name only) (end)");
        return popup;
    }

    public Popup CreatePopup(GameObject popupPrefab, GameObject parent, int caseRefID, UnityAction buttonAction1)
    {
        Debug.Log($"PopupFactory.CreatePopup (start)");

        Popup popup = Instantiate(popupPrefab, parent.transform).GetComponent<Popup>();

        popup.title = ImportData.popups.popup[caseRefID].title.ToString();
        popup.titleText.text = popup.title;

        popup.body = ImportData.popups.popup[caseRefID].body.ToString();
        popup.bodyText.text = popup.body;

        popup.confirmBtn = Instantiate(confirmBtnPrefab, popup.buttonHolder.transform);
        popup.confirmBtnText = popup.confirmBtn.transform.GetChild(0).GetComponent<TMP_Text>();
        popup.confirmText = ImportData.popups.popup[caseRefID].confirmText.ToString();
        popup.confirmBtnText.text = popup.confirmText;
        popup.buttons.Add(popup.confirmBtn);

        popup.confirmBtn.onClick.AddListener(buttonAction1);

        // In Unity inspector name
        popup.name = popup.title;

        Debug.Log($"PopupFactory.CreatePopup, return popup(local var): {popup.name}(popup.name only) (end)");
        return popup;
    }

    public Popup CreatePopup(GameObject popupPrefab, GameObject parent, int caseRefID, UnityAction buttonAction1, UnityAction buttonAction2)
    {
        Debug.Log($"PopupFactory.CreatePopup (start)");

        Popup popup = Instantiate(popupPrefab, parent.transform).GetComponent<Popup>();

        popup.title = ImportData.popups.popup[caseRefID].title.ToString();
        popup.titleText.text = popup.title;

        try
        {
            popup.timeUsedText.text = roundData.timeUsed.ToString();
        }
        catch
        {
            Debug.Log("Not a gameover popup!");
        }

        popup.body = ImportData.popups.popup[caseRefID].body.ToString();
        popup.bodyText.text = popup.body;

        try
        {
            popup.expGainedText.text = roundData.expGained.ToString();
            popup.coinGainedText.text = roundData.coinGained.ToString();
            popup.jadeGainedText.text = roundData.jadeGained.ToString();
        }
        catch
        {
            Debug.Log("Not a gameover popup!");
        }

        popup.dismissBtn = Instantiate(dismissBtnPrefab, popup.buttonHolder.transform);
        popup.dismissBtnText = popup.dismissBtn.transform.GetChild(0).GetComponent<TMP_Text>();
        popup.dismissText = ImportData.popups.popup[caseRefID].dismissText.ToString();
        popup.dismissBtnText.text = popup.dismissText;
        popup.buttons.Add(popup.dismissBtn);

        popup.confirmBtn = Instantiate(confirmBtnPrefab, popup.buttonHolder.transform);
        popup.confirmBtnText = popup.confirmBtn.transform.GetChild(0).GetComponent<TMP_Text>();
        popup.confirmText = ImportData.popups.popup[caseRefID].confirmText.ToString();
        popup.confirmBtnText.text = popup.confirmText;
        popup.buttons.Add(popup.confirmBtn);

        popup.dismissBtn.onClick.AddListener(buttonAction1);
        popup.confirmBtn.onClick.AddListener(buttonAction2);

        // In Unity inspector name
        popup.name = popup.title;

        Debug.Log($"PopupFactory.CreatePopup, return popup(local var): {popup.name}(popup.name only) (end)");
        return popup;
    }
    #endregion
}