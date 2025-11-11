using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace cdse_presets
{
    public class ToggleCollider : UdonSharpBehaviour
    {
        [Header("这是一个碰撞体开关脚本")]
        [Header("目标碰撞体")]
        public Collider[] TargetCollider;
        [Header("开关")]
        public Toggle toggleCollider;

        public void Start()
        {
            if (TargetCollider != null && toggleCollider != null)
            {
                foreach (var collider in TargetCollider)
                {
                    collider.enabled = toggleCollider.isOn;
                }
            }
        }

        public void isTrigger()
        {
            if (TargetCollider != null && toggleCollider != null)
            {
                foreach (var collider in TargetCollider)
                {
                    collider.enabled = toggleCollider.isOn;
                }
            }
        }
    }
}