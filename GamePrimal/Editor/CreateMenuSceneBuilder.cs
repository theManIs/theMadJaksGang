using Assets.TeamProjects.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.SeparateComponents.ListsOfStuff;
using UnityEditor;
using UnityEngine;
using UnityStandardAssets.Cameras;

namespace Assets.TeamProjects.GamePrimal.Editor
{
    public class CreateMenuSceneBuilder : ScriptableObject
    {
        [MenuItem("Assets/Add/SceneBuilder")]
        static void AddSceneBuilder() => AddObject<SceneBuilder>("SceneBuilder", true);

        [MenuItem("Assets/Add/MainScene")]
        private static void AddMainScene() => AddObject<MainScene.MainScene>("MainScene", true);

        [MenuItem("Assets/Add/FreeCameraRig")]
        private static void AddFreeCameraRig() => AddCameraRig("FreeCameraRig", true);

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
        //        Hud_11982

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

            GameObject cameraRigPrefab = Resources.Load<GameObject>("FreeLookCameraRig_14626");
            Transform sBuilder = GetSceneBuilder();

            if (cameraRigPrefab && sBuilder) 
                Instantiate(cameraRigPrefab, sBuilder.transform.position, Quaternion.identity);
        }
    }
}