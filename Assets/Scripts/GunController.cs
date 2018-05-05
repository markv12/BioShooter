﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public Transform playerT;
    public Transform shootPoint;
    public Transform myT;
    public Transform imageT;

    public GameObject bulletPrefab;

    public Camera theCamera;
    public static float cameraDistance;
    private const float DISTANCE_FROM_BODY = 3f;
    private const float GUN_HEIGHT_ADJUSTMENT = 1.3f;

    void Awake() {
        cameraDistance = -theCamera.transform.position.z;
    }

    private static readonly Quaternion theIQ = Quaternion.identity;
    private static readonly Quaternion shotRotation = Quaternion.Euler(0, 0, -7);
    private static readonly Quaternion shotRotationReverse = Quaternion.Euler(0, 0, 7);
    void Update() {
        Vector3 v3 = Input.mousePosition;
        v3.z = cameraDistance;
        Vector3 gunPos = GetGunPosition(theCamera.ScreenToWorldPoint(v3));
        float angleFromPlayer = GetGunAngle(gunPos);
        AdjustImageForAngle(angleFromPlayer);
        myT.rotation = Quaternion.Euler(0, 0, angleFromPlayer);
        myT.position = gunPos;
        imageT.localRotation = Quaternion.Lerp(imageT.localRotation, theIQ, Time.deltaTime);
        if (Input.GetMouseButtonDown(0)) {
            FireGun();
        }
    }

    private void FireGun() {
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.transform.SetPositionAndRotation(shootPoint.position, shootPoint.rotation);
        AudioManager.instance.PlayGunSound();
        imageT.localRotation *= reversed ? shotRotationReverse : shotRotation;
    }

    private bool reversed = false;
    private void AdjustImageForAngle(float angleFromPlayer) {
        if (angleFromPlayer > -90 && angleFromPlayer < 90) {
            if (!reversed) {
                imageT.localScale = new Vector3(-1, 1, 1);
                imageT.localPosition = new Vector3(0, -GUN_HEIGHT_ADJUSTMENT, 0);
                imageT.localRotation = theIQ;
                reversed = true;
            }
        } else {
            if (reversed) {
                imageT.localScale = new Vector3(-1, -1, 1);
                imageT.localPosition = new Vector3(0, GUN_HEIGHT_ADJUSTMENT, 0);
                imageT.localRotation = theIQ;
                reversed = false;
            }
        }
    }

    private float GetGunAngle(Vector3 gunPos) {
        Vector3 relPosFromPlayer = gunPos - playerT.position;
        return Mathf.Atan2(relPosFromPlayer.y, relPosFromPlayer.x) * 57.2958f;
    }

    private Vector3 GetGunPosition(Vector3 gunPos) {
        Vector3 result;
        Vector3 relPosFromPlayer = gunPos - playerT.position;
        relPosFromPlayer.z = 0;
        if (relPosFromPlayer.magnitude > DISTANCE_FROM_BODY) {
            Vector3 normalizedAngle = relPosFromPlayer.normalized * DISTANCE_FROM_BODY;
            result = playerT.position + normalizedAngle;
        } else {
            result = gunPos;
        }
        return result;
    }
}
