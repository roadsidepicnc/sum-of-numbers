using System.Collections.Generic;
using System.Linq;
using Gameplay;
using Newtonsoft.Json;
using UnityEngine;
using Utilities;
using Utilities.Signals;
using Zenject;

namespace LevelManagement
{
    public class LevelManager : Manager
    {
        [Inject] private SignalBus _signalBus;
        
        public int LevelId => PlayerPrefs.GetInt(Constants.CurrentLevelIndex);
        public int LevelNumber => LevelId + 1;
        
        private List<LevelTemplate> _levelTemplateList;

        private LevelData _levelData; 
        
        public override void Initialize()
        {
            base.Initialize();
            
            _levelTemplateList = Resources.LoadAll<LevelTemplate>("Levels").ToList();
            if (_levelTemplateList.Count <= 0)
            {
                Debug.LogError("Levels could not be loaded");
                Application.Quit();
                return;
            }

            if (DoesSavedLevelExist)
            {
                LoadLevelFromSave();
            }
            else
            {
                var index = LevelId % _levelTemplateList.Count;
                SetLevelDataFromTemplate(index);
            }
            
            IsInitialized = true;
        }

        public override void Subscribe()
        {
            _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        public override void Unsubscribe()
        {
            _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
        }

        private void SaveLevelData()
        {
            PlayerPrefs.SetString(Constants.CurrentLevelData, JsonConvert.SerializeObject(_levelData));
        }

        private void LoadLevelFromSave()
        {
            _levelData = JsonConvert.DeserializeObject<LevelData>(PlayerPrefs.GetString(Constants.CurrentLevelData));
        }

        public CellData GetCellData(int row, int column)
        {
            return _levelData.cellDataList[CoordinateConverter.Convert(row, column, _levelData.columnCount)];
        }

        private LevelData CreateLevelDataFromTemplate(LevelTemplate levelTemplate) => new (levelTemplate.rowCount, levelTemplate.columnCount, levelTemplate.heartCount, levelTemplate.heartCount, levelTemplate.cellDataList);

        private void IncreaseLevelId() => PlayerPrefs.SetInt(Constants.CurrentLevelIndex, LevelNumber);
        public void DecreaseHeartAtLevelData() => _levelData.currentHeartCount--;
        public bool DoesSavedLevelExist => PlayerPrefs.HasKey(Constants.CurrentLevelData);
        private void DeleteSavedLevel() => PlayerPrefs.DeleteKey(Constants.CurrentLevelData);
        
        public int CurrentLevelRowCount => _levelData.rowCount;
        public int CurrentLevelColumnCount => _levelData.columnCount;
        public int CurrentLevelCurrentHeartCount => _levelData.currentHeartCount;
        public int CurrentLevelMaxHeartCount => _levelData.maxHeartCount;
        
        private void OnGameStateChanged(GameStateChangedSignal gameStateChangedSignal)
        {
            int index;
            switch (gameStateChangedSignal.GameState)
            {
                case GameState.Won:
                {
                    IncreaseLevelId();
                    index = LevelId % _levelTemplateList.Count;
                    SetLevelDataFromTemplate(index);
                    break;
                }
                case GameState.Lost:
                    index = LevelId % _levelTemplateList.Count;
                    SetLevelDataFromTemplate(index);
                    break;
                case GameState.SceneIsChanging:
                    if (GameManager.SceneState == SceneState.Gameplay)
                    {
                        SaveLevelData();
                    }
                    
                    break;
            }
        }

        public void SetLevelDataFromTemplate(int index)
        {
            DeleteSavedLevel();
            var levelTemplate = _levelTemplateList.Find(x => x.id == index);
            _levelData = CreateLevelDataFromTemplate(levelTemplate);
        }

        private void OnApplicationQuit()
        {
            SaveLevelData();
        }
    }
}