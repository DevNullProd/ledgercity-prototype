using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    //**MANAGE Object constant Rotation*//
    //**ATTACHED TO Game object which you wish to rotate*//
    /*NOTES:
     * 
     */

    [Header("Rotation Speed")]
    public float RotSpeed = 10f;
    [Header("Rotation Axis can be: x, y, z")]
    public string Axis = "y";

    // Update is called once per frame
    void Update()
    {
        switch (Axis)
        {
            case "x":
                transform.localEulerAngles = transform.localEulerAngles + new Vector3(RotSpeed * Time.deltaTime, 0, 0);
                break;

            case "y":
                transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, RotSpeed * Time.deltaTime, 0);
                break;

            case "z":
                transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, 0, RotSpeed * Time.deltaTime);
                break;
        }
    }
}
