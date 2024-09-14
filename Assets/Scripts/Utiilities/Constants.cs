using UnityEngine;

namespace Utilities
{
    public class Constants
    {
        public const int DefaultScreenWidth = 1080; 
        public const int DefaultScreenHeight = 1920; 
        public const float DefaultScreenResolution = (float) DefaultScreenWidth / DefaultScreenHeight; 
        
        public const float DefaultCellWidth = 200f;
        public const float DefaultCellHeight = 200f;
        
        public const float DefaultTargetScoreWidth = 200f;
        public const float DefaultTargetScoreHeight = 120f;
        
        public const int DefaultGridRowCount = 4;
        public const int DefaultGridColumnCount = 4;

        public static Color TargetScoreBackgroundCompletedColor = new (226f / 255f, 226f / 255f, 226f / 255f, .4f);
        public static Color TargetScoreBackgroundNotCompletedColor = new (188f / 255f, 198f / 255f, 204f / 255f, 1f);
        public static Color TargetScoreTextNotCompletedColor = Color.black;
        public static Color TargetScoreTextCompletedColor = new (133f / 255f, 133f / 255f, 133f / 255f, .8f);
        public const float TargetScoreColorChangeDuration = .5f;
    }
}