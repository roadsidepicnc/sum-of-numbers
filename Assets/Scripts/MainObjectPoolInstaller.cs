using Gameplay;
using GridManagement;
using LevelManagement;
using ObjectPoolManagement;
using UI;
using UnityEngine;
using Zenject;
using CustomObjectPool = ObjectPoolManagement.ObjectPool;

public class MainObjectPoolInstaller : MonoInstaller<MainObjectPoolInstaller>
{
    [SerializeField] private ObjectPoolManager objectPoolManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameManager gameManager;
    
    public override void InstallBindings()
    {
        Container.Bind<ObjectPoolManager>().FromInstance(objectPoolManager).AsSingle().NonLazy();
        
        Container.Bind<GridManager>().FromInstance(gridManager).AsSingle().NonLazy();
        Container.Bind<GridCreator>().AsSingle().NonLazy();
        
        Container.Bind<UIManager>().FromInstance(uiManager).AsSingle();

        Container.Bind<GameplayManager>().FromInstance(gameplayManager).AsSingle();
        
        Container.Bind<LevelManager>().FromInstance(levelManager).AsSingle();
        
        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
        
        Container.BindFactory<CustomObjectPool, CustomObjectPool.Factory>();
    }
}