using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileLogic : MonoBehaviour, IDamage {
    [SerializeField] private float projectileSpeed = 15f;
    [SerializeField] private float delayFire = 0.5f;
    [SerializeField] private float deleteProjectile = 3.0f;
    private float currentTime = 0;
    private bool detachFromWand = false;

    public static event Action <GameObject> OnDamage;

    private void Update() {
        fireProjectile();
    }

    private void fireProjectile() {
        if (currentTime >= delayFire) {
            if (!detachFromWand) {
                this.transform.SetParent(null);
                detachFromWand = true;
            }
             transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        if (currentTime > deleteProjectile) {
            Destroy(gameObject);
        }

        currentTime += Time.deltaTime; 
    }

    public void Hit(GameObject Player) {
        if (OnDamage == null) return;
        OnDamage.Invoke(Player);
    }

    private void OnEnable() {
        OnDamage += ProjectileDamage;
    }

    private void OnDisable() {
        OnDamage -= ProjectileDamage;
    }

    private void ProjectileDamage(GameObject Player) {
        if (Player.name.Equals("Enemy")) {
            Player2.health--;

            if (Player2.health < 0) {
                Destroy(Player);
            }
        // } else {
        //     Player1.health--;

        //     if (Player1.health <= 0) {
                
        //     }
        }
        DestroyProjectile();
    }

    private void DestroyProjectile() {
        Destroy(gameObject);
    }
}
