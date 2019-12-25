using Assets.TeamProjects.GamePrimal.Mono;
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

        private static Transform GetSceneBuilder() => Object.FindObjectOfType<SceneBuilder>()?.transform;
        private static bool DoesScriptExist<T>() where T : Object => (bool)Object.FindObjectOfType<T>();

        private static void AddObject<T>(string objectName, bool theLock = false) where T : Component
        {
            if (theLock && DoesScriptExist<T>()) return;

            GameObject gm = new GameObject(objectName);

            gm.AddComponent<T>();
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