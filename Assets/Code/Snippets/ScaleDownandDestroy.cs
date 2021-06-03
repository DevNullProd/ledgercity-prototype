using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleDownandDestroy : MonoBehaviour
{
    //**MANAGE Scale Down & Destroy*//
    //**ATTACHED TO GameManager*//
    /*NOTES:
     * 
     */

    float DestroyTimer;
    float ScaleDownSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup > DestroyTimer)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * ScaleDownSpeed);

            if (transform.localScale.x < 0.05f)
                Destroy(this.gameObject);
        }
    }

    public void SetScaleDownAndDestroy(float timeToActivate, float scaleDownSpeed = 5f ,float timeToScaleDown =  1f)
    {
        DestroyTimer = Time.realtimeSinceStartup + timeToActivate;
        ScaleDownSpeed = scaleDownSpeed;
        //Debug.Log(timeToActivate);
    }
}
