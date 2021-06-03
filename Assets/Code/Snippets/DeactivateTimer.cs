using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateTimer : MonoBehaviour
{
    //**MANAGE object deactivation*//
    //**ATTACHED TO any GameObject*//
    /*NOTES:
     * 
     */

    public float LifeTime = 0.8f;

    private void OnEnable()
    {
        StartCoroutine(DeactivateObject(LifeTime));
    }

    IEnumerator DeactivateObject(float delay)
    {
        yield return new WaitForSeconds(delay);

        this.gameObject.SetActive(false);
    }
}
