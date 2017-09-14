using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	// Use this for initialization
	public void ChangeScene (string scene) {
		SceneManager.LoadScene(scene);
	}
}
