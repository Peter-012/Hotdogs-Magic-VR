using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerData")]
public class PlayerData : ScriptableObject {
    public string DominantSide;
    public int health = 10;
    public int reloadDelayCounter = 2;
}
