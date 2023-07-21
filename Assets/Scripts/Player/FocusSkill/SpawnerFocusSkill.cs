using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Spawner;


[CreateAssetMenu(fileName = "NewFocusSkill", menuName = "Focus Skill/Spawner Skill", order = 141)]
public class SpawnerFocusSkill : FocusSkill {
    public enum ComboFloatMode {
        None,
        Linear,
        Inverse,
        Pow
    }

    [System.Serializable]
    public struct ComboFloat {
        public ComboFloatMode mode;
        public float scale;
        public float baseValue;
        public float maxValue;

        public float Value(int combo) {
            switch (mode) {
                case ComboFloatMode.Linear:
                    return Mathf.Min(baseValue + combo * scale, maxValue);
                case ComboFloatMode.Inverse:
                    return Mathf.Min(baseValue + scale / combo, maxValue);
                case ComboFloatMode.Pow:
                    return Mathf.Min(baseValue * Mathf.Pow(scale, combo), maxValue);
                default:
                    return baseValue;
            }
        }
    }

    [System.Serializable]
    public struct ComboInt {
        public float scale;
        public float baseValue;
        public int maxValue;

        public int Value(int combo) {
            return Mathf.Min((int)(baseValue + combo * scale), maxValue);
        }
    }

    [System.Serializable]
    public struct ComboSpawnData {
        public GameObject prefab;
        public ComboInt shots;
        public ComboFloat spread;
        public ComboFloat interval;
        public ComboFloat startDelay;
        //if this field exists, prefab is ignored
        public NestedSpawnData[] subemitters;
    }

    public ComboSpawnData[] emitter = { };
    public bool overrideRotation = false;
    public float overrideRotationValue = 0f;
    public float spiral = 0f;

    private SpawnData[] spawnData;

    public override void Init() {
        InitSpawnData();
    }

    private void InitSpawnData() {
        spawnData = new SpawnData[emitter.Length];
    }

    private void PrepareSpawnData(int combo) {
        for (int i = 0; i < spawnData.Length; i++) {
            var sp = new SpawnData();
            var osp = emitter[i];

            sp.prefab = osp.prefab;
            sp.shots = osp.shots.Value(combo);
            sp.spread = osp.spread.Value(combo);
            sp.interval = osp.interval.Value(combo);
            sp.startDelay = osp.startDelay.Value(combo);
            sp.subemitters = osp.subemitters;
            spawnData[i] = sp;
        }
    }

    protected override void Invoke(int combo) {
        PrepareSpawnData(combo);
        Spawner spawner = GameManager.main.control.sword.spawner;

        spawner.emitter = spawnData;
        spawner.overrideRotation = overrideRotation;
        spawner.overrideRotationValue = overrideRotationValue;
        spawner.spiral = spiral;

        spawner.Play();
    }
}
