using Assets.TeamProjects.GamePrimal.SeparateComponents.ListsOfStuff;

namespace Assets.TeamProjects.GamePrimal.Navigation.HighlightFrame
{
    public class RedHighlight : AbstractHighlight
    {


        #region LifeCycle

        public RedHighlight() => OnStart(ResourcesList.RedAura); 

        #endregion


    }
}