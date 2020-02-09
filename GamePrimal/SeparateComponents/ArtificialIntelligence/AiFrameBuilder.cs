using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.GamePrimal.Mono;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.ArtificialIntelligence
{
    public class AiFrameBuilder
    {
        public static AiFrame BuildAiFrame(bool enabled = true, Transform transform = null, MonoMechanicus monomech = null) => 
            new AiFrame {Enabled = enabled, CurrentTransform = transform, monomech = monomech};
    }
}
