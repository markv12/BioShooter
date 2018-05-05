﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Collider2D mainCollider;
    public GunController gun;

    private float health = 2;
    public float Health {
        get {
            return health;
        }
        set {
            health = value;
            if (health <= 0) {
                Destroy(mainCollider);
                Destroy(gun);
                AudioManager.instance.PlayGrunt();
                GameManager.instance.ShowKillInfo();
                StopCoroutine(attackRoutine);
                Destroy(gameObject, 5);
            }
        }
    }

    private Coroutine attackRoutine;
    void Start() {
        gun.ownerT = transform;
        attackRoutine = StartCoroutine(AttackRoutine());
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
        if (Vector3.Distance(transform.position, Player.instance.transform.position) > 50) {
            Destroy(gameObject);
        }
    }

    private IEnumerator AttackRoutine() {
        while (true) {
            yield return new WaitForSeconds(2f + Random.Range(-1f, 1f));
            gun.FireGun();
        }
    }
}
