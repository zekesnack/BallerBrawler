using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ApplicationManager : MonoBehaviour {
	public void Quit () {
		print("fuk");
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
