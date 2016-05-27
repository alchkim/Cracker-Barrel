using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SlotGenerator : MonoBehaviour {
    public GameObject slot;
    public GameObject parent;
    int loops = 0;

    // Use this for initialization
    void Start () {
        string name = SceneManager.GetActiveScene().name;
        if (name == "_Easy") {
            //generate 4 lines
            loops = 4;
        } else if (name == "_Medium") {
            //generate 5 lines
            loops = 5;
        } else if (name == "_Hard") {
            //generate 6 lines
            loops = 6;
        }

        createSlot (loops);
    }
	
    //Creates an empty slot and assigns it a location
    void createSlot (int loops) {
        int count = 0;
        for (int i=0, num=1; i<loops; i++, num++) {
            for(int j=0; j<num; j++) {
                GameObject newSlot = Instantiate(slot);
                newSlot.name = count.ToString();
                newSlot.transform.SetParent(parent.transform);
                count++;
            }
        }
        //No location assigned atm
    }
}
