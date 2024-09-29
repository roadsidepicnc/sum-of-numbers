using Gameplay;
using UnityEngine;
using Utilities;

namespace UI
{ 
    public class UIManager : Manager
    {
        public float CanvasWidth { get; private set; }
        public float CanvasHeight { get; private set; }
        public float UnitSize { get; private set; }
        

        [SerializeField] private Canvas canvas;
        
        public override void Initialize()
        {
            base.Initialize();

            CalculateUnitSize();
            
            IsInitialized = true;
        }
        
        private void CalculateUnitSize()
        {
            CanvasHeight = canvas.GetComponent<RectTransform>().sizeDelta.y;
            CanvasWidth = canvas.GetComponent<RectTransform>().sizeDelta.x;
            UnitSize = canvas.referencePixelsPerUnit * ( CanvasHeight / Constants.DefaultScreenHeight);
        }
        
        public float SafeAreaChangeInUnits()
        {
            return ScreenSafeAreaTopDifferenceInPixels() * 0.5f / UnitSize;
        }

        public float ScreenSafeAreaTopDifferenceInPixels()
        {
            var screenSafeAreaTopDifferenceInPixels = Mathf.Abs(Screen.safeArea.max.y - Screen.height);
            if (screenSafeAreaTopDifferenceInPixels > 0f)
            {
                screenSafeAreaTopDifferenceInPixels += UnitSize / 2f; 
            }
        
            return screenSafeAreaTopDifferenceInPixels;
        }
    }
}