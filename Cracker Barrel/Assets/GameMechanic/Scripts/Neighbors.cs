using UnityEngine;
using System.Collections;

public class Neighbors : MonoBehaviour {
    //12 size array with evens as neighbors and 
    //odds as whether peg exists
    //-1 implies edge
    public int[] neighbors;

    //Nodes go from 0-5 clockwise order starting from top left corner
    //exist is essentially a bool
    public void updateNeighbors(int visitorNode, int exist) {
        neighbors[visitorNode*2+1] = exist;
    }
}