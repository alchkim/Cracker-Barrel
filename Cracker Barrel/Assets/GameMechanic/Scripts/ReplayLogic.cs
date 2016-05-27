using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
public class ReplayLogic : MonoBehaviour {
    public RectTransform timer;
    public RectTransform result;
    public RectTransform goToMenu;
    public GameObject eventSystem;
    public GameObject fourboard;
    public GameObject fiveboard;
    public GameObject sixboard;
    public GameObject fourslots;
    public GameObject fiveslots;
    public GameObject sixslots;
    string lastLevel;

    // Use this for initialization
    void Start () {
        eventSystem = GameObject.Find("EventSystem");
        Debug.Log(eventSystem.GetComponent<ChangeScene>().recall());
        lastLevel = eventSystem.GetComponent<ChangeScene>().recall();
        createInitialBoard(lastLevel);

        timer.anchoredPosition = new Vector2(-Screen.width / 2 + timer.rect.width * 5 / 8,
                                            Screen.height / 2 - timer.rect.height);
        result.anchoredPosition = new Vector2(-Screen.width / 2 + result.rect.width * 5 / 8,
                                            Screen.height / 2 - timer.rect.height - 1.5F * result.rect.height);
        goToMenu.anchoredPosition = new Vector2(Screen.width / 2 - goToMenu.rect.width,
                                                -Screen.height / 2 + goToMenu.rect.height);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void createInitialBoard (string lastLevel) {
        if (lastLevel.Equals("_Easy")) {
            Instantiate(fourboard);
            Instantiate(fourslots);
        } else if (lastLevel.Equals("_Medium")) {
            Instantiate(fiveboard);
            Instantiate(fiveslots);
        } else if (lastLevel.Equals("_Hard")) {
            Instantiate(sixboard);
            Instantiate(sixslots);
        }
    }
}
