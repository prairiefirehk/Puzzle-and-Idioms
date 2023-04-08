using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BlueAndWhite.Objects;
using BlueAndWhite.RoundManaging;
using BlueAndWhite.UIManaging;


namespace BlueAndWhite.Buttons
{
    public class SettingBtn : MonoBehaviour
    {
        #region Scripts
        private UIManage uiManage;
        #endregion

        #region Game object reference
        [SerializeField] private Button settingBtn;
        #endregion

        #region Flow
        void Awake()
        {
            Debug.Log("SettingBtn.Awake (start)");

            // Not ideal place
            //uiManage = GameObject.Find("UI Manager").GetComponent<UIManage>();
            //settingBtn = gameObject.GetComponent<Button>();

            Debug.Log("SettingBtn.Awake (end)");
        }


        void OnEnable()
        {
            Debug.Log("SettingBtn.OnEnable (start)");
            Debug.Log("SettingBtn.OnEnable (end)");
        }
        
        void Start()
        {
            Debug.Log("SettingBtn.Start (start)");

            uiManage = GameObject.Find("UI Manager").GetComponent<UIManage>();
            settingBtn = gameObject.GetComponent<Button>();

            Debug.Log("SettingBtn.Start (end)");
        }

        void Update()
        {

        }
        
        void OnDisable()
        {
            Debug.Log($"SettingBtn.OnDisable (start)");
            Debug.Log($"SettingBtn.OnDisable (end)");
        }

        void OnDestroy() 
        {
            Debug.Log($"SettingBtn.OnDestroy (start)");
            Debug.Log($"SettingBtn.OnDestroy (end)");
        }
        #endregion

        #region Button functions
        public void OnClick()
        {
            Debug.Log($"SettingBtn.OnClick (start)");

            uiManage.SpawnPopup("medium", 3, () => uiManage.currentPopup.DestroyPopup(uiManage.currentPopup), () => StopAdventure());

            Debug.Log($"SettingBtn.OnClick (end)");
        }

        public static void StopAdventure()
        {
            Debug.Log($"SettingBtn.StopAdventure (static) (start)");

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

            Debug.Log($"SettingBtn.StopAdventure (static) (end)");
        }
        #endregion
    }
}

