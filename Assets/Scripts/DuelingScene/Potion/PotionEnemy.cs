using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PotionEnemy : PotionAbstract {

    public override IEnumerator DelayPotion() {
        yield return new WaitForSeconds(delayTime);

        Rigidbody potionRigid = 
            gameObject.GetComponent<Rigidbody>();
        potionRigid.useGravity = false;
        potionRigid.isKinematic = true;

        // Reset orientation of the potion
        transform.rotation = Quaternion.identity;
        transform.Rotate(0, 90, 0);

        GameObject playerObject = GameObject.Find("Camera");
        AimPotion(playerObject);

        delayPotion = false;
    }
}
