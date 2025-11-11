
using System;
using System.Globalization;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

/// <summary>
/// 测试版，暂时显示ELO分数
/// </summary>
public class UIS_ContentLoader : UdonSharpBehaviour
{
	#region EditVariable

	[SerializeField] private Text _UserName = null;
	[SerializeField] private Text _Score = null;
	[SerializeField] private Text _Rate = null;
	[SerializeField] private Text _Count = null;

	[SerializeField] private Text _Leave = null;
	[SerializeField] private Text _Rate1 = null;

	[SerializeField] private Image _Image = null;

	#endregion

	#region PubilcAPI

	public void OnClick(string userName, string userData)
	{
		/* 分数 胜率 游玩次数 */
		var data = userData.Split('#');

		string cleanedStr = data[1].TrimEnd('%');       // 去掉百分号
		float rate = float.Parse(cleanedStr) / 100f;    // 转换为 float 并除以 100
		var count = Convert.ToInt32(data[2]);

		var calculateData = calculateRating(rate, count);

		_UserName.text = (userName.Length > 7 ? $"{userName.Substring(0, 7)}.." : userName);
		_Score.text = "分数: " + data[0];
		_Rate.text = "胜率: " + rate.ToString("P2", CultureInfo.InvariantCulture);
		_Count.text = "回合数: " + data[2].PadRight(7, ' ');

		var nextLeave = (float)Convert.ToDouble(calculateData[1]) / 100;

		_Rate1.text = nextLeave.ToString("0%",CultureInfo.InvariantCulture);
		_Leave.text = calculateData[0];

		_Image.fillAmount = Mathf.Lerp(0.058f, 0.78f, nextLeave);
	}

	#endregion

	#region Func

	/// <summary>
	/// 根据胜率计算百分比
	/// 暂且使用，晚点修改
	/// </summary>
	/// <param name="wins">胜利数</param>
	/// <param name="totalRounds">总回合数</param>
	/// <returns>玩家评级及距离下一级的百分比</returns>
	private static string[] calculateRating(double winRate, int totalRounds)
	{
		if (totalRounds == 0)
		{
			return new string[] { "N", "0" };
		}

		// 初始化评级和距离下一个评级的百分比
		string rating;
		double percentageToNext;

		// 根据胜率判断评级和距离下一级的进度
		if (winRate > 0.9)
		{
			rating = "<color=#FFD700>S</color>";
			percentageToNext = 100; // S 已经是最高级
		}
		else if (winRate > 0.7)
		{
			rating = "<color=#F1361F>A</color>";
			percentageToNext = (winRate - 0.7) / (0.9 - 0.7) * 100;
		}
		else if (winRate > 0.5)
		{
			rating = "<color=#F11FE0>B</color>";
			percentageToNext = (winRate - 0.5) / (0.7 - 0.5) * 100;
		}
		else if (winRate > 0.3)
		{
			rating = "<color=#1F33F1>C</color>";
			percentageToNext = (winRate - 0.3) / (0.5 - 0.3) * 100;
		}
		else
		{
			rating = "<color=#1FF13D>D	</color>";
			percentageToNext = winRate / 0.3 * 100;
		}

		return new string[] { rating, percentageToNext.ToString() };
	}

	#endregion
}
