using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource gunSound;
    public AudioSource impactSound;

    public AudioSource gruntSource;
    public AudioClip[] grunts;

    public static AudioManager instance;

    void Awake() {
        instance = this;
    }

    public void PlayGunSound() {
        gunSound.PlayOneShot(gunSound.clip);
    }

    public void PlayImpactSound() {
        impactSound.PlayOneShot(impactSound.clip);
    }

    public void PlayGrunt() {
        gruntSource.PlayOneShot(grunts[Random.Range(0, grunts.Length)], 0.7f);
    }
}
