using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static VRC.SDK3.Dynamics.PhysBone.PhysBoneMigration.DynamicBoneColliderData;

namespace WangQAQ.PoolBuild
{
	public class EloDownloadBinder : IPlugInitializer
	{
		public bool Init(string worldGuid, string worldKey)
		{
			var _eloDownloadObject = Component.FindObjectOfType<EloDownload>();
			var _leaderboardObjects = Component.FindObjectsOfType<LeaderboardV2>();

			if (_eloDownloadObject == null || _leaderboardObjects == null)
				return false;

			_eloDownloadObject._leaderboards = _leaderboardObjects;

			PrefabUtility.RecordPrefabInstancePropertyModifications(_eloDownloadObject);

			return true;
		}
	}
}