using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTexture : MonoBehaviour
{
    //**MANAGE Building UI*//
    //**Attached to CanvasBuilding*//
    /*NOTES:
     * 
     */

    [Header("References")]
    [SerializeField] Texture[] Rnd_Texture = null;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.SetTexture("_BaseMap", Rnd_Texture[Random.Range(0, Rnd_Texture.Length)]);
    }
}
