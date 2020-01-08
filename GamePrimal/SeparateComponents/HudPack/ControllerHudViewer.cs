using Assets.GamePrimal.Controllers;
using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.Helpers.InterfaceHold;
using Assets.TeamProjects.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.HudPack.Mono;
using Assets.TeamProjects.GamePrimal.SeparateComponents.HudPack.Scripts;
using Assets.TeamProjects.GamePrimal.SeparateComponents.InterfaceHold;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Assets.TeamProjects.GamePrimal.Proxies.StaticProxyRouter;
using static Assets.TeamProjects.GamePrimal.SeparateComponents.ListsOfStuff.ResourcesList;
using static UnityEngine.Resources;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.HudPack
{
    public class ControllerHudViewer : MonoBehaviourBaseClass, IPointerEnterHandler, IPointerExitHandler
    {
        private ExperienceHolder _expHolder;
        private HealthHolder _healthHolder;
        private BeltHolder _betlHolder;
        private ActionPointsHolder _actionPointsHolder;
        private HudViewer _hudViewer;
        private ControllerFocusSubject _subjectFocus;
        private ControllerDrumSpinner _cDrumSpinner;

        private void Awake()
        {
            _expHolder = GetComponentInChildren<ExperienceHolder>();
            _healthHolder = GetComponentInChildren<HealthHolder>();
            _betlHolder = GetComponentInChildren<BeltHolder>();
            _actionPointsHolder = GetComponentInChildren<ActionPointsHolder>();
            _cDrumSpinner = GetControllerDrumSpinner();
            _subjectFocus = GetControllerFocusSubject();
            AwakeParams ap = new AwakeParams() {CDrumSpinner = _cDrumSpinner};
            _hudViewer = new HudViewer().UserAwakeInstantiator(ref ap);
        }

        private void Start()
        {
            LoadExperienceSlider(_expHolder.transform, ExperienceBackground, ExperienceFullLine, ExperienceFiller);
            LoadExperienceSlider(_healthHolder.transform, HealthBackground, HealthFullLine, HealthFiller);
            LoadBelt();
            LoadBackground();
            LoadActionPoints();
        }

        private void Update()
        {
            Transform mySubject = _subjectFocus.GetHardFocus();
            
            if (mySubject)
                _hudViewer.UserUpdate(new UpdateParams()
                {
                    ActualInvoker = mySubject, 
                    AmplifierRpg = mySubject.GetComponent<MonoAmplifierRpg>()
                });
        }

        #region LoadActionPoints

        private void LoadActionPoints()
        {
            _actionPointsHolder.GetComponent<Image>().sprite = Load<Sprite>(ActionPointsHolderPic);

            foreach (Transform t in _actionPointsHolder.transform)
                t.GetComponent<Image>().sprite = Load<Sprite>(ActionPoint);
        }

        #endregion

        #region LoadBelt
        private void LoadBackground()
        {
            transform.Find("Black").GetComponent<Image>().sprite = Load<Sprite>(BackgroundNizBlac);
        }

        private void LoadBelt()
        {
            _betlHolder.gameObject.GetComponent<Image>().sprite = Load<Sprite>(BeltRemen);
            _betlHolder.transform.Find("Botle").GetComponent<Image>().sprite = Load<Sprite>(BeltBotle);
            _betlHolder.transform.Find("Pis Of Poias").GetComponent<Image>().sprite = Load<Sprite>(BeltRemenMini);
            _betlHolder.transform.Find("Menu").GetComponent<Image>().sprite = Load<Sprite>(BeltMenuIkon);
        }
        #endregion

        #region LoadExperienceSlider
        private void LoadExperienceSlider(Transform someHolder, string back, string full, string filler)
        {
            LoadExperience(someHolder, back);
            LoadSlider(someHolder, full);
            LoadFiller(someHolder, filler);
        }

        private void LoadSlider(Transform someHolder, string spriteName) => 
            someHolder.GetChild(0).Find("Background").GetComponent<Image>().sprite = Load<Sprite>(spriteName);
        private void LoadFiller(Transform someHolder, string spriteName) => 
            someHolder.GetChild(0).Find("Fill Area").transform.GetChild(0).GetComponent<Image>().sprite = Load<Sprite>(spriteName);
        private void LoadExperience(Transform someHolder, string spriteName) => 
            someHolder.GetComponent<Image>().sprite = Load<Sprite>(spriteName);
        #endregion

        public void OnPointerEnter(PointerEventData eventData)
        {
            StaticProxyStateHolder.UserOnUi = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StaticProxyStateHolder.UserOnUi = false;
        }
    }
}