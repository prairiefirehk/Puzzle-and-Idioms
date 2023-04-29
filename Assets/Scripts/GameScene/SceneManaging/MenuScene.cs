using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{
    public GameObject[] panels;
    public Button[] buttons;
    public GameObject homePanel;
    public Button homeButton;
    void Awake()
    {
        Debug.Log($"{Time.time} MenuScene.Awake (start)");

        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        homePanel.SetActive(true);

        foreach (Button button in buttons)
        {
            button.transform.GetChild(0).gameObject.SetActive(false);
            button.transform.GetChild(1).gameObject.SetActive(true);
            button.transform.GetChild(2).gameObject.SetActive(false);
            button.transform.GetChild(3).gameObject.SetActive(true);
        }

        homeButton.transform.GetChild(0).gameObject.SetActive(true);
        homeButton.transform.GetChild(1).gameObject.SetActive(false);
        homeButton.transform.GetChild(2).gameObject.SetActive(true);
        homeButton.transform.GetChild(3).gameObject.SetActive(false);

        Debug.Log($"{Time.time} MenuScene.Awake (end)");
    }

    void OnEnable()
    {
        Debug.Log($"{Time.time} MenuScene.OnEnable (start)");
        Debug.Log($"{Time.time} MenuScene.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log($"{Time.time} MenuScene.Start (start)");
        Debug.Log($"{Time.time} MenuScene.Start (end)");
    }
    void Update()
    {

    }

    void OnDisable()
    {
        Debug.Log($"{Time.time} MenuScene.OnDisable (start)");
        Debug.Log($"{Time.time} MenuScene.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"{Time.time} MenuScene.OnDestroy (start)");
        Debug.Log($"{Time.time} MenuScene.OnDestroy (end)");
    }
}

