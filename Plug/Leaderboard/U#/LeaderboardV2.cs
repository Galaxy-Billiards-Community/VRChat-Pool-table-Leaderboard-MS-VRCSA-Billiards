
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

public class LeaderboardV2 : ILeaderboard
{
	#region EditTimeVariable

	[Header("UI Object")]
	[Tooltip("UI 分数榜")]
	[SerializeField] private Text ScoreText;
	[SerializeField] private Text NameText;

	/* 玩家个人等级排行 */
	[SerializeField] private Text PlayerRateText;

	[Space(10)]
	[SerializeField] private RectTransform SelectInfo;

	[SerializeField] private RectTransform SelectSeason;
	[SerializeField] private RectTransform SelectMonth;
	[SerializeField] private RectTransform SelectWeek;
	[SerializeField] private RectTransform SelectPinnacle;

	[Space(10)]
	[Header("Text Setting")]
	[Tooltip("ELO Text Len")]
	[SerializeField] private int stringLen = 10;

	#endregion

	#region PrivateVariable

	private IEloDownload _eloDownload;

	// 0 is Season ,1 is Month ,2 is Week   ,3 is Pinnacle
	private int nowSelect = 0;

	private bool isAnimation = false;

	#endregion

	#region PublicApi

	public void Update()
	{
		tickUI();
	}

	public override void _Init(IEloDownload eloDownload)
	{
		_eloDownload = eloDownload;
	}

	public override void _Refresh()
	{
		loadString();
	}

	#endregion

	#region PrivateFunc

	private void tickUI()
	{
		if (!isAnimation)
			return;

		switch (nowSelect)
		{
			case 0:
				SelectInfo.position = Vector3.MoveTowards(
					SelectInfo.position,
					SelectSeason.position,
					Time.deltaTime
				);

				if ((SelectInfo.position - SelectSeason.position).sqrMagnitude < 0.000001)
					isAnimation = false;
				break;
			case 1:
				SelectInfo.position = Vector3.MoveTowards(
					SelectInfo.position,
					SelectMonth.position,
					Time.deltaTime
				);

				if ((SelectInfo.position - SelectMonth.position).sqrMagnitude < 0.000001)
					isAnimation = false;
				break;
			case 2:
				SelectInfo.position = Vector3.MoveTowards(
					SelectInfo.position,
					SelectWeek.position,
					Time.deltaTime
				);

				if ((SelectInfo.position - SelectWeek.position).sqrMagnitude < 0.000001)
					isAnimation = false;
				break;
			case 3:
				SelectInfo.position = Vector3.MoveTowards(
					SelectInfo.position,
					SelectPinnacle.position,
					Time.deltaTime
				);

				if ((SelectInfo.position - SelectPinnacle.position).sqrMagnitude < 0.000001)
					isAnimation = false;
				break;

		}
	}

	/* 序列化排行榜字符串 */
	private void loadString()
	{
		/*
		if (_eloDownload._eloSeasonNameList == null ||
			_eloDownload._eloSeasonDataList == null)
		{
			return;
		}*/

		// 格式化字符串
		string leaderBoardNameString = "";
		string leaderBoardScoreString = "";
		string leaderBoardPlayerRate = "# NotFind";
		string localPlayerName = Networking.LocalPlayer.displayName;


		switch (nowSelect)
		{
			case 0:
				DataList seasonNames = _eloDownload._eloSeasonNameList;
				DataList seasonScores = _eloDownload._eloSeasonDataList;

				for (int i = 0; i < seasonNames.Count; i++)
				{
					var nameTmp = seasonNames[i].ToString() /*.Replace(" ", " ")*/ ;
					// 转码，去除小数点，格式化，替换空格 \u0020 到 \u00A0 ,裁剪长度
					leaderBoardNameString += $"{(i + 1)}.{(nameTmp.Length > stringLen ? nameTmp.Substring(0, stringLen) : nameTmp)}\n";
					leaderBoardScoreString += seasonScores[i].Int.ToString() + "\n";

					/* 尝试查找本地玩家 */
					if (nameTmp == localPlayerName)
					{
						leaderBoardPlayerRate = $"# {(i + 1).ToString()}";
					}
				}
				break;
			case 1:
				DataList monthNames = _eloDownload._eloMonthNameList;
				DataList monthScores = _eloDownload._eloMonthDataList;
				
				for (int i = 0; i < monthNames.Count; i++)
				{
					var nameTmp = monthNames[i].ToString() /*.Replace(" ", " ")*/ ;
					// 转码，去除小数点，格式化，替换空格 \u0020 到 \u00A0 ,裁剪长度
					leaderBoardNameString += $"{(i + 1)}.{(nameTmp.Length > stringLen ? nameTmp.Substring(0, stringLen) : nameTmp)}\n";
					leaderBoardScoreString += monthScores[i].Int.ToString() + "\n";

					/* 尝试查找本地玩家 */
					if (nameTmp == localPlayerName)
					{
						leaderBoardPlayerRate = $"# {(i + 1).ToString()}";
					}
				}
				break;
			case 2:
				DataList weekNames = _eloDownload._eloWeekNameList;
				DataList weekScores = _eloDownload._eloWeekDataList;
				
				for (int i = 0; i < weekNames.Count; i++)
				{
					var nameTmp = weekNames[i].ToString() /*.Replace(" ", " ")*/ ;
					// 转码，去除小数点，格式化，替换空格 \u0020 到 \u00A0 ,裁剪长度
					leaderBoardNameString += $"{(i + 1)}.{(nameTmp.Length > stringLen ? nameTmp.Substring(0, stringLen) : nameTmp)}\n";
					leaderBoardScoreString += weekScores[i].Int.ToString() + "\n";

					/* 尝试查找本地玩家 */
					if (nameTmp == localPlayerName)
					{
						leaderBoardPlayerRate = $"# {(i + 1).ToString()}";
					}
				}
				break;
			case 3:
				DataList pinnacleNames = _eloDownload._eloPinnacleNameList;
				DataList pinnacleScores = _eloDownload._eloPinnacleDataList;

				for (int i = 0; i < pinnacleNames.Count; i++)
				{
					var nameTmp = pinnacleNames[i].ToString() /*.Replace(" ", " ")*/ ;
					var scoreData = pinnacleScores[i].String.Split("|#|");    // [0] : Score [1]: Level

					// 转码，去除小数点，格式化，替换空格 \u0020 到 \u00A0 ,裁剪长度
					leaderBoardNameString += $"{scoreData[1]}.{(nameTmp.Length > stringLen ? nameTmp.Substring(0, stringLen) : nameTmp)}\n";
					leaderBoardScoreString += scoreData[0] + "\n";

					/* 尝试查找本地玩家 */
					if (nameTmp == localPlayerName)
					{
						leaderBoardPlayerRate = $"# {(i + 1).ToString()}";
					}
				}
				break;
		}

		// Loading String
		NameText.text = leaderBoardNameString;
		ScoreText.text = leaderBoardScoreString;
		PlayerRateText.text = leaderBoardPlayerRate;
	}

	#endregion

	#region UIEvent 

	public void _OnSeasonLeaderboard()
	{
		if (nowSelect == 0)
			return;

		nowSelect = 0;
		isAnimation = true;

		_Refresh();
	}

	public void _OnMonthLeaderboard()
	{
		if (nowSelect == 1)
			return;

		nowSelect = 1;
		isAnimation = true;

		_Refresh();
	}

	public void _OnWeekLeaderboard()
	{
		if (nowSelect == 2)
			return;

		nowSelect = 2;
		isAnimation = true;

		_Refresh();
	}

	public void _OnPinnacleLeaderboard()
	{
		if (nowSelect == 3)
			return;

		nowSelect = 3;
		isAnimation = true;

		_Refresh();
	}

	#endregion

}
