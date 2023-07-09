using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepToPlayer : MonoBehaviour {
    [SerializeField] private Enemy self;
    public float speed = 1f;

    private void FixedUpdate() {
        self.move = self.transform.right * speed;
    }
}
