using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SoundButton : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        
        [Header("Sprites")]
        [SerializeField] private Sprite activeSprite;
        [SerializeField] private Sprite passiveSprite;
        
        private const string SoundOn = "SoundOn";
        
        public void Initialize()
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
            
            var isActive = PlayerPrefs.GetInt(SoundOn) == 1;
            image.sprite = isActive ? activeSprite : passiveSprite;
        }

        private void OnClick()
        {
            var isActive = PlayerPrefs.GetInt(SoundOn) == 1;
            PlayerPrefs.SetInt(SoundOn, isActive ? 0 : 1);
            image.sprite = isActive ? activeSprite : passiveSprite;
            
        }
    }
}