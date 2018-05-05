using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource gunSound;
    public AudioSource impactSound;

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
}
