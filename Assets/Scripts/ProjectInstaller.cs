using LevelManagement;
using Zenject;

public class ProjectInstaller : MonoInstaller<ProjectInstaller>
{
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameManager>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesAndSelfTo<SignalManager>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelManager>().FromNewComponentOnNewGameObject().AsSingle();
    }
}