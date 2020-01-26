﻿using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.Proxies
{
    public struct StatesList
    {
        public bool UserOnUi;
        public bool LockModeOn;
    }

    public class StaticProxyStateHolder
    {
        public static bool UserOnUi;
        public static bool LockModeOn;

        public static StatesList GetStatesList()
        {
            return new StatesList()
            {
                UserOnUi = UserOnUi,
                LockModeOn = LockModeOn
            };
        }
    }
}