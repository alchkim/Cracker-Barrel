using UnityEngine;
using System.Collections;

public class Peg : MonoBehaviour {
    public static bool somethingMoving = false;
    enum State { Grounded, GtoM, MtoG, MidPoint, MtoH, HtoM, Held };

    State status;
    Transform curSlotObj;
    Slot curSlot;

    Color origColor;
    public GameLogic manager;
    public int curSpot;
    int destSpot;
    public float speed;
    float startTime;
    float journeyLength;

    // Use this for initialization
    void Start () {
        status = State.Grounded;
    }

    void Update() {
        switch (status) {
            case State.GtoM:
                movement(curSlot.slotPos, curSlot.midPoint);
                if (gameObject.transform.position == curSlot.midPoint) {
                    status = State.MtoH;
                    startTime = Time.time;
                }
                break;
            case State.MtoH:
                movement(curSlot.midPoint, GameLogic.heldPos);
                if (gameObject.transform.position == GameLogic.heldPos) {
                    status = State.Held;
                    GameLogic.somethingMoving = false;
                }
                break;
            case State.HtoM:
                movement(GameLogic.heldPos, curSlot.midPoint);
                if (gameObject.transform.position == curSlot.midPoint) {
                    status = State.MtoG;
                    startTime = Time.time;
                }
                break;
            case State.MtoG:
                movement(curSlot.midPoint, curSlot.slotPos);
                if (gameObject.transform.position == curSlot.slotPos) {
                    status = State.Grounded;
                    GameLogic.somethingMoving = false;
                }
                break;
            default:    //Grounded, Midpoint, Held states should all be here
                break;
        }
    }

    public void setSlot (Transform slot) {
        curSlotObj = slot;
        curSlot = curSlotObj.GetComponent<Slot>();
    }

    //When mouse is entered on selected peg, the peg's color is changed to original
    //color of peg.
    void OnMouseEnter() {
        origColor = gameObject.GetComponent<MeshRenderer>().material.color;
    }

    //Highlights peg if moused over peg is a valid selection, nothing is being held,
    //and nothing is moving.
    void OnMouseOver() {
        if (isValidPeg() && !manager.isHolding() && !GameLogic.somethingMoving) {
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(origColor.r + 155,
                                                                                origColor.g + 155,
                                                                                origColor.b + 155);
        }
    }

    //When mouse is no longer hovering over peg, color of the peg is changed to
    //original color
    void OnMouseExit() {
        gameObject.GetComponent<MeshRenderer>().material.color = origColor;
    }

    //When mouse click is released, current peg is to be held if possible.
    void OnMouseUp() {
        if (isValidPeg() && !manager.isHolding()) {
            moveToHold();
        }
    }

    //Checks if this peg can be picken up
    public bool isValidPeg() {
        Neighbors curNode = gameObject.GetComponentInParent<Neighbors>();
        for (int i = 0; i < 6; i++) {
            int name = curNode.neighbors[i * 2];
            if (name != -1) {
                if (curNode.neighbors[i * 2 + 1] == 1) {
                    if (GameObject.Find(name.ToString()).GetComponent<Neighbors>().nearBlank((i))) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    //Returns an int array that contains all the valid slots current peg can move to
    public int[] showValidMoves() {
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

    //Starts moving peg to holding slot
    public void moveToHold() {
        startTime = Time.time;
        journeyLength = Vector3.Distance(curSlot.slotPos, curSlot.midPoint) + Vector3.Distance(curSlot.midPoint, GameLogic.heldPos);
        manager.holdPeg(curSlot.slotNum);
        manager.setValidMoves(showValidMoves());
        GameLogic.somethingMoving = true;
        status = State.GtoM;
    }

    //Starts moving peg from held position to destSlot
    public void moveToPeg(Transform destSlotObj) {
        Slot destSlot = destSlotObj.GetComponent<Slot>();
        startTime = Time.time;
        journeyLength = Vector3.Distance(GameLogic.heldPos, curSlot.midPoint) + Vector3.Distance(curSlot.midPoint, destSlot.slotPos);
        if (curSlot.slotNum != destSlot.slotNum) {
            Neighbors start, end;
            start = curSlotObj.GetComponent<Neighbors>();
            //Updates original slot
            manager.GetComponent<PegGenerator>().tellNeighbors(start, 0);
            gameObject.transform.SetParent(destSlotObj);
            setSlot(destSlotObj);
            //Updates new slot
            end = destSlotObj.GetComponent<Neighbors>();
            manager.GetComponent<PegGenerator>().tellNeighbors(end, 1);
            //Removes peg in between slots
            Debug.Log(start);
            Neighbors middle = GameObject.Find(end.inMiddle(start).ToString()).GetComponent<Neighbors>();
            delete(middle);
        }
        //        manager.tw.Write("M " + manager.holdingWhat() + " " + dest + " " + manager.timer.GetComponent<Text>().text + "\r\n");
        manager.holdPeg(-1);
        manager.setValidMoves(GameLogic.noValidMoves);
        GameLogic.somethingMoving = true;
        status = State.HtoM;
    }

    //Moves the peg from one position to another position.
    //Takes in two vector3, starting position pA and ending position pB.
    void movement(Vector3 pA, Vector3 pB) {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        gameObject.transform.position = Vector3.Lerp(pA, pB, fracJourney);
    }

    //Deletes peg at current slot and updates surrounding slots' neighbors
    void delete(Neighbors target) {
        Transform targetPeg = target.transform.GetChild(2).transform;
//        manager.tw.Write("R " + target.name + " " + manager.timer.GetComponent<Text>().text + "\r\n");
        manager.numPegs.Remove(targetPeg);
        Destroy(targetPeg.gameObject);
        manager.GetComponent<PegGenerator>().tellNeighbors(target, 0);
    }
}
