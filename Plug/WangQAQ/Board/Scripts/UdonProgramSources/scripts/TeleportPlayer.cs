using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace cdse_presets
{
    public class TeleportPlayer : UdonSharpBehaviour
    {
        [Header("这是一个玩家传送脚本")]
        [Header("目标位置")]
        public Transform targetPosition;
        
        [Header("按钮")]
        public Button buttonTeleport;

        public void isTrigger()
        {
            Networking.LocalPlayer.TeleportTo(targetPosition.position, targetPosition.rotation);
        }
    }
}