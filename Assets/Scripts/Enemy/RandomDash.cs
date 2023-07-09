using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDash : MonoBehaviour {
    [SerializeField] private Enemy self;
    public float speed = 1f;
    public float interval = 0.5f;
    public float reloadTime = 3f;
    public int dashes = 3;
    public float targetplayerChance = 0.33f;

    private float reload = 0f;

    private void Start() {
        reload = reloadTime;
    }

    void Update() {
        reload -= Time.deltaTime;
        if(reload <= 0) {
            StartCoroutine(IDash());
            reload = reloadTime;
        }
    }

    IEnumerator IDash() {
        for (int i = 0; i < dashes; i++) {
            if (!self.Alive) {
                yield break;
            }
            if(Random.Range(0f, 1f) < targetplayerChance) {
                self.vel = (GameManager.main.player.transform.position - transform.position).normalized * speed;
            }
            else {
                self.vel = (Vector2.right * speed).RotateBy(Random.Range(0f, 360f));
            }
            yield return new WaitForSeconds(interval);
        }
    }
}
