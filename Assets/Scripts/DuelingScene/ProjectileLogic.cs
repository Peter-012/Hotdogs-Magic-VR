using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileLogic : MonoBehaviour, IDamage {
    [SerializeField] private float projectileSpeed = 15f;
    [SerializeField] private float delayFire = 0.5f;
    [SerializeField] private float deleteProjectile = 1.0f;
    private float currentTime = 0;
    private bool detachFromWand = false;

    public static event Action OnDamage;

    private void Update() {
        fireProjectile();
    }

    private void fireProjectile() {
        if (currentTime >= delayFire) {
            // if (!detachFromWand) {
            //     GameObject wandObject = GameObject.Find("Wand");
            //     FixedJoint joint = wandObject.GetComponent<FixedJoint>();
            //     Destroy(joint);

            //     detachFromWand = true;
            // }
            //  transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
        }

        // if (currentTime > deleteProjectile) {
        //     Destroy(gameObject);
        // }

        currentTime += Time.deltaTime; 
    }

    public void Type() {
        if (OnDamage == null) return;
        OnDamage.Invoke();
    }

    private void OnEnable() {
        OnDamage += ProjectileDamage;
    }

    private void OnDisable() {
        OnDamage -= ProjectileDamage;
    }

    private void ProjectileDamage() {
        PlayerData player = Resources.Load<PlayerData>("Player2");
        player.health--;
        if (player.health <= 0) {
            GameObject enemyObject = GameObject.Find("Enemy");
            Destroy(enemyObject);
        }
    }
}
