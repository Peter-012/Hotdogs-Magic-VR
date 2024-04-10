using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileLogic : MonoBehaviour, IDamage {
    [SerializeField] private float speed = 15f;

    public static event Action OnDamage;

    private void Update() {
        movement();
    }

    private void movement() {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
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
