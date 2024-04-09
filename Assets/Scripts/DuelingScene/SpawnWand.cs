using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWand : MonoBehaviour {
    [SerializeField] private string wandPath = "Wand";
    private Object wandPrefab;
    
    private void Start() {
        // Load Wand Prefab
        wandPrefab = Resources.Load<Object>(wandPath);

        Vector3 position = transform.position + transform.forward;
        Quaternion rotation = transform.rotation;

        // Spawn the wand into player's hand
        GameObject wandObject = Instantiate(wandPrefab, position, rotation) as GameObject;
        wandObject.transform.SetParent(transform);

        // Move and scale wand to fit in player's hand
        wandObject.transform.localScale = new Vector3(60f, 60f, 35f);
        wandObject.transform.Translate(0, 0, -1.15f);

        // Add component for wand logic
        wandObject.AddComponent<WandLogic>();
    }
}
