using System;
using Assets.GamePrimal.Helpers;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using Assets.TeamProjects.GamePrimal.SeparateComponents.ListsOfStuff;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Assets.TeamProjects.GamePrimal.Proxies.StaticProxyInput;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.SceneShifter.Monobeh
{
    [Serializable]
    public struct SceneManagerStruct
    {
        public SceneField PureWeaponScene;
        public SceneField AnimationDemoScene;
        public SceneField MapScene;
        public SceneField ChurchFirstFloor;
        public SceneField TheNextScene;
    }

    public class ControllerSceneShift : MonoBehaviour
    {
        #region Fields
        public SceneField ActualScene = new SceneField();
        public SceneField TheNextScene = new SceneField();
        public SceneField MapSceneField = new SceneField();

        [SerializeField]
        private SceneManagerStruct SManager = new SceneManagerStruct()
        {
            AnimationDemoScene = new SceneField(),
            MapScene = new SceneField(),
            PureWeaponScene = new SceneField(),
            ChurchFirstFloor = new SceneField(),
        }; 
        #endregion


        #region MonoBehavior
        private void Awake()
        {
            if (FindObjectsOfType<ControllerSceneShift>().Length > 1)
                Destroy(gameObject);
        }

        private void Start()
        {
//            DontDestroyOnLoad(gameObject);
            ActualScene.SetFromScene(SceneManager.GetActiveScene());
            SManager.PureWeaponScene.SetFromPath(ResourcesList.PureWeaponScene);
            SManager.AnimationDemoScene.SetFromPath(ResourcesList.AnimationDemoScene);
            SManager.MapScene.SetFromPath(ResourcesList.MapScene);
            MapSceneField.SetFromPath(ResourcesList.MapScene);
            SManager.ChurchFirstFloor.SetFromPath(ResourcesList.ChurchTheFirstFloor);
        }

        private void Update()
        {
            //            if (P)
            //                LoadPureWeaponScene();
            //            else if (L)
            //                LoadMapScene();
            //            else if (O)
            //                LoadChurchFirstFloorScene();
        } 
        private void OnEnable() => StaticProxyEvent.EMatchHasComeToAnEnd.Event += MatchHandler;
        private void OnDisable() => StaticProxyEvent.EMatchHasComeToAnEnd.Event += MatchHandler;
        #endregion


        private void MatchHandler(EventMatchHasComeToAnEndParams acp) => LoadAnyScene(TheNextScene);

//        public void LoadMapScene() => LoadAnyScene(SManager.MapScene);
//        public void LoadPureWeaponScene() => LoadAnyScene(SManager.PureWeaponScene);
//        public void LoadChurchFirstFloorScene() => LoadAnyScene(SManager.ChurchFirstFloor);

        private void LoadAnyScene(string sceneName)
        {
//            StaticProxyEvent.EEndOfRound.Invoke(new EventParamsBase());
            SceneManager.LoadScene(sceneName);
        }
    }
}