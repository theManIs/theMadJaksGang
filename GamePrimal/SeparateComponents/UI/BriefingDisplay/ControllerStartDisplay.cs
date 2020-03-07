using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using Assets.TeamProjects.GamePrimal.SeparateComponents.ListsOfStuff;
using UnityEngine;
using static Assets.TeamProjects.GamePrimal.SeparateComponents.UI.BriefingDisplay.SceneCanvasStates;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.UI.BriefingDisplay
{
    public class ControllerStartDisplay : MonoBehaviour
    {
        public SceneCanvasStates CanvasStates;
        private EndCanvasHolder _endCanvas;
        public bool NextLevelLocked = false;
        public readonly float WaiterLimit = 5f;
        public float WaiterLimitLeft = 5f;
        private ControllerDrumSpinner _cDrumSpinner;
        private StartCanvasHoler _startCanvas;

        public void SetState(SceneCanvasStates scs) => CanvasStates = scs;

        private void Start()
        {
            bool isStartCanvas = StartScene == CanvasStates || AllScenes == CanvasStates;
            bool isEndCanvas = EndScene == CanvasStates || AllScenes == CanvasStates;

            if (isStartCanvas && !FindObjectOfType<StartCanvasHoler>()) { }
                _startCanvas = Instantiate(Resources.Load<StartCanvasHoler>(ResourcesList.StartDisplay));

            if (isEndCanvas && !FindObjectOfType<EndCanvasHolder>())
                _endCanvas = Instantiate(Resources.Load<EndCanvasHolder>(ResourcesList.EndDisplay));

            _cDrumSpinner = StaticProxyRouter.GetControllerDrumSpinner();

            _cDrumSpinner.EnterIdleMode();
        }

        private void Update()
        {
            if (_cDrumSpinner.IdleRunning)
                if (StaticProxyInput.Space && !_startCanvas.gameObject.activeSelf)
                    _cDrumSpinner.LeaveIdleMode();

            if (StaticProxyInput.Space)
                _startCanvas.gameObject.SetActive(false);


            if (NextLevelLocked)
            {
                if (WaiterLimitLeft > 0)
                    WaiterLimitLeft -= Time.deltaTime;
                else
                {
                    WaiterLimitLeft = WaiterLimit;
                    NextLevelLocked = false;

                    StaticProxyEvent.EMatchHasComeToAnEnd.Invoke(new EventMatchHasComeToAnEndParams());
                }

                return;
            }

            MonoMechanicus[] monomechs = StaticProxyObjectFinder.FindObjectOfType<MonoMechanicus>();

            if (monomechs.Length <= 0)
                return;

            bool isAllTheSame = true;
            bool isBlueTeam = monomechs[0].IsBlueTeam;

            foreach (MonoMechanicus m in monomechs)
            {

                if (m.IsBlueTeam != isBlueTeam)
                    isAllTheSame = false;  

                isBlueTeam = m.IsBlueTeam;
            }

            if (isAllTheSame)
            {
                _endCanvas.gameObject.SetActive(true);

                NextLevelLocked = true;
            }
        }
    }
}
