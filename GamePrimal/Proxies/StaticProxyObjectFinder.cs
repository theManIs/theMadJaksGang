using Assets.GamePrimal.Mono;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.Proxies
{
    public class StaticProxyObjectFinder
    {
        public static T[] FindObjectOfType<T>() where T : Object => Object.FindObjectsOfType<T>();
    }
}