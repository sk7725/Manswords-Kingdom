using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AllyProjectile : MonoBehaviour {
    public Rigidbody2D rigid;

    [SerializeField] private int damage = 10;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float knockback = 5f;
    [SerializeField] private float acceleration = -1f;
    [SerializeField] private int penetrate = 1;

    [Header("Fx")]
    [SerializeField] private GameObject spawnFx;
    [SerializeField] private GameObject despawnFx;

    [Header("Events")]
    [SerializeField] private AllyProjectileDespawnEvent onDespawn = new();

    private float time = 0f;
    private int penetrateCount = 0;

    [System.Serializable]
    public class AllyProjectileDespawnEvent : UnityEvent { }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            Hit(collision.gameObject.transform.GetComponentInParent<Enemy>());
        }
        else if (collision.gameObject.CompareTag("Ground")) {
            HitGround();
        }
    }

    protected virtual void Start() {
        rigid.velocity = new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed, Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed);
        gameObject.PlayFx(spawnFx);
        time = 0f;
    }

    protected virtual void Update() {
        time += Time.deltaTime;
        if (acceleration > 0f) {
            float s = Time.deltaTime * acceleration;
            rigid.velocity += new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * s, Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * s);
        }
        if (time > lifetime) Despawn();
    }

    public virtual void Hit(Enemy enemy) {
        //todo
        penetrateCount++;
        if(penetrateCount >= penetrate) Despawn();
    }

    public virtual void HitGround() {
        Despawn();
    }

    public virtual void Despawn() {
        onDespawn.Invoke();
        gameObject.PlayFx(despawnFx);
        Destroy(gameObject);
    }
}
