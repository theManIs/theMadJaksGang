using Assets.TeamProjects.GamePrimal.SeparateComponents.ListsOfStuff;

namespace Assets.TeamProjects.GamePrimal.Navigation.HighlightFrame
{
    public class BlueHighlight : AbstractHighlight
    {


        #region LifeCycle

        public BlueHighlight() => OnStart(ResourcesList.BlueAura); 

        #endregion


    }
}