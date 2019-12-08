using Assets.GamePrimal.Controllers;
using UnityEngine;

namespace Assets.GamePrimal.Mono
{
    [RequireComponent(typeof(MonoAmplifierRpg))]
    public class MonoMechanicus : MonoBehaviour
    {
        private ControllerDrumSpinner _cDrumSpinner;
        private Camera _mainCamera;
        public bool _iAmMoving = false;

        // Start is called before the first frame update
        void Start()
        {
            _cDrumSpinner = ControllerRouter.GetControllerDrumSpinner();
            _mainCamera = GameObject.Find("GlobalCamera").GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            bool doIReallyMove = _cDrumSpinner.DoIMove(transform);

            if (doIReallyMove)
            {
                Debug.Log(Time.time + " " + gameObject.name + " " + GetComponent<MonoAmplifierRpg>().GetInitiative());

                _iAmMoving = true;
                
                CameraFollow();
            }
            
            if (_iAmMoving)
                if (Input.GetKeyDown(KeyCode.Space))
                    if (_cDrumSpinner.ReleaseRound())
                        _iAmMoving = false;
        }

        private void CameraFollow()
        {
//            Quaternion.Slerp(_mainCamera.transform.rotation, transform.rotation, Time.deltaTime);
            _mainCamera.gameObject.transform.LookAt(transform);
//            _mainCamera.transform.position = ( transform.position);
        }
    }
}
