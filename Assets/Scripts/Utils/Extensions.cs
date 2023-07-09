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

    public static string ToTimeString(this double time) {
        if (time < 0) return "--:--.---";
        int hours = (int)(time / 60 / 60);
        return hours > 0 ? string.Format("{0}:{1,2:D2}:{2,2:D2}.{3,3:D3}", hours, (int)(time / 60 % 60), (int)(time % 60), (int)(time * 1000 % 1000))
            : string.Format("{0}:{1,2:D2}.{2,3:D3}", (int)(time / 60 % 60), (int)(time % 60), (int)(time * 1000 % 1000));
    }

    public static string ToTimeString(this float time) {
        if (time < 0) return "--:--.---";
        int hours = (int)(time / 60 / 60);
        return hours > 0 ? string.Format("{0}:{1,2:D2}:{2,2:D2}.{3,3:D3}", hours, (int)(time / 60 % 60), (int)(time % 60), (int)(time * 1000 % 1000))
            : string.Format("{0}:{1,2:D2}.{2,3:D3}", (int)(time / 60 % 60), (int)(time % 60), (int)(time * 1000 % 1000));
    }
}
