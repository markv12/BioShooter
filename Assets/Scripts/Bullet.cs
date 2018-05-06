using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Vector3 speed;
    public Transform myT;

    void Update () {
        myT.Translate(speed * Time.deltaTime);
        if(Vector3.Distance(myT.position, Player.p.transform.position) > 50) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player") {
            col.gameObject.GetComponentInParent<Player>().Health -= 1;
        } else if(col.gameObject.tag == "Enemy") {
            col.gameObject.GetComponentInParent<Enemy>().Health -= 1;
        }
        AudioManager.instance.PlayImpactSound();
        Destroy(gameObject);
    }


}
