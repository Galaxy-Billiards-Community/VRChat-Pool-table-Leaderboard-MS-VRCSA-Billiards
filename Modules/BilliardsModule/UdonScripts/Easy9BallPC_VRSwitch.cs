
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Easy9BallPC_VRSwitch : UdonSharpBehaviour
{
    [SerializeField] private Renderer VrObject = null;
    [SerializeField] private Renderer PcObject = null; 

    void Start()
    {
        if (Networking.LocalPlayer.IsUserInVR())
        {
			VrObject.enabled = true;
			PcObject.enabled = false;
		}
        else
        {
			VrObject.enabled = false;
			PcObject.enabled = true;
		}
    }
}
