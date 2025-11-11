using UdonSharp;
using UnityEngine;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using UnityEngine.UI;

namespace WangQAQ.UdonPlug
{
	public class GetMainContext : UdonSharpBehaviour
	{
		[SerializeField] private Text MainContext;

		[SerializeField] public VRCUrl[] urls;
		[HideInInspector] public byte[] key = null;

		[HideInInspector] public uint _UrlID = 0;

		private bool isLoad = false;

		public void Start()
		{
			if (urls == null ||
				key == null)
				return;

		}

		public void GetContext()
		{
			if (!isLoad)
            {
				MainContext.text = "<size=40>Loading...</size>";
				VRCStringDownloader.LoadUrl(urls[_UrlID], (IUdonEventReceiver)this);
			}
			isLoad = true;
		}

		#region URL
		// 字符串下载成功回调
		public override void OnStringLoadSuccess(IVRCStringDownload result)
		{
			MainContext.text = result.Result;
			isLoad = false;
		}

		//字符串下载失败回调
		public override void OnStringLoadError(IVRCStringDownload result)
		{
			MainContext.text = "加载失败";
			isLoad = false;
		}

		#endregion
	}
}

