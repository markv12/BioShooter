using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public static bool gameRunning = false;
    public CanvasGroup BlackCurtain;
    public TMP_Text deathText;
    private static string lastDeathMessage;

    public Player player;
    public Transform playerT;

    private static bool firstGame = true;

	void Awake () {
        deathText.text = lastDeathMessage;
        instance = this;
        playerT = player.transform;
        if (!firstGame) {
            StartCoroutine(FadeToGame());
        } else {
            firstGame = false;
            gameRunning = true;
        }
	}

    void Update() {
        if(playerT.position.y <= -7) {
            RestartGame();
        }

        if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKeyDown(KeyCode.Q)) {
            QuitGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            QuitGame();
        }
    }

    public void RestartGame() {
        instance.StartCoroutine(instance.RestartGame("Dead!"));
    }

    private static void QuitGame() {
#if UNITY_EDITOR
        if (Application.isEditor) {
            UnityEditor.EditorApplication.isPlaying = false;
        } else
#endif
            Application.Quit();
    }

    private static readonly WaitForSeconds wait = new WaitForSeconds(0.3f);
    private const float FadeTime = 0.4f;
    private IEnumerator RestartGame(string deathMessage) {
        lastDeathMessage = deathMessage;
        deathText.text = deathMessage;
        deathText.fontSize = 200;
        deathText.enableWordWrapping = false;
        float progress = 0;
        float elapsedTime = 0;
        while (progress <= 1) {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / FadeTime;
            BlackCurtain.alpha = progress;
            yield return null;
        }
        BlackCurtain.alpha = 1;
        yield return null;
        SceneManager.LoadScene("MainScene");
    }

    private IEnumerator FadeToGame() {
        BlackCurtain.alpha = 1;
        gameRunning = false;
        yield return wait;
        float progress = 0;
        float elapsedTime = 0;
        while (progress <= 1) {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / FadeTime;
            BlackCurtain.alpha = 1 - progress;
            yield return null;
        }
        BlackCurtain.alpha = 0;
        gameRunning = true;
    }

}
