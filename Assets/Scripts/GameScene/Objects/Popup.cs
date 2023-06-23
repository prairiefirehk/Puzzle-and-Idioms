using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Popup : MonoBehaviour
{
    #region Scripts
    public UIManage uiManage;
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

    #region Popup data
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
    [SerializeField] private int _popupID;
    public int popupID { get { return _popupID; } set { _popupID = value; } }
    [SerializeField] private float _popupDuration = 2f;
    public float popupDuration { get { return _popupDuration; } set { _popupDuration = value; } }

    // Just for turn msg
    [SerializeField] private TurnState.State _spawnTurnState;
    public TurnState.State spawnTurnState { get { return _spawnTurnState; } set { _spawnTurnState = value; } }

    #endregion


    #region Flow
    void Awake()
    {
        Debug.Log($"{Time.time} {name} Popup.Awake (start)");

        uiManage = GameObject.Find("UI Manager").GetComponent<UIManage>();

        Debug.Log($"{Time.time} {name} Popup.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} {name} Popup.OnEnable (start)");
        Debug.Log($"{Time.time} {name} Popup.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} {name} Popup.Start (start)");
        Debug.Log($"{Time.time} {name} Popup.Start (end)");
    }

    void Update()
    {
        
    }
    
    void OnDisable()
    {
        Debug.Log($"{Time.time} {name} Popup.OnDisable (start)");
        Debug.Log($"{Time.time} {name} Popup.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} {name} Popup.OnDestroy (start)");
        Debug.Log($"{Time.time} {name} Popup.OnDestroy (end)");
    }
    #endregion

    #region Popup functions
    public void DestroyPopup(int popupID)
    {
        Debug.Log($"{Time.time} {name} Popup.DestroyPopup (start)");

        Debug.Log($"{Time.time} Popup being destroyed: {uiManage.currentPopups[popupID].name}");
        Destroy(uiManage.currentPopups[popupID].gameObject);
        uiManage.currentPopups[popupID].transform.SetParent(null);
        uiManage.currentPopups.Remove(popupID);

        Debug.Log($"{Time.time} {name} Popup.DestroyPopup (end)");
    }
    #endregion
}
