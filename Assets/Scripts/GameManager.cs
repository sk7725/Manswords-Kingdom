using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager main;

    public ControlPoint control;
    public Player player;

    [SerializeField] private GameObject swordDeathFx;
    [SerializeField] private float deathDelay = 0.9f;

    private void Awake() {
        main = this;
        Time.timeScale = 1f;
    }

    public void GameOver() {
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
        //todo show gameover UI
    }
}
