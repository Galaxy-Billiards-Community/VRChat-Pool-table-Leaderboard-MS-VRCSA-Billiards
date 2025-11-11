
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Components;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class SettingUrlUpload : UdonSharpBehaviour
{
	[Header("目标输入框")]
	[SerializeField] private InputField copyInput;
	[SerializeField] private VRCUrlInputField pasteField;

	public void _Upload()
    {
		VRCUrl url = pasteField.GetUrl();
		pasteField.SetUrl(VRCUrl.Empty);

		if (url.ToString() != copyInput.text && copyInput.text != string.Empty)
			return;

		VRCStringDownloader.LoadUrl(url, (IUdonEventReceiver)this);

		copyInput.text = string.Empty;
	}

	public override void OnStringLoadSuccess(IVRCStringDownload result)
	{
		Debug.Log("Done.");
		copyInput.text = "Done.";
	}
}
