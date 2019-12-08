using Assets.GamePrimal.CharacterOrtJoyPrafabs.Enemies.Skeleton;
using UnityEngine;
using static Assets.GamePrimal.Controllers.ControllerEvent;

namespace Assets.GamePrimal.Controllers
{
    public class ControllerAttackCapture
    {
        public event HitDetected HitDetectedHandler;
        private AnimationControllerSkeleton _acs;

        public bool HitFixated { get; private set; }

        private MainScene.MainScene _theMainScene;

        public void Start()
        {
            _theMainScene = Object.FindObjectOfType<MainScene.MainScene>();
        }

        public void Update()    
        {
            HitFixated = false;
            Transform focused = _theMainScene.GetFocus();
            Transform captured = _theMainScene.GetCapture();

            if (Input.GetKeyDown(KeyCode.Mouse0) && focused && captured)
                if (focused.GetInstanceID() != captured.GetInstanceID())
                {
                    HitFixated = true;

                    ControllerRouter.GetControllerEvent().HitDetectedInvoke(captured, focused);
                }
        }
    }
}
