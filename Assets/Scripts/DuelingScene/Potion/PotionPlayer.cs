using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PotionPlayer : PotionAbstract {

    private GameObject targetObject;

    public override void Start() {
        targetObject = GameObject.Find("Enemy");
        base.Start();
    }

    public override IEnumerator DelayPotion() {
        yield return new WaitForSeconds(delayTime);

        Rigidbody potionRigid = 
            gameObject.GetComponent<Rigidbody>();
        potionRigid.useGravity = false;
        potionRigid.isKinematic = true;

        // Reset orientation of the potion
        transform.rotation = Quaternion.identity;
        transform.Rotate(0, -90, 0);

        AimPotion();

        delayPotionBool = false;
    }

    public override void AimPotion() {
        GameObject rigObject = GameObject.Find("[CameraRig]");

        //// Calculating Horizontal Angle ////

        // Pythagorean Theorem
        
        // Opposite
        float potionToEnemyZ = 
            Mathf.Abs(gameObject.transform.position.z - targetObject.transform.position.z);

        // Adjacent
        float potionToEnemyX = 
            Mathf.Abs(targetObject.transform.position.x - gameObject.transform.position.x);
        
        // Hypotenuse
        float enemyToPlayerHorizontal = 
            Mathf.Sqrt(Mathf.Pow(potionToEnemyZ, 2f) + Mathf.Pow(potionToEnemyX, 2f));

        float horizontalAngle = Mathf.Rad2Deg * Mathf.Asin(potionToEnemyZ/enemyToPlayerHorizontal);

        // Negative depending on if potion is past enemy Z position
        if (targetObject.transform.position.z - gameObject.transform.position.z < 0) 
            horizontalAngle = -horizontalAngle;


        //// Calculating Vertical Angle ////

        // Pythagorean Theorem

        // Adjust to aim at enemy's center of mass
        float enemyCenter = 
            Mathf.Abs(targetObject.transform.position.y - rigObject.transform.position.y);

        // Add back the camera rig offset
        enemyCenter = enemyCenter + rigObject.transform.position.y;

        // Opposite
        float PlayerToPotion = 
            Mathf.Abs(enemyCenter - gameObject.transform.position.y);

        // Adjacent is already calculated from the horizontal angle calculation

        // Hypotenuse
        float enemyToPlayerVertical = 
            Mathf.Sqrt(Mathf.Pow(potionToEnemyX, 2f) + Mathf.Pow(PlayerToPotion, 2f));
        
        float verticalAngle = Mathf.Rad2Deg * Mathf.Asin(PlayerToPotion/enemyToPlayerVertical);

        // Negative value when enemy center is above line of enemy fire
        if (enemyCenter - gameObject.transform.position.y > 0) 
            verticalAngle = -verticalAngle;

        // Adjust the trajectory of potion
        gameObject.transform.Rotate(verticalAngle, horizontalAngle, 0);
    }
}
