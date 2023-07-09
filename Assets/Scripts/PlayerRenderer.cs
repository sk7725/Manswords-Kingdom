using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour {
    public SpriteRenderer sprite;
    public SpriteRenderer face;
    public PlayerBlinker blinker;

    [SerializeField] private Sprite[] faces = { };
    [SerializeField] private Color hurtColor = Color.red;
    [SerializeField] private int hurtFaceID;

    private float faceDuration = -1;

    private void Start() {
        SetFace(0);
    }

    private void Update() {
        if(faceDuration > 0) {
            faceDuration -= Time.deltaTime;
            if(faceDuration <= 0) {
                faceDuration = -1;
                SetFace(0);
            }
        }
        else {
            if(Random.Range(0f, 1f) < 0.003f) {
                //blink
                SetFaceFor(1, 0.1f);
            }
            else {
                if (GameManager.main.player.HealthFraction < 0.15f) SetFace(2);
                else SetFace(0);
            }
        }
    }

    public void SetFace(int id) {
        face.sprite = faces[id];
    }

    public void SetFaceFor(int id, float duration) {
        faceDuration = duration;
        SetFace(id);
    }

    public void Hurt() {
        blinker.Color = hurtColor;
        blinker.Blink();
        SetFaceFor(hurtFaceID, 1f);
    }
}
