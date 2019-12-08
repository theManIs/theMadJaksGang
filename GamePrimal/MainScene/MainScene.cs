using System;
using System.Collections;
using System.Collections.Generic;
using Assets.GamePrimal.Controllers;
using Assets.GamePrimal.Helpers;
using Assets.GamePrimal.Navigation.HighlightFrame;
using Assets.GamePrimal.Navigation.Pathfinder;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


namespace Assets.GamePrimal.MainScene
{
    public class MainScene : MonoBehaviour
    {
        private GetRealHeight _getRealHeight;
        private SubjectFocus _subjectFocus;
        private MovableObjects _movableObjects;
        private TracerProjectileScript _tracerProjectileScript;
        private FetchMovablePoint _fetchMovablePoint;
        private ControllerAttackCapture _controllerAttackCapture;
        private ControllerInput _controllerInput;

        void Awake()
        {
            _getRealHeight = new GetRealHeight();
            _subjectFocus = new SubjectFocus();
            _movableObjects = FindObjectOfType<MovableObjects>();
            _tracerProjectileScript = new TracerProjectileScript();
            _fetchMovablePoint = new FetchMovablePoint();
            _controllerAttackCapture = new ControllerAttackCapture();
            _controllerInput = ControllerRouter.GetControllerInput();
        }

        public Transform GetFocus() => _subjectFocus.GetFocus();
        public Transform GetCapture() => _subjectFocus.RetrieveRaycastCapture();

        // Start is called before the first frame update
        void Start()
        {
            _getRealHeight.Start(_movableObjects.transform);
            _subjectFocus.Start();
            _controllerAttackCapture.Start();
            _tracerProjectileScript.Start();
            _controllerInput.Start();
        }

        // FixedUpdate is called once per frame
        void Update()
        {
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
            
        }
    }
}