using System.Collections.Generic;
using Assets.GamePrimal.Controllers;
using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.HudPack.Mono;
using Assets.TeamProjects.GamePrimal.SeparateComponents.InterfaceHold;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.HudPack.Scripts
{
    public class HudViewer : IUserAwake, IUpdate, IUserAwakeInstantiator<HudViewer>
    {
        private ActionPointsHolder _actionPointsHolder;
        private ControllerFocusSubject _cFocusSubject;
        private HealthHolder _healthHolder;
        private InitiativeHolder _initiativeHolder;
        private ControllerDrumSpinner _cDrupSpinner;
        private ExperienceHolder _expHolder;
        private bool _debugFlag = false;

        public HudViewer UserAwakeInstantiator(ref AwakeParams ap)
        {
            UserAwake(ap);

            return this;
        }

        public void UserAwake(AwakeParams ap)
        {
            _cFocusSubject = StaticProxyRouter.GetControllerFocusSubject();
            _actionPointsHolder = Object.FindObjectOfType<ActionPointsHolder>();
            _healthHolder = Object.FindObjectOfType<HealthHolder>();
            _initiativeHolder = Object.FindObjectOfType<InitiativeHolder>();
            _expHolder = Object.FindObjectOfType<ExperienceHolder>();
            _cDrupSpinner = ap.CDrumSpinner;
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

        private void SetImage(ref int childNum, Transform focusPoint)
        {
            Transform nthChild = _initiativeHolder.transform.GetChild(childNum);
            Image nthImage = nthChild.GetComponent<Image>();
            nthImage.sprite = !focusPoint || !focusPoint.GetComponent<MonoAmplifierRpg>() ?
                null : focusPoint.GetComponent<MonoAmplifierRpg>().GetCharacterPortrait();

            if (nthImage.sprite)
            {
                Sprite portraitSprite = focusPoint.GetComponent<MonoAmplifierRpg>().GetCharacterPortrait();

                nthImage.sprite = portraitSprite;

                if (_debugFlag && !portraitSprite) Debug.Log("Does not have portrait sprite: " + portraitSprite);
            }

            childNum++;

            if (_debugFlag && !nthChild) Debug.Log("Does not have the first icon: " + nthChild);
            if (_debugFlag && !nthImage) Debug.Log("Does not have the first image: " + nthImage + " " + nthImage.sprite);
        }

        public void ShowInitiativeList(MonoAmplifierRpg mar, Transform actualInvoker)
        {
            if (actualInvoker != _cFocusSubject.GetHardFocus()) return;
            if (!_cFocusSubject.GetHardFocus()) return;

            int childCount = _initiativeHolder.transform.childCount;
            int iconsLocker = 0;

            FillTheLineGap(ref iconsLocker, childCount, _cDrupSpinner.ActualDrum, _cDrupSpinner.GetWhoseTurn());

            if (iconsLocker < childCount) 
                FillTheLineGap(ref iconsLocker, childCount, _cDrupSpinner.DrumBlank, null);
        }

        private void FillTheLineGap(ref int iconsLocker, int globalLock, Queue<Transform> localQueue, Transform theFirst)
        {
            int startAmount = localQueue.Count;

            SetImage(ref iconsLocker, theFirst);

            for (int i = 0; i < startAmount && iconsLocker < globalLock; i++)
                SetImage(ref iconsLocker, localQueue.Dequeue());
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