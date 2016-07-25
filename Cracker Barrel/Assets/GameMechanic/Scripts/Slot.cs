using UnityEngine;
using System.Collections;

public class Slot : MonoBehaviour {
    public Vector3 slotPos;             //Better
    public Vector3 midPoint;            //Better
    public int slotNum;

	// Use this for initialization
	void Start () {
        slotPos = gameObject.transform.position;
        midPoint = slotPos + new Vector3(0, 2, 0);
        slotNum = int.Parse(gameObject.transform.name);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
