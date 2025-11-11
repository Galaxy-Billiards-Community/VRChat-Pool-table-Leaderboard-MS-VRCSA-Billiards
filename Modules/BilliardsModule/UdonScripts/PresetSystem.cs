using System.Linq;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Data;

public class PresetSystem : UdonSharpBehaviour
{

    [SerializeField] private Text presetName;
    [SerializeField] private TextAsset presetData;
    [SerializeField] private InputField targetInputField;

    private DataList presetDataNames;
    private DataList presetDataValues;

	private int presetIndex;

    public void Start()
    {
        presetIndex = -1;

        if (VRCJson.TryDeserializeFromJson(presetData.text, out var data))
        {
            var dataDic = data.DataDictionary;

			presetDataNames = dataDic.GetKeys();
            presetDataValues = dataDic.GetValues();
        }
        else
        {
            this.enabled = false;
        }
    }

    public void _Next()
    {
		presetIndex = (presetIndex + 1) % presetDataNames.Count;

		refreshData();
	}

    public void _Previous()
    {
		presetIndex = (presetIndex - 1 + presetDataNames.Count) % presetDataNames.Count;

		refreshData();
	}

    private void refreshData()
    {
		presetName.text = presetDataNames[presetIndex].String;
		targetInputField.text = presetDataValues[presetIndex].String;
	}
}
