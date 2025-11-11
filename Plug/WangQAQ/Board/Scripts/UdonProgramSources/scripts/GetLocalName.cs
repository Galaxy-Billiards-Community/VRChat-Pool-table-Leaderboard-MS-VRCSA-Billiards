using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace cdse_presets
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class GetLocalName : UdonSharpBehaviour
    {
        //这是一个显示本地玩家名称的脚本
        [SerializeField] private Text PlayerName;

        void Start()
        {
            PlayerName.text = Networking.LocalPlayer.displayName;
        }
    }
}