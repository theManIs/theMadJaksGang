using UnityEngine;


namespace Assets.TeamProjects.GamePrimal.Navigation.HighlightFrame
{
    public class HighlightBuilder
    {


        #region Fields

        private Transform _parentTransform;

        #endregion


        #region Methods

        public static HighlightBuilder CreateWithParent(Transform trans)
        {
            HighlightBuilder hb = new HighlightBuilder();

            hb._parentTransform = trans;

            return hb;
        }

        public AbstractHighlight GetRedHighlight() => SetStandardSettings(new RedHighlight());
        public AbstractHighlight GetBluHighlight() => SetStandardSettings(new BlueHighlight());

        public AbstractHighlight GetRedBlueHighlight(bool isBlue) => isBlue ? GetBluHighlight() : GetRedHighlight();

        private AbstractHighlight SetStandardSettings(AbstractHighlight highlight)
        {
            highlight.SetScaleFactor(Vector3.one * 0.08f);
            highlight.SetEulerRotation(new Vector3(90, 0, 0));
            highlight.SetParentObject(_parentTransform);

            return highlight;
        } 

        #endregion


    }
}