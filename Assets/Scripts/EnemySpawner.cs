using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefab;
    public GameObject enemy2Prefab;
    public Camera theCamera;

	void Start () {
        StartCoroutine(SpawnRoutine());
	}

    private static readonly WaitForSeconds spawnWait = new WaitForSeconds(4.5f);
	private IEnumerator SpawnRoutine() {
        while (true) {
            yield return spawnWait;
            float rand = Random.Range(0f, 1f);
            GameObject toSpawn = (rand > 0.75f) ? enemy2Prefab : enemyPrefab;
            GameObject newEnemy = Instantiate(toSpawn);
            float theX = (Random.Range(0, 2) == 0) ? Screen.width + 25 : -25;
            Vector3 spawnPos = theCamera.ScreenToWorldPoint(new Vector3(theX, 0, GunController.cameraDistance));
            spawnPos.y = Player.p.transform.position.y + 2;
            newEnemy.transform.position = spawnPos;
        }
    }
}
