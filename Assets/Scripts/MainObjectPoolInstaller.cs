using Gameplay;
using GridManagement;
using ObjectPool;
using UnityEngine;
using Zenject;

public class MainObjectPoolInstaller : MonoInstaller<MainObjectPoolInstaller>
{
    [SerializeField] private ObjectPoolManager objectPoolManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameManager gameManager;
    
    public override void InstallBindings()
    {
        Container.Bind<ObjectPoolManager>().FromInstance(objectPoolManager).AsSingle().NonLazy();
        
        Container.Bind<GridManager>().FromInstance(gridManager).AsSingle().NonLazy();
        Container.Bind<GridCreator>().AsSingle().NonLazy();
        
        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
    }
}