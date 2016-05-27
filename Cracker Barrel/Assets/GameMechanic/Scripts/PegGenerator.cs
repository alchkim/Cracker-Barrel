using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PegGenerator : MonoBehaviour {
    public GameObject slotList;
    public GameObject peg;
    Transform[] tList;

	// Use this for initialization
	void Start () {
        int random;

        tList = slotList.GetComponentsInChildren<Transform> ();
        random = Random.Range(0, tList.Length);
        foreach (Transform child in tList) {
            if (child.name == "SlotList") {
                continue;
            }
            if (int.Parse(child.name) != random) {
                createPeg(child);
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

        Neighbors curSlot = slot.GetComponent<Neighbors>();
        tellNeighbors (curSlot);
//        Debug.Log(slot.name + ": " + curSlot.neighbors[1] + " " + curSlot.neighbors[3] + " " +
//            curSlot.neighbors[5] + " " + curSlot.neighbors[7] + " " + curSlot.neighbors[9] + " " +
//            curSlot.neighbors[11]);
    }

    //Update surrounding neighbors
    void tellNeighbors (Neighbors curSlot) {
        for (int i=0; i<6; i++) {
            int name = curSlot.neighbors[i*2];
            if(name != -1) {
                GameObject curNeighbor = GameObject.Find(name.ToString());
                curNeighbor.GetComponent<Neighbors>().updateNeighbors((i+3)%6, 1);
            }
        }
    }
}
