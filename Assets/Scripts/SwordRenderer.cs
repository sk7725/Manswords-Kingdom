using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordRenderer : MonoBehaviour {
    public SpriteRenderer sprite;
    public SpriteRenderer outline;
    public SpriteRenderer arrow;

    private void Start() {
        outline.color = Color.black;
    }

    private void Update() {
        switch (GameManager.main.control.State) {
            case ControlPoint.ControlState.Idle:
                outline.color = Color.black;
                break;
            case ControlPoint.ControlState.Drag:
                outline.color = GameManager.main.control.dragColor;
                break;
            case ControlPoint.ControlState.Shift:
                outline.color = GameManager.main.control.shiftColor;
                break;
            case ControlPoint.ControlState.Focus:
                outline.color = GameManager.main.control.focusColor;
                break;
        }
    }
}
