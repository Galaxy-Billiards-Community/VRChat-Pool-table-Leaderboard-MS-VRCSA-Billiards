using Cheese;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WangQAQ.PoolBuild;

namespace WangQAQ.PoolBuild
{
	public class BilliardsBinder : IPlugInitializer
	{
		public bool Init(string worldGuid, string worldKey)
		{
			var _billiardsModule = Component.FindObjectsOfType<BilliardsModule>();
			var _eloDownloadObject = Component.FindObjectOfType<EloDownload>();
			var _personalDataCounter = Component.FindObjectOfType<PersonalDataCounter>();
			var _tableHook = Component.FindFirstObjectByType<TableHook>();

			if (_eloDownloadObject == null ||
				_personalDataCounter == null ||
				_tableHook == null)
				return false;

			foreach (var a in _billiardsModule)
			{
				a._eloDownload = _eloDownloadObject;
				a.personalData = _personalDataCounter;
				a.tableHook = _tableHook;

				PrefabUtility.RecordPrefabInstancePropertyModifications(a);
			}

			return true;
		}
	}
}