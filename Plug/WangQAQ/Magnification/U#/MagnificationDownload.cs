using System;
using System.Linq;
using System.Text;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.Core;
using VRC.SDK3.Data;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

public class MagnificationDownload : UdonSharpBehaviour
{
	/// <summary>
	/// 下面参数不需要手动添加，会自动绑定
	/// </summary>
	public VRCUrl url = null;

	// Magnification字典对象
	[HideInInspector] public DataDictionary _Magnification = null;

	private bool isLoading = false;

	private void Start()
	{
		if (url == null)
			return;

		VRCStringDownloader.LoadUrl(url, (IUdonEventReceiver)this);
		isLoading = true;
	}

	#region URL
	// 字符串下载成功回调
	public override void OnStringLoadSuccess(IVRCStringDownload result)
	{
		if (VRCJson.TryDeserializeFromJson(result.Result, out var json))
		{
			_Magnification = json.DataDictionary;
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
