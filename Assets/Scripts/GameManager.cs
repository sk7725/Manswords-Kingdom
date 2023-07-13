using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager main;

    public ControlPoint control;
    public Player player;
    public FocusManager focus;

    public float reloadMultiplier = 2.5f;

    public double time = 0f;
    public int wave = 0;
    public float waveInterval = 10f;

    public EnemySpawnData[] enemyData = { };
    public List<EnemySpawnData> standbyEnemies = new List<EnemySpawnData>();
    public List<EnemySpawnData> enemyPool = new List<EnemySpawnData>();
    public Transform[] spawnPoints = { };

    public GameObject summonFx, gameOverPanel;
    [SerializeField] private GameObject swordDeathFx;
    [SerializeField] private float deathDelay = 0.9f;

    private float waveStandby = 0;

    private void Awake() {
        main = this;
        Time.timeScale = 1f;
        time = 0f;
        standbyEnemies.AddRange(enemyData);
        waveStandby = waveInterval;
    }

    private void Update() {
        time += Time.deltaTime;
        waveStandby -= Time.deltaTime;

        if(time > 60f) {
            reloadMultiplier -= 1 / 300f * Time.deltaTime;
            if(reloadMultiplier < 1f) reloadMultiplier = 1f;
        }

        for (int i = 0; i < standbyEnemies.Count; i++) {
            if (standbyEnemies[i].firstRelease <= time) {
                enemyPool.Add(standbyEnemies[i]);
                standbyEnemies.RemoveAt(i);
                i--;
            }
        }

        if(waveStandby <= 0) {
            waveStandby = waveInterval;
            StartWave();
        }
    }

    public void StartWave() {
        StartCoroutine(IStartWave());
    }

    WaitForSeconds _waiter = new WaitForSeconds(0.15f);

    IEnumerator IStartWave() {
        int n = WaveSpawns();
        for(int i = 0; i < n; i++) {
            var enemy = enemyPool[Random.Range(0, enemyPool.Count)];
            Transform pos = spawnPoints[Random.Range(0, spawnPoints.Length)];

            enemy.Summon(time, pos.transform.position);
            yield return _waiter;
        }
        wave++;
    }

    private int WaveSpawns() {
        int init = time > 60f ? (time > 120f ? 3 : 2) : 1;
        if (wave % 30 == 0) {
            return init + 4;
        }
        if (wave % 10 == 0) {
            return init + 2;
        }
        else if(wave % 5 == 0){
            return init + 1;
        }
        return init;
    }

    public void GameOver() {
        focus.enabled = false;
        Time.timeScale = 0f;
        control.gameObject.SetActive(false);
        StartCoroutine(IGameOver());
    }

    IEnumerator IGameOver() {
        yield return new WaitForSecondsRealtime(deathDelay);
        Fx.Play(swordDeathFx, control.sword.transform.position);
        control.sword.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        End();
    }

    public void End() {
        gameOverPanel.SetActive(true);
    }
}
