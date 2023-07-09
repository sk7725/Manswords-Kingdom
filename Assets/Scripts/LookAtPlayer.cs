using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {
    void Update() {
        transform.right = GameManager.main.player.transform.position - transform.position;
    }
}
