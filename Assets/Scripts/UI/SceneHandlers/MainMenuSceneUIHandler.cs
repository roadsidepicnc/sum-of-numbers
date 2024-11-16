using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Signals;

public class MainMenuSceneUIHandler : SceneUIHandler
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button achievementsButton;
    
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI levelNumberText;
    
    protected override void  SetTexts()
    {
        levelNumberText.text = "Level " + LevelManager.LevelNumber;
    }

    protected override void SetButtons()
    {
        playButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        achievementsButton.onClick.RemoveAllListeners();
        
        playButton.onClick.AddListener(OnPlayButtonClick);
        restartButton.onClick.AddListener(OnRestartButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
        achievementsButton.onClick.AddListener(OnAchievementsButtonClick);

        restartButton.gameObject.SetActive(LevelManager.DoesSavedLevelExist);
    }

    private void OnPlayButtonClick()
    {
        SignalBus.Fire(new GameStateChangedSignal(GameState.SceneIsChanging));
    }
    
    private void OnRestartButtonClick()
    {
        LevelManager.SetLevelDataFromTemplate(LevelManager.LevelId);
        SignalBus.Fire(new GameStateChangedSignal(GameState.SceneIsChanging));
    }

    private void OnSettingsButtonClick()
    {
        PopupManager.Show(PopupType.SettingsPopup);
    }
    
    private void OnAchievementsButtonClick()
    {
    }
}