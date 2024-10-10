using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsPopup : Popup
    {
        [SerializeField] private Button backButton;
        [SerializeField] private SoundButton soundButton;
        
        public override void Initialize()
        {
            base.Initialize();
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(() => Close());
            
            soundButton.Initialize();
        }
    }
}