using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC.Core;
using VRC.SDKBase.Editor.Api;
using WangQAQ.PoolBuild;
using WangQAQ.UdonPlug;

public class Initializer
{
	const string mapPathUrl = "https://galaxy-pool.imoe.xyz/UploadMapKey";

	[Serializable]
	public class PostMap
	{
		public string WorldGuid;
		public string WorldName;
		public string WorldKey;
		public string Key;
	}

	[Serializable]
	private class MapInformationData
	{
		public string Key;
		public string WorldGuid;
		public string WorldName;
		public string WorldKey;
	}

	IPlugInitializer[] plugInitializer = new IPlugInitializer[]
	{
		new BilliardsBinder(),
		new ColorBinder(),
		new EloDownloadBinder(),
		new NewsBinder(),
		new ScoreBinder(),
		new SettingLoaderBinder()
	};

	public async Task<string> init(bool isOnline)
	{
		try
		{
			// 检查组件安装情况
			check();

			// 上传WorldGuid和WorldKey，获得鉴权密钥
			string worldGuid = string.Empty,
				   worldName = string.Empty,
				   worldKey = string.Empty,
				   key = string.Empty;

			if (isOnline)
			{
				(worldGuid, worldName) = await getInformationFromSDK();
				key = tryGetKeyFromFile(worldGuid);

				// 未上传过密钥，开始上传密钥
				if (key == null)
					key = generateRandomKey(16);

				worldKey = generateRandomKey(32);
				await uploadMapKey(worldGuid, worldName, worldKey, key);
			}

			// 绑定脚本
			foreach(var a in plugInitializer)
			{
				a.Init(worldGuid, worldKey);
			}

			// 初始化碰撞
			MS_VRCSA_Menu.setPoolTableCollisionLayers();
		}
		catch (ArgumentException ex)
		{
			return ex.Message;
		}
		catch (HttpRequestException)
		{
			return "失败，网络错误。";
		}
		catch (Exception)
		{
			return "发生未知错误。";
		}

		return "安装完成";
	}

	#region Check
	private void check()
	{
		var tableHook = Component.FindObjectsOfType<TableHook>();

		if (tableHook.Length != 1)
			throw new ArgumentException("请确保有且仅有一个Pad组件");
	}
	#endregion

	#region Key
	private async Task<(string worldGuid, string worldName)> getInformationFromSDK()
	{
		VRCWorld vrcWorldOBJ;
		// 获取世界对象
		try
		{
			var pipelineOBJ = Component.FindObjectsOfType<PipelineManager>().Single();

			vrcWorldOBJ = await VRCApi.GetWorld(pipelineOBJ.blueprintId);

			if (vrcWorldOBJ.Name == null || vrcWorldOBJ.ID == null)
				throw new ArgumentException("关键性错误 - E100");
		}
		catch
		{
			throw new ArgumentException("关键性错误 - E101");
		}

		var GUID = vrcWorldOBJ.ID.Split('_')[1];
		var Name = vrcWorldOBJ.Name;

		return (GUID, Name);
	}

	private string tryGetKeyFromFile(string worldGuid)
	{
		string path = PluginPathHelper.GetPluginFolder<BuildTool>();
		string infoPath = Path.Combine(path, $"Keys/{worldGuid}.json");

		if (!File.Exists(infoPath))
			return null;

		string jsonInfo = File.ReadAllText(infoPath);
		var obj = JsonUtility.FromJson<MapInformationData>(jsonInfo);

		return obj.Key;
	}

	private string generateRandomKey(int length)
	{
		// 每个字符可以表示为 4 位（二进制）或者 8 位（ASCII），我们用 Base64 编码
		byte[] randomBytes = new byte[length];
		using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(randomBytes);
		}

		// 使用 Base64 编码，使结果更加可读
		return Convert.ToBase64String(randomBytes).Substring(0, length);
	}

	private async Task uploadMapKey(string worldGuid, string worldName, string worldKey, string Key)
	{
		var data = new MapInformationData
		{
			Key = Key,
			WorldGuid = worldGuid,
			WorldName = worldName,
			WorldKey = worldKey
		};

		var jsonString = JsonUtility.ToJson(data, true);

		using (HttpClient _client = new HttpClient())
		{
			var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _client.PostAsync(mapPathUrl, content);

			// 可选：读取返回内容
			string responseBody = await response.Content.ReadAsStringAsync();
			Console.WriteLine(responseBody);

			if (!response.IsSuccessStatusCode)
				throw new ArgumentException("网络错误 - E200");

			string path = PluginPathHelper.GetPluginFolder<BuildTool>();
			string infoPath = Path.Combine(path, $"Keys/{worldGuid}.json");

			File.WriteAllText(infoPath, jsonString);
		}
	}
	#endregion
}
