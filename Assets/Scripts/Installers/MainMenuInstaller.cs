using Zenject;
using CustomObjectPool = ObjectPoolManagement.ObjectPool;

public class MainMenuInstaller : MonoInstaller<MainMenuInstaller>
{
    public override void InstallBindings()
    {
        Container.BindFactory<CustomObjectPool, CustomObjectPool.Factory>();
    }
}