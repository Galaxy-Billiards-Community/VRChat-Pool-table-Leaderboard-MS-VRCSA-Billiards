using System;
using UdonSharp;
using UnityEngine;
using VRC.Udon;
using UnityEngine.UI;
using VRC.SDKBase;
using TMPro;
using VRC.SDK3.Data;
using Unity.Mathematics;
using VRC.SDK3.Components;
using VRC.SDK3.Persistence;

public class TableHook : UdonSharpBehaviour
{
	[Header("设置")]
	//Data
	[SerializeField] public Texture2D[] cueSkins;
    //[SerializeField] public TextMeshProUGUI PlayerID;
    [NonSerialized] public float TableColor;
    [NonSerialized] public float TableColorLightness;
    [NonSerialized] public float cueHue;

    [NonSerialized] public bool CueTrajectory = false;
	[NonSerialized] public bool OtherTrajectory = false;

	[NonSerialized] public bool TargetBallHint = true;

	// Cue Skin & Ball Skin
	[NonSerialized] public int inOwner;
    [NonSerialized] public byte outCanUse;
    private byte outCanUseTmp = 0;
    [NonSerialized] public byte ball;
    public byte DefaultCue;
    private int isRotating;
    [NonSerialized] private int maxRotation=180;

    [Space(10)]
    [Header("插件")]
	//Save & Load
    [SerializeField] private SettingManager _settingManager;
    [SerializeField] private Translations hookTranslation;

	[Space(10)]
	[Header("UI")]

	[SerializeField] private InputField inputField;
    [SerializeField] private VRCUrlInputField urlField;

	[SerializeField] private Slider tableColorSlider;
	[SerializeField] private Slider tableColorLightnessSlider;
	[SerializeField] private Material tableShow;

	[SerializeField] private Slider cueSizeSlider;
	[SerializeField] private TextMeshProUGUI cueSizeText;

	[SerializeField] private Slider cueThicknessSlider;
	[SerializeField] private TextMeshProUGUI cueThicknessText;

	[SerializeField] private Slider cueSmoothingSlider;
	[SerializeField] private TextMeshProUGUI cueSmoothingText;

	[SerializeField] private Slider cueColorShiftSlider;
	[SerializeField] private TextMeshProUGUI cueColorShiftText;

	[SerializeField] private Renderer renderer;

    [SerializeField] private InputField codeInput = null;

	[SerializeField] private Toggle cueTrajectoryToggle;
	[SerializeField] private Toggle otherTrajectoryToggle;

	[SerializeField] private Toggle targetBallHintToggle;

	public float cueThicknessValue => cueThicknessSlider.value;

	private DataList table = new DataList();
    private DataList _translations = new DataList();

	private Material cueStickMaterial;
	private Material cueCentermaterial;

	public void OnEnable()
    {
        _settingManager._Init(this);
		_translations.Add(hookTranslation);

        //PlayerID.text = Networking.LocalPlayer.displayName;

        cueHue = 0;
        outCanUse = 0;
        ball = 0;
        outCanUseTmp = DefaultCue;
        isRotating = maxRotation;
        //Table
        TableColor = 0;
        TableColorLightness = 1
            ;
        //CUE
        Material[] materials = renderer.materials;
        cueCentermaterial=materials[0];
        cueStickMaterial = materials[1];
        cueCentermaterial.name = cueCentermaterial + "forTableHook";
        cueStickMaterial.name = cueStickMaterial + "forTableHook";
        renderer.materials = new Material[] {cueCentermaterial,cueStickMaterial};//create a new instance for hook


    }

    public void Reset()
    {
        _Cue0();
        outCanUse = 0;
        outCanUseTmp = 0;
        ball=0;

        tableColorSlider.value = 0;
        tableColorLightnessSlider.value = 1;
        tableShow.SetFloat("_ClothHue", 0);
        tableShow.SetFloat("_ClothSaturation", 1);
        TableColor = 0;
        TableColorLightness = 1;

        cueSizeSlider.value = 10;
        cueSmoothingSlider.value = 10;
        cueColorShiftSlider.value = 0;
        setCueSize();
        setCueSmoothing();
        setCueColorShift();

    }
    public void _CanUseCueSkin()
    {
            outCanUse = outCanUseTmp;
    }

    private void ChangeMaterial()
    {
        if (cueSkins[outCanUseTmp] != null)
        {
                renderer.materials[1].SetTexture("_MainTex", cueSkins[outCanUseTmp]);
        }
        isRotating = 0;
    }
    void Update()
    {
        if (isRotating < maxRotation)
        {
            renderer.transform.Rotate(new Vector3(1, 0.05f, 0.05f), Mathf.Clamp(maxRotation-isRotating,0,3), Space.Self);
            isRotating++;
        }
    }
    public void AddTranslation(Translations translations)
    {
        _translations.Add(translations);
    }

    public void AddBilliardsModule(BilliardsModule module)
    {
        table.Add(module);
    }

    public void setTableColor()
    {
        TableColor = tableColorSlider.value;
        tableShow.SetFloat("_ClothHue", TableColor);
    }
    public void setTableColorLightness()
    {
        TableColorLightness = tableColorLightnessSlider.value;
        tableShow.SetFloat("_ClothSaturation", TableColorLightness);
    }

	public void setCueSize()
	{
		float newScale = cueSizeSlider.value / 10f;
		float newThickness = cueThicknessSlider.value / 10f;
		renderer.transform.localScale = new Vector3(0.3f * newScale, 0.3f * newThickness, newThickness * 0.3f);
		foreach (var tabletmp in table.ToArray())
		{
			((BilliardsModule)tabletmp.Reference).cueControllers[0].setScale(newScale, newThickness);
			((BilliardsModule)tabletmp.Reference).cueControllers[1].setScale(newScale, newThickness);
			((BilliardsModule)tabletmp.Reference).menuManager.cueSizeSlider.value = cueSizeSlider.value;
			((BilliardsModule)tabletmp.Reference).menuManager.cueSizeText.text = newScale.ToString("F1");
		}
		cueSizeText.text = newScale.ToString("F1");
		cueThicknessText.text = newThickness.ToString("F1");
	}

	public void setCueSmoothing()
    {
        float newSmoothing = cueSmoothingSlider.value / 10f;
        foreach (var tabletmp in table.ToArray())
        {
            ((BilliardsModule)tabletmp.Reference).cueControllers[0].setSmoothing(newSmoothing);
            ((BilliardsModule)tabletmp.Reference).cueControllers[1].setSmoothing(newSmoothing);
            ((BilliardsModule)tabletmp.Reference).menuManager.cueSmoothingSlider.value = cueSmoothingSlider.value;
            ((BilliardsModule)tabletmp.Reference).menuManager.cueSmoothingText.text = newSmoothing.ToString("F1");
        }
        cueSmoothingText.text = newSmoothing.ToString("F1");
    }

    public void setCueColorShift()
    {
        float newShift = cueColorShiftSlider.value;
        cueHue = newShift;
        Color color = new Color(1,1,1);
        if(cueHue != 0)
        {
            color = Color.HSVToRGB(newShift, 1f, 1f);
        }

        renderer.materials[1].color = color;

        cueColorShiftText.text = newShift.ToString("F1");

    }

    public void setTrajectory()
    {
        CueTrajectory = cueTrajectoryToggle.isOn;
        OtherTrajectory = otherTrajectoryToggle.isOn;
	}

    public void setTargetBallHint()
    {
		TargetBallHint = targetBallHintToggle.isOn;
	}

	//Sava and load system
	#region ConvertFunction
	private void floatToBytes(byte[] data, int pos, float v)
    {
        byte[] bytes = BitConverter.GetBytes(v);
        Array.Copy(bytes, 0, data, pos, 4);
    }

    public float bytesToFloat(byte[] data, int pos)
    {
        byte[] floatBytes = new byte[4];
        Array.Copy(data, pos, floatBytes, 0, 4);
        return BitConverter.ToSingle(floatBytes, 0);
    }

	private void bool16ToBytes(byte[] data, int pos, bool[] values)
	{
		if (values.Length != 16)
			return;

		for (int i = 0; i < 2; i++)
			data[pos + i] = 0;

		for (int i = 0; i < 16; i++)
		{
			if (values[i])
			{
				data[pos + i / 8] |= (byte)(1 << (i % 8));
			}
		}
	}

	private bool[] bytesToBool16(byte[] data, int pos)
	{
		bool[] result = new bool[16];

		for (int i = 0; i < 16; i++)
		{
			result[i] = (data[pos + i / 8] & (1 << (i % 8))) != 0;
		}

		return result;
	}

	private bool isInvalidBase64Char(char value)
    {
        var intValue = (int)value;

        // 1 - 9
        if (intValue >= 48 && intValue <= 57)
            return false;

        // A - Z
        if (intValue >= 65 && intValue <= 90)
            return false;

        // a - z
        if (intValue >= 97 && intValue <= 122)
            return false;

        // + or /
        return intValue != 43 && intValue != 47;
    }

    private bool isValidBase64(string value)
    {
        if (value == null || value.Length == 0 || value.Length % 4 != 0
            || value.Contains(" ") || value.Contains("\t") || value.Contains("\r") || value.Contains("\n"))
            return false;
        var index = value.Length - 1;

        if (value[index] == '=')
            index--;

        if (value[index] == '=')
            index--;

        for (var i = 0; i <= index; i++)
            if (isInvalidBase64Char(value[i]))
                return false;

        return true;
    }
	#endregion

	#region Save & Load
	// I Call it : Cheese Version ,for short CV,rewrite from "NetworingManagers" 
	uint LocalDataLengthV1 = 23;
	uint LocalDataLengthV2 = 27;
	uint LocalDataLengthV3 = 29;
	private string EncodeLocalData()
	{
		byte[] gameState = new byte[LocalDataLengthV3];
		int encodePos = 0;
		gameState[encodePos] = outCanUseTmp;
		encodePos += 1;
		gameState[encodePos] = ball;
		encodePos += 1;
		floatToBytes(gameState, encodePos, TableColor);
		encodePos += 4;
		floatToBytes(gameState, encodePos, TableColorLightness);
		encodePos += 4;

		// CV1
		floatToBytes(gameState, encodePos, cueSizeSlider.value);
		encodePos += 4;
		floatToBytes(gameState, encodePos, cueSmoothingSlider.value);
		encodePos += 4;
		floatToBytes(gameState, encodePos, cueHue);
		encodePos += 4;

		// CV2
		floatToBytes(gameState, encodePos, cueThicknessSlider.value);
		encodePos += 4;

		// CV3 开关状态
		bool[] toggleState = new bool[16];

        // 轨迹
        toggleState[0] = CueTrajectory; 
        toggleState[1] = OtherTrajectory;
        toggleState[2] = TargetBallHint;

		bool16ToBytes(gameState, encodePos, toggleState);
		encodePos += 2;

		return "CV3:" + Convert.ToBase64String(gameState);
	}

	private void LoadLocalDataV0(string gameStateStr)
    {
        if (!isValidBase64(gameStateStr)) return;

        byte[] gameState = Convert.FromBase64String(gameStateStr);

        int encoodePos = 0;

        outCanUseTmp = gameState[encoodePos];
        encoodePos += 1;
        ball = gameState[encoodePos];
        encoodePos += 1;
        TableColor = bytesToFloat(gameState, encoodePos);
        encoodePos += 4;
        TableColorLightness = bytesToFloat(gameState,encoodePos);
        encoodePos += 4;

        ChangeMaterial();
    }
	private void LoadLocalDataV1(string gameStateStr)
    {
        if (!isValidBase64(gameStateStr)) return;

        byte[] gameState = Convert.FromBase64String(gameStateStr);
        if (gameState.Length != LocalDataLengthV1) return;

        int encoodePos = 0;

        outCanUseTmp = gameState[encoodePos];
        encoodePos += 1;
        ball = gameState[encoodePos];
        encoodePos += 1;
        TableColor = bytesToFloat(gameState, encoodePos);
        encoodePos += 4;
        TableColorLightness = bytesToFloat(gameState, encoodePos);
        encoodePos += 4;
        tableColorSlider.value = TableColor;
        tableColorLightnessSlider.value = TableColorLightness;
        tableShow.SetFloat("_ClothHue", TableColor);
        tableShow.SetFloat("_ClothSaturation", TableColorLightness);

        //dif
        cueSizeSlider.value = bytesToFloat(gameState, encoodePos);
        encoodePos += 4;
        cueSmoothingSlider.value =bytesToFloat(gameState, encoodePos);
        encoodePos += 4;
        cueHue = bytesToFloat(gameState, encoodePos);
        cueColorShiftSlider.value = cueHue;
        encoodePos += 4;

        setCueSize();
        setCueSmoothing();
        setCueColorShift();

        ChangeMaterial();
    }
	private void LoadLocalDataV2(string gameStateStr)
	{
		if (!isValidBase64(gameStateStr)) return;

		byte[] gameState = Convert.FromBase64String(gameStateStr);
		if (gameState.Length != LocalDataLengthV2) return;

		int encoodePos = 0;

		outCanUseTmp = gameState[encoodePos];
		encoodePos += 1;
		ball = gameState[encoodePos];
		encoodePos += 1;
		TableColor = bytesToFloat(gameState, encoodePos);
		encoodePos += 4;
		TableColorLightness = bytesToFloat(gameState, encoodePos);
		encoodePos += 4;
		tableColorSlider.value = TableColor;
		tableColorLightnessSlider.value = TableColorLightness;
		tableShow.SetFloat("_ClothHue", TableColor);
		tableShow.SetFloat("_ClothSaturation", TableColorLightness);

		//dif
		cueSizeSlider.value = bytesToFloat(gameState, encoodePos);
		encoodePos += 4;
		cueSmoothingSlider.value = bytesToFloat(gameState, encoodePos);
		encoodePos += 4;
		cueHue = bytesToFloat(gameState, encoodePos);
		cueColorShiftSlider.value = cueHue;
		encoodePos += 4;

		//dif2
		cueThicknessSlider.value = bytesToFloat(gameState, encoodePos);
		encoodePos += 4;

		setCueSize();
		setCueSmoothing();
		setCueColorShift();

		ChangeMaterial();
	}
	private void LoadLocalDataV3(string gameStateStr)
	{
		if (!isValidBase64(gameStateStr)) return;

		byte[] gameState = Convert.FromBase64String(gameStateStr);
		if (gameState.Length != LocalDataLengthV3) return;

		int encoodePos = 0;

		outCanUseTmp = gameState[encoodePos];
		encoodePos += 1;
		ball = gameState[encoodePos];
		encoodePos += 1;
		TableColor = bytesToFloat(gameState, encoodePos);
		encoodePos += 4;
		TableColorLightness = bytesToFloat(gameState, encoodePos);
		encoodePos += 4;
		tableColorSlider.value = TableColor;
		tableColorLightnessSlider.value = TableColorLightness;
		tableShow.SetFloat("_ClothHue", TableColor);
		tableShow.SetFloat("_ClothSaturation", TableColorLightness);

		//dif
		cueSizeSlider.value = bytesToFloat(gameState, encoodePos);
		encoodePos += 4;
		cueSmoothingSlider.value = bytesToFloat(gameState, encoodePos);
		encoodePos += 4;
		cueHue = bytesToFloat(gameState, encoodePos);
		cueColorShiftSlider.value = cueHue;
		encoodePos += 4;

		//dif2
		cueThicknessSlider.value = bytesToFloat(gameState, encoodePos);
		encoodePos += 4;

		// CV3 开关状态
		bool[] toggleState = bytesToBool16(gameState, encoodePos);
		encoodePos += 2;

		// 轨迹
		cueTrajectoryToggle.isOn = toggleState[0];
		otherTrajectoryToggle.isOn = toggleState[1];

		targetBallHintToggle.isOn = !toggleState[2];

		setCueSize();
		setCueSmoothing();
		setCueColorShift();
        setTrajectory();
        setTargetBallHint();

		ChangeMaterial();
	}

	private void LoadLocalData(string gameStateStr)
    {
		if (gameStateStr.StartsWith("CV3:"))
		{
			LoadLocalDataV3(gameStateStr.Substring(4));
		}
		else if (gameStateStr.StartsWith("CV2:"))
		{
			LoadLocalDataV2(gameStateStr.Substring(4));
		}
		else if (gameStateStr.StartsWith("CV1:"))
        {
            LoadLocalDataV1(gameStateStr.Substring(4));
        }
        else if (gameStateStr.StartsWith("CV:"))
        {
            LoadLocalDataV0(gameStateStr.Substring(3));
        }
    }
    /*
    public void OnGenerateUrlButtonPushed()
    {
        if (ReferenceEquals(null, inputField))
        {
            Debug.Log("Table Hook::OnSaveButtonPushed() inputField property is not set !");
            return;
        }

        var playerName = Networking.LocalPlayer.displayName;
        inputField.text = _settingManager._GenerateUrl(playerName, EncodeLocalData());
    }
    */

    public void LoadFromNetwork(string setting)
    {
        LoadLocalData(setting);
    }

	public void OnSaveButtonPushed()
    {
        if(codeInput == null)
        {
            return;
        }

        codeInput.text = EncodeLocalData();
    }

	public void OnLoadButtonPushed()
    {

	    if (ReferenceEquals(null, codeInput))
	    {
		    Debug.Log("Table Hook::OnSaveButtonPushed() inputField property is not set !");
		    return;
	    }

	    if (string.IsNullOrEmpty(codeInput.text))
	    {
		    return;
	    }

	    //if (!_IsPlayer(Networking.LocalPlayer)) return; //not load on others game

	    LoadLocalData(codeInput.text);

    }

	#endregion

	#region Cue & Ball
	public void _Cue0()
    {
        outCanUseTmp = 0;
        ChangeMaterial();
    }

    public void _Cue1()
    {
        outCanUseTmp = 1;
        ChangeMaterial();
    }
    public void _Cue2()
    {
        outCanUseTmp = 2;
        ChangeMaterial();
    }

    public void _Cue3()
    {
        outCanUseTmp = 3;
        ChangeMaterial();
    }
    public void _Cue4()
    {
        outCanUseTmp = 4;
        ChangeMaterial();
    }

    public void _Cue5()
    {
        outCanUseTmp = 5;
        ChangeMaterial();
    }
    public void _Cue6()
    {
        outCanUseTmp = 6;
        ChangeMaterial();
    }

    public void _Cue7()
    {
        outCanUseTmp = 7;
        ChangeMaterial();
    }
    public void _Cue8()
    {
        outCanUseTmp = 8;
        ChangeMaterial();
    }
    public void _Cue9()
    {
        outCanUseTmp = 9;
        ChangeMaterial();
    }

    public void _Cue10()
    {
        outCanUseTmp = 10;
        ChangeMaterial();
    }
    public void _Cue11()
    {
        outCanUseTmp = 11;
        ChangeMaterial();
    }
    public void _Cue12()
    {
        outCanUseTmp = 12;
        ChangeMaterial();
    }

    public void _Cue13()
    {
        outCanUseTmp = 13;
        ChangeMaterial();
    }
    public void _Cue14()
    {
        outCanUseTmp = 14;
        ChangeMaterial();
    }
    public void _Cue15()
    {
        outCanUseTmp = 15;
        ChangeMaterial();
    }
    public void _Cue16()
    {
        outCanUseTmp = 16;
        ChangeMaterial();
    }
    public void _Cue17()
    {
        outCanUseTmp = 17;
        ChangeMaterial();
    }
    public void _Cue18()
    {
        outCanUseTmp = 18;
        ChangeMaterial();
    }
    public void _Cue19()
    {
        outCanUseTmp = 19;
        ChangeMaterial();
    }
    public void _Cue20()
    {
        outCanUseTmp = 20;
        ChangeMaterial();
    }
    public void _Ball0()
    {
        ball = 0;
    }
    public void _Ball1()
    {
        ball = 4;
    }
    public void _Ball2()
    {
        ball = 5;
    }
    public void _Ball3()
    {
        ball = 6;
    }
    #endregion

    #region Language

    public void SetLanguage(string language)
    {
        foreach(var translate in _translations.ToArray()) ((Translations)translate.Reference).SetLanguage(language);
    }

    public void zh() { SetLanguage("zh"); }
    public void en() { SetLanguage("en"); }
    public void ja() { SetLanguage("ja"); }

    #endregion
}