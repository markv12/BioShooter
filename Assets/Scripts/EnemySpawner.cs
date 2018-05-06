using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefab;
    public Camera theCamera;

	void Start () {
        StartCoroutine(SpawnRoutine());
	}

    private static readonly WaitForSeconds spawnWait = new WaitForSeconds(4f);
	private IEnumerator SpawnRoutine() {
        while (true) {
            yield return spawnWait;
            GameObject newEnemy = Instantiate(enemyPrefab);
            float theX = (Random.Range(0, 2) == 0) ? Screen.width + 25 : -25;
            Vector3 spawnPos = theCamera.ScreenToWorldPoint(new Vector3(theX, 0, GunController.cameraDistance));
            spawnPos.y = Player.p.transform.position.y + 2;
            newEnemy.transform.position = spawnPos;
        }
    }
}
