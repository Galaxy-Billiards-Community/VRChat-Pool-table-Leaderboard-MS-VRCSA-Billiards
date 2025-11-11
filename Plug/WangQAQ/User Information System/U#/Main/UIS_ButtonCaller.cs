
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls;
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace WangQAQ.UdonPlug
{
	public class UIS_ButtonCaller : UdonSharpBehaviour
	{
		#region EditVariable

		[SerializeField] private Text TextName = null;

		#endregion

		#region RunTimeVariable

		[NonSerialized] public string Name = null;
		[NonSerialized] public string Ranking = null;
		[NonSerialized] public string UserData = null;

		private UIS_ContentLoader _ContentLoader = null;

		#endregion

		#region PublicAPI

		public void _Init()
		{
			_ContentLoader = transform.Find("../../../../../../U#/Info").GetComponent<UIS_ContentLoader>();

			TextName.text =
			(Ranking + ".").PadRight(5,' ')
			+ " "
			+ (Name.Length > 5 ? $"{Name.Substring(0, 5)}.." : Name);
		}

		public void _OnClick()
		{
			_ContentLoader.OnClick(Name, UserData);
			/* 调用函数 */
		}
		#endregion
	}
}