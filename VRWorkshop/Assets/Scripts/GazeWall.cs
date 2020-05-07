using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GazeWall : MonoBehaviour
{
    public Text text;
    // Start is called before the first frame update
    public void OnGazeEnter() {
        text.text = "Looking At";
    }

    public void OnGazeExit() {
        text.text = "";
    }

}
