using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace cdse_presets
{
    public class AudioVolumeChange : UdonSharpBehaviour
    {
        [Header("这是一个音量控制脚本")]
        [Header("目标音源（Audio Source）")]
        public AudioSource audioSource;
        [Header("滑块（Slider）")]
        public Slider audioVolumeSlider;

        public void Start()
        {
            audioVolumeSlider.SetValueWithoutNotify(audioSource.volume);
        }

        public void OnValueChange()
        {
            audioSource.volume = audioVolumeSlider.value;
        }
    }
}