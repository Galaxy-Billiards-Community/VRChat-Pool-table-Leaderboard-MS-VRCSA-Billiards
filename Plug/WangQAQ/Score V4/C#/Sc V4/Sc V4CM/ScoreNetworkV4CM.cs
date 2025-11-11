
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

namespace WangQAQ.UdonPlug
{
	public class ScoreNetworkV4CM : UdonSharpBehaviour
	{
		// 玩家VRC ID
		[HideInInspector][UdonSynced] public int PlayerA;
		[HideInInspector][UdonSynced] public int PlayerB;

		// 分数
		[HideInInspector][UdonSynced] public int PlayerAScore;
		[HideInInspector][UdonSynced] public int PlayerBScore;

		// 是否第一局游戏已结束
		[HideInInspector][UdonSynced] public bool isFirstGameDone;

		// Status (0 = Synced 1= need sync)
		private bool bufferStatus = false;

		// DL
		private ScoreManagerV4CM _scoreManager;

		#region dl

		public void _Init(ScoreManagerV4CM score)
		{
			_scoreManager = score;
		}

		#endregion

		#region Sync

		public void _SetBufferStatus()
		{
			bufferStatus = true;
		}

		public void _FlushBuffer()
		{
			if (!bufferStatus) return;

			bufferStatus = false;

			VRCPlayerApi localPlayer = Networking.LocalPlayer;
			if (!ReferenceEquals(null, localPlayer))
			{
				Networking.SetOwner(localPlayer, gameObject);
			}

			RequestSerialization();
			OnDeserialization();
		}

		/* 接受数据 */
		public override void OnDeserialization()
		{
			_scoreManager._OnRemoteDeserialization();
		}

		/* 发送失败重传 */
		public override void OnPostSerialization(SerializationResult result)
		{
			if (!result.success)
			{
				this.RequestSerialization();
			}
		}

		#endregion
	}
}
