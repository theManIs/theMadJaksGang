using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.Proxies
{
    public class StaticProxyInput
    {
        public static bool P => Input.GetKeyDown(KeyCode.P);
        public static bool O => Input.GetKeyDown(KeyCode.O);
        public static bool L => Input.GetKeyDown(KeyCode.L);
        public static bool N => Input.GetKeyDown(KeyCode.N);
        public static bool LeftMouse => Input.GetKeyDown(KeyCode.Mouse0);
        public static bool Space => Input.GetKey(KeyCode.Space);
        public static bool Esc => Input.GetKeyDown(KeyCode.Escape);
    }
}