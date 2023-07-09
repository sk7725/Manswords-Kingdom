using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinUpdater : MonoBehaviour {
    public float speed = 60f;

    void Update() {
        transform.localRotation *= Quaternion.Euler(0, 0, speed * Time.deltaTime);
    }
}
