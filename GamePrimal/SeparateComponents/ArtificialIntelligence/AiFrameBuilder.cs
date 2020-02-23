using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;  
using System.Threading.Tasks;
using Assets.GamePrimal.Mono;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.ArtificialIntelligence
{
    public class AiFrameBuilder : IArtificial
    {
        private readonly AiFrame _internalAiFrame;
        public AiFrameBuilder(AiFrameParams afp)  => _internalAiFrame = new AiFrame {Attr = afp};
        public void DoAny() => _internalAiFrame.DoAny();
        public void StartAssault() => _internalAiFrame.StartAssault();
        public bool CanNotDoAnyAction() => _internalAiFrame.CanNotDoAnyAction();
    }
}
