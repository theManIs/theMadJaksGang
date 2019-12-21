using System.Collections;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses
{
    public class DebugInfo
    {
        public static void Log<T>(T msg)
        {
            Debug.Log(msg + " time:" + Time.time);
        }

        public static void DebugList<T>(T list) where T : IEnumerable
        {
            foreach (var element in list)
            {
                Debug.Log(element);
            }
        }
    }
}