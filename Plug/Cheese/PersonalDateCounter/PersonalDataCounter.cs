
using System;
using TMPro;
using UdonSharp;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Persistence;
using VRC.SDKBase;

namespace Cheese
{
    public class PersonalDataCounter : UdonSharpBehaviour
    {
        //todo 还有平均出杆时间等
        [Header("Text")]
        [SerializeField] private Text _gameCountText;
		[SerializeField] private Text _winCountText;
		[SerializeField] private Text _loseCountText;
		[SerializeField] private Text _pocketCountText;
		[SerializeField] private Text _inningCountText;
		[SerializeField] private Text _shotCountText;
		[SerializeField] private Text _scratchCountText;
		[SerializeField] private Text _foulEndText;
		[SerializeField] private Text _breakFoulText;
		[SerializeField] private Text _foulCountText;
		[SerializeField] private Text _lossOfChangeText;
		[SerializeField] private Text _goldenBreakText;
		[SerializeField] private Text _clearanceText;
		[SerializeField] private Text _breakClearanceText;

        [SerializeField] private Text _victoryRateText;
		[SerializeField] private Text _shotAccuracyText; 
		[SerializeField] private Text _potSuccessText; 
		[SerializeField] private Text _clearancePerText;
		[SerializeField] private Text _leaveText;
        [SerializeField] private Text _progressText;

		[SerializeField] private Text _gameCountSnookerText;
		[SerializeField] private Text _pocketCountSnookerText;
		[SerializeField] private Text _shotCountSnookerText;
		[SerializeField] private Text _inningCountSnookerText;
        [SerializeField] private Text _shotAccuracySnookerText;
		[SerializeField] private Text _heightBreakText;

        [SerializeField] private Text _shotCountHomeText;
        [SerializeField] private Text _clearanceHomeText;

		public Image image;

        //Datas
        [NonSerialized] public int gameCount = 0;      //场次
		[NonSerialized] public int winCount = 0;       //胜场
        [NonSerialized] public int loseCount = 0;      //败场
        [NonSerialized] public int pocketCount = 0;    //进球数
        [NonSerialized] public int inningCount = 0;    //回合数
        [NonSerialized] public int shotCount = 0;       //空杆数  
        [NonSerialized] public int scratchCount = 0;    //摔袋数

        [NonSerialized] public int foulEnd = 0;        //犯规结束
        [NonSerialized] public int breakFoul = 0;      //开球犯规
        [NonSerialized] public int foulCount = 0;      //犯规次数
        [NonSerialized] public int lossOfChange = 0;   //开球失机(开球没下次数)
        [NonSerialized] public int goldenBreak = 0;     //黄金开球
        [NonSerialized] public int clearance = 0;      //一杆清台次数
        [NonSerialized] public int breakClearance = 0; //炸清次数

        [NonSerialized] public int gameCountSnooker = 0;    //斯诺克场次
        [NonSerialized] public int pocketCountSnooker = 0;  //斯诺克进球数
        [NonSerialized] public int shotCountSnooker = 0;    //斯诺克回合数
        [NonSerialized] public int inningCountSnooker = 0;  //斯诺克击球数
        [NonSerialized] public int heightBreak = 0;         //单杆最高分

        //Keys to save/load persist data
        private const string GAME_COUNT = "GameCount";
        private const string WIN_COUNT = "WinCount";
        private const string LOSE_COUNT = "LoseCount";
        private const string POCKET_COUNT = "PocketCount";
        private const string INNING_COUNT = "InningCount";
        private const string SHOT_COUNT = "ShotCount";
        private const string SCRATCH_COUNT = "ScratchCount";
        private const string FOUL_END = "FoulEnd";
        private const string BREAK_FOUL = "BreakFoul";
        private const string FOUL_COUNT = "FoulCount";
        private const string LOSS_OF_CHANGE = "LossOfChange";
        private const string GOLDEN_BREAK = "GoldenBreak";
        private const string CLEARANCE = "Clearance";
        private const string BREAK_CLEARANCE = "BreakClearance";

        private const string GAME_COUNT_SNOOKER = "GameCountSnooker";
        private const string POCKET_COUNT_SNOOKER = "PocketCountSnooker";
        private const string SHOT_COUNT_SNOOKER = "ShotCountSnooker";
        private const string INNNING_COUNT_SNOOKER = "InningCountSnooker";
        private const string HEIGHT_BREAK = "HeightBreak";

        void Start()
        {
            UpdateDataText();
        }

        public override void OnPlayerRestored(VRCPlayerApi player)
        {
            if (!player.isLocal) return;

            if (PlayerData.HasKey(player, GAME_COUNT)) gameCount = PlayerData.GetInt(player, GAME_COUNT);
            if (PlayerData.HasKey(player, WIN_COUNT)) winCount = PlayerData.GetInt(player, WIN_COUNT);
            if (PlayerData.HasKey(player, LOSE_COUNT)) loseCount = PlayerData.GetInt(player, LOSE_COUNT);
            if (PlayerData.HasKey(player, POCKET_COUNT)) pocketCount = PlayerData.GetInt(player, POCKET_COUNT);
            if (PlayerData.HasKey(player, INNING_COUNT)) inningCount = PlayerData.GetInt(player, INNING_COUNT);
            if (PlayerData.HasKey(player, SHOT_COUNT)) shotCount = PlayerData.GetInt(player, SHOT_COUNT);
            if (PlayerData.HasKey(player, SCRATCH_COUNT)) scratchCount = PlayerData.GetInt(player, SCRATCH_COUNT);
            if (PlayerData.HasKey(player, FOUL_END)) foulEnd = PlayerData.GetInt(player, FOUL_END);
            if (PlayerData.HasKey(player, BREAK_FOUL)) breakFoul = PlayerData.GetInt(player, BREAK_FOUL);
            if (PlayerData.HasKey(player, FOUL_COUNT)) foulCount = PlayerData.GetInt(player, FOUL_COUNT);
            if (PlayerData.HasKey(player, LOSS_OF_CHANGE)) lossOfChange = PlayerData.GetInt(player, LOSS_OF_CHANGE);
            if (PlayerData.HasKey(player, GOLDEN_BREAK)) goldenBreak = PlayerData.GetInt(player, GOLDEN_BREAK);
            if (PlayerData.HasKey(player, CLEARANCE)) clearance = PlayerData.GetInt(player, CLEARANCE) >= 0 ? PlayerData.GetInt(player, CLEARANCE) : 0;
            if (PlayerData.HasKey(player, BREAK_CLEARANCE)) breakClearance = PlayerData.GetInt(player, BREAK_CLEARANCE) >= 0 ? PlayerData.GetInt(player, BREAK_CLEARANCE) : 0;
            //snooker
            if (PlayerData.HasKey(player, GAME_COUNT_SNOOKER)) gameCountSnooker = PlayerData.GetInt(player, GAME_COUNT_SNOOKER);
            if (PlayerData.HasKey(player, POCKET_COUNT_SNOOKER)) pocketCountSnooker = PlayerData.GetInt(player, POCKET_COUNT_SNOOKER);
            if (PlayerData.HasKey(player, SHOT_COUNT_SNOOKER)) shotCountSnooker = PlayerData.GetInt(player, SHOT_COUNT_SNOOKER);
            if (PlayerData.HasKey(player, INNNING_COUNT_SNOOKER)) inningCountSnooker = PlayerData.GetInt(player, INNNING_COUNT_SNOOKER);
            if (PlayerData.HasKey(player, HEIGHT_BREAK)) heightBreak = PlayerData.GetInt(player, HEIGHT_BREAK);

            SendCustomEventDelayedSeconds("syncData", 5f);

            UpdateDataText();
        }
        // Method to save player data
        public void SaveData()
        {
            PlayerData.SetInt(GAME_COUNT, gameCount);
            PlayerData.SetInt(WIN_COUNT, winCount);
            PlayerData.SetInt(LOSE_COUNT, loseCount);
            PlayerData.SetInt(POCKET_COUNT, pocketCount);
            PlayerData.SetInt(INNING_COUNT, inningCount);
            PlayerData.SetInt(SHOT_COUNT, shotCount);
            PlayerData.SetInt(SCRATCH_COUNT, scratchCount);
            PlayerData.SetInt(FOUL_END, foulEnd);
            PlayerData.SetInt(BREAK_FOUL, breakFoul);
            PlayerData.SetInt(FOUL_COUNT, foulCount);
            PlayerData.SetInt(LOSS_OF_CHANGE, lossOfChange);
            PlayerData.SetInt(GOLDEN_BREAK, goldenBreak);
            PlayerData.SetInt(CLEARANCE, clearance);
            PlayerData.SetInt(BREAK_CLEARANCE, breakClearance);
            //snooker
            PlayerData.SetInt(GAME_COUNT_SNOOKER, gameCountSnooker);
            PlayerData.SetInt(POCKET_COUNT_SNOOKER, pocketCountSnooker);
            PlayerData.SetInt(SHOT_COUNT_SNOOKER, shotCountSnooker);
            PlayerData.SetInt(INNNING_COUNT_SNOOKER, inningCountSnooker);
            PlayerData.SetInt(HEIGHT_BREAK, heightBreak);

            UpdateDataText();

        }
        public void UpdateDataText()
        {
            _gameCountText.text = gameCount.ToString();
            _winCountText.text = winCount.ToString();
            _loseCountText.text = loseCount.ToString();
            _pocketCountText.text = pocketCount.ToString();
            _inningCountText.text = inningCount.ToString();
            _shotCountText.text = shotCount.ToString();
			_shotCountHomeText.text = shotCount.ToString();
			_scratchCountText.text = scratchCount.ToString();
            _foulEndText.text = foulEnd.ToString();
            _breakFoulText.text = breakFoul.ToString();
            _foulCountText.text = foulCount.ToString();
            _lossOfChangeText.text = lossOfChange.ToString();
            _goldenBreakText.text = goldenBreak.ToString();
            _clearanceText.text = clearance.ToString();
			_clearanceHomeText.text = clearance.ToString();
			_breakClearanceText.text = breakClearance.ToString();

            float victoryRate = (gameCount != 0) ? (float)winCount / gameCount : 0;         //胜率


			float shotAccuracy = (inningCount != 0) ? (float)pocketCount / inningCount : 0; // 击球成功率，避免除数为零
            float potSuccess = (shotCount != 0) ? (float)pocketCount / shotCount : 0;         // 单杆进球率，避免除数为零
            float clearancePer = (gameCount != 0) ? (float)clearance / gameCount : 0;        // 一杆清台率，避免除数为零

            float shotAccuracySnooker = (inningCountSnooker != 0) ? (float)pocketCountSnooker / inningCountSnooker : 0;

            string Level = "N";  // 默认等级
			float NextLevelProgress = 1f;  // 百分比

			if (potSuccess > 1.8f)
			{
				Level = "<color=#FFD700>A+</color>";  // 金色 A+
				if (clearancePer > 0.08f)
					Level = "<color=#800080>SA</color>";  // 紫色 SA
				NextLevelProgress = 1f; // 已是最高等级
			}
			else if (potSuccess >= 1.6f)
			{
				Level = "<color=#FF0000>A</color>";  // 红色 A
				NextLevelProgress = (potSuccess - 1.6f) / (1.8f - 1.6f);
			}
			else if (potSuccess >= 1.5f)
			{
				Level = "<color=#FF0000>A-</color>";  // 红色 A-
				NextLevelProgress = (potSuccess - 1.5f) / (1.6f - 1.5f);
			}
			else if (potSuccess >= 1.4f)
			{
				Level = "<color=#0000FF>B+</color>";  // 蓝色 B+
				NextLevelProgress = (potSuccess - 1.4f) / (1.5f - 1.4f);
			}
			else if (potSuccess >= 1.2f)
			{
				Level = "<color=#0000FF>B</color>";  // 蓝色 B
				NextLevelProgress = (potSuccess - 1.2f) / (1.4f - 1.2f);
			}
			else if (potSuccess >= 1f)
			{
				Level = "<color=#0000FF>B-</color>";  // 蓝色 B-
				NextLevelProgress = (potSuccess - 1.0f) / (1.2f - 1.0f);
			}
			else if (potSuccess >= 0.66f)
			{
				Level = "<color=#FFFF00>C+</color>";  // 黄色 C+
				NextLevelProgress = (potSuccess - 0.66f) / (1.0f - 0.66f);
			}
			else if (potSuccess >= 0.5f)
			{
				Level = "<color=#FFFF00>C</color>";  // 黄色 C
				NextLevelProgress = (potSuccess - 0.5f) / (0.66f - 0.5f);
			}
			else if (potSuccess >= 0.33f)
			{
				Level = "<color=#FFFF00>C-</color>";  // 黄色 C-
				NextLevelProgress = (potSuccess - 0.33f) / (0.5f - 0.33f);
			}
			else
			{
				Level = "<color=#808080>D</color>";  // 灰色 D
				NextLevelProgress = potSuccess / 0.33f;
			}

			if (image)
				image.fillAmount = remap(NextLevelProgress,0, 1, 0, 0.777f);
			_progressText.text = NextLevelProgress.ToString("P0");

			_gameCountSnookerText.text = gameCountSnooker.ToString();
            _pocketCountSnookerText.text = pocketCountSnooker.ToString();
            _shotCountSnookerText.text = shotCountSnooker.ToString();
			_inningCountSnookerText.text = inningCountSnooker.ToString();
            _shotAccuracySnookerText.text = shotAccuracySnooker.ToString("P0");
            _heightBreakText.text = heightBreak.ToString();

            _victoryRateText.text = victoryRate.ToString("P0");
			_shotAccuracyText.text = shotAccuracy.ToString("P0");
            _potSuccessText.text = potSuccess.ToString("F2");
            _clearancePerText.text = clearancePer.ToString("P0");
            _leaveText.text = Level;
		}

		private float remap(float x, float t1, float t2, float s1, float s2)
		{
			return (s2 - s1) / (t2 - t1) * (x - t1) + s1;
		}

	}

}