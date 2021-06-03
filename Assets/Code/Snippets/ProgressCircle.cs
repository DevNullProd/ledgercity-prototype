using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressCircle : MonoBehaviour {

    [Header("Speed shold be 10")]
    public float Speed = 3;
    float TranAnimator = 0f;
    float AnimSpeed = 1f;
    float AnimSpeedMultiplayer = 1f;
    Image Img;
    [SerializeField] Text textPercent;
    Color Col = Color.red;

    [SerializeField] Button ButtonPerk;

    bool AnimInProgress = false;

    // Use this for initialization
    void Awake () {
        //Set();
    }

    void OnEnable()
    {
        //if (Speed > 0)
        //    Set();
    }

    public void Set()
    {
        this.gameObject.SetActive(true);

        //if (AnimInProgress)
        //    return;

        if (Speed > 25f)
            AnimSpeedMultiplayer = 10f;
        else if (Speed > 8)
            AnimSpeedMultiplayer = 4f;
        else
            AnimSpeedMultiplayer = 2f;

        AnimInProgress = true;
        TranAnimator = 0f;
        AnimSpeed = 1f / Speed;
        Img = this.GetComponent<Image>();
        Img.fillAmount = 0;
        Col = Color.red;
    }

    // Update is called once per frame
    void Update () {
        TranAnimator += AnimSpeed * Time.deltaTime;

        if (TranAnimator > 0.75f)
            Col = Color.green;
        else if (TranAnimator > 0.6f)
            Col = Color.yellow;

        //Debug.Log(TranAnimator);

        if (TranAnimator >= 1f)
        {
            AnimSpeed = 0.5f;

            if (TranAnimator >= 1f & TranAnimator < 1.07f)
            {
                Img.color = new Color(1, 1, 1, 0);
                textPercent.color = new Color(1, 1, 1, 0);
            }

            if (TranAnimator >= 1.07f & TranAnimator < 1.1f)
            {
                Img.color = Col;
                textPercent.color = Col;
            }

            if (TranAnimator >= 1.1f & TranAnimator < 1.17f)
            {
                Img.color = new Color(1, 1, 1, 0);
                textPercent.color = new Color(1, 1, 1, 0);
            }

            if (TranAnimator >= 1.17f & TranAnimator < 1.2f)
            {
                Img.color = Col;
                textPercent.color = Col;
            }

            //ANIMATION DONE
            if (TranAnimator >= 1.2f)
            {
                //Speed = -1;
                ButtonPerk.GetComponent<Button>().interactable = true;
                ButtonPerk.interactable = true;
                AnimInProgress = false;
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            Img.fillAmount = TranAnimator;
            Img.color = Color.white;
            textPercent.text = ((int)(TranAnimator * 100f)).ToString() + "%";
            textPercent.color = Col;    /// new Color(1f - TranAnimator, TranAnimator, 0);
        }
    }
}
