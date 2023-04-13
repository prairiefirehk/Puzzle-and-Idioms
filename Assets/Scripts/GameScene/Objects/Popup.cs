using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Popup : MonoBehaviour
{
    #region Data of popup
    public PopupData popupData;
    [SerializeField] private string _sizeType;
    public string sizeType { get { return _sizeType; } set { _sizeType = value; } }
    [SerializeField] private int _caseRefID;
    public int caseRefID { get { return _caseRefID; } set { _caseRefID = value; } }
    [SerializeField] private string _title;
    public string title { get { return _title; } set { _title = value; } }
    [SerializeField] private string _body;
    public string body { get { return _body; } set { _body = value; } }
    [SerializeField] private string _confirmText;
    public string confirmText { get { return _confirmText; } set { _confirmText = value; } }
    [SerializeField] private string _dismissText;
    public string dismissText { get { return _dismissText; } set { _dismissText = value; } }
    #endregion

    #region Game object references
    public GameObject popupWindowBox;
    public GameObject popupWindowBackground;
    public List<Button> buttons;

    public Image titleImage;
    public Image titleBackground;
    public TMP_Text titleText;
    public GameObject bodyHolder;
    public GameObject bodyTextBox;
    public TMP_Text bodyText;
    public GameObject buttonHolder;
    public Button confirmBtn;
    public TMP_Text confirmBtnText;
    public Button dismissBtn;
    public TMP_Text dismissBtnText;

    // Special elements inside gg popup
    public Image timeIcon;
    public GameObject timeUsedTextBox;
    public TMP_Text timeUsedText;

    public GameObject starPrefab;
    public GameObject starIconBackground;
    public GameObject starIconHolder;

    public Image toolIcon;
    public GameObject toolGainedTextBox;
    public TMP_Text toolGainedText;

    public Image expIcon;
    public GameObject expGainedTextBox;
    public TMP_Text expGainedText;

    public Image coinIcon;
    public GameObject coinGainedTextBox;
    public TMP_Text coinGainedText;

    public Image jadeIcon;
    public GameObject jadeGainedTextBox;
    public TMP_Text jadeGainedText;

    public GameObject toolGainedHolder;
    #endregion

    #region Flow
    void Awake()
    {
        Debug.Log($"{name} Popup.Awake (start)");
        Debug.Log($"{name} Popup.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{name} Popup.OnEnable (start)");
        Debug.Log($"{name} Popup.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{name} Popup.Start (start)");
        Debug.Log($"{name} Popup.Start (end)");
    }

    void Update()
    {
        
    }
    
    void OnDisable()
    {
        Debug.Log($"{name} Popup.OnDisable (start)");
        Debug.Log($"{name} Popup.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{name} Popup.OnDestroy (start)");
        Debug.Log($"{name} Popup.OnDestroy (end)");
    }

    public void DestroyPopup(Popup popupPrefab)
    {
        Debug.Log($"{name} Popup.DestroyPopup (start)");

        Debug.Log($"Popup being destroyed: {popupPrefab.name}");

        Destroy(popupPrefab.gameObject);
        popupPrefab.transform.SetParent(null);

        Debug.Log($"{name} Popup.DestroyPopup (end)");
    }
    #endregion
}
