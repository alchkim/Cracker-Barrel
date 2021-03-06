﻿using UnityEngine;
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

    //Nextby should be form 0-5. Only checks if across from nextby is empty
    public bool nearBlank(int nextby) {
        int name = neighbors[nextby*2];
        if (name != -1) {
            if (neighbors[nextby * 2 + 1] != 1) {
                return true;
            }
        }
        return false;
    }

    //Checks if the neighbor's neighbor across is blank
    public bool neighborNearBlank (int nextby) {
        for (int i=0; i<6; i++) {
            int name = neighbors[i * 2];
            if (name != -1) {
                Neighbors curNeighbor = GameObject.Find(name.ToString()).GetComponent<Neighbors>();
                if (curNeighbor.nearBlank((i+3)%6)) {
                    return true;
                }
            }
        }
        return false;
    }

    //Returns a neighbor based on the number of nextby
    //Nextby should be between 0 inclusive to 6 exclusive
    public int returnAcross (int nextby) {
        return neighbors[nextby * 2];
    }

    //Gets neighbors member variable from the Neighbor of named slot
    public Neighbors getNeighbor (int name) {
        return GameObject.Find(name.ToString()).GetComponent<Neighbors>();
    }

    //Returns the name of the slot between two Neighbors
    //Returns -1 if no neighbor between the two;
    public int inMiddle (Neighbors curr) {
        for (int i=0; i<6; i++) {
            int a = neighbors[i * 2];
            int b = curr.neighbors[(i + 3) % 6 * 2];
            if (a == b && a!= -1 && b != -1) {
                return a;
            }
        }
        return -1;
    }
}