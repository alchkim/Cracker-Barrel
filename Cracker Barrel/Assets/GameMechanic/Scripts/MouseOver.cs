using UnityEngine;
using System.Collections;

public class MouseOver : MonoBehaviour {
    Color origColor;
    bool clickable = true;
    public Transform startMarker;
    public Transform endMarker;
    public Transform holdingMarker;
    public GameLogic manager;
    public int curSpot;

    void OnMouseEnter () {
        origColor = gameObject.GetComponent<MeshRenderer>().material.color;
    }

    void OnMouseOver () {
        if (isValidPeg() && (!manager.isHolding() || (manager.holdingWhat() == curSpot))) {
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(origColor.r + 155,
                                                                                origColor.g + 155,
                                                                                origColor.b + 155);
        }
    }

    void OnMouseExit () {
        gameObject.GetComponent<MeshRenderer>().material.color = origColor;
    }

    void OnMouseUp () {
        if (isValidPeg()) {
            if (!manager.isHolding()) {
                manager.holdPeg(curSpot);
                moveToHolding();
            } else if (manager.holdingWhat() == curSpot) {
                manager.holdPeg(-1);
                moveBack();
            }
        }
    }

    //Checks if this peg can be picken up
    public bool isValidPeg () {
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

    public int[] showValidMoves () {
        Neighbors curNode = gameObject.GetComponentInParent<Neighbors>();
        int[] validMoves = new int[6];
        for (int i = 0; i < 6; i++) {
            int name = curNode.neighbors[i * 2];
            if (name != -1) {
                if (curNode.neighbors[i * 2 + 1] == 1) {
                    Neighbors adjacent = GameObject.Find(name.ToString()).GetComponent<Neighbors>();
                    if (adjacent.nearBlank((i))) {
                        validMoves[i] = adjacent.returnAcross(i);
                    } else {
                        validMoves[i] = -1;
                    }
                }
            }
        }
        return validMoves;
    }

    bool part1 = false;
    bool part2 = false;
    float speed = 2.0F;
    float startTime;
    float journeyLength;

    void moveToHolding () {
        part1 = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        manager.setValidMoves(showValidMoves());
    }

    void moveBack() {
        part1 = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(holdingMarker.position, endMarker.position);
        manager.setValidMoves(new int[] { -1, -1, -1, -1, -1, -1 });
    }

    void Update () {
        if (part1) {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            gameObject.transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);
            if (gameObject.transform.position == endMarker.position) {
                part1 = false;
                part2 = true;
                startTime = Time.time;
            }
        }
        if (part2) {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            gameObject.transform.position = Vector3.Lerp(endMarker.position, holdingMarker.position, fracJourney);
            if (gameObject.transform.position == holdingMarker.position) {
                part2 = false;
                Transform temp = startMarker;
                startMarker = holdingMarker;
                holdingMarker = temp;
            }
        }
    }
}
