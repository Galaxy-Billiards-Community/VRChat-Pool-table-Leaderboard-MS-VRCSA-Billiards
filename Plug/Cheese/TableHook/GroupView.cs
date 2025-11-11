
using UdonSharp;
using UnityEngine;
using VRC.Economy;
using VRC.SDKBase;
using VRC.Udon;

public class GroupView : UdonSharpBehaviour
{
    [SerializeField] private string groupID = string.Empty;

    public void _OpenGroup()
    {
		Store.OpenGroupPage(groupID);
	}
}
