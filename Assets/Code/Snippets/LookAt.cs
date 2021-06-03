using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {

    //**orient card toward camera*//
    //**ATTACHED TO Playerbase.CardBase*//

    [SerializeField] Camera Cam = null;

    // Update is called once per frame
    void Update () {
        //orient card toward camera
        if (Cam == null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
        else
            transform.rotation = Quaternion.LookRotation(transform.position - Cam.transform.position);
    }
}
