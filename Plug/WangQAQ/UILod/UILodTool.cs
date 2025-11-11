
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
namespace WangQAQ.UdonPlug
{
	public class UILodTool : UdonSharpBehaviour
	{
		[Header("是否在开始时启用")]
		[SerializeField] private bool IsLodOnStart;

		[Space(10)]
		[Header("回落选项")]
		[SerializeField] private GameObject[] _original;
		[SerializeField] private GameObject[] _fallBack;

		public void OnEnable()
		{
			if (IsLodOnStart) 
				OnPlayerTriggerExit(Networking.LocalPlayer);
		}

		public override void OnPlayerTriggerEnter(VRCPlayerApi player)
		{
			if (!player.isLocal)
				return;

			foreach (GameObject obj in _fallBack)
			{
				obj.SetActive(false);
			}

			foreach (GameObject obj in _original)
			{
				obj.SetActive(true);
			}
		}

		public override void OnPlayerTriggerExit(VRCPlayerApi player)
		{
			if (!player.isLocal)
				return;

			foreach (GameObject obj in _fallBack)
			{
				obj.SetActive(true);
			}

			foreach (GameObject obj in _original)
			{
				obj.SetActive(false);
			}
		}
	}
}

