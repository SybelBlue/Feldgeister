using UnityEngine;

using static UnityEngine.Input;

namespace Feldgeister
{
    internal static class Input
    {
        public static Vector3 currentWorldMousePosition 
            => Camera.main.ScreenToWorldPoint(mousePosition);
        public static bool LeftDown() => GetAxis("Horizontal") < 0;
        public static bool RightDown() => GetAxis("Horizontal") > 0;
        
        public static bool mouseHasMoved
            => GetAxis("Mouse X") != 0 || GetAxis("Mouse Y") != 0;
        
        private static IRegion _lastFocused = null;
        public static IRegion lastFocused 
        { 
            get
            {
                return _lastFocused;
            }
            set
            {
                if (_lastFocused == value) return;

                _lastFocused?.OnHoverExit();
                
                _lastFocused = value;

                _lastFocused?.OnHoverEnter();
            }
        }

        public static void SendClick()
            => lastFocused?.OnClick();
    }
}