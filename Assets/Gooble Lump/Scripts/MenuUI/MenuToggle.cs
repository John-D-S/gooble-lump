using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace Menu
{
    public enum MenuToggleType
    {
        Fullscreen,
        Mute
    }

    public class MenuToggle : MonoBehaviour
    {
        [SerializeField]
        private MenuToggleType toggleType = MenuToggleType.Fullscreen;

        [SerializeField, HideInInspector]
        private Toggle toggle;
        [SerializeField, HideInInspector]
        private TextMeshProUGUI toggleText;

        private MenuHandler menuHandler;

        private void OnValidate()
        {
            if (!toggle)
                toggle = GetComponent<Toggle>();
            if (!toggleText)
                toggleText = GetComponentInChildren<TextMeshProUGUI>();

            toggleText.text = toggleType.ToString().Replace("_", " ");
            toggle.onValueChanged.RemoveAllListeners();
        }

        private void Start()
        {
            menuHandler = TheMenuHandler.theMenuHandler;

            switch (toggleType)
            {
                case MenuToggleType.Fullscreen:
                    if (PlayerPrefs.HasKey("isFullScreen"))
                        toggle.isOn = PlayerPrefs.GetInt("IsFullScreen") == 1;
                    else
                        toggle.isOn = false;
                    break;
                case MenuToggleType.Mute:
                    if (PlayerPrefs.HasKey("isMuted"))
                        toggle.isOn = PlayerPrefs.GetInt("IsMuted") == 1;
                    else
                        toggle.isOn = false;
                    break;
                default:
                    break;
            }

            toggle.onValueChanged.AddListener(delegate { PerformFunction( toggle.isOn ); } );
        }

        private void PerformFunction(bool _value)
        {
            switch (toggleType)
            {
                case MenuToggleType.Fullscreen:
                    menuHandler.SetFullscreen(_value);
                    break;
                case MenuToggleType.Mute:
                    menuHandler.SetMute(_value);
                    break;
                default:
                    break;
            }
        }
    }
}