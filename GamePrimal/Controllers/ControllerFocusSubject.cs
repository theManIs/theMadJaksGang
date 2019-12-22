using Assets.GamePrimal.Navigation.HighlightFrame;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using UnityEngine;
using static Assets.GamePrimal.Controllers.ControllerRouter;

namespace Assets.TeamProjects.GamePrimal.Controllers
{
    public class ControllerFocusSubject
    {
        private readonly SubjectFocus _subjectFocus = new SubjectFocus();

        private int _recentFrame;

        public ControllerFocusSubject() => GetControllerDrumSpinner().ETurnWasFound.Event += TurnFoundHandler;

        ~ControllerFocusSubject() => GetControllerDrumSpinner().ETurnWasFound.Event -= TurnFoundHandler;

        private void TurnFoundHandler(EventTurnWasFoundParams args) => SetHadFocus(args.TurnApplicant);

        public void UpdateOnce()
        {
            if (_recentFrame == Time.frameCount) return;

            _recentFrame = Time.frameCount;

            _subjectFocus.UserUpdate();
        }

        public bool HasFocused() => _subjectFocus.HasFocused();
        public Transform GetFocus() => _subjectFocus.GetFocus();
        public Transform GetHardFocus() => _subjectFocus.GetHardFocus();
        public Transform GetSoftFocus() => _subjectFocus.GetSoftFocus();
        private void SetHadFocus(Transform hardFocus) => _subjectFocus.SetHardFocus(hardFocus);
    }
}