/*
 *  MIT License
 *  Copyright (c) 2024 WangQAQ
 *
 *  新计分器系统网络同步模块
 */

using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UIElements;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;
using WangQAQ.UdonPlug;

/// <summary>
///  请注意用户加入那一块的同步冲突问题
/// </summary>

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ScoreNetwork : UdonSharpBehaviour
{

	//Sync Value
	// 玩家名
	[HideInInspector][UdonSynced] public string PlayerA;
    [HideInInspector][UdonSynced] public string PlayerB;

    // 分数
    [HideInInspector][UdonSynced] public int PlayerAScore;
    [HideInInspector][UdonSynced] public int PlayerBScore;

    // 游戏模式
    [HideInInspector][UdonSynced] public uint Mode;

    // 状态量
    [HideInInspector][UdonSynced] public byte State;
    [HideInInspector][UdonSynced] public bool MessagesState = false;
	[HideInInspector][UdonSynced] public bool MessagesPracticeState = false;

	// 上传系统状态量 仅限 player change 事件
	[HideInInspector][UdonSynced] public bool UnlockUrl = false;

	// 上传时同步日期
	[HideInInspector][UdonSynced] public string Date = "";

    // 反转状态
    [HideInInspector][UdonSynced] public bool isInvert = false;

	/* 同步镜像数据 */
	#region Mirror

	[HideInInspector][UdonSynced] public byte funcStack0;
    [HideInInspector][UdonSynced] public byte funcStack1;
	[HideInInspector][UdonSynced] public byte funcStack2;
	[HideInInspector][UdonSynced] public byte funcStack3;
	[HideInInspector][UdonSynced] public int funcStackTop = 0;
	#endregion

	/* 本地数据 */
	[HideInInspector] public byte[] funcStack = new byte[4];
	/* 栈顶 */
	[HideInInspector] public int funcStackTopLocal = 0;

	// Status (0 = Synced 1= need sync)
	private bool bufferStatus = false;

    // DL
    private ScoreManagerV4 _scoreManager;

    #region dl

    public void _Init(ScoreManagerV4 score)
    {
        _scoreManager = score;

        // 初始化为-1
        for (int i = 0; i < funcStack.Length; i++) 
        {
            funcStack[i] = 0XFF;
        }
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
            Networking.SetOwner(localPlayer, this.gameObject);
        }

        this.RequestSerialization();
    }

    /* 发送前同步镜像 */
	public override void OnPreSerialization()
	{
        Debug.Log("[SCM] : OnPreSerialization");

		ArrayToBytes();
        funcStackTop = funcStackTopLocal;

        /* 调用本地接受事件（仅限主机） */
		OnDeserialization();
	}

	/* 接受数据 */
	public override void OnDeserialization()
    {
        BytesToArray();
		funcStackTopLocal = funcStackTop;

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

	void ArrayToBytes()
    {
        funcStack0 = funcStack[0];
        funcStack1 = funcStack[1];
		funcStack2 = funcStack[2];
		funcStack3 = funcStack[3];
	}

    void BytesToArray()
    {
        funcStack[0] = funcStack0;
        funcStack[1] = funcStack1;
        funcStack[2] = funcStack2;
        funcStack[3] = funcStack3;
    }
}
