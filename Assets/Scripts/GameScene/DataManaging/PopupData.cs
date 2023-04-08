using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlueAndWhite.Objects;
using BlueAndWhite.RoundManaging;

namespace BlueAndWhite.DataManaging
{
    [System.Serializable]
    public class PopupData
    {
        public string sizeType;
        public int caseRefID;
        public string title;
        public string body;
        public string confirmText;
        public string dismissText;
    }
}