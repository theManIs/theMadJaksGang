using System.Collections;


namespace Assets.TeamProjects.GamePrimal.SeparateComponents.ArtificialIntelligence
{
    public class AiFrameBuilderNullObject : IArtificial
    {
        #region Methods

        public IEnumerator HitTargetAsSoonAsPossible()
        {
            throw new System.NotImplementedException();
        }

        public IArtificial MoveToTarget()
        {
            throw new System.NotImplementedException();
        }

        public void DoAny() { }
        public void StartAssault() { }
        public bool CanNotDoAnyAction() => false; 

        #endregion
    }
}