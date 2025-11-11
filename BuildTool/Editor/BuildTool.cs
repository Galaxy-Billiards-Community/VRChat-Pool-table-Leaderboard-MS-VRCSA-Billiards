using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
namespace WangQAQ.PoolBuild
{
	
	public class BuildTool : EditorWindow
	{
		// 是否请求密钥
		private bool isOnOnlineMode = false;
		private string message = string.Empty;
		private bool canClick = true;

		private Initializer _initializer = new Initializer();

		/*public async void OnGUI()
		{
			isOnOnlineMode = GUILayout.Toggle(isOnOnlineMode, "启用排行榜功能，注：使用该功能会读取世界的 WorldGUID 与 WorldName 信息。");

			GUI.enabled = canClick;
			if (GUILayout.Button($"初始化{message}", GUILayout.Width(475), GUILayout.Height(30)))
			{
				canClick = false;
				message = " - " + await _initializer.init(isOnOnlineMode);
				canClick = true;
			}
			GUI.enabled = true;
		}*/
	}
	public class MenuItemBuildTool
	{
		[MenuItem("MS-VRCSA/Auto Init System", false, 0)]
		public static async Task ShowWindowAsync()
		{
			var _initializer = new Initializer();
			await _initializer.init(false);
		}
	}
}
