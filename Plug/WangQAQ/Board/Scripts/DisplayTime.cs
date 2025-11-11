
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace cdse_presets
{
    public class DisplayTime : UdonSharpBehaviour
    {
        public Text timeDis;
        public string disType = "t";
        private void Update()
        {
            timeDis.text = System.DateTime.Now.ToString(disType);
        }
    }
}