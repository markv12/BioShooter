using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefab;
    public Camera theCamera;

	void Start () {
        StartCoroutine(SpawnRoutine());
	}

    private bool lastSpawnToTheLeft = false;
    private static readonly WaitForSeconds spawnWait = new WaitForSeconds(4f);
	private IEnumerator SpawnRoutine() {
        while (true) {
            yield return spawnWait;
            GameObject newEnemy = Instantiate(enemyPrefab);
            float theX = lastSpawnToTheLeft ? Screen.width + 20 : -20;
            lastSpawnToTheLeft = !lastSpawnToTheLeft;
            Vector3 spawnPos = theCamera.ScreenToWorldPoint(new Vector3(theX, 0, GunController.cameraDistance));
            spawnPos.y = Player.instance.transform.position.y + 2;
            newEnemy.transform.position = spawnPos;
        }
    }
}
