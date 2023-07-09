using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipper : MonoBehaviour {
    [SerializeField] private SpriteRenderer sprite;

    private float lastAngle = 0f;

    private void Start() {
        if(Mathf.Abs(Mathf.DeltaAngle(transform.parent.rotation.eulerAngles.z, 0f)) > 100f) {
            lastAngle = 180f;
            sprite.flipX = true;
        }
    }

    private void Update() {
        transform.rotation = Quaternion.identity;
        if(Mathf.Abs(Mathf.DeltaAngle(transform.parent.rotation.eulerAngles.z, lastAngle)) > 100f) {
            if(lastAngle == 0f) {
                lastAngle = 180f;
                sprite.flipX = true;
            }
            else {
                lastAngle = 0f;
                sprite.flipX = false;
            }
        }
    }
}
