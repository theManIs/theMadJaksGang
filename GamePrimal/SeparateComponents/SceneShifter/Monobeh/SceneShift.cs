using System;
using Assets.GamePrimal.Helpers;
using Assets.TeamProjects.GamePrimal.SeparateComponents.ListsOfStuff;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

namespace Assets.TeamProjects.GamePrimal.SeparateComponents.SceneShifter.Monobeh
{
    [Serializable]
    public struct SceneManagerStruct
    {
        public SceneField PureWeaponScene;
        public SceneField AnimationDemoScene;
        public SceneField MapScene;
        public SceneField ChurchFirstFloor;
    }

    public class SceneShift : UnityEngine.MonoBehaviour
    {
        public SceneField ActualScene;

        [SerializeField] 
        private SceneManagerStruct SManager = new SceneManagerStruct()
        {
            AnimationDemoScene = new SceneField(),
            MapScene = new SceneField(),
            PureWeaponScene = new SceneField(),
            ChurchFirstFloor = new SceneField()
        };

        public void Awake()
        {
            ActualScene.SetFromScene(SceneManager.GetActiveScene());
        }

        public void Start()
        {
//            DontDestroyOnLoad(gameObject);
            SManager.PureWeaponScene.SetFromPath(ResourcesList.PureWeaponScene);
            SManager.AnimationDemoScene.SetFromPath(ResourcesList.AnimationDemoScene);
            SManager.MapScene.SetFromPath(ResourcesList.MapScene);
            SManager.ChurchFirstFloor.SetFromPath(ResourcesList.ChurchTheFirstFloor);

        }
        public void LoadMapScene() => SceneManager.LoadScene(SManager.MapScene);
        public void LoadPureWeaponScene() => SceneManager.LoadScene(SManager.PureWeaponScene);
        public void LoadChurchFirstFloorScene() => SceneManager.LoadScene(SManager.ChurchFirstFloor);
    }
}