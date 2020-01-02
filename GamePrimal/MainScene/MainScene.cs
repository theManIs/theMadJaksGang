using Assets.GamePrimal.Controllers;
using Assets.GamePrimal.Navigation.HighlightFrame;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using Assets.TeamProjects.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Navigation.HighlightFrame;
using Assets.TeamProjects.GamePrimal.Navigation.Pathfinder;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.InterfaceHold;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.MainScene
{
    public class MainScene : MonoBehaviour
    {
        private bool _engaged = true;
        private HighlightFrame _highlightFrame;
        private ControllerFocusSubject _subjectFocus;
//        private MovableObjects _movableObjects;
//        private TracerProjectileScript _tracerProjectileScript;
        private ControllerCharacterMovement _controllerCharacterMovement;
        private ControllerAttackCapture _controllerAttackCapture;
        private ControllerInput _controllerInput;
//        private SceneBuilder _sceneBuilder;
        private ControllerPathFinding _cPathFinding;

        public ControllerInput ControllerInput => _controllerInput ?? StaticProxyRouter.GetControllerInput().UserAwake();

        void Awake()
        {
            if (!Camera.main)
            {
                _engaged = false;

                return;
            }

            _highlightFrame = new HighlightFrame();
            _subjectFocus = StaticProxyRouter.GetControllerFocusSubject();
//            Debug.Log("Subject focus hash " +_subjectFocus.GetHashCode());
//            _movableObjects = FindObjectOfType<MovableObjects>();
//            _tracerProjectileScript = new TracerProjectileScript();
//            _controllerCharacterMovement = new ControllerCharacterMovement();
            _cPathFinding = StaticProxyRouter.GetControllerPathFinding();
            _controllerAttackCapture = StaticProxyRouter.GetControllerAttackCapture();
            _controllerInput = ControllerInput;
        }


        public Transform GetFocus() => _subjectFocus.GetFocus();
//        public Transform GetCapture() => _subjectFocus.RetrieveRaycastCapture();

        // Start is called before the first frame update
        void Start()
        {
            if (!_engaged) return;

//            if (_movableObjects)
                _highlightFrame.Start();
//            else
//                _highlightFrame.Engaged = false;

//            _subjectFocus.Start();
            _controllerAttackCapture.Start();
//            _tracerProjectileScript.UserStart();
            _cPathFinding.UserStart(new StartParams());
            ControllerInput.Start();
        }

        // UserUpdate is called once per frame
        void Update()
        {
            if (!_engaged) return;

            ControllerInput.Update();
            _subjectFocus.UpdateOnce();
            _controllerAttackCapture.Update();
            _highlightFrame.FixedUpdate(_subjectFocus.GetFocus());
//            _tracerProjectileScript.SetNavAgent(_subjectFocus.GetFocus());
//            _controllerCharacterMovement.UserUpdate(_subjectFocus.GetFocus(), _subjectFocus.HasFocused());
//            _tracerProjectileScript.UserUpdate();
            _cPathFinding.UserUpdate(new UpdateParams());
        }

        void FixedUpdate()
        {
            if (!_engaged) return;

        }

        private void OnEnable()
        {
            ControllerInput.UserEnable();
        }

        private void OnDisable()
        {
            ControllerInput.UserDisable();
        }
    }
}