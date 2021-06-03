using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database_Comm : MonoBehaviour
{
    //**MANAGE Communication with SQL base*//
    //**ATTACHED TO Communication.GameObject*//
    /*NOTES:
     * Call with Database_Comm.DbaseInst
     */

    [Header("References")]
    public static Database_Comm DbaseInst;

    void Awake()
    {
        DbaseInst = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
