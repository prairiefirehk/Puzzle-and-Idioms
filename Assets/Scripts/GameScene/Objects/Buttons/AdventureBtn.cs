using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlueAndWhite.Buttons
{
    public class AdventureBtn : MonoBehaviour
    {
        #region Flow
        void Awake()
        {
            Debug.Log("AdventureBtn.Awake (start)");
            Debug.Log("AdventureBtn.Awake (end)");
        }

        void OnEnable()
        {
            Debug.Log("AdventureBtn.OnEnable (start)");
            Debug.Log("AdventureBtn.OnEnable (end)");
        }
        
        void Start()
        {
            Debug.Log("AdventureBtn.Start (start)");
            Debug.Log("AdventureBtn.Start (end)");
        }

        void Update()
        {

        }
        
        void OnDisable()
        {
            Debug.Log($"AdventureBtn.OnDisable (start)");
            Debug.Log($"AdventureBtn.OnDisable (end)");
        }

        void OnDestroy() 
        {
            Debug.Log($"AdventureBtn.OnDestroy (start)");
            Debug.Log($"AdventureBtn.OnDestroy (end)");
        }
        #endregion

        #region Button functions
        public void StartAdventure()
        {
            Debug.Log($"AdventureBtn.StartAdventure (start)");

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            Debug.Log($"AdventureBtn.StartAdventure (end)");
        }
        #endregion
    }
}
