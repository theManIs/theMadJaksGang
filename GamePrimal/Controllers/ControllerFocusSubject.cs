using System;
using Assets.GamePrimal.Navigation.HighlightFrame;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using UnityEngine;
using static Assets.TeamProjects.GamePrimal.Proxies.StaticProxyRouter;

namespace Assets.TeamProjects.GamePrimal.Controllers
{
    public class ControllerFocusSubject
    {
        private readonly SubjectFocus _subjectFocus = new SubjectFocus();

        private int _recentFrame;

        public ControllerFocusSubject() => StaticProxyEvent.ETurnWasFound.Event += TurnFoundHandler;
        ~ControllerFocusSubject() => StaticProxyEvent.ETurnWasFound.Event += TurnFoundHandler;

        private void TurnFoundHandler(EventTurnWasFoundParams args) => SetHadFocus(args.TurnApplicant);

        public void UpdateOnce()
        {
            if (_recentFrame == Time.frameCount) return;

            _recentFrame = Time.frameCount;
            MouseInput localMouseInput = StaticProxyInput.MouseInput;
            localMouseInput.RightMouse = localMouseInput.RightMouse && !StaticProxyStateHolder.LockModeOn;

            _subjectFocus.UserUpdate(localMouseInput, Camera.main.ScreenPointToRay(localMouseInput.MousePosition));

            ReleaseLockMode();
        }

        private void ReleaseLockMode()
        {
            if (StaticProxyInput.RightMouse && StaticProxyStateHolder.LockModeOn)
                StaticProxyStateHolder.LockModeOn = false;
        }

        public bool HasFocused() => _subjectFocus.HasFocused();
        public Transform GetFocus() => _subjectFocus.GetFocus();
        public Transform GetHardFocus() => _subjectFocus.GetHardFocus();
        public Transform GetSoftFocus() => _subjectFocus.GetSoftFocus();
        private void SetHadFocus(Transform hardFocus) => _subjectFocus.SetHardFocus(hardFocus);

    }
}