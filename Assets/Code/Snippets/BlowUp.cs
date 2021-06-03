using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlowUp : MonoBehaviour
{
    [SerializeField] float Speed = 10f;
    [SerializeField] float DelayStart = 0f;
    float DelayTimer;
    [SerializeField] bool Rot = false;
    [SerializeField] bool BlowUpAndDeactivate = false;
    //REST IS RELEVANT ONLY IF BlowUpAndDeactivate IS TRUE
    [SerializeField] bool FadeOut = false;
    [SerializeField] int BlowUpRepeats = 0;     //Works if > 0
    int BlowUpRepeatsDone = 0;
    [SerializeField] bool ChangeColor = false;
    [SerializeField] Color StartColor = Color.white;
    [SerializeField] Color FinalColor = Color.white;

    [SerializeField] Vector3 EndScale = Vector3.one;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    StartColor = this.gameObject.GetComponent<Image>().color;
    //    Debug.Log(StartColor);
    //}

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup < DelayTimer)
            return;

        this.gameObject.transform.localScale = Vector3.Lerp(this.gameObject.transform.localScale, EndScale, Speed * Time.deltaTime);
        if (Rot)
            this.gameObject.transform.rotation = Quaternion.Lerp(this.gameObject.transform.rotation, Quaternion.Euler(0, 0, 0), Speed * Time.deltaTime);

        //THIS EXECUTES ONLY IF BlowUpAndDeactivate IS TRUE
        if (ChangeColor)
            this.gameObject.GetComponent<Image>().color = Color.Lerp(this.gameObject.GetComponent<Image>().color, FinalColor, Speed * 1f * Time.deltaTime);

        if (FadeOut)
            this.gameObject.GetComponent<Image>().color = new Color(this.gameObject.GetComponent<Image>().color.r, this.gameObject.GetComponent<Image>().color.g,
                this.gameObject.GetComponent<Image>().color.b, 1.3f - transform.localScale.x);

        if (BlowUpAndDeactivate && this.gameObject.transform.localScale.x > 0.99f)
        {
            if (BlowUpRepeatsDone >= BlowUpRepeats - 1)
                this.gameObject.SetActive(false);
            else
            {
                BlowUpRepeatsDone++;
                this.gameObject.GetComponent<Image>().color = StartColor;
                this.gameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            }
        }
    }

    private void OnEnable()
    {
        if (BlowUpAndDeactivate == false)
        {
            FadeOut = false;
            ChangeColor = false;
            FadeOut = false;
        }

        if (ChangeColor)
            this.gameObject.GetComponent<Image>().color = StartColor;

        BlowUpRepeatsDone = 0;

        this.gameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        if (Rot)
            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);

        if (DelayStart > 0.1f)
            DelayTimer = Time.realtimeSinceStartup + DelayStart;
    }
}
