using Assets.GamePrimal.Controllers;
using Assets.GamePrimal.Navigation.HighlightFrame;
using Assets.GamePrimal.Navigation.Pathfinder;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Mono;
using UnityEngine;

namespace Assets.TeamProjects.GamePrimal.MainScene
{
    public class MainScene : MonoBehaviour
    {
        private bool _engaged = true;
        private GetRealHeight _getRealHeight;
        private SubjectFocus _subjectFocus;
        private MovableObjects _movableObjects;
        private TracerProjectileScript _tracerProjectileScript;
        private FetchMovablePoint _fetchMovablePoint;
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

            _getRealHeight = new GetRealHeight();
            _subjectFocus = new SubjectFocus();
            _movableObjects = FindObjectOfType<MovableObjects>();
            _tracerProjectileScript = new TracerProjectileScript();
            _fetchMovablePoint = new FetchMovablePoint();
            _controllerAttackCapture = new ControllerAttackCapture();
            _controllerInput = ControllerRouter.GetControllerInput().UserAwake();
        }

        public Transform GetFocus() => _subjectFocus.GetFocus();
        public Transform GetCapture() => _subjectFocus.RetrieveRaycastCapture();

        // Start is called before the first frame update
        void Start()
        {
            if (!_engaged) return;

            if (_movableObjects)
                _getRealHeight.Start(_movableObjects.transform);
            else
                _getRealHeight.Engaged = false;

            _subjectFocus.Start();
            _controllerAttackCapture.Start();
            _tracerProjectileScript.Start();
            _controllerInput.Start();
        }

        // FixedUpdate is called once per frame
        void Update()
        {
            if (!_engaged) return;

            _controllerInput.Update();
            _subjectFocus.FixedUpdate();
            _controllerAttackCapture.Update();
            _getRealHeight.FixedUpdate(_subjectFocus.GetFocus());
            _tracerProjectileScript.SetNavAgent(_subjectFocus.GetFocus());
            _fetchMovablePoint.FixedUpdate(_subjectFocus.GetFocus(), _subjectFocus.HasFocused());
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