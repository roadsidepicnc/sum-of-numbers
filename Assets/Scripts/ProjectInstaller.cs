using LevelManagement;
using ObjectPoolingSystem;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller<ProjectInstaller>
{
    [SerializeField] private ObjectPoolManager objectPoolManager;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameManager>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesAndSelfTo<SignalManager>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelManager>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesAndSelfTo<ObjectPoolContainer>().FromNewComponentOnNewGameObject().AsSingle();
    }
}