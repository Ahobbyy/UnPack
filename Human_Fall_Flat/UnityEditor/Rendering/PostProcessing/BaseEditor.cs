using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	public class BaseEditor<T> : Editor where T : MonoBehaviour
	{
		protected T m_Target => (T)(object)((Editor)this).get_target();

		protected SerializedProperty FindProperty<TValue>(Expression<Func<T, TValue>> expr)
		{
			return ((Editor)this).get_serializedObject().FindProperty(RuntimeUtilities.GetFieldPath(expr));
		}

		public BaseEditor()
			: this()
		{
		}
	}
}
