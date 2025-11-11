/*
 *  MIT License
 *  Copyright (c) 2024 WangQAQ
 *
 *  Elo 下载器 Json格式
 */
/*
 * 
 * API格式
 * {
 *     "scores": {
 *        "name" : "分数#胜率#总回合数",
 *         ...
 *     }
 *  }
 * 
 */
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

public class EloDownload : IEloDownload
{
	// Elo下载URL
	[Header("URL")]
	[SerializeField] public VRCUrl url;
	public ILeaderboard[] _leaderboards;

	[HideInInspector] public int ReloadSecond = 60;

	public void Start()
	{
		_eloSeasonNameList = new DataList();
		_eloSeasonDataList = new DataList();
		_eloMonthNameList = new DataList();
		_eloMonthDataList = new DataList();
		_eloWeekNameList = new DataList();
		_eloWeekDataList = new DataList();

		VRCStringDownloader.LoadUrl(url, (IUdonEventReceiver)this);

		foreach (var leaderboard in _leaderboards)
		{
			if (leaderboard != null)
			{
				leaderboard._Init(this);
			}
		}
	}

	#region URL

	// 字符串下载成功回调
	public override void OnStringLoadSuccess(IVRCStringDownload result)
	{
		Debug.Log("Load");
		if (VRCJson.TryDeserializeFromJson(result.Result, out var json))
		{
			clearDataList();

			var eloData = json.DataDictionary["eloData"].DataDictionary;
			var pinnacleData = json.DataDictionary["pinnacleData"].DataDictionary;

			/* 保存排序和非排序字典对象 */
			_eloDictionary = eloData;
			loadData(_eloDictionary);

			var pinnacle = pinnacleData;
			_eloPinnacleNameList = pinnacle.GetKeys();
			_eloPinnacleDataList = pinnacle.GetValues();

			for (var i = 0; i < _leaderboards.Length; i++)
			{
				/* 延迟加载，防止 low 帧 */
				_leaderboards[i]._Refresh();
			}
		}
		SendCustomEventDelayedSeconds("_AutoReload", ReloadSecond);
	}

	//字符串下载失败回调
	public override void OnStringLoadError(IVRCStringDownload result)
	{
		SendCustomEventDelayedSeconds("_AutoReload", ReloadSecond);
	}

	//重新加载字符串函数
	public void _AutoReload()
	{
		//VRC下载API
		VRCStringDownloader.LoadUrl(url, (IUdonEventReceiver)this);
	}

	private void clearDataList()
	{
		_eloSeasonNameList.Clear();
		_eloSeasonDataList.Clear();
		_eloMonthNameList.Clear();
		_eloMonthDataList.Clear();
		_eloWeekNameList.Clear();
		_eloWeekDataList.Clear();
		_eloPinnacleNameList.Clear();
		_eloPinnacleDataList.Clear();
	}

	private void loadData(DataDictionary data)
	{
		var eloName = _eloDictionary.GetKeys();
		var eloScore = _eloDictionary.GetValues();

		var seasonName = new string[eloName.Count];
		var seasonData = new int[eloName.Count];
		var monthName = new string[eloName.Count];
		var monthData = new int[eloName.Count];
		var weekName = new string[eloName.Count];
		var weekData = new int[eloName.Count];

		for (int i = 0; i < eloName.Count; i++)
		{
			var eloScoreData = eloScore[i].String.Split('#');
			if (eloScoreData[0] != "0")
			{
				seasonName[i] = eloName[i].String;
				seasonData[i] = Convert.ToInt32(eloScoreData[0]);
			}

			if (eloScoreData[1] != "0")
			{
				monthName[i] = eloName[i].String;
				monthData[i] = Convert.ToInt32(eloScoreData[1]);
			}


			if (eloScoreData[2] != "0")
			{
				weekName[i] = eloName[i].String;
				weekData[i] = Convert.ToInt32(eloScoreData[2]);
			}
		}

		Array.Sort((Array)seasonData, (Array)seasonName);
		Array.Sort((Array)monthData, (Array)monthName);
		Array.Sort((Array)weekData, (Array)weekName);

		Array.Reverse((Array)seasonData);
		Array.Reverse((Array)monthData);
		Array.Reverse((Array)weekData);
		Array.Reverse((Array)seasonName);
		Array.Reverse((Array)monthName);
		Array.Reverse((Array)weekName);

		for (int i = 0;i < seasonData.Length; i++)
		{
			if (seasonData[i] == 0)
				continue;

			_eloSeasonNameList.Add(seasonName[i]);
			_eloSeasonDataList.Add(seasonData[i]);
		}

		for (int i = 0; i < monthName.Length; i++)
		{
			if (monthData[i] == 0)
				continue;

			_eloMonthNameList.Add(monthName[i]);
			_eloMonthDataList.Add(monthData[i]);
		}

		for (int i = 0; i < weekName.Length; i++)
		{
			if (weekData[i] == 0)
				continue;

			_eloWeekNameList.Add(weekName[i]);
			_eloWeekDataList.Add(weekData[i]);
		}
	}

	#endregion

	#region API

	// 读取玩家对应Elo分数
	override public string GetEloV2(string name)
	{
		if (string.IsNullOrEmpty(name))
			return null;

		if (_eloDictionary == null)
			return null;

		string data = _eloDictionary[name].ToString();

		if (!string.IsNullOrEmpty(data))
		{
			if (data != "KeyDoesNotExist")
			{
				/* 解包数据 */
				var SerializationData = data.Split('#', StringSplitOptions.RemoveEmptyEntries);

				if (!string.IsNullOrEmpty(SerializationData[0]))
					return SerializationData[0];
			}
		}

		return "0";
	}

	#endregion
}
