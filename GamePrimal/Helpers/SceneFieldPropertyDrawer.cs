using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
#if UNITY_EDITOR

#endif
namespace Assets.GamePrimal.Helpers
{
	[Serializable]
	public class SceneField
	{
		[SerializeField] private Object _sceneAsset;

        public static implicit operator string(SceneField sceneField) => sceneField._sceneAsset != null ? sceneField._sceneAsset.name : String.Empty;

//        public void SetFromScene(Scene scene) => _sceneAsset =
//            new Object()
//            {
//                name =
//                    scene.name
//            };
//#if UNITY_EDITOR
//        public void SetFromAScene(SceneAsset sa) => _sceneAsset = sa;
//        public void SetFromPath(string assetPath) => SetFromAScene(AssetDatabase.LoadAssetAtPath<SceneAsset>(assetPath));
//#endif


        public Object SceneAsset => _sceneAsset;
        public override string ToString() => this;
    }

#if UNITY_EDITOR

	[CustomPropertyDrawer(typeof(SceneField))]
	public class SceneFieldPropertyDrawer : PropertyDrawer
	{
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var sceneAsset = property.FindPropertyRelative("_sceneAsset");

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var value = EditorGUI.ObjectField(position, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);

            sceneAsset.objectReferenceValue = value;
        }
	}

#endif
}
