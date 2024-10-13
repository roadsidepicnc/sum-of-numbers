using UI;
using Zenject;
using CustomObjectPool = ObjectPoolingSystem.ObjectPool;

public class MainMenuInstaller : MonoInstaller<MainMenuInstaller>
{
    public override void InstallBindings()
    {
        Container.BindFactory<CustomObjectPool, CustomObjectPool.Factory>();
    }
}