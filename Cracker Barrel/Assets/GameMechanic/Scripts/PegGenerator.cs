using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PegGenerator : MonoBehaviour {
    public GameObject slotList;
    public GameObject peg;
    public Transform holdingMarker;
    Neighbors[] tList;

	// Use this for initialization
	void Start () {
        int random;

        tList = slotList.GetComponentsInChildren<Neighbors> ();
        //random = Random.Range(0, tList.Length);
        random = 3;
        foreach (Neighbors child in tList) {
            if (child.name == "SlotList") {
                continue;
            }
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
        MouseOver newMouse = newPeg.GetComponent<MouseOver>();
        newMouse.startMarker = slot.GetChild(0);
        newMouse.endMarker = slot.GetChild(1);
        newMouse.holdingMarker = holdingMarker;
        newMouse.manager = gameObject.transform.GetComponent<GameLogic>();
        newMouse.curSpot = int.Parse(slot.name);
        Neighbors curSlot = slot.GetComponent<Neighbors>();
        tellNeighbors (curSlot, 1);
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
