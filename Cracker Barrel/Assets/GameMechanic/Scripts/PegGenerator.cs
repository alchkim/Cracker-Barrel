using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PegGenerator : MonoBehaviour {
    public GameObject slotList;
    public GameObject peg;
    public Transform holdingMarker;
    public Vector3 holdPos;
    GameLogic logic;
    Neighbors[] tList;

	// Use this for initialization
	void Start () {
        int random;
        logic = transform.GetComponent<GameLogic>();
        tList = slotList.GetComponentsInChildren<Neighbors>();
        random = Random.Range(0, tList.Length);
        foreach (Neighbors child in tList) {
            if (int.Parse(child.name) != random) {
                createPeg(child.transform);
            }
        }
	}

    //Creates a peg for the slot
    void createPeg (Transform slot) {
        GameObject newPeg = Instantiate(peg);
        newPeg.transform.SetParent(slot);
        newPeg.transform.position = new Vector3(slot.transform.position.x,
                                                slot.transform.position.y,
                                                slot.transform.position.z);
        newPeg.transform.GetComponent<Peg>().setSlot(slot);         //New code to be used
        MouseOver newMouse = newPeg.GetComponent<MouseOver>();
        newMouse.startMarker = slot.GetChild(0);                    //Old code to be deprecated
        newMouse.endMarker = slot.GetChild(1);                      //Old code to be deprecated
        newMouse.holdingMarker = holdingMarker;                     //Old code to be deprecated
        newMouse.slotPos = slot.position;                           //New code to replace transforms
        newMouse.midPoint = slot.position + new Vector3(0, 2, 0);   //New code to replace transforms
        newMouse.heldPos = holdPos;                                 //New code to replace transforms
        newMouse.manager = gameObject.transform.GetComponent<GameLogic>();
        newMouse.curSpot = int.Parse(slot.name);
        Neighbors curSlot = slot.GetComponent<Neighbors>();
        tellNeighbors (curSlot, 1);
        logic.numPegs.Add(newPeg.transform);
    }

    //Update surrounding neighbors
    public void tellNeighbors (Neighbors curSlot, int exist) {
        for (int i=0; i<6; i++) {
            int name = curSlot.neighbors[i*2];
            if (name != -1) {
                GameObject curNeighbor = GameObject.Find(name.ToString());
                curNeighbor.GetComponent<Neighbors>().updateNeighbors((i+3)%6, exist);
            }
        }
    }
}
