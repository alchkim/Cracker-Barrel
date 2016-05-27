using UnityEngine;
using System.Collections;

public class EmptySlots : MonoBehaviour {
    public GameLogic manager;
    public int curSpot;
    MeshRenderer rend;

    void Start () {
        rend = gameObject.transform.GetComponent<MeshRenderer>();
    }

    //Turns on renderer if slot is a valid move for current peg
    void OnMouseOver () {
        if (manager.isValidMove(curSpot)) {
            rend.enabled = true;
        }
    }

    //Turns of renderer
    void OnMouseExit () {
        rend.enabled = false;
    }

    //Moves peg to the current slot from the previous slot it was in
    void OnMouseUp () {
        Transform curPegHeld = GameObject.Find(manager.holdingWhat().ToString()).transform.GetChild(2);
        curPegHeld.GetComponent<MouseOver>().moveBack();
        curPegHeld.GetComponent<MouseOver>().moveTo(curSpot);
        rend.enabled = false;
    }
}
