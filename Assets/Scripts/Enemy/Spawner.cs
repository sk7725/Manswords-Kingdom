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
    public bool updateRotation = false;

    public float chargeDuration = -1;
    public GameObject chargeFx, chargeFx2;
    public float chargeOffset = 0.5f;

    public ChargeEvent onCharge = new();
    public SpawnEvent onSpawnStart = new();
    public SpawnEndEvent onSpawnEnd = new();

    [System.Serializable]
    public class SpawnEvent : UnityEvent { }
    [System.Serializable]
    public class ChargeEvent : UnityEvent { }
    [System.Serializable]
    public class SpawnEndEvent : UnityEvent { }

    private float _reload, _rotation;
    private GameObject _lastChargeFx1, _lastChargeFx2;

    private void Start() {
        if (!repeat) {
            Spawn(emitter, 0f);
            if (destroyOnStop) Destroy(gameObject);
            else enabled = false;
        }
        else _reload = reloadTime;
    }

    private void Update() {
        _reload -= Time.deltaTime;
        if (_reload <= 0) {
            Spawn(emitter, 0f);
            _reload = reloadTime;
        }
    }

    public void Spawn(SpawnData[] emitter, float angleOffset) {
        StartCoroutine(ISpawn(emitter, angleOffset));
    }

    IEnumerator ISpawn(SpawnData[] emitter, float angleOffset) {
        _rotation = transform.rotation.eulerAngles.z;
        onCharge.Invoke();

        //charge
        if (chargeDuration > 0) {
            _lastChargeFx1 = Fx.PlayAsChild(chargeFx, transform);
            _lastChargeFx2 = Fx.PlayAsChild(chargeFx2, transform);
            if (_lastChargeFx1 != null) _lastChargeFx1.transform.position += transform.right * chargeOffset;
            if (_lastChargeFx2 != null) _lastChargeFx2.transform.position += transform.right * chargeOffset;

            yield return new WaitForSeconds(chargeDuration);
        }

        onSpawnStart.Invoke();

        //shoot
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

        onSpawnEnd.Invoke();
    }

    public void Spawn(NestedSpawnData[] emitter, float angleOffset) {
        StartCoroutine(ISpawnNested(emitter, angleOffset));
    }

    public void Stop() {
        if (destroyOnStop) Destroy(gameObject);
        else {
            if(_lastChargeFx1 != null) Destroy(_lastChargeFx1);
            if(_lastChargeFx2 != null) Destroy(_lastChargeFx2);
            enabled = false;
        }
    }

    public void Play() {
        enabled = true;
        if (!repeat) {
            Spawn(emitter, 0f);
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
        if (updateRotation) _rotation = transform.rotation.eulerAngles.z;
        Instantiate(prefab, transform.position, Quaternion.Euler(0, 0, angle + _rotation));
    }
}
