using UnityEditor;
using System.IO;
using UnityEngine;

public static class PluginPathHelper
{
	/// <summary>
	/// 获取插件核心类所在目录（无需创建临时对象）
	/// </summary>
	public static string GetPluginFolder<T>() where T : UnityEngine.Object
	{
		// 获取所有 MonoScript 资源
		string[] guids = AssetDatabase.FindAssets($"t:MonoScript");
		foreach (var guid in guids)
		{
			string path = AssetDatabase.GUIDToAssetPath(guid);
			MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
			if (script == null) continue;

			// 找到类型匹配的脚本
			if (script.GetClass() == typeof(T))
			{
				return Path.GetDirectoryName(path).Replace("\\", "/");
			}
		}

		return null;
	}
}
