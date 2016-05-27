using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {
    public string lastPlayedLevel;
    //Figuring this out later

    //void Awake() {
    //    DontDestroyOnLoad(transform.gameObject);
    //}

    //Changes scene
    public void changeScene (string name) {
        //lastPlayedLevel = SceneManager.GetActiveScene().name;
        //if (name == "_Menu") {
        //    gameObject.SetActive(false);
        //}
        SceneManager.LoadScene(name);
    }

    public string recall () {
        return lastPlayedLevel;
    }

    //void OnApplicationQuit () {
    //    lastPlayedLevel = "NULL";
    //}
}
