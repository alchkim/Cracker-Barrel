using UnityEngine;
using System.Collections;

public class EmptySlots : MonoBehaviour {
    public GameLogic manager;
    public int curSpot;
    MeshRenderer rend;

    void Start () {
        rend = gameObject.transform.GetComponent<MeshRenderer>();
    }

    void OnMouseOver () {
        if (manager.isValidMove(curSpot)) {
            rend.enabled = true;
        }
    }

    void OnMouseExit () {
        rend.enabled = false;
    }

    void OnMouseUp () {

    }
}
