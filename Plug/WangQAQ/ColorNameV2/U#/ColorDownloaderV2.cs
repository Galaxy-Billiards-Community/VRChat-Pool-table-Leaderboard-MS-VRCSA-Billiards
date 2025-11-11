/*
 *  MIT License
 *  Copyright (c) 2024 WangQAQ
 *
 *	第二版彩色名称下载
 */
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace WangQAQ.UdonPlug
{
	public class ColorDownloaderV2 : UdonSharpBehaviour
	{
		[SerializeField] private VRCUrl url;

		/// <summary>
		/// name,color
		/// </summary>
		[HideInInspector] public DataDictionary _colors;

		private bool isLoading = false;
		void Start()
		{
			if (url == null)
				return;

			VRCStringDownloader.LoadUrl(url, (IUdonEventReceiver)this);
		}

		#region URL
		// 字符串下载成功回调
		public override void OnStringLoadSuccess(IVRCStringDownload result)
		{
			if (VRCJson.TryDeserializeFromJson(result.Result, out var json))
			{
				_colors = json.DataDictionary;
			}
		}

		//字符串下载失败回调
		public override void OnStringLoadError(IVRCStringDownload result)
		{
			isLoading = false;
			SendCustomEventDelayedSeconds("_AutoReload", 60);
		}

		//重新加载字符串函数
		public void _AutoReload()
		{
			//VRC下载API
			if (!isLoading)
			{
				VRCStringDownloader.LoadUrl(url, (IUdonEventReceiver)this);
				isLoading = true;
			}
		}

		#endregion
	}
}

