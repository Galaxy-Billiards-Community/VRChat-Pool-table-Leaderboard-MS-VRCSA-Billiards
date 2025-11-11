using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WangQAQ.PoolBuild;

public class SettingLoaderBinder : IPlugInitializer
{
	public bool Init(string worldGuid, string worldKey)
	{
		var _settingManager = Component.FindObjectsOfType<SettingManager>();

		foreach(var a in _settingManager)
		{
			a._guid = worldGuid;
			a._key = worldKey;

			PrefabUtility.RecordPrefabInstancePropertyModifications(a);
		}

		return true;
	}
}
