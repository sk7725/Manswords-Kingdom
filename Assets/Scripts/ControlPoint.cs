using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlPoint : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private float dragPower = 10000;
    [SerializeField] private float shiftPower = 2000;
    [SerializeField] private float focusSpeed = 60f;
    public Color dragColor = Color.cyan, shiftColor = Color.yellow, focusColor = Color.red;

    [Header("Physics")]
    public Rigidbody2D rigid;
    public RelativeJoint2D jointToSword;
    public Sword sword;

    [Header("UI")]
    [SerializeField] private LineRenderer line;

    private Camera cam;
    private bool mouseOverUI = false;
    private ControlState state;

    public ControlState State => state;

    public enum ControlState {
        Idle,
        Drag,
        Shift,
        Focus
    }

    private void Start() {
        cam = Camera.main;
        mouseOverUI = false;

        SetDragPower(0);
        sword.render.arrow.color = focusColor;
        sword.render.arrow.enabled = false;
    }

    void Update() {
        //input
        if (Input.GetMouseButtonDown(0)) {
            mouseOverUI = EventSystem.current.IsPointerOverGameObject();
        }
        else if (!Input.GetMouseButton(0)) {
            mouseOverUI = false;
        }

        //set state
        bool mouseDown = IsMouseDown();
        if (mouseDown) {
            if (IsShiftDown()) {
                state = ControlState.Shift;
            }
            else {
                state = ControlState.Drag;
            }
        }
        else {
            if (IsFocusDown()) {
                state = ControlState.Focus;
            }
            else {
                state = ControlState.Idle;
            }
        }

        //drag
        if (mouseDown) {
            rigid.position = CursorPosition();
            line.SetPosition(1, sword.transform.position);
            line.SetPosition(0, rigid.position);
        }

        //focus
        if(state == ControlState.Focus) {
            float target = Vector2.SignedAngle(Vector2.right, CursorPosition() - sword.transform.position);
            float angle = Mathf.LerpAngle(sword.rigid.rotation, target, focusSpeed * Time.deltaTime);
            //attempt to move player
            Vector3 diff = GameManager.main.player.transform.position - sword.transform.position;
            diff = diff.Rotate2DBy(angle);
            GameManager.main.player.rigid.MovePosition(sword.transform.position + diff);
            GameManager.main.player.rigid.SetRotation(angle);

            //move sword
            sword.rigid.SetRotation(angle);
            sword.render.arrow.enabled = true;
        }
        else {
            sword.render.arrow.enabled = false;
        }

        //drag power and ui
        switch (state) {
            case ControlState.Focus:
            case ControlState.Idle:
                SetDragPower(0);
                line.enabled = false;
                break;
            case ControlState.Drag:
                SetDragPower(dragPower);
                line.enabled = true;
                SetLineColor(dragColor);
                break;
            case ControlState.Shift:
                SetDragPower(shiftPower);
                line.enabled = true;
                SetLineColor(shiftColor);
                break;
        }
    }

    private Vector3 CursorPosition() {
        var wp = cam.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(wp.x, wp.y, 0f);
    }

    private bool IsMouseDown() {
        return !mouseOverUI && Input.GetMouseButton(0);
    }

    private bool IsFocusDown() {
        //return !mouseOverUI && Input.GetMouseButton(1);
        return false;
    }

    private bool IsShiftDown() {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    private void SetDragPower(float power) {
        jointToSword.maxForce = power;
    }

    private void SetLineColor(Color color) {
        line.startColor = line.endColor = color;
    }
}
