using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Fx {
    public static GameObject Play(GameObject prefab, Vector3 position) {
        return Play(prefab, position, Quaternion.identity);
    }

    public static GameObject Play(GameObject prefab, Vector3 position, Quaternion rotation) {
        if (prefab == null) return null;
        return GameObject.Instantiate(prefab, position, rotation);
    }

    public static GameObject PlayAsChild(GameObject prefab, Transform parent) {
        if (prefab == null) return null;
        return GameObject.Instantiate(prefab, parent);
    }
}
