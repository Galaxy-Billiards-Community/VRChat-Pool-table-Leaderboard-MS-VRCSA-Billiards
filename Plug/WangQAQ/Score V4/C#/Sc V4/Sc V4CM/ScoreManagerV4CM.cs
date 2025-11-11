
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WangQAQ.UdonPlug
{
	public class ScoreManagerV4CM : IScoreAPI
	{
		#region Plug
		[Header("NameText")]
		[SerializeField] public TextMeshProUGUI RedNameTMP = null;
		[SerializeField] public TextMeshProUGUI BlueNameTMP = null;
		[Header("Score")]
		[SerializeField] public TextMeshProUGUI RedScoreTMP = null;
		[SerializeField] public TextMeshProUGUI BlueScoreTMP = null;
		[Header("Elo")]
		[SerializeField] public TextMeshProUGUI Elo1 = null;
		[SerializeField] public TextMeshProUGUI Elo2 = null;
		[Header("Plug")]
		[SerializeField] private ScoreNetworkV4CM _network = null;
		[SerializeField] public  IEloDownload _eloAPI = null;
		#endregion

		public override void _Init(BilliardsModule billiardsModule)
		{
			//  判空
			if (
				RedNameTMP == null ||
				BlueNameTMP == null ||
				RedScoreTMP == null ||
				BlueScoreTMP == null ||
				Elo1 == null ||
				Elo2 == null ||
				_network == null
			  )
			{
				this.enabled = false;
				return;
			}

			//Init
			_network._Init(this);
		}

		public override void _Tick()
		{
			// 查看Buffer是否需要同步
			_network._FlushBuffer();
		}

		#region LogicFuncion

		private void _Reflash()
		{
			var playerApiA = VRCPlayerApi.GetPlayerById(_network.PlayerA);
			var playerApiB = VRCPlayerApi.GetPlayerById(_network.PlayerB);

			var playerNameA = playerApiA == null ? string.Empty : playerApiA.displayName;
			var playerNameB = playerApiB == null ? string.Empty : playerApiB.displayName;

			RedNameTMP.text = playerNameA;
			BlueNameTMP.text = playerNameB;
			RedScoreTMP.text = _network.PlayerAScore.ToString();
			BlueScoreTMP.text = _network.PlayerBScore.ToString();
		}

		private void _ReflashEloScore()
		{
			var playerApiA = VRCPlayerApi.GetPlayerById(_network.PlayerA);
			var playerApiB = VRCPlayerApi.GetPlayerById(_network.PlayerB);

			var playerNameA = playerApiA == null ? string.Empty : playerApiA.displayName;
			var playerNameB = playerApiB == null ? string.Empty : playerApiB.displayName;

			Elo1.text = _eloAPI.GetEloV2(playerNameA);
			Elo2.text = _eloAPI.GetEloV2(playerNameB);
		}

		private void _ResetValue()
		{
			_network.PlayerA = -1;
			_network.PlayerB = -1;
			_network.PlayerAScore = 0;
			_network.PlayerBScore = 0;

			_network.isFirstGameDone = false;
		}

		private void _SetName(int[] name)
		{
			if (name == null)
			{
				Debug.Log("NameNULL");
				return;
			}

			_network.PlayerA = name[0];
			_network.PlayerB = name[1];
		}

		#endregion

		#region Network

		public void _OnRemoteDeserialization()
		{
			// 刷新
			_Reflash();
			_ReflashEloScore();
		}

		#endregion

		#region Local

		private void playerChangedLocal(int[] nowPlayerList)
		{
			if (_network.isFirstGameDone)
			{
				// 如果已经存在记录时换人，则重置
				if ((nowPlayerList[0] != -1 &&
					_network.PlayerA != nowPlayerList[0] &&
					_network.PlayerB != nowPlayerList[0]) ||
					(nowPlayerList[1] != -1 &&
					_network.PlayerA != nowPlayerList[1] &&
					_network.PlayerB != nowPlayerList[1]))
				{
					_ResetValue();
					_SetName(nowPlayerList);
				}
			}
			else
			{
				// 没有记录则直接添加
				_SetName(nowPlayerList);
			}

			_network._SetBufferStatus();
		}

		private void gameEndLocal(uint winningTeamLocal, int[] playerID)
		{
			var isInvert = false;

			if (playerID != null &&
				playerID[0] == _network.PlayerB &&
				playerID[1] == _network.PlayerA)
				isInvert = true;

			if (playerID != null &&
			   ((playerID[0] != _network.PlayerA &&
				 playerID[0] != _network.PlayerB ) ||
				(playerID[1] != _network.PlayerA &&
				 playerID[1] != _network.PlayerB ) ))
				return;

			// 判断黄金开局
			if (winningTeamLocal != 2)
			{
				if (isInvert)
					winningTeamLocal = (uint)(winningTeamLocal == 1 ? 0 : 1);

				if (winningTeamLocal == 0)
					_network.PlayerAScore++;
				else if (winningTeamLocal == 1)
					_network.PlayerBScore++;
			}

			_network.isFirstGameDone = true;
			_network._SetBufferStatus();
		}

		private void gameResetLocal()
		{
			Debug.Log("[SCM] ResetSC");

			_ResetValue();
			_network._SetBufferStatus();
		}

		#endregion

		#region HookAPIs

		//API winningTeamLocal _GameEnd_WithID
		public override void _GameEnd_WithID(uint winningTeamLocal, int[] playerID)
		{
			gameEndLocal(winningTeamLocal, playerID);
		}

		//NoAPI Value
		public override void _PlayerChanged_ID(int[] nowPlayerList)
		{
			playerChangedLocal(nowPlayerList);
		}

		public override void _LobbyOpen_ID(int[] nowPlayerList)
		{
			playerChangedLocal(nowPlayerList);
		}

		//NoAPI Value
		public override void _GameReset()
		{
			gameResetLocal();
		}

		public override void _GameEnd(uint winningTeamLocal) { /* 已废弃 */ }
		public override void _LobbyOpen(string[] lobbyPlayerList) { /* 已废弃 */ }
		public override void _PlayerChanged(string[] nowPlayerList, int gameState) { /* 已废弃 */ }
		public override void _GameStarted(string[] startPlayerList) { /* 已废弃 */ }
		public override void _SetPracticeMode(bool value) { /* 已废弃 */ }

		#endregion

		public override void OnPlayerLeft(VRCPlayerApi player)
		{
			if(player.playerId.Equals(_network.PlayerA) || player.playerId.Equals(_network.PlayerB))
			{
				_GameReset();
			}
		}
	}
}
