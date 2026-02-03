

using UnityEngine;
using UnityEngine.Assertions;


namespace Extensions {
	public static class MonoBehaviourExtensions {
		public static void ValidateInspectorField(this MonoBehaviour monoBehaviour, Object target) {
			if (target == null) {
				Debug.LogError(monoBehaviour.name + ": Is missing component reference to: " + target.GetType().Name, monoBehaviour);
			}

			//Assert.IsNotNull(target, string.Format("{0}: Is missing component reference to: {1}", monoBehaviour.name, target.GetType().Name));
		}
	}



	public static class ScriptableObjectExtensions {
		public static void ValidateInspectorField(this ScriptableObject scriptable, Object target) {
			if (target == null) {
				Debug.LogError(scriptable.name + ": Is missing component reference to: " + target.GetType().Name, scriptable);
			}

			//Assert.IsNotNull(target, string.Format("{0}: Is missing component reference to: {1}", scriptable.name, target.GetType().Name));
		}
	}
}