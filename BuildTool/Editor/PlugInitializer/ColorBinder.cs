using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WangQAQ.UdonPlug;

namespace WangQAQ.PoolBuild
{
	public class ColorBinder : IPlugInitializer
	{
		public bool Init(string worldGuid,string worldKey)
		{
			/* 初始化计分器 */
			var ColorManager = Component.FindObjectsOfType<ColorManager>();
			var ColorDownload = Component.FindFirstObjectByType<ColorDownloaderV2>();

			if (ColorManager == null || ColorDownload == null)
				return false;

			foreach (var scoreManager in ColorManager)
			{
				scoreManager._colorDownloaderV2 = ColorDownload;

				// 显式地记录变更
				PrefabUtility.RecordPrefabInstancePropertyModifications(scoreManager);
			}

			return true;
		}
	}

}
