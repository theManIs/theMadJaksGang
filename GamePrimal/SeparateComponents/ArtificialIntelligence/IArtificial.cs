namespace Assets.TeamProjects.GamePrimal.SeparateComponents.ArtificialIntelligence
{
    public interface IArtificial
    {
        #region Methods

        void DoAny();

        void StartAssault();

        bool CanNotDoAnyAction();

        void ClearControlAndTurnEnded();

        #endregion
    }
}
