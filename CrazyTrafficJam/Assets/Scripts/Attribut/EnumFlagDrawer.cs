﻿using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace IronSideStudio.CrazyTrafficJam
{
	[CustomPropertyDrawer(typeof(EnumFlagAttribute))]
	public class EnumFlagDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			//Enum targetEnum = GetBaseProperty<Enum>(property);

			//string propName = property.name;

			//EditorGUI.BeginProperty(position, label, property);
			//Enum enumNew = EditorGUI.EnumFlagsField(position, propName, targetEnum);
			//property.intValue = (int)Convert.ChangeType(enumNew, targetEnum.GetType());
			//EditorGUI.EndProperty();

			property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
		}

		static T GetBaseProperty<T>(SerializedProperty prop)
		{
			// Separate the steps it takes to get to this property
			string[] separatedPaths = prop.propertyPath.Split('.');

			// Go down to the root of this serialized property
			System.Object reflectionTarget = prop.serializedObject.targetObject as object;
			// Walk down the path to get the target object
			foreach (var path in separatedPaths)
			{
				FieldInfo fieldInfo = reflectionTarget.GetType().GetField(path);
				if (fieldInfo != null)
					reflectionTarget = fieldInfo.GetValue(reflectionTarget);
			}
			return (T)reflectionTarget;
		}
	}
}