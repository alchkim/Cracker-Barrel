using UnityEngine;
using System.Collections;

public class MouseOver : MonoBehaviour {
    Color origColor;

    void OnMouseEnter () {
        origColor = gameObject.GetComponent<MeshRenderer>().material.color;
    }

    void OnMouseOver () {
        if (isValid()) {
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(origColor.r + 155,
                                                                                origColor.g + 155,
                                                                                origColor.b + 155);
        }
    }

    void OnMouseExit () {
        gameObject.GetComponent<MeshRenderer>().material.color = origColor;
    }

    void OnMouseClick () {
        print(gameObject.transform.parent.name);
    }

    public bool isValid () {
        Neighbors curNode = gameObject.GetComponentInParent<Neighbors>();
        for (int i=0; i<6; i++) {
            int name = curNode.neighbors[i*2];
            if (name != -1) {
                if (curNode.neighbors[i*2+1] == 1) {
                    if (GameObject.Find(name.ToString()).GetComponent<Neighbors>().nearBlank((i))) {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
