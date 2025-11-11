
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ButtonTeleportation : UdonSharpBehaviour
{

	private VRCPlayerApi _player = null;

	public void OnEnable()
	{
		_player = Networking.LocalPlayer;

		if(_player.IsUserInVR())
			this.enabled = false;
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			var pos = _player.GetBonePosition(HumanBodyBones.Head);
			var rot = _player.GetBoneRotation(HumanBodyBones.Head);

			Vector3 tgtPos = pos + (rot * new Vector3(0, 0, 2));
			Quaternion tgtRot = rot * new Quaternion(0, 0.707f, 0, 0.707f);

			transform.rotation = tgtRot;
			transform.position = tgtPos;
		}
	}
}
