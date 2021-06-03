using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    //**MANAGE object Destruction*//
    //**ATTACHED TO any GameObject on SPAWN*//
    /*NOTES:
     * 
     */

    //public float LifeTime = 0.8f;

    //private void OnEnable()
    //{
    //    StartCoroutine(DestroyObject(LifeTime));
    //}

    public void SetDestroyTimer(float lifeTime)
    {
        StartCoroutine(DestroyObject(lifeTime));
    }

    IEnumerator DestroyObject(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(this.gameObject);
    }
}
