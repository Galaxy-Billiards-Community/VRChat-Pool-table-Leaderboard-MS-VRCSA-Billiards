using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace cdse_presets
{
    public class GetAvatarHeight : UdonSharpBehaviour
    {
        [Header("这是一个玩家身高追踪脚本")]
        [Header("目标对象")]
        [SerializeField] private Transform[] targetGameObject = null;

        public void Start()
        {
            foreach (var gameObject in targetGameObject)
            {
                if (targetGameObject == null)
                    return;

                var localPlayer = Networking.LocalPlayer;

                if (Utilities.IsValid(localPlayer))
                {
                    Vector3 tmp = gameObject.localPosition;
                    tmp.y = localPlayer.GetAvatarEyeHeightAsMeters();
                    gameObject.localPosition = tmp;
                }
            }
        }

        public override void OnAvatarEyeHeightChanged(VRCPlayerApi player, float prevEyeHeightAsMeters)
        {
            foreach (var gameObject in targetGameObject)
            {
                if (Utilities.IsValid(player))
                {
                    Vector3 tmp = gameObject.localPosition;
                    tmp.y = player.GetAvatarEyeHeightAsMeters();
                    gameObject.localPosition = tmp;
                }
            }
        }
    }
}