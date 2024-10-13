using InputManagement;
using LevelManagement;
using ObjectPoolingSystem;
using UI;
using UnityEngine;
using Utilities.Signals;
using Zenject;

public class ProjectInstaller : MonoInstaller<ProjectInstaller>
{
    [SerializeField] private PopupPrefabCatalog popupPrefabCatalogPrefab;
    
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<GameStateChangedSignal>().OptionalSubscriber();
        Container.DeclareSignal<CellInteractedSignal>();
        Container.DeclareSignal<ClickModeChangedSignal>();
        
        Container.BindInterfacesAndSelfTo<GameManager>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesAndSelfTo<InputManager>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelManager>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesAndSelfTo<ObjectPoolContainer>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesAndSelfTo<PopupPrefabCatalog>().FromComponentInNewPrefab(popupPrefabCatalogPrefab).AsSingle();
    }
}