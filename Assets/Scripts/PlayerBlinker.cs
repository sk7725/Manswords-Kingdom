using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlinker : MonoBehaviour
{
    public SpriteRenderer outline;
    public Color Color {
        get {
            return outline.color;
        }
        set {
            outline.color = value;
        }
    }

    private void Start() {
        outline.enabled = false;
    }

    public void Blink() {
        StopAllCoroutines();
        StartCoroutine(IBlink());
    }

    public void Stop() {
        StopAllCoroutines();
        outline.enabled = false;
    }

    private WaitForSeconds _waiter = new WaitForSeconds(0.1f);
    IEnumerator IBlink() {
        for (int i = 0; i < 3; i++) {
            outline.enabled = true;
            yield return _waiter;
            outline.enabled = false;
            yield return _waiter;
        }
    }
}
