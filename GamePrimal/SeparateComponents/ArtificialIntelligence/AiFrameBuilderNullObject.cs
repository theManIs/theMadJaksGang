using System.Collections;


namespace Assets.TeamProjects.GamePrimal.SeparateComponents.ArtificialIntelligence
{
    public class AiFrameBuilderNullObject : IArtificial
    {
        #region IArtificial

        public void DoAny()
        {
            return;
        }

        public void StartAssault()
        {
            return;
        }

        public bool CanNotDoAnyAction() => false;

        public void ClearControlAndTurnEnded()
        {
            return;
        }

        #endregion
    }
}