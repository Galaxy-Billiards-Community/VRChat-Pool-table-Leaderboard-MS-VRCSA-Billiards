
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace WangQAQ.UdonPlug
{
	public class GetNewsList : UdonSharpBehaviour
	{
		[SerializeField] private GameObject _Prefab;
		[SerializeField] private Transform ObjParent;

		[SerializeField] public VRCUrl url;

		[HideInInspector] public DataDictionary NewsList;

		private bool isLoading = false;

		public void Start()
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
				NewsList = json.DataDictionary;
				buildList();
			}
		}

		//字符串下载失败回调
		public override void OnStringLoadError	(IVRCStringDownload result)
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

		#region FUNC

		private void buildList()
		{
			var title = NewsList.GetKeys().ToArray();
			var description = NewsList.GetValues().ToArray();

			for (int i = 0; i < NewsList.Count; i++)
			{
				var a = Instantiate(_Prefab, ObjParent);
				var cmpObj = a.GetComponent<UdonBehaviour>();
				cmpObj.SetProgramVariable("Title", title[i].ToString());
				cmpObj.SetProgramVariable("Description", description[i].ToString());
				cmpObj.SetProgramVariable("UrlID", i);
				cmpObj.SendCustomEvent("_Init");
			}
		}

		#endregion
	}
}
