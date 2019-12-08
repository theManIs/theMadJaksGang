using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR

#endif
namespace Assets.GamePrimal.Helpers
{
	[Serializable]
	public class SceneField
	{
		[SerializeField] private Object sceneAsset;

		public Object SceneAsset => sceneAsset;

		public static implicit operator string(SceneField sceneField)
		{
			return sceneField.sceneAsset != null ? sceneField.sceneAsset.name : String.Empty;
		}

		public override string ToString()
		{
			return this;
		}
	}

#if UNITY_EDITOR

	[CustomPropertyDrawer(typeof(SceneField))]
	public class SceneFieldPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var sceneAsset = property.FindPropertyRelative("sceneAsset");

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			var value = EditorGUI.ObjectField(position, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);

			sceneAsset.objectReferenceValue = value;
		}
	}

#endif
}
