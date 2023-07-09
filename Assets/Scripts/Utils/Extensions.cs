using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {
    public static Vector2 RotateBy(this Vector2 v, float delta) {
        float d = delta * Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(d) - v.y * Mathf.Sin(d),
            v.x * Mathf.Sin(d) + v.y * Mathf.Cos(d)
        );
    }

    public static Vector2 Rotate2DBy(this Vector3 v, float delta) {
        float d = delta * Mathf.Deg2Rad;
        return new Vector3(
            v.x * Mathf.Cos(d) - v.y * Mathf.Sin(d),
            v.x * Mathf.Sin(d) + v.y * Mathf.Cos(d),
            v.z
        );
    }

    public static GameObject PlayFx(this GameObject go, GameObject fxPrefab) {
        return Fx.Play(fxPrefab, go.transform.position, go.transform.rotation);
    }
}
