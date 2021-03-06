﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public bool playerGun;

    public Transform ownerT;
    public Transform shootPoint;
    public Transform myT;
    public Transform imageT;

    public GameObject bulletPrefab;

    public Camera theCamera;
    public static float cameraDistance;
    private const float DISTANCE_FROM_BODY = 3.5f;
    private const float GUN_HEIGHT_ADJUSTMENT = 1.3f;

    void Awake() {
        if (playerGun) {
            cameraDistance = -theCamera.transform.position.z;
        }
    }

    private static readonly Quaternion theIQ = Quaternion.identity;
    private static readonly Quaternion shotRotation = Quaternion.Euler(0, 0, -16);
    private static readonly Quaternion shotRotationReverse = Quaternion.Euler(0, 0, 16);
    void Update() {
        Vector3 aimPosition;
        if (playerGun) {
            Vector3 v3 = Input.mousePosition;
            v3.z = cameraDistance;
            aimPosition = theCamera.ScreenToWorldPoint(v3);
        } else {
            aimPosition = Player.p.transform.position;
        }

        Vector3 gunPos = GetGunPosition(aimPosition);
        float angleFromPlayer = GetGunAngle(gunPos);
        AdjustImageForAngle(angleFromPlayer);
        myT.rotation = Quaternion.Euler(0, 0, angleFromPlayer);
        myT.position = gunPos;
        if (playerGun && Input.GetMouseButtonDown(0)) {
            FireGun();
        }
        imageT.localRotation = Quaternion.Lerp(imageT.localRotation, theIQ, Time.deltaTime*3);
    }

    public void FireGun() {
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.transform.SetPositionAndRotation(shootPoint.position, shootPoint.rotation);
        AudioManager.instance.PlayGunSound();
        imageT.localRotation *= (dState == DirectionState.right) ? shotRotationReverse : shotRotation;
    }

    private DirectionState dState = DirectionState.initial;
    private void AdjustImageForAngle(float angleFromPlayer) {
        if (angleFromPlayer > -90 && angleFromPlayer < 90) {
            if (dState != DirectionState.right) {
                imageT.localScale = new Vector3(-1, 1, 1);
                imageT.localPosition = new Vector3(0, -GUN_HEIGHT_ADJUSTMENT, 0);
                imageT.localRotation = theIQ;
                dState = DirectionState.right;
            }
        } else {
            if (dState != DirectionState.left) {
                imageT.localScale = new Vector3(-1, -1, 1);
                imageT.localPosition = new Vector3(0, GUN_HEIGHT_ADJUSTMENT, 0);
                imageT.localRotation = theIQ;
                dState = DirectionState.left;
            }
        }
    }

    private float GetGunAngle(Vector3 gunPos) {
        Vector3 relPosFromPlayer = gunPos - ownerT.position;
        return Mathf.Atan2(relPosFromPlayer.y, relPosFromPlayer.x) * 57.2958f;
    }

    private Vector3 GetGunPosition(Vector3 gunPos) {
        Vector3 result;
        Vector3 relPosFromPlayer = gunPos - ownerT.position;
        relPosFromPlayer.z = 0;
        if (relPosFromPlayer.magnitude > DISTANCE_FROM_BODY) {
            Vector3 normalizedAngle = relPosFromPlayer.normalized * DISTANCE_FROM_BODY;
            result = ownerT.position + normalizedAngle;
        } else {
            result = gunPos;
        }
        return result;
    }
}

public enum DirectionState {
    initial,
    left,
    right
}
