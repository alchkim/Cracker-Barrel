using UnityEngine;
using System.Collections;

public class MouseOver : MonoBehaviour {
    Color origColor;

    void OnMouseEnter () {
        origColor = gameObject.GetComponent<MeshRenderer>().material.color;
    }

    void OnMouseOver () {
        gameObject.GetComponent<MeshRenderer>().material.color = new Color (origColor.r + 155, 
                                                                            origColor.g + 155, 
                                                                            origColor.b + 155);
    }

    void OnMouseExit () {
        gameObject.GetComponent<MeshRenderer>().material.color = origColor;
    }

    void OnMouseClick () {
        print(gameObject.transform.parent.name);
    }
}
