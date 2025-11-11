
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace WangQAQ.UdonPlug
{
	public class UIS_ListLoader : UdonSharpBehaviour
	{
		#region EditVariable

		[SerializeField] private Transform _Scroll;

		/* 按钮预制体 */
		[SerializeField] private GameObject _Button;

		/* Elo下载器 */
		[SerializeField] private IEloDownload _eloDownload;

		#endregion

		public void Start()
		{
			/* 开局等待30秒刷新一次 */
			SendCustomEventDelayedSeconds(nameof(_Reload), 30);
		}

		#region Event

		public void _Reload()
		{
			/* 调用刷新函数 */
			refreshList();
		}

		#endregion

		#region PrivateFunc

		private bool refreshList()
		{
			// 确保 _eloDownload 和 _eloNameList 不为空
			if (_eloDownload == null || _eloDownload._eloSeasonNameList == null || _eloDownload._eloSeasonNameList.Count == 0)
			{
				return false;
			}

			// 遍历 _eloNameList 列表，检查 _Scroll 下的现有按钮
			for (int i = 0; i < _eloDownload._eloSeasonNameList.Count; i++)
			{
				// 检查 _Scroll 下的子物体，看看是否已经有对应的按钮
				UIS_ButtonCaller obj;
				if (i < _Scroll.childCount)
				{
					// 如果有现有的按钮，重用它
					obj = _Scroll.GetChild(i).GetComponent<UIS_ButtonCaller>();
				}
				else
				{
					// 如果没有现有按钮，创建一个新的按钮
					obj = Instantiate(_Button, _Scroll).GetComponent<UIS_ButtonCaller>();
				}

				// 更新按钮的内容
				obj.Name = _eloDownload._eloSeasonNameList[i].ToString();
				obj.UserData = _eloDownload._eloSeasonDataList[i].Int.ToString();
				obj.Ranking = (i + 1).ToString();
				obj._Init();
			}

			// 如果 _Scroll 中有多余的子物体（即已经存在的按钮数量大于 _eloNameList），则销毁多余的按钮
			for (int i = _eloDownload._eloSeasonNameList.Count; i < _Scroll.childCount; i++)
			{
				Destroy(_Scroll.GetChild(i).gameObject);
			}

			return true;
		}

		#endregion
	}
}
