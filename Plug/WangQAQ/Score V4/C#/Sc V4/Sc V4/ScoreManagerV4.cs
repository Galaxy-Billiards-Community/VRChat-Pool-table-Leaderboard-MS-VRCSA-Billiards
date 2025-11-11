/*
 *  MIT License
 *  Copyright (c) 2024 WangQAQ
 *
 *  新计分器系统
 *  API : ScoreManagerHook
 */

/*
                   _ooOoo_
                  o8888888o
                  88" . "88
                  (| -_- |)
                  O\  =  /O
               ____/`---'\____
             .'  \\|     |//  `.
            /  \\|||  :  |||//  \
           /  _||||| -:- |||||-  \
           |   | \\\  -  /// |   |
           | \_|  ''\---/''  |   |
           \  .-\__  `-`  ___/-. /
         ___`. .'  /--.--\  `. . __
      ."" '<  `.___\_<|>_/___.'  >'"".
     | | :  `- \`.;`\ _ /`;.`/ - ` : | |
     \  \ `-.   \_ __\ /__ _/   .-` /  /
======`-.____`-.___\_____/___.-`____.-'======
                   `=---='
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            佛祖保佑       永无BUG
*/

using Cysharp.Threading.Tasks.Triggers;
using System;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WangQAQ.UdonPlug
{
	[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
	public class ScoreManagerV4 : IScoreAPI
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
		[Header("Info")]
		[SerializeField] public GameObject Messages = null;
		[SerializeField] public GameObject MessagesPractice = null;
		[Header("Plug")]
		[SerializeField] public ScoreNetwork Network = null;
		[SerializeField] public RankingSystemNULL _RankingSystem = null;
		[SerializeField] public IEloDownload EloAPI = null;

		// Ioc
		private BilliardsModule _billiardsModule;
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
				Messages == null ||
				MessagesPractice == null ||
				Network == null ||
				_RankingSystem == null
			  )
			{
				this.enabled = false;
				return;
			}

			//Init
			Network._Init(this);
			_RankingSystem._Init(this);

			_billiardsModule = billiardsModule;
		}

		public override void _Tick()
		{
			// 查看Buffer是否需要同步
			Network._FlushBuffer();
		}

		#region LogicFuncion

		private void _Reflash()
		{
			RedNameTMP.text = Network.PlayerA;
			BlueNameTMP.text = Network.PlayerB;
			RedScoreTMP.text = Network.PlayerAScore.ToString();
			BlueScoreTMP.text = Network.PlayerBScore.ToString();

			Messages.SetActive(Network.MessagesState);
			MessagesPractice.SetActive(Network.MessagesPracticeState);
			//ELO 
		}

		private void _ResetValue()
		{
			Network.PlayerA = "";
			Network.PlayerB = "";
			Network.PlayerAScore = 0;
			Network.PlayerBScore = 0;

			Network.isInvert = false;
		}

		private void _SetName(string[] name)
		{
			if (name == null)
				return;

			Network.PlayerA = name[0];
			Network.PlayerB = name[1];
		}

		private void _ReflashEloScore()
		{
			Elo1.text = EloAPI.GetEloV2(Network.PlayerA);
			Elo2.text = EloAPI.GetEloV2(Network.PlayerB);
		}

		#endregion

		// 用于处理TMP界面跟新
		#region Remote

		public void _OnRemoteDeserialization()
		{

			Debug.Log("[SCM]" + Network.State);
			// 调用栈
			for (int i = 0; i <= Network.funcStackTopLocal; i++)
			{
				int inFunc = Network.funcStack[i];

				switch (inFunc)
				{
					case 0:
						lobbyOpenRemote();
						break;
					case 1:
						playerChangedRemote();
						break;
					case 2:
						gameStartedRemote();
						break;
					case 3:
						gameEndRemote();
						break;
					case 4:
						gameResetRemote();
						break;
					case 0XFF:
						break;
				}
			}

			// 使用完成后清理栈
			for (int i = 0; i < Network.funcStack.Length; i++)
			{
				Network.funcStack[i] = 0XFF;
			}
			Network.funcStackTopLocal = 0;
		}

		//ID0
		public void lobbyOpenRemote()
		{
			Debug.Log("[SCM] lobbyOpenRemote");

			_RankingSystem.LockUrl();

			_ReflashEloScore();
			_Reflash();
		}

		// ID1
		public void playerChangedRemote()
		{
			Debug.Log("[SCM] playerChangedRemote");

			if(Network.UnlockUrl)
			{
				/* 解锁URL */
				_RankingSystem.UnlockUrl();
			}
			else
			{
				/* 锁定URL */
				_RankingSystem.LockUrl();
			}

			_ReflashEloScore();
			_Reflash();
		}

		// ID2
		public void gameStartedRemote()
		{
			Debug.Log("[SCM] gameStartedRemote");

			_RankingSystem.LockUrl();

			_ReflashEloScore();
			_Reflash();
		}

		// ID3
		public void gameEndRemote()
		{
			// 分数上传系统 (当正常结束，没有触发换人反作弊时生成链接)
			if (Network.State == 3)
			{
				_RankingSystem.UpdateCopyData(Network.PlayerA, Network.PlayerB, Network.PlayerAScore.ToString(), Network.PlayerBScore.ToString(), Network.Mode, Network.Date);
			}

			_RankingSystem.UnlockUrl();

			Debug.Log("[SCM] gameEndRemote");
			_ReflashEloScore();
			_Reflash();
		}

		// ID4
		public void gameResetRemote()
		{
			_RankingSystem.UnlockUrl();
			_RankingSystem.ClearURL();

			Debug.Log("[SCM] gameResetRemote");
			_ReflashEloScore();
			_Reflash();
		}

		#endregion

		// 用于处理需要同步的数值
		#region Locals

		// ID0
		public void lobbyOpenLocal(string[] lobbyPlayerList)
		{
			Debug.Log("[SCM] LobbyOpened");

			Network.MessagesPracticeState = false;

			if ((
			lobbyPlayerList[0] != Network.PlayerA &&
			lobbyPlayerList[0] != Network.PlayerB &&
			Network.State == 3) ||
			Network.State == 0
			)
			{
				_ResetValue();

				if (lobbyPlayerList == null)
					return;

				// 赋值玩家名
				_SetName(lobbyPlayerList);

				Network.State = 1;
			}

			// 释放数组
			lobbyPlayerList = null;

			// 跟新状态
			if (Network.funcStackTopLocal < 4)
			{
				Network.funcStack[Network.funcStackTopLocal] = 0;
				Network.funcStackTopLocal++;
			}
			Network._SetBufferStatus();
		}

		// ID1
		public void playerChangedLocal(string[] nowPlayerList, int gameState)
		{
			Debug.Log("[SCM] playerChanged");

			if (nowPlayerList == null)
				return;

			// 跟新状态
			if (
				string.IsNullOrEmpty(nowPlayerList[0]) &&
				string.IsNullOrEmpty(nowPlayerList[1]) &&
				Network.State == 1 &&
				gameState != 2
				)
			{
				/* 玩家为空，自动重置 */
				Debug.Log("[SCM] Empty");

				_ResetValue();

				/* 解锁 */
				Network.UnlockUrl = true;
				Network.State = 0;
			}
			else if (Network.State == 2)
			{
				/* 中途加入，自动触发PAC */
				_ResetValue();

				// 赋值玩家名
				_SetName(nowPlayerList);

				/* 锁定 */
				Network.UnlockUrl = false;

				Network.MessagesState = true;
				Network.State = 1;
			}
			else
			{
				/* 判断是否是回合结束状态 */
				if (Network.State == 3)
				{
					/* 回合结束时状态 */
					Debug.Log("[SCM] get" + nowPlayerList[0] + ";" + nowPlayerList[1]);
					if (
						(nowPlayerList[0] == Network.PlayerA ||
						nowPlayerList[0] == Network.PlayerB ||
						string.IsNullOrEmpty(nowPlayerList[0])) &&
						(nowPlayerList[1] == Network.PlayerA ||
						nowPlayerList[1] == Network.PlayerB ||
						string.IsNullOrEmpty(nowPlayerList[1]))
						)
					{
						/* 状态判断玩家是否相等，相等状态不变（用于下一回合） */
						Network.State = 3;

						/* 当玩家全部退出，解锁URL */
						if(string.IsNullOrEmpty(nowPlayerList[0]) && string.IsNullOrEmpty(nowPlayerList[1]))
						{
							/* 全部退出后解锁 */
							Network.UnlockUrl = true;
						}
						else
						{
							/* 若有玩家锁定URL输入 */
							Network.UnlockUrl = false;
						}
					}
					else
					{
						/* 玩家名称不相等的时候默认重置 */
						Network.State = 1;

						/* 锁定 */
						Network.UnlockUrl = false;

						_ResetValue();
						_SetName(nowPlayerList);
					}
				}
				else if(gameState != 2)
				{
					/* 正常加入时切换玩家状态（若桌子在游戏中不触发） */
					Network.State = 1;

					/* 玩家加入后锁定 */
					Network.UnlockUrl = false;

					_ResetValue();
					_SetName(nowPlayerList);
				}
			}

			// 释放数组
			nowPlayerList = null;

			if (Network.funcStackTopLocal < 4)
			{
				Network.funcStack[Network.funcStackTopLocal] = 1;
				Network.funcStackTopLocal++;
			}
			Network._SetBufferStatus();
		}

		// ID2
		public void gameStartedLocal(string[] startPlayerList)
		{
			if (Network.State == 3 || Network.State == 1)
			{
				//是否反转
				if (startPlayerList[0] == Network.PlayerB || startPlayerList[1] == Network.PlayerA)
				{
					Network.isInvert = true;
				}
				else
				{
					Network.isInvert = false;
				}

				// 同步开局玩家名到本地变量(废弃)
				//Network.PlayerAStart = Network.PlayerA;
				//Network.PlayerBStart = Network.PlayerB;

				Network.State = 2;
			}

			startPlayerList = null;
			if (Network.funcStackTopLocal < 4)
			{
				Network.funcStack[Network.funcStackTopLocal] = 2;
				Network.funcStackTopLocal++;
			}
			Network._SetBufferStatus();
		}

		// ID3
		public void gameEndLocal(uint winningTeamLocal = 0xFFFFFFFF)
		{
			Debug.Log("[SCM] gameEndLocal");

			if (winningTeamLocal == (uint)0xFFFFFFFF)
				return;

			if (Network.State == 1)
			{
				_ResetValue();
				Network.MessagesState = false;
				Network.State = 0;
			}
			else if (Network.State == 2)
			{
				// 判断黄金开局
				if (winningTeamLocal != 2)
				{
					if (Network.isInvert)
						winningTeamLocal = (uint)(winningTeamLocal == 1 ? 0 : 1);

					if (winningTeamLocal == 0)
						Network.PlayerAScore++;
					else if (winningTeamLocal == 1)
						Network.PlayerBScore++;
				}

				Network.Date = DateTime.UtcNow.ToString("o");
				Network.Mode = _billiardsModule.gameModeLocal;
				Network.State = 3;
			}

			if (Network.funcStackTopLocal < 4)
			{
				Network.funcStack[Network.funcStackTopLocal] = 3;
				Network.funcStackTopLocal++;
			}

			winningTeamLocal = 0xFFFFFFFF;
			Network._SetBufferStatus();
		}

		// ID4
		public void gameResetLocal()
		{
			Debug.Log("[SCM] ResetSC");

			if (Network.funcStackTopLocal < 4)
			{
				Network.funcStack[Network.funcStackTopLocal] = 4;
				Network.funcStackTopLocal++;
			}

			/* PAC 状态重置 */
			Network.MessagesState = false;

			_ResetValue();
			Network.State = 0;
			Network._SetBufferStatus();
		}

		#endregion

		#region HookAPIs

		//API lobbyPlayerList
		public override void _LobbyOpen(string[] lobbyPlayerList)
		{
			lobbyOpenLocal(lobbyPlayerList);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="nowPlayerList">当前玩家列表</param>
		/// <param name="gameState">游戏状态 0 未初始化，1大厅，2游戏中，3游戏结束</param>
		public override void _PlayerChanged(string[] nowPlayerList, int gameState)
		{
			playerChangedLocal(nowPlayerList, gameState);
		}

		//API startPlayerList
		public override void _GameStarted(string[] startPlayerList)
		{
			gameStartedLocal(startPlayerList);
		}

		//API winningTeamLocal
		public override void _GameEnd(uint winningTeamLocal)
		{
			gameEndLocal(winningTeamLocal);
		}

		//NoAPI Value
		public override void _GameReset()
		{
			gameResetLocal();
		}

		public override void _SetPracticeMode(bool value)
		{
			Network.MessagesPracticeState = value;
		}

		#endregion

		public override void OnPlayerLeft(VRCPlayerApi player)
		{
			string leftPlayerName = player.displayName;
			if (leftPlayerName == Network.PlayerA || leftPlayerName == Network.PlayerB)
			{
				_RankingSystem.ClearURL();
				_GameReset();
			}
		}
	}
}