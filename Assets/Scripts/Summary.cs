using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Gameplay
{
    public class Summary : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreLabel;
        [SerializeField] private Button okayButton;

        public void Initialize()
        {
            okayButton.onClick.RemoveAllListeners();
            okayButton.onClick.AddListener(OnOkayButtonClick);
        }

        public void UpdateContent(int score)
        {
            gameObject.SetActive(true);
            scoreLabel.text = score.ToString();
        }

        private void OnOkayButtonClick()
        {
            Signals.GameStateChanged?.Invoke(GameState.Running, 0);
        }

        public void Reset()
        {
            scoreLabel.text = "";
            gameObject.SetActive(false);
        }
    }
}