using Assets.GamePrimal.Controllers;
using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.MiscClasses;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.Controllers
{
    public struct AttackCaptureParams
    {
        public Transform Source;
        public Transform Target;
        public bool HasHit;
        public bool HasDied;
    }

    public class ControllerAttackCapture
    {
        public bool HitFixated { get; private set; } = false;
        public bool HasHit { get; private set; } = false;

        private MainScene.MainScene _theMainScene;
        private ControllerFocusSubject _cFocusSubject;

        public void Start()
        {
            _theMainScene = Object.FindObjectOfType<MainScene.MainScene>();
            _cFocusSubject = StaticProxyRouter.GetControllerFocusSubject();
        }

        public void ReleaseFixated() => HasHit = false;
        public void LockTarget() => HasHit = true;

        public void Update()
        {
            HitFixated = false;
            Transform focused = _cFocusSubject.GetHardFocus();
            Transform captured = _cFocusSubject.GetSoftFocus();

            if (!StaticProxyStateHolder.UserOnUi)
                if (Input.GetKeyDown(KeyCode.Mouse0) && focused && captured && captured.GetComponent<MonoMechanicus>())
                    if (focused.GetInstanceID() != captured.GetInstanceID())
                    {
                        AttackCaptureParams acp = new AttackCaptureParams() { Source = captured, Target = focused, HasHit = HasHit };

                        StaticProxyRouter.GetControllerEvent().HitDetectedInvoke(acp);
    //                    Debug.Log("Locked " + Time.time);
                        HitFixated = true;

                        StaticProxyStateHolder.LockModeOn = false;
                    }
        }
    }
}
