using UnityEngine;
using UnityEngine.UI;

namespace UI.Popup
{
    public class WinPopup : Panel
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