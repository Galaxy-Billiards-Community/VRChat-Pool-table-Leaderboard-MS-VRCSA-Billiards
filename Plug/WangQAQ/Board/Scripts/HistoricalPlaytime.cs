
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Persistence;
using VRC.SDKBase;
using VRC.Udon;

namespace WangQAQ.UdonPlug
{
	public class HistoricalPlaytime : UdonSharpBehaviour
	{
		[SerializeField] private Text _text;

		private const string Key_HistoricalTime = "NN_Time_Key";

		private long HistoricalTime;
		private bool isDone = false;

		void Start()
		{
			if (_text == null)
				return;

			this.SendCustomEventDelayedSeconds(nameof(_Tick), 120);
		}

		public override void OnPlayerRestored(VRCPlayerApi player)
		{
			if (!player.isLocal)
				return;

			HistoricalTime = PlayerData.GetLong(player, Key_HistoricalTime);
			_text.text = getTimeString(HistoricalTime);
			isDone = true;
		}

		public void _Tick()
		{
			if (isDone)
			{
				HistoricalTime += 120;
				PlayerData.SetLong(Key_HistoricalTime, HistoricalTime);

				_text.text = getTimeString(HistoricalTime);
			}

			this.SendCustomEventDelayedSeconds(nameof(_Tick), 120);
		}

		private string getTimeString(long time)
		{
			string ret = "游玩时间 : ";

			TimeSpan ts = TimeSpan.FromSeconds(time);
			if ((int)ts.TotalDays > 0) ret += $"{(int)ts.TotalDays}天";
			if (ts.Hours > 0) ret += $"{ts.Hours}时";
			ret += $"{ts.Minutes}分";

			return ret;
		}
	}
}
