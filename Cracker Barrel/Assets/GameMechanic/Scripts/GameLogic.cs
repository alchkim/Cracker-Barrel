using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameLogic : MonoBehaviour {
    public static bool somethingMoving = false;
    public int held = -1;
    public int[] validMoves;
    public List<Transform> numPegs;
    public RectTransform timer;
    public RectTransform result;
    public RectTransform goToMenu;
    public RectTransform replay;
    public TextWriter tw;
    public float timeRemaining;
    float elapsedTime;
    bool canPlay;
    bool seenOne;

    //Sees if something is being held
    public bool isHolding () {
        if (held != -1) {
            return true;
        } else {
            return false;
        }
    }

    //Holds peg at slot
    public void holdPeg (int name) {
        held = name;
    }

    //Shows peg slot currently held
    public int holdingWhat () {
        return held;
    }

    //Writes down the valid moves one can do with held peg
    public void setValidMoves (int[] curValidMoves) {
        validMoves = curValidMoves;
    }

    //Checks if peg can move to destination slot
    public bool isValidMove (int dest) {
        for (int i=0; i<6; i++) {
            if (validMoves[i] == dest) {
                return true;
            }
        }
        return false;
    }

    //Counts down the time
    void tickDown (float elapsedTime) {
        elapsedTime += Time.deltaTime;
        timeRemaining -= elapsedTime;
        if (timeRemaining > 0) {
            int minutes = (int)(timeRemaining / 60);
            int seconds = (int)(timeRemaining % 60);

            if (seconds < 10) {
                timer.GetComponent<Text>().text = minutes + ":0" + seconds;
            } else {
                timer.GetComponent<Text>().text = minutes + ":" + seconds;
            }
        }
    }

    //Enables post game UI
    void decision(string participant) {
        result.GetComponent<Text>().text = participant;
        held = -2;
        goToMenu.gameObject.SetActive(true);
        replay.gameObject.SetActive(true);
        tw.Close();
    }

    ////Click goes to _Menu
    //public void menuClick () {
    //    SceneManager.LoadScene("_Menu");
    //}

    ////Click goes to _Replay
    //public void replayClick () {
    //    SceneManager.LoadScene("_Replay");
    //}

    //Initializes game UI
    void Start () {
        canPlay = true;
        timer.anchoredPosition = new Vector2(-Screen.width/2 + timer.rect.width * 5/8, 
                                            Screen.height/2 - timer.rect.height);
        result.anchoredPosition = new Vector2(-Screen.width/2 + result.rect.width * 5/8,
                                            Screen.height/2 - timer.rect.height - 1.5F*result.rect.height);
        goToMenu.anchoredPosition = new Vector2(Screen.width/2 - goToMenu.rect.width,
                                                -Screen.height/2 + goToMenu.rect.height);
        replay.anchoredPosition = new Vector2(Screen.width / 2 - goToMenu.rect.width,
                                            -Screen.height / 2 + goToMenu.rect.height + replay.rect.height);
        tw = new StreamWriter(Application.persistentDataPath + @"/replay.txt");
        tw.Write("S ");
        foreach (Transform peg in numPegs) {
            tw.Write(peg.GetComponent<MouseOver>().curSpot + " ");
        }
        tw.Write("\r\n");
        elapsedTime = 0;
    }

    //Checks if number of pegs is equal to one, if so declare winner
    //Checks if valid moves still exist, if not declare loser
    //Ticks down timer, if timer hits 0:00 declares loser
    void Update () {
        seenOne = false;
        if (numPegs.Count == 1) {
            decision("You Win");
        } else {
            tickDown(elapsedTime);
            foreach (Transform peg in numPegs) {
                if (peg.GetComponent<MouseOver>().isValidPeg()) {
                    seenOne = true;
                }
            }
            if (!seenOne || (timeRemaining <= 0)) {
                decision("You Lose");
            }
        }
    }
}
