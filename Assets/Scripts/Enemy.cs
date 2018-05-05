using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Collider2D mainCollider;

    private float health = 2;
    public float Health {
        get {
            return health;
        }
        set {
            health = value;
            if (health <= 0) {
                Destroy(mainCollider);
                Destroy(gameObject, 5);
            }
        }
    }

    private const float MOVE_SPEED = 5;
    private const float MIN_DISTANCE = 8.5f;

    void Update() {
        if (health >= 0) {
            float distanceFromPlayer = Player.instance.transform.position.x - transform.position.x;
            if (Mathf.Abs(distanceFromPlayer) > MIN_DISTANCE) {
                float theX = (MOVE_SPEED * Time.deltaTime * Mathf.Sign(distanceFromPlayer));
                transform.position += new Vector3(theX, 0, 0);
            }
        }
    }
}
