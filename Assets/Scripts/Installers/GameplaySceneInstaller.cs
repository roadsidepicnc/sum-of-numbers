using Gameplay;
using GridManagement;
using Zenject;
using CustomObjectPool = ObjectPoolingSystem.ObjectPool;

public class GameplaySceneInstaller : MonoInstaller<MainMenuInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GridManager>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesAndSelfTo<GameplayManager>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindFactory<CustomObjectPool, CustomObjectPool.Factory>();
    }
}