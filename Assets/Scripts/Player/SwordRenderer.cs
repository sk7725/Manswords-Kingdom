using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordRenderer : MonoBehaviour {
    public SpriteRenderer sprite;
    public SpriteRenderer outline;
    public SpriteRenderer arrow;

    [SerializeField] private Material normalMaterial, focusMaterial;

    private void Start() {
        outline.sharedMaterial.color = Color.black;
    }

    private void Update() {
        switch (GameManager.main.control.State) {
            case ControlPoint.ControlState.Idle:
                outline.sharedMaterial.color = Color.black;
                break;
            case ControlPoint.ControlState.Drag:
                outline.sharedMaterial.color = GameManager.main.control.dragColor;
                break;
            case ControlPoint.ControlState.Shift:
                outline.sharedMaterial.color = GameManager.main.control.shiftColor;
                break;
            case ControlPoint.ControlState.Focus:
                outline.sharedMaterial.color = GameManager.main.control.focusColor;
                break;
        }

        if (GameManager.main.control.State == ControlPoint.ControlState.Focus) {
            sprite.sharedMaterial = focusMaterial;
        }
        else {
            sprite.sharedMaterial = normalMaterial;
        }
    }
}
