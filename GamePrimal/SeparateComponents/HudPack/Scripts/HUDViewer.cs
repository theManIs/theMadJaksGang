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
    public class HudViewer : IUserAwake, IUpdate, IUserAwakeInstantiator<HudViewer>
    {
        public bool DebugFlag = false;
        private ActionPointsHolder _actionPointsHolder;
        private ControllerFocusSubject _cFocusSubject;
        private HealthHolder _healthHolder;
        private InitiativeHolder _initiativeHolder;
        private ControllerDrumSpinner _cDrupSpinner;
        private ExperienceHolder _expHolder;
        public HudViewer UserAwakeInstantiator(AwakeParams ap)
        {
            UserAwake(ap);

            return this;
        }

        public void UserAwake(AwakeParams ap)
        {
            _cFocusSubject = ControllerRouter.GetControllerFocusSubject();
            _actionPointsHolder = Object.FindObjectOfType<ActionPointsHolder>();
            _healthHolder = Object.FindObjectOfType<HealthHolder>();
            _initiativeHolder = Object.FindObjectOfType<InitiativeHolder>();
            _expHolder = Object.FindObjectOfType<ExperienceHolder>();
            _cDrupSpinner = ControllerRouter.GetControllerDrumSpinner();
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

            Slider healthSlider = _healthHolder.GetComponentInChildren<Slider>();

            if (healthSlider)
            {
                healthSlider.value = health;
                healthSlider.maxValue = maxHealth;
            }
        }

        public void ShowExperienceBar(int exp, int maxExp, Transform actualInvoker)
        {
            if (actualInvoker != _cFocusSubject.GetHardFocus()) return;

            Slider expSlider = _expHolder.GetComponentInChildren<Slider>();

            if (expSlider)
            {
                expSlider.value = exp;
                expSlider.maxValue = maxExp;
            }
        }

        public void ShowInitiativeList(MonoAmplifierRpg mar, Transform actualInvoker)
        {
            if (actualInvoker != _cFocusSubject.GetHardFocus()) return;

            Queue<Transform> localDrum = _cDrupSpinner.GetDrum();
            Transform hardFocus = _cDrupSpinner.GetWhoseTurn();
            
            if (DebugFlag) Debug.Log("Does not have a focus: " + hardFocus);

            if (hardFocus)
            {   
                Transform nthChild = _initiativeHolder.transform.GetChild(0);
                Image nthImage = nthChild.GetComponent<Image>();
                Sprite portraitSprite = hardFocus.GetComponent<MonoAmplifierRpg>().GetCharacterPortrait();

                if (nthImage)
                    nthChild.GetComponent<Image>().sprite = portraitSprite;

                if (DebugFlag && !nthChild) Debug.Log("Does not have the first icon: " + nthChild);
                if (DebugFlag && !portraitSprite) Debug.Log("Does not have portrait sprite: " + portraitSprite);
                if (DebugFlag && !nthImage) Debug.Log("Does not have the first image: " + nthImage + " " + nthImage.sprite);
            }

            for (int i = 1; i < 5; i++)
            {
                Transform nthChild = _initiativeHolder.transform.GetChild(i);
//                Debug.Log(localDrum.Count);
                if (localDrum.Count > 0)
                {
                    Transform recentChar = localDrum.Dequeue();
                    Debug.Log(recentChar);
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