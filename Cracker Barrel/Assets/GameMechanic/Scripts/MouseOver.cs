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
    int destSpot;

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
            } else {
                validMoves[i] = name;
            }
        }
        return validMoves;
    }

    bool part1 = false;
    bool part2 = false;
    bool part3 = false;
    bool part4 = false;
    bool part5 = false;
    float speed = 2.0F;
    float startTime;
    float journeyLength;

    public void moveToHolding () {
        part1 = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        manager.setValidMoves(showValidMoves());
    }

    public void moveBack () {
        part1 = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(holdingMarker.position, endMarker.position);
        manager.setValidMoves(new int[] { -1, -1, -1, -1, -1, -1 });
    }

    public void moveTo (int dest) {
        part3 = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        manager.holdPeg(-1);
        destSpot = dest;
    }

    void Update () {
        if (part1) {
            movement(startMarker, endMarker);
            if (gameObject.transform.position == endMarker.position) {
                part1 = false;
                part2 = true;
                startTime = Time.time;
            }
        } else if (part2) {
            movement(endMarker, holdingMarker);
            if (gameObject.transform.position == holdingMarker.position) {
                part2 = false;
                Transform temp = startMarker;
                startMarker = holdingMarker;
                holdingMarker = temp;
                startTime = Time.time;
            }
        } else if (part3) {
            movement(startMarker, endMarker);
            if (gameObject.transform.position == endMarker.position) {
                part3 = false;
                part4 = true;
                startTime = Time.time;
            }
        } else if (part4) {
            startMarker = endMarker;
            endMarker = GameObject.Find(destSpot.ToString()).transform.GetChild(1);
            movement(startMarker, endMarker);
            if (gameObject.transform.position == endMarker.position) {
                part4 = false;
                part5 = true;
                startTime = Time.time;
            }
        } else if (part5) {
            startMarker = GameObject.Find(destSpot.ToString()).transform.GetChild(0);
            movement(endMarker, startMarker);
            if (gameObject.transform.position == startMarker.position) {
                part5 = false;
                Neighbors curNeighbors, start;
                start = GameObject.Find(curSpot.ToString()).GetComponent<Neighbors>();
                curNeighbors = start;

                manager.GetComponent<PegGenerator>().tellNeighbors(curNeighbors, 0);
                gameObject.transform.SetParent(GameObject.Find(destSpot.ToString()).transform);
                curSpot = destSpot;

                curNeighbors = GameObject.Find(curSpot.ToString()).GetComponent<Neighbors>();
                manager.GetComponent<PegGenerator>().tellNeighbors(curNeighbors, 1);

                Neighbors middle = GameObject.Find(curNeighbors.inMiddle(start).ToString()).GetComponent<Neighbors>();
                delete(middle);
            }
        }
    }

    void movement (Transform pA, Transform pB) {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        gameObject.transform.position = Vector3.Lerp(pA.position, pB.position, fracJourney);
    }

    //Deletes peg at current slot and updates surrounding slots' neighbors
    void delete (Neighbors target) {
        Transform targetPeg = target.transform.GetChild(2).transform;
        manager.numPegs.Remove(targetPeg);
        Destroy(targetPeg.gameObject);
        manager.GetComponent<PegGenerator>().tellNeighbors(target, 0);
    }
}
