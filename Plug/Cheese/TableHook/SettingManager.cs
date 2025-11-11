using BestHTTP.Extensions;
using Blake2Sharp;
using System;
using System.Runtime.InteropServices;
using System.Text;
using UdonSharp;
using UnityEngine;
using UnityEngine.InputSystem;
using VRC.SDK3.Data;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using VRC.Udon.ProgramSources;

public class SettingManager : UdonSharpBehaviour
{
    [Header("URL")]
    [SerializeField] private VRCUrl url;
	[SerializeField] private string uploadUrlBase;

    [Space(10)]
	[Header("设置")]
	[SerializeField] private int maxReloadCount = 4;
    [SerializeField] public string _guid;
    [SerializeField] public string _key;

    private int reloadCount = 0;

	private TableHook _tableHook;

    public void _Init(TableHook tableHook)
    {
		_tableHook = tableHook;
	}

	public void Start()
    {
        VRCStringDownloader.LoadUrl(url, (IUdonEventReceiver)this);
    }

	public override void OnStringLoadSuccess(IVRCStringDownload result)
    {
        if(VRCJson.TryDeserializeFromJson(result.Result,out var dataToken))
        {
            var dataDic = dataToken.DataDictionary;
			var localPlayerName = Networking.LocalPlayer.displayName;

			if (dataDic != null && dataDic.TryGetValue(localPlayerName,out var playerToken))
            {
                _tableHook.LoadFromNetwork(playerToken.ToString());
			}
		}
    }

    public override void OnStringLoadError(IVRCStringDownload result)
    {
        if(reloadCount > maxReloadCount)
            return;

        SendCustomEventDelayedSeconds(nameof(_AutoReload), 30);

        reloadCount++;
	}

	private void _AutoReload()
	{
		VRCStringDownloader.LoadUrl(url, (IUdonEventReceiver)this);
	}

	private static string ToUrl(string base64)
	{
		return base64.Replace("+", "-").Replace("/", "_").Replace("=", "~");
	}
}
