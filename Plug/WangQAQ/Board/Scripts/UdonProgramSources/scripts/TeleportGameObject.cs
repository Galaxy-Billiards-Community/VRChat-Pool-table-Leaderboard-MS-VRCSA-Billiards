using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace cdse_presets
{
    public class TeleportGameObject : UdonSharpBehaviour
    {
        [Header("这是一个对象传送脚本")]
        [Header("目标对象")]
        public Transform targetGameObject;
        [Header("目标位置")]
        public Transform targetPosition;
        [Header("按钮")]
        public Button buttonTeleport;

        public void isTrigger()
        {
            targetGameObject.position = targetPosition.position;
            targetGameObject.rotation = targetPosition.rotation;
        }
    }
}