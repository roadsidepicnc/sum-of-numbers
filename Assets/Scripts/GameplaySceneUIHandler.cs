using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Signals;

public class GameplaySceneUIHandler : SceneUIHandler
{
    [Header("Buttons")]
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button homeButton;
    
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI levelNumberText;
    
    protected override void SetButtons()
    {
        homeButton.onClick.RemoveAllListeners();
        homeButton.onClick.AddListener(OnHomeButtonClick);
        
        settingsButton.onClick.RemoveAllListeners();
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
    }
    
    protected override void SetTexts()
    {
        levelNumberText.text = "Level " + LevelManager.LevelNumber;
    }

    private void OnHomeButtonClick()
    {
        SignalBus.Fire(new GameStateChangedSignal(GameState.SceneIsChanging));
    }
    
    private void OnSettingsButtonClick()
    {
        PopupManager.Show(PopupType.SettingsPopup);
    }
}