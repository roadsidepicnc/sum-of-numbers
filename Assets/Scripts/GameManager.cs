using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GridManagement;
using ObjectPool;
using Zenject;

namespace Gameplay
{
    public class GameManager : BaseManager
    {
        private GridManager _gridManager;
        private ObjectPoolManager _objectPoolManager;

        private List<BaseManager> _managers;
        
        private void Start()
        {
            Initialize();
        }
        
        [Inject]
        public void InstallDependencies(GridManager gridManager, ObjectPoolManager objectPoolManager)
        {
            _gridManager = gridManager;
            _objectPoolManager = objectPoolManager;
        }

        public override async void Initialize()
        {
            _managers = new();
            
            _managers.Add(_objectPoolManager);
            _managers.Add(_gridManager);

            foreach (var manager in _managers)
            {
                manager.Initialize();
            }

            await UniTask.WaitUntil(AreAllManagersInitialized);
            IsInitialized = true;

            return;

            bool AreAllManagersInitialized()
            {
                foreach (var manager in _managers)
                {
                    if (!manager.IsInitialized)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}