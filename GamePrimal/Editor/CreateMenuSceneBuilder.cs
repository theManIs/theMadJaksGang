using Assets.GamePrimal.Controllers;
using Assets.TeamProjects.GamePrimal.CameraRigs.CamerasScripts;
using Assets.TeamProjects.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.SeparateComponents.ListsOfStuff;
using Assets.TeamProjects.GamePrimal.SeparateComponents.SceneShifter;
using Assets.TeamProjects.GamePrimal.SeparateComponents.SceneShifter.Monobeh;
using UnityEditor;
using UnityEngine;
using UnityStandardAssets.Cameras;

namespace Assets.TeamProjects.GamePrimal.Editor
{
    public class CreateMenuSceneBuilder : ScriptableObject
    {
        [MenuItem("Assets/Add/Scene/SceneBuilder")]
        static void AddSceneBuilder() => AddObject<SceneBuilder>("SceneBuilder", true);

        [MenuItem("Assets/Add/Scene/MainScene")]
        private static void AddMainScene() => AddObject<MainScene.MainScene>("MainScene", true);

        [MenuItem("Assets/Add/Scene/SceneShift")]
        private static void AddSceneShift() => AddObject<ControllerSceneShift>("SceneShift", true);

        [MenuItem("Assets/Add/Cameras/FreeCameraRig")]
//        private static void AddFreeCameraRig() => AddCameraRig("FreeCameraRig", true);
        private static void AddFreeCameraRig() => AddPrefab<FreeLookCamWithUserInput>(ResourcesList.FreeLookCameraRig, true);

        [MenuItem("Assets/Add/Cameras/GlobalMapCamera")]
        private static void GlobalMapCamera() => AddPrefab<FreeLookCamWithUserInput>(ResourcesList.GlobalMapCamera, true);

        [MenuItem("Assets/Add/Characters/FelineGargoil")]
        private static void AddGargoil() => AddPrefab<GameObject>(ResourcesList.FelineGargoil, false);

        [MenuItem("Assets/Add/Characters/AkaiArcher")]
        private static void AddAkaiArcher() => AddPrefab<GameObject>(ResourcesList.AkaiArcherCharacter, false);

        [MenuItem("Assets/Add/Characters/FearsomeParasite")]
        private static void AddFearsomeParasite() => AddPrefab<GameObject>(ResourcesList.FearsomeParasiteCharacter, false);

        [MenuItem("Assets/Add/Characters/HeartlessVampire")]
        private static void AddHeartlessVampire() => AddPrefab<GameObject>(ResourcesList.HeartlessVampireCharacter, false);

        [MenuItem("Assets/Add/Characters/MeleeAxeFighter")]
        private static void AddMeleeAxeFighter() => AddPrefab<GameObject>(ResourcesList.MeleeAxeFighterCharacter, false);

        [MenuItem("Assets/Add/Characters/FemaleSwordsman")]
        private static void AddMFemaleSwordsman() => AddPrefab<GameObject>(ResourcesList.FemaleSwordsmanCharacter, false);

        [MenuItem("Assets/Add/HeadUpDisplay")]
        private static void AddHud() => AddPrefab<GameObject>(ResourcesList.HeadUpDisplay);

        private static Transform GetSceneBuilder() => Object.FindObjectOfType<SceneBuilder>()?.transform;
        private static bool DoesScriptExist<T>() where T : Object => (bool)Object.FindObjectOfType<T>();

        private static void AddObject<T>(string objectName, bool theLock = false) where T : Component
        {
            if (theLock && DoesScriptExist<T>()) return;

            GameObject gm = new GameObject(objectName);

            gm.AddComponent<T>();
        }

        private static void AddPrefab<T>(string prefabName, bool theLock = false) where T : Object
        {
            if (theLock && DoesScriptExist<T>()) return;

            GameObject cameraRigPrefab = Resources.Load<GameObject>(prefabName);
            Transform sBuilder = GetSceneBuilder();

            if (cameraRigPrefab && sBuilder)
                Instantiate(cameraRigPrefab, sBuilder.transform.position, Quaternion.identity);
        }
        private static void AddCameraRig(string objectName, bool theLock = false)
        {
            if (theLock && DoesScriptExist<FreeLookCamWithUserInput>()) return;

            GameObject cameraRigPrefab = Resources.Load<GameObject>(ResourcesList.FreeLookCameraRig);
            Transform sBuilder = GetSceneBuilder();

            if (cameraRigPrefab && sBuilder) 
                Instantiate(cameraRigPrefab, sBuilder.transform.position, Quaternion.identity);
        }
    }
}