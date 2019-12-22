using Assets.GamePrimal.Navigation.HighlightFrame;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.Controllers
{
    public class ControllerFocusSubject
    {
        private SubjectFocus _subjectFocus;
        private int _recentFrame;

        public ControllerFocusSubject()
        {
            _subjectFocus = new SubjectFocus();
        }

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
    }
}