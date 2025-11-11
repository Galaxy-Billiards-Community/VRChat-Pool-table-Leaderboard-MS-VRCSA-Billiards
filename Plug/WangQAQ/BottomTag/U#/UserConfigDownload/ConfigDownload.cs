/*
 *  MIT License
 *  Copyright (c) 2024 WangQAQ
 *
 *  配置下载器
 */
using System;
using System.Text;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

namespace WangQAQ.UdonPlug
{
	public class ConfigDownload : UdonSharpBehaviour
	{
		#region PublicValueEdit

		// 创建一个序列化变量防止VRCSDK不打包
		public VRCUrl ConfigUrl;

		#endregion

		#region ValueEdit

		[Header("ReloadTime")]
		[SerializeField] private int autoReloadTime = 60;

		#endregion

		#region PubilcValueAPI

		public DataDictionary Config => configDic;

		#endregion

		#region PrivateValue

		private IManger _manger = null;

		private DataDictionary configDic = null;

		#endregion

		#region PublicAPI

		public void _Init(IManger manger)
		{
			_manger = manger;
		}

		public void Start()
		{
			if (ConfigUrl == null)
				return;

			Debug.Log(ConfigUrl);

			VRCStringDownloader.LoadUrl(ConfigUrl, (IUdonEventReceiver)this);
		}

		/// <summary>
		/// -1    NoFind
		/// Other TagIndex
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public int GetPlayerTag(string name)
		{
			if (configDic == null)
				return -1;

			if (configDic.TryGetValue(name, out DataToken obj))
			{
				return Convert.ToInt32(obj.ToString());
			}

			return -1;
		}

		#endregion

		#region CallBack

		public override void OnStringLoadSuccess(IVRCStringDownload result)
		{
			if (VRCJson.TryDeserializeFromJson(result.Result, out var json))
			{
				configDic = json.DataDictionary;
			}

			_manger.OnConfigDownloadDone();
		}

		public override void OnStringLoadError(IVRCStringDownload result)
		{
			SendCustomEventDelayedSeconds(nameof(autoReload), autoReloadTime);
		}

		#endregion

		#region Func

		private void autoReload()
		{
			VRCStringDownloader.LoadUrl(ConfigUrl, (IUdonEventReceiver)this);
		}

		#endregion

		#region Debug

#if DEBUG

		[HideInInspector] public string key;
		[HideInInspector] public string value;

		public void CreateDic()
		{
			configDic = new DataDictionary();
		}

		public void AddKeyValue()
		{
			configDic.Add(key, value);
		}

		public void SendDoneEvent()
		{
			_manger.OnConfigDownloadDone();
		}

#endif

		#endregion
	}
}