using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WangQAQ.PoolBuild
{
	public interface IPlugInitializer
	{
		/* 检查绑定的脚本是否有效 */
		public bool Init(string worldGuid,string worldKey);
	}
}
