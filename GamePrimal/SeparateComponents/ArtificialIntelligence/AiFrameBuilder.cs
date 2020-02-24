namespace Assets.TeamProjects.GamePrimal.SeparateComponents.ArtificialIntelligence
{
    public class AiFrameBuilder : IArtificial
    {
        #region Fields

        private readonly AiFrame _internalAiFrame;

        #endregion


        #region ClassLifeCycles

        public AiFrameBuilder(AiFrameParams afp) => _internalAiFrame = new AiFrame { Attr = afp };

        #endregion


        #region Methods

        public void DoAny() => _internalAiFrame.DoAny();
        public void StartAssault() => _internalAiFrame.StartAssault();
        public bool CanNotDoAnyAction() => _internalAiFrame.CanNotDoAnyAction();
        public void ClearControlAndTurnEnded() => _internalAiFrame.ClearControlAndTurnEnded();

        #endregion
    }
}
