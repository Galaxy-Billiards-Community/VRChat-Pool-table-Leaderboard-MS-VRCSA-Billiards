
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

public class IEloDownload : UdonSharpBehaviour
{
	#region PublicAPI

	virtual public string GetEloV2(string name)
	{
		/* Nop */
		return null;
	}

	#endregion

	#region Data

	// Elo排序列表

	/* 名称列表(赛季) */
	[HideInInspector] public DataList _eloSeasonNameList;
	/* 数据列表(赛季) */
	[HideInInspector] public DataList _eloSeasonDataList;

	/* 名称列表(月) */
	[HideInInspector] public DataList _eloMonthNameList;
	/* 数据列表(月) */
	[HideInInspector] public DataList _eloMonthDataList;

	/* 名称列表(周) */
	[HideInInspector] public DataList _eloWeekNameList;
	/* 数据列表(周) */
	[HideInInspector] public DataList _eloWeekDataList;

	/* 名称列表(巅峰) */
	[HideInInspector] public DataList _eloPinnacleNameList;
	/* 数据列表(巅峰) */
	[HideInInspector] public DataList _eloPinnacleDataList;

	// Elo字典(非顺序)
	[HideInInspector] public DataDictionary _eloDictionary;

	#endregion
}
