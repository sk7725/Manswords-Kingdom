using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlPoint : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private float dragPower = 5000;
    [SerializeField] private float snapPower = 10000;
    [SerializeField] private Color dragColor = Color.cyan, snapColor = Color.yellow;

    [Header("Physics")]
    public Rigidbody2D rigid;
    public RelativeJoint2D jointToSword;

    [Header("UI")]
    [SerializeField] private LineRenderer line;

    private Camera cam;
    private bool mouseOverUI = false;
    private ControlState state;

    public ControlState State => state;

    public enum ControlState {
        Idle,
        Drag,
        Snap
    }

    private void Start() {
        cam = Camera.main;
        mouseOverUI = false;
        SetDragPower(0);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            mouseOverUI = EventSystem.current.IsPointerOverGameObject();
        }
        else if (!Input.GetMouseButton(0)) {
            mouseOverUI = false;
        }

        bool mouseDown = IsMouseDown();
        if (mouseDown) {
            if (ShouldSnapPosition()) {
                state = ControlState.Snap;
            }
            else {
                state = ControlState.Drag;
            }
        }
        else {
            state = ControlState.Idle;
        }

        if (mouseDown) {
            rigid.position = CursorPosition();
            line.SetPosition(1, rigid.position);
        }

        switch (state) {
            case ControlState.Idle:
                SetDragPower(0);
                line.enabled = false;
                break;
            case ControlState.Drag:
                SetDragPower(dragPower);
                line.enabled = true;
                SetLineColor(dragColor);
                break;
            case ControlState.Snap:
                SetDragPower(snapPower);
                line.enabled = true;
                SetLineColor(snapColor);
                break;
        }
    }

    private Vector3 CursorPosition() {
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private bool IsMouseDown() {
        return !mouseOverUI && (Input.GetMouseButton(0) || Input.GetMouseButton(1));
    }

    private bool ShouldSnapPosition() {
        return !mouseOverUI && Input.GetMouseButton(1);
    }

    private void SetDragPower(float power) {
        jointToSword.maxForce = power;
    }

    private void SetLineColor(Color color) {
        line.startColor = line.endColor = color;
    }
}
