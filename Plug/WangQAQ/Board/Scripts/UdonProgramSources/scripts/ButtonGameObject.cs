using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace cdse_presets
{
    public class ButtonGameObject : UdonSharpBehaviour
    {
        [Header("这是一个对象按钮脚本")]
        [Header("目标开启对象")]
        public GameObject[] TargetGameObjectOn;
        [Header("目标关闭对象")]
        public GameObject[] TargetGameObjectOff;
        [Header("按钮")]
        public Button buttonGameObject;

        public void isTrigger()
        {
            foreach (var gameObject in TargetGameObjectOn)
            {
                gameObject.SetActive(true);
            }
            foreach (var gameObject in TargetGameObjectOff)
            {
                gameObject.SetActive(false);
            }
        }
    }
}