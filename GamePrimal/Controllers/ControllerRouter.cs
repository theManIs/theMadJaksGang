using UnityEngine;

namespace Assets.GamePrimal.Controllers
{
    public class ControllerRouter
    {
        private static ControllerInput _cInput;
        private static ControllerDrumSpinner _cDrum;

        public static ControllerEvent GetControllerEvent()
        {
            return new ControllerEvent();
        }

        public static ControllerInput GetControllerInput() => _cInput ?? (_cInput = new ControllerInput());
        public static ControllerDrumSpinner GetControllerDrumSpinner() => _cDrum ?? (_cDrum = new ControllerDrumSpinner());
    }
}
