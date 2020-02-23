﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AI;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.ArtificialIntelligence
{
    public interface IArtificial
    {
        void DoAny();
        void StartAssault();
        bool CanNotDoAnyAction();
    }
}
