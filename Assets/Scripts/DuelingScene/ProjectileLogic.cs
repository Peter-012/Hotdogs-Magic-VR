using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour {
    [SerializeField] private float speed = 1f;
    [SerializeField] private string enemyObjectName = "Enemy";

    private void Update() {
        movement();
    }

    // private void OnTriggerEnter(Collider other) {
    //     // Determine if projectile hit enemy
    //     if (other.name.Contains(wandGameObjectName)) {
    //         wandObject = other.gameObject;
    //     }
    // }

    // private void OnTriggerExit(Collider other) {
    //     // Determine if hand has left the wand object
    //     if (other.name.Contains(wandGameObjectName)) {
    //         wandObject = null;
    //     }
    // }

    private void movement() {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
