using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {
    int held = -1;
    public int[] validMoves;

    public bool isHolding () {
        if (held != -1) {
            return true;
        } else {
            return false;
        }
    }

    public void holdPeg (int name) {
        held = name;
    }

    public int holdingWhat () {
        return held;
    }

    public void setValidMoves (int[] curValidMoves) {
        validMoves = curValidMoves;
    }

    public bool isValidMove (int dest) {
        for (int i=0; i<6; i++) {
            if (validMoves[i] == dest) {
                return true;
            }
        }
        return false;
    }
}
