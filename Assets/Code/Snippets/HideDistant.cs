using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideDistant : MonoBehaviour
{
    float Hide_Distance = 1500;
    [SerializeField] GameObject[] What_To_Hide = null;


    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("Hide", 0.1f, 0.1f);
        //Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Hide()
    {
        if (Vector3.Distance(this.transform.position, Camera.main.transform.position) > Hide_Distance)
        {
            for (int x = 0; x < What_To_Hide.Length; x++)
                What_To_Hide[x].SetActive(false);
        }
        else
        {
            for (int x = 0; x < What_To_Hide.Length; x++)
                What_To_Hide[x].SetActive(true);
        }
    }
}
