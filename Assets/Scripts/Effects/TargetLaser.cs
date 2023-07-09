using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLaser : MonoBehaviour {
    [SerializeField] private LineRenderer line;
    [SerializeField] private LayerMask layerMask;

    public Color color1 = Color.red, color2 = Color.yellow;
    public float distance = 50f;
    public float lifetime = 1.5f;
    public float flickerTime = 0.1f;
    public bool alwaysUpdate = true;
    public bool updateRotation = false;

    private static RaycastHit2D[] temp_hit = new RaycastHit2D[2];
    private float time = 0f;
    private Vector3 initialRight;

    void Start() {
        initialRight = transform.right;
        Raycast();
        time = 0f;
    }

    private void Update() {
        time += Time.deltaTime;
        if (time > lifetime) Destroy(gameObject);
        else {
            if(alwaysUpdate) Raycast();
            SetLineColor((int)(time / flickerTime) % 2 == 0 ? color1 : color2);
        }
    }

    private void Raycast() {
        line.SetPosition(0, transform.position);
        if (updateRotation) initialRight = transform.right;
        //TIME TO RAYCAST
        int n = Physics2D.RaycastNonAlloc(transform.position, initialRight, temp_hit, distance, layerMask);
        Vector3 dest;
        if (n > 0) {
            dest = temp_hit[0].point;
        }
        else {
            dest = transform.position + initialRight * distance;
        }
        line.SetPosition(1, dest);
    }

    private void SetLineColor(Color color) {
        //line.colorGradient.colorKeys[line.colorGradient.colorKeys.Length - 1].color = line.endColor = color;
        line.startColor = line.endColor = color;
    }
}
