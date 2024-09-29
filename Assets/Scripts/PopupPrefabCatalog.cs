using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class PopupPrefabCatalog : MonoBehaviour
    {
        [SerializeField] private List<Popup> popupPrefabs;

        public Popup GetPrefab(PopupType popupType) => popupPrefabs.Find(x => x.PopupType == popupType);
    }
}