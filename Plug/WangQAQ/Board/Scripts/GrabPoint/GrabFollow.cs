
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace WangQAQ.UdonPlug
{
	public class GrabFollow : UdonSharpBehaviour
	{
		[SerializeField] private Transform _target;
		[SerializeField] private Quaternion _rotation;

		private VRCPlayerApi _player;
		private bool _onPickUp = false;

		public void Start()
		{
			_player = Networking.LocalPlayer;
			if (!_player.IsUserInVR())
			{	
				this.gameObject.SetActive(false);
				this.enabled = false;
			}
		}

		public void Update()
		{
			if (_onPickUp)
				return;

			var pos = _player.GetBonePosition(HumanBodyBones.Head);
			var rot = _player.GetBoneRotation(HumanBodyBones.Head);

			Vector3 offset = rot * Vector3.forward * -0.2f;
			this.transform.position = pos + offset;
			this.transform.rotation = rot;
		}

		public override void OnPickup()
		{
			_target.parent = this.transform;
			_target.localPosition = new Vector3(0,0.45f, 0);	
			_target.localRotation = _rotation;

			_onPickUp = true;
		}

		public override void OnDrop()
		{
			_target.parent = null;

			_onPickUp = false;
		}
	}

}
