using Assets.GamePrimal.Controllers;
using Assets.GamePrimal.Navigation.HighlightFrame;
using Assets.GamePrimal.Navigation.Pathfinder;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Navigation.HighlightFrame;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.MainScene
{
    public class MainScene : MonoBehaviour
    {
        private bool _engaged = true;
        private HighlightFrame _highlightFrame;
        private SubjectFocus _subjectFocus;
        private MovableObjects _movableObjects;
        private TracerProjectileScript _tracerProjectileScript;
        private ControllerCharacterMovement _controllerCharacterMovement;
        private ControllerAttackCapture _controllerAttackCapture;
        private ControllerInput _controllerInput;
        private SceneBuilder _sceneBuilder;

        void Awake()
        {
            if (!Camera.main)
            {
                _engaged = false;

                return;
            }

            _highlightFrame = new HighlightFrame();
            _subjectFocus = new SubjectFocus();
//            _movableObjects = FindObjectOfType<MovableObjects>();
            _tracerProjectileScript = new TracerProjectileScript();
//            _controllerCharacterMovement = new ControllerCharacterMovement();
            _controllerAttackCapture = ControllerRouter.GetControllerAttackCapture();
            _controllerInput = ControllerRouter.GetControllerInput().UserAwake();
        }

        public Transform GetFocus() => _subjectFocus.GetFocus();
        public Transform GetCapture() => _subjectFocus.RetrieveRaycastCapture();

        // Start is called before the first frame update
        void Start()
        {
            if (!_engaged) return;

//            if (_movableObjects)
                _highlightFrame.Start();
//            else
//                _highlightFrame.Engaged = false;

            _subjectFocus.Start();
            _controllerAttackCapture.Start();
            _tracerProjectileScript.Start();
            _controllerInput.Start();
        }

        // UserUpdate is called once per frame
        void Update()
        {
            if (!_engaged) return;

            _controllerInput.Update();
            _subjectFocus.UserUpdate();
            _controllerAttackCapture.Update();
            _highlightFrame.FixedUpdate(_subjectFocus.GetFocus());
            _tracerProjectileScript.SetNavAgent(_subjectFocus.GetFocus());
//            _controllerCharacterMovement.UserUpdate(_subjectFocus.GetFocus(), _subjectFocus.HasFocused());
            _tracerProjectileScript.FixedUpdate();
        }

        void FixedUpdate()
        {
            if (!_engaged) return;

        }

        private void OnEnable()
        {
            _controllerInput.UserEnable();
        }

        private void OnDisable()
        {
            _controllerInput.UserDisable();
        }
    }
}