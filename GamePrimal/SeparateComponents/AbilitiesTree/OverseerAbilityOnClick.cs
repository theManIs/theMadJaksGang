using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.AbilitiesTree
{
    public class OverseerAbilityOnClick : MonoBehaviour
    {
        public string AbilityName;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(GoToLockMode);
        }

        private void Update()
        {
            if (StaticProxyInput.RightMouse && StaticProxyStateHolder.LockModeOn)
            {
                StaticProxyStateHolder.LockModeOn = false;
//                Transform hardFocus = StaticProxyRouter.GetControllerFocusSubject().GetHardFocus();

//                if (hardFocus)
//                    hardFocus.GetComponent<MonoMechanicus>().ResetCurrentAbility();

                StaticProxyEvent.EActiveAbilityChanged.Invoke(new EventActiveAbilityChangedParams()
                {
                    AbilityInLockMode = false,
                    AbilityName = AbilityName
                });
            }
        }

        private void GoToLockMode()
        {
            StaticProxyStateHolder.LockModeOn = true;
//            Transform hardFocus = StaticProxyRouter.GetControllerFocusSubject().GetHardFocus();

//            if (hardFocus)
//                hardFocus.GetComponent<MonoMechanicus>().SetActiveAbility(AbilityName);

            StaticProxyEvent.EActiveAbilityChanged.Invoke(new EventActiveAbilityChangedParams()
            {
                AbilityInLockMode = true, AbilityName = AbilityName
            });
        }
    }
}
