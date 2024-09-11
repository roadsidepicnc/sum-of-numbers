using Gameplay;
using GridManagement;
using LevelManagement;
using Zenject;
using CustomObjectPool = ObjectPoolManagement.ObjectPool;

public class MainObjectPoolInstaller : MonoInstaller<MainObjectPoolInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GridManager>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesAndSelfTo<GameplayManager>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelManager>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindFactory<CustomObjectPool, CustomObjectPool.Factory>();
    }
}