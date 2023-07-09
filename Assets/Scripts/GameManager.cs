using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager main;

    public ControlPoint control;
    public Player player;

    private void Awake() {
        main = this;
    }
}
