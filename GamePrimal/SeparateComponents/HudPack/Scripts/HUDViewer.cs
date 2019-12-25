using System.Collections.Generic;
using Assets.GamePrimal.Controllers;
using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using Assets.TeamProjects.GamePrimal.SeparateComponents.HudPack.Mono;
using Assets.TeamProjects.GamePrimal.SeparateComponents.InterfaceHold;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.HudPack.Scripts
{
    public class HudViewer : IUserAwake, IUpdate
    {
        private ActionPointsHolder _actionPointsHolder;
        private ControllerFocusSubject _cFocusSubject;
        private HealthHolder _healthHolder;
        private InitiativeHolder _initiativeHolder;
        private ControllerDrumSpinner _cDrupSpinner;
        private ExperienceHolder _expHolder;

        public void UserAwake(AwakeParams ap)
        {
            _cFocusSubject = ControllerRouter.GetControllerFocusSubject();
            _actionPointsHolder = Object.FindObjectOfType<ActionPointsHolder>();
            _healthHolder = Object.FindObjectOfType<HealthHolder>();
            _initiativeHolder = Object.FindObjectOfType<InitiativeHolder>();
            _expHolder = Object.FindObjectOfType<ExperienceHolder>();
            _cDrupSpinner = ControllerRouter.GetControllerDrumSpinner();
        }

        public void UserStart(StartParams sp)
        {
            throw new System.NotImplementedException();
        }


        public void ShowTurnPoints(int points, Transform actualInvoker)
        {
            if (actualInvoker == _cFocusSubject.GetHardFocus())
                foreach (Transform t in _actionPointsHolder.transform)
                {
                    t.GetComponent<Image>().enabled = points > 0;
                    points -= 1;
                }
        }

        public void ShowHealthBar(int health, int maxHealth, Transform actualInvoker)
        {
            if (actualInvoker != _cFocusSubject.GetHardFocus()) return;

            Slider healthSlider = _healthHolder.GetComponent<Slider>();

            if (healthSlider)
            {
                _healthHolder.GetComponent<Slider>().value = health;
                _healthHolder.GetComponent<Slider>().maxValue = maxHealth;
            }
        }

        public void ShowExperienceBar(int exp, int maxExp, Transform actualInvoker)
        {
            if (actualInvoker != _cFocusSubject.GetHardFocus()) return;

            Slider expSlider = _expHolder.GetComponent<Slider>();

            if (expSlider)
            {
                _expHolder.GetComponent<Slider>().value = exp;
                _expHolder.GetComponent<Slider>().maxValue = maxExp;
            }
        }

        public void ShowInitiativeList(MonoAmplifierRpg mar, Transform actualInvoker)
        {
            if (actualInvoker != _cFocusSubject.GetHardFocus()) return;

            Queue<Transform> localDrum = _cDrupSpinner.GetDrum();
            Transform hardFocus = _cDrupSpinner.GetWhoseTurn();
//            Debug.Log(localDrum.Count);

            if (hardFocus)
            {   
                Transform nthChild = _initiativeHolder.transform.GetChild(0);

                if (nthChild.GetComponent<Image>())
                    nthChild.GetComponent<Image>().sprite = hardFocus.GetComponent<MonoAmplifierRpg>().GetCharacterPortrait();
            }

            for (int i = 1; i < 5; i++)
            {
                Transform nthChild = _initiativeHolder.transform.GetChild(i);

                if (localDrum.Count > 0)
                {
                    Transform recentChar = localDrum.Dequeue();

                    if (nthChild.GetComponent<Image>())
                        nthChild.GetComponent<Image>().sprite = recentChar.GetComponent<MonoAmplifierRpg>().GetCharacterPortrait();
                }
                else
                {
                    if (nthChild.GetComponent<Image>())
                        nthChild.GetComponent<Image>().sprite = null;
                }
                 

            }
        }

        public void UserUpdate(UpdateParams up)
        {
            _cFocusSubject.UpdateOnce();

            if (_actionPointsHolder)
                this.ShowTurnPoints(up.AmplifierRpg.GetTurnPoints(), up.ActualInvoker);

            if (_healthHolder)
                this.ShowHealthBar(up.AmplifierRpg.ViewHealth(), up.AmplifierRpg.MaxHealth, up.ActualInvoker);

            if (_initiativeHolder)
                this.ShowInitiativeList(up.AmplifierRpg, up.ActualInvoker);

            if (_expHolder)
                this.ShowExperienceBar(up.AmplifierRpg.ExperienceActual, up.AmplifierRpg.ExperienceMax, up.ActualInvoker);
        }

    }
}