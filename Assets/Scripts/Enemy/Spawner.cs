using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour {
    [System.Serializable]
    public struct SpawnData {
        public GameObject prefab;
        public int shots;
        public float spread;
        public float interval;
        public float startDelay;
        //if this field exists, prefab is ignored
        public NestedSpawnData[] subemitters;
    }

    [System.Serializable]
    public struct NestedSpawnData {
        public GameObject prefab;
        public int shots;
        public float spread;
        public float interval;
        public float startDelay;
    }

    public SpawnData[] emitter = { };
    public float reloadTime = 3f;
    public bool repeat = true;
    public bool destroyOnStop = false;

    public SpawnEvent onSpawnStart = new();

    [System.Serializable]
    public class SpawnEvent : UnityEvent { }

    private float _reload;

    private void Start() {
        Spawn(emitter, transform.rotation.eulerAngles.z);
        if (!repeat) {
            if(destroyOnStop) Destroy(gameObject);
            else enabled = false;
        }
        else _reload = reloadTime;
    }

    private void Update() {
        _reload -= Time.deltaTime;
        if (_reload <= 0) {
            Spawn(emitter, transform.rotation.eulerAngles.z);
            _reload = reloadTime;
        }
    }

    public void Spawn(SpawnData[] emitter, float angleOffset) {
        onSpawnStart.Invoke();
        StartCoroutine(ISpawn(emitter, angleOffset));
    }

    IEnumerator ISpawn(SpawnData[] emitter, float angleOffset) {
        for (int i = 0; i < emitter.Length; i++) {
            var e = emitter[i];
            if(e.startDelay > 0.01f) {
                yield return new WaitForSeconds(e.startDelay);
            }

            for (int j = 0; j < e.shots; j++) {
                if (e.subemitters != null && e.subemitters.Length > 0) {
                    Spawn(e.subemitters, EmitAngle(e.shots, e.spread, j) + angleOffset);
                }
                else {
                    SpawnSingle(e.prefab, EmitAngle(e.shots, e.spread, j) + angleOffset);
                }

                if (e.interval > 0.01f) {
                    yield return new WaitForSeconds(e.interval);
                }
            }
        }
    }

    public void Spawn(NestedSpawnData[] emitter, float angleOffset) {
        StartCoroutine(ISpawnNested(emitter, angleOffset));
    }

    public void Stop() {
        if (destroyOnStop) Destroy(gameObject);
        else enabled = false;
    }

    public void Play() {
        enabled = true;
        Spawn(emitter, transform.rotation.eulerAngles.z);
        if (!repeat) {
            if (destroyOnStop) Destroy(gameObject);
            else enabled = false;
        }
        else _reload = reloadTime;
    }

    IEnumerator ISpawnNested(NestedSpawnData[] emitter, float angleOffset) {
        for (int i = 0; i < emitter.Length; i++) {
            var e = emitter[i];
            if (e.startDelay > 0.01f) {
                yield return new WaitForSeconds(e.startDelay);
            }

            for (int j = 0; j < e.shots; j++) {
                SpawnSingle(e.prefab, EmitAngle(e.shots, e.spread, j) + angleOffset);

                if (e.interval > 0.01f) {
                    yield return new WaitForSeconds(e.interval);
                }
            }
        }
    }

    private float EmitAngle(int shots, float spread, int i) {
        float c = shots / 2f - 0.5f;
        return (i - c) * spread;
    }

    private void SpawnSingle(GameObject prefab, float angle) {
        Instantiate(prefab, transform.position, Quaternion.Euler(0, 0, angle));
    }
}
