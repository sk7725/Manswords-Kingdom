using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy Spawn Data", order = 140)]
public class EnemySpawnData : ScriptableObject {
    public GameObject enemy;
    public GameObject elite;
    public float firstRelease = 30f;
    public float eliteChance = 0.05f;
    public float eliteChanceMax = 0.2f;

    public float EliteChance(double time) {
        return Mathf.Min((float)(time - firstRelease) * (eliteChance / 60f), eliteChanceMax);
    }

    public GameObject Summon(double time, Vector3 pos) {
        var go = enemy;
        if (Random.Range(0f, 1f) < EliteChance(time)) {
            go = elite;
        }

        var o = Instantiate(go, pos, Quaternion.identity);
        o.PlayFx(GameManager.main.summonFx);
        return o;
    }
}
