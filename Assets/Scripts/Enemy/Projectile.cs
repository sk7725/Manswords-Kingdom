using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour {
    [SerializeField] private Rigidbody2D rigid;

    [SerializeField] private int damage = 10;
    [SerializeField] private float speed = 3f;

    [Header("Fx")]
    [SerializeField] private GameObject spawnFx, despawnFx, hitSwordFx;

    [Header("Events")]
    [SerializeField] private ProjectileDespawnEvent onDespawn;

    public class ProjectileDespawnEvent: UnityEvent { }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            HitPlayer(GameManager.main.player);
        }
        else if (collision.gameObject.CompareTag("Sword")) {
            HitSword(GameManager.main.control.sword);
        }
        else if (collision.gameObject.CompareTag("Ground")) {
            HitGround();
        }
    }

    protected virtual void Start() {
        rigid.velocity = new Vector2(Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed, Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad) * speed);
        gameObject.PlayFx(spawnFx);
    }

    public virtual void HitPlayer(Player player) {
        player.Damage(damage);
        Despawn();
    }

    public virtual void HitSword(Sword sword) {
        sword.Hit();
        gameObject.PlayFx(hitSwordFx);
        Despawn();
    }

    public virtual void HitGround() {
        Despawn();
    }

    public virtual void Despawn() {
        onDespawn.Invoke();
        gameObject.PlayFx(despawnFx);
    }
}
