using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

using SceneAsset = UnityEditor.SceneAsset;

namespace Assets.GamePrimal.Helpers
{
	[Serializable]
	public class SceneField
	{
		[SerializeField] private Object _sceneAsset;

        public static implicit operator string(SceneField sceneField) => sceneField._sceneAsset != null ? sceneField._sceneAsset.name : String.Empty;

        public void SetFromAScene(SceneAsset sa) => _sceneAsset = sa;
        public void SetFromScene(Scene scene) => this.SetFromPath(scene.path);
        public void SetFromPath(string assetPath) => SetFromAScene(AssetDatabase.LoadAssetAtPath<SceneAsset>(assetPath));
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
