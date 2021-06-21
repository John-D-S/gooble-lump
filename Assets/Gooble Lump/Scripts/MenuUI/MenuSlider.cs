using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace Menu
{
    public enum MenuSliderType
    {
        Music,
        Sound_Effects    
    }

    public class MenuSlider : MonoBehaviour
    {
        [SerializeField]
        private MenuSliderType sliderType = MenuSliderType.Music;

        [SerializeField, HideInInspector]
        private Slider slider;
        [SerializeField, HideInInspector]
        private TextMeshProUGUI sliderText;

        private MenuHandler menuHandler;

        private void OnValidate()
        {
            slider.maxValue = 1;
            slider.minValue = 0;
            
            if (!slider)
                slider = GetComponent<Slider>();
            if (!sliderText)
                sliderText = GetComponentInChildren<TextMeshProUGUI>();

            sliderText.text = sliderType.ToString().Replace("_", " ");
            slider.onValueChanged.RemoveAllListeners();
        }

        private void Start()
        {
            menuHandler = TheMenuHandler.theMenuHandler;
            slider.onValueChanged.AddListener(PerformFunction);
            switch (sliderType)
            {
                case MenuSliderType.Music:
                    slider.value = Mathf.Pow((PlayerPrefs.GetFloat("MusicVolume") + 80) / 80, 2);
                    break;
                case MenuSliderType.Sound_Effects:
                    slider.value = Mathf.Pow((PlayerPrefs.GetFloat("SFXVolume") + 80) / 80, 2);
                    break;
                default:
                    break;
            }

        }

        private void PerformFunction(float _value)
        {
            switch (sliderType)
            {
                case MenuSliderType.Music:
                    menuHandler.ChangeMusicVolume(Mathf.Sqrt(_value) * 80 - 80);
                    break;
                case MenuSliderType.Sound_Effects:
                    menuHandler.ChangeSFXVolume(Mathf.Sqrt(_value) * 80 - 80);
                    break;
                default:
                    break;
            }
        }
    }
}