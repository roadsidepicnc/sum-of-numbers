using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class PanelPrefabHelper : MonoBehaviour
    {
        [SerializeField] private List<Panel> panelPrefabs;

        public Panel GetPrefab(PanelType panelType) => panelPrefabs.Find(x => x.PanelType == panelType);
    }
}