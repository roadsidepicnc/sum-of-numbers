using LevelManagement;
using ObjectPoolingSystem;
using UI;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller<ProjectInstaller>
{
    [SerializeField] private PopupPrefabCatalog popupPrefabCatalogPrefab;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameManager>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesAndSelfTo<SignalManager>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelManager>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesAndSelfTo<ObjectPoolContainer>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesAndSelfTo<PopupPrefabCatalog>().FromComponentInNewPrefab(popupPrefabCatalogPrefab).AsSingle();
    }
}