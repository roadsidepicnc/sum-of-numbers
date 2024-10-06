using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WinPopup : Popup
    {
        [SerializeField] private Button confirmButton;
        [SerializeField] private Button homeButton;

        public override void Initialize()
        {
            base.Initialize();
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(() => Close());
        }
    }
}