using UnityEngine;
using System.Collections;

public class GenerateMenu : MonoBehaviour {
    public GameObject easy;
    public GameObject medium;
    public GameObject hard;
    public GameObject text;
    public GameObject canvas;

    RectTransform canvasRect;
    RectTransform buttonRect;

    // Use this for initialization
    void Start () {
        canvasRect = canvas.GetComponent<RectTransform>();
        buttonRect = easy.GetComponent<RectTransform>();

        float buttonX = 0;

        //Placed text
        float buttonY = 0 + (buttonRect.rect.height*2);
        place(buttonX, buttonY, text);

        //Placed easy
        buttonY = 0 + (buttonRect.rect.height);
        createButton(buttonX, buttonY, easy);

        //Placed medium
        buttonY = 0;
        createButton(buttonX, buttonY, medium);

        //Placed hard
        buttonY = 0 - (buttonRect.rect.height);
        createButton(buttonX, buttonY, hard);
    }

    //Places UI component based on buttonX and buttonY position
    void place (float buttonX, float buttonY, GameObject ui) {
        ui.GetComponent<RectTransform>().anchoredPosition = new Vector2(buttonX, buttonY);
    }

    //Creates a new button and places it based on buttonX and buttonY position
    void createButton (float buttonX, float buttonY, GameObject button) {
        GameObject newButton = Instantiate(button);
        newButton.transform.SetParent(canvas.transform);
        place(buttonX, buttonY, newButton);
    }
}
