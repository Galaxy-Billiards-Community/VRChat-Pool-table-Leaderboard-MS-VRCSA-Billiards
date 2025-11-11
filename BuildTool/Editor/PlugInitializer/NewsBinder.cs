
using UnityEditor;
using UnityEngine;
using VRC.SDKBase;
using WangQAQ.UdonPlug;

namespace WangQAQ.PoolBuild
{
	public class NewsBinder : IPlugInitializer
	{
		public bool Init(string worldGuid, string worldKey) 
		{
			var _newsDownload = Component.FindObjectsOfType<GetNewsList>();
			var _newsGetContext = Component.FindFirstObjectByType<GetMainContext>();

			if (_newsDownload == null ||
				_newsGetContext == null) 
				return false;

			foreach (var a in _newsDownload)
			{
				a.url = new VRCUrl("https://wangqaq.com/AspAPI/table/GetNews");

				PrefabUtility.RecordPrefabInstancePropertyModifications(a);
			}

			_newsGetContext.urls = new VRCUrl[100];

			for(int i = 0; i < 100; i++)
			{
				_newsGetContext.urls[i] = new VRCUrl($"https://wangqaq.com/AspAPI/table/GetNews/{i}");
			}

			PrefabUtility.RecordPrefabInstancePropertyModifications(_newsGetContext);

			return true;
		}
	}
}
