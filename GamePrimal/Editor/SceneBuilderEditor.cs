using Assets.GamePrimal.Controllers;
using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.CameraRigs.CamerasScripts;
using Assets.TeamProjects.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.SeparateComponents.SceneShifter;
using Assets.TeamProjects.GamePrimal.SeparateComponents.SceneShifter.Monobeh;
using UnityEditor;
using UnityEngine;
using UnityStandardAssets.Cameras;
using Object = UnityEngine.Object;

namespace Assets.TeamProjects.GamePrimal.Editor
{
    [CustomEditor(typeof(SceneBuilder))]
    public class SceneBuilderEditor : UnityEditor.Editor
    {
        private SceneBuilder _target;
        private bool _isPressedCameraRig;
        private bool _isPressedSceneShift;
        private bool _isMainScenePressed;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            _target = (SceneBuilder)target;

            if (EditorApplication.isPlaying) return;

//            bool hasCameraRig = DoesScriptExist<FreeLookCamWithUserInput>();
//            bool hasSceneShift = DoesScriptExist<ControllerSceneShift>();
//            bool hasMainScene = DoesScriptExist<MainScene.MainScene>();
//
//            GUILayout.Space(10);
//            GUILayout.BeginHorizontal();
//            GUILayout.Label("Free camera rig");
//
//            _isPressedCameraRig = GUILayout.Button(!hasCameraRig ? "Add" : "Remove");
//            
//
//            GUILayout.EndHorizontal();
//
//            GUILayout.Space(10);
//            GUILayout.BeginHorizontal();
//            GUILayout.Label("Scene shift");
//
//            _isPressedSceneShift = GUILayout.Button(hasSceneShift ? "Remove": "Add");
//
//            GUILayout.EndHorizontal();
//
//            GUILayout.Space(10);
//            GUILayout.BeginHorizontal();
//            GUILayout.Label("Main Scene");
//
//            _isMainScenePressed = GUILayout.Button(hasMainScene ? "Remove" : "Add");
//
//            GUILayout.EndHorizontal();


//            bool isAddDrakePressed = AddButton("Treacherous Drake", false);
//            bool isPreposterousSkeletonPressed = AddButton("Preposterous Skeleton", false);
//            bool isHeartlessVampire = AddButton("HeartlessVampire", false);
//            bool isFemaleSwordsman = AddButton("FemaleSwordsman", false);
//            bool isFearsomeParasite = AddButton("FearsomeParasite", false);
//            bool isMeleeAxeFighter = AddButton("MeleeAxeFighter", false);
//            bool isAkaiArcher = AddButton("AkaiArcher", false);

//            CameraRigPressed(_isPressedCameraRig, hasCameraRig);
//            CreateAndAddPressed<ControllerSceneShift>("SceneShift", _isPressedSceneShift, hasSceneShift);
//            CreateAndAddPressed<MainScene.MainScene>("MainScene", _isMainScenePressed, hasMainScene);
//            CreateAndAddCharacter("TreacherousDrake", isAddDrakePressed);
//            CreateAndAddCharacter("PreposterousSkeleton", isPreposterousSkeletonPressed);
//            CreateAndAddCharacter("HeartlessVampire", isHeartlessVampire);
//            CreateAndAddCharacter("FemaleSwordsman", isFemaleSwordsman);
//            CreateAndAddCharacter("FearsomeParasite", isFearsomeParasite);
//            CreateAndAddCharacter("MeleeAxeFighter", isMeleeAxeFighter);
//            CreateAndAddCharacter("AkaiArcher", isAkaiArcher);
        }

        private void CreateAndAddCharacter(string gameObjectName, bool isPressed)
        {
            if (!isPressed) return;

            GameObject drakePrefab = Resources.Load<GameObject>(gameObjectName);

            if (!drakePrefab) return;

            GameObject drake = Instantiate(drakePrefab, _target.transform.position, Quaternion.identity);

            drake.AddComponent<MonoMechanicus>();

            Selection.activeGameObject = drake;
        }

        private bool AddButton(string buttonName, bool hasInstanceExists)
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label(buttonName);

            bool isPressed = GUILayout.Button(hasInstanceExists ? "Remove" : "Add");

            GUILayout.EndHorizontal();

            return isPressed;
        }

        private void CreateAndAddPressed<T>(string objectName, bool isPressed, bool hasMainScene) where T : Component
        {
            if (!isPressed) return;

            if (hasMainScene)
            {
                DestroyImmediate((Object.FindObjectOfType<T>()).gameObject);
            }
            else
            {
                GameObject gm = new GameObject(objectName);
                gm.transform.position = _target.transform.position;

                gm.AddComponent<T>();
            }
        }

    //        private void OnSceneGUI()
    //        {
    //        }

    private void CameraRigPressed(bool isPressed, bool hasCameraRig)
        {
            if (!isPressed) return;

            if (hasCameraRig)
            {
                DestroyImmediate(Object.FindObjectOfType<FreeLookCamWithUserInput>().gameObject);
            }
            else
            {
                GameObject cameraRigPrefab = Resources.Load<GameObject>("FreeLookCameraRig_14626");

                if (cameraRigPrefab)
                    _target.CameraRig = Instantiate(cameraRigPrefab, _target.transform.position, Quaternion.identity)
                        .transform.GetChild(0).GetComponentInChildren<Camera>();
            }
        }

        private bool DoesScriptExist<T>() where T : Object
        {
            return (bool)Object.FindObjectOfType<T>();
        }
    }
}
