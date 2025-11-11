using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace cdse_presets
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class ToggleGameObject : UdonSharpBehaviour
    {
        [Header("这是一个对象开关脚本")]
        [Header("是否同步？")]
        [SerializeField] private bool isGlobal = false;
        [Header("目标对象(开启)")]
        [SerializeField] private GameObject[] targetGameObject;
        [UdonSynced] private bool[] isEnabled;

		public void Start()
		{
			isEnabled = new bool[targetGameObject.Length];

		    for (int i = 0; i < targetGameObject.Length; i++)
            {
                isEnabled[i] = targetGameObject[i].activeSelf;
            }
		}

		public void isTrigger()
        {
            if (!isGlobal)
            {
                ToggleLocal();
            }
            else
            {
                if (!Networking.IsOwner(gameObject))
                {
                    Networking.SetOwner(Networking.LocalPlayer, gameObject);
                }
                for (int num = 0; num < targetGameObject.Length; num++)
                {
                    if (targetGameObject[num] != null)
                    {
                        isEnabled[num] = !isEnabled[num];
                    }
                }
                ToggleGlobal();
                RequestSerialization();
            }
        }

        public override void OnDeserialization()
        {
            if (isGlobal && !Networking.IsOwner(gameObject))
            {
                ToggleGlobal();
            }
        }

        public void ToggleGlobal()
        {
            for (int num = 0; num < targetGameObject.Length; num++)
            {
                if (targetGameObject[num] != null)
                {
                    targetGameObject[num].SetActive(isEnabled[num]);
                }
            }
        }

        public void ToggleLocal()
        {
            for (int num = 0; num < targetGameObject.Length; num++)
            {
                if (targetGameObject[num] != null)
                {
                    targetGameObject[num].SetActive(!targetGameObject[num].activeSelf);
                }
            }
        }
    }
}