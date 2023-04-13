using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
    public GameObject[] panels;
    public Button[] buttons;

    void Awake()
    {
        Debug.Log($"MenuNavigation.Awake (start)");

        Debug.Log($"MenuNavigation.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"MenuNavigation.OnEnable (start)");

        Debug.Log($"MenuNavigation.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"MenuNavigation.Start (start)");
        Debug.Log($"MenuNavigation.Start (end)");
    }
    void Update()
    {

    }

    void OnDisable()
    {
        Debug.Log($"MenuNavigation.OnDisable (start)");
        Debug.Log($"MenuNavigation.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"MenuNavigation.OnDestroy (start)");
        Debug.Log($"MenuNavigation.OnDestroy (end)");
    }
    
    public void navigationPanelChange(GameObject activePanel)
    {
        Debug.Log($"MenuNavigation.navigationPanelChange (start)");

        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        activePanel.SetActive(true);

        Debug.Log($"MenuNavigation.navigationPanelChange (end)");
    }

    public void navigationBarItemChange(Button buttonOnActive)
    {
        Debug.Log($"MenuNavigation.navigationBarItemChange (start)");

        foreach (Button button in buttons)
        {
            button.transform.GetChild(0).gameObject.SetActive(false);
            button.transform.GetChild(1).gameObject.SetActive(true);
            button.transform.GetChild(2).gameObject.SetActive(false);
            button.transform.GetChild(3).gameObject.SetActive(true);
        }
        buttonOnActive.transform.GetChild(0).gameObject.SetActive(true);
        buttonOnActive.transform.GetChild(1).gameObject.SetActive(false);
        buttonOnActive.transform.GetChild(2).gameObject.SetActive(true);
        buttonOnActive.transform.GetChild(3).gameObject.SetActive(false);

        Debug.Log($"MenuNavigation.navigationBarItemChange (end)");
    }
}
