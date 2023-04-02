using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlueAndWhite.Buttons
{
    public class StartBtn : MonoBehaviour
    {
        #region Flow
        void Awake()
        {
            Debug.Log("StartBtn.Awake (start)");
            Debug.Log("StartBtn.Awake (end)");
        }


        void OnEnable()
        {
            Debug.Log("StartBtn.OnEnable (start)");
            Debug.Log("StartBtn.OnEnable (end)");
        }
        
        void Start()
        {
            Debug.Log("StartBtn.Start (start)");
            Debug.Log("StartBtn.Start (end)");
        }

        void Update()
        {

        }
        
        void OnDisable()
        {
            Debug.Log($"StartBtn.OnDisable (start)");
            Debug.Log($"StartBtn.OnDisable (end)");
        }

        void OnDestroy() 
        {
            Debug.Log($"StartBtn.OnDestroy (start)");
            Debug.Log($"StartBtn.OnDestroy (end)");
        }
        #endregion

        #region Button functions
        public void StartPlaying()
        {
            Debug.Log($"StartBtn.StartPlaying (start)");

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            Debug.Log($"StartBtn.StartPlaying (end)");
        }
        #endregion
    }
}
