using Assets.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Mono;
using UnityEngine;

namespace Assets.GamePrimal.Mono
{
    [RequireComponent(typeof(MonoAmplifierRpg))]
    public class MonoMechanicus : MonoBehaviour
    {
        private ControllerDrumSpinner _cDrumSpinner;
        public bool _iAmMoving = false;

        // Start is called before the first frame update
        void Start()
        {
            _cDrumSpinner = ControllerRouter.GetControllerDrumSpinner();
        }

        // Update is called once per frame
        void Update()
        {
            if (!enabled) return;

            bool doIReallyMove = _cDrumSpinner.DoIMove(transform);

            if (doIReallyMove)
            {
                Debug.Log(Time.time + " " + gameObject.name + " " + GetComponent<MonoAmplifierRpg>().GetInitiative());

                _iAmMoving = true;
            }
            
            if (_iAmMoving)
                if (Input.GetKeyDown(KeyCode.Space))
                    if (_cDrumSpinner.ReleaseRound())
                        _iAmMoving = false;
        }
    }
}
