using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    //Changes scene
	public void changeScene (string name) {
        SceneManager.LoadScene(name);
    }
}
