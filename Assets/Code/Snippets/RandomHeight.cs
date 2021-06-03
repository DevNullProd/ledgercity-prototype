using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHeight : MonoBehaviour
{
    float ScaleMin = 2f;
    float ScaleMax = 5f;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(transform.localScale.x, Random.Range(ScaleMin, ScaleMax), transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
