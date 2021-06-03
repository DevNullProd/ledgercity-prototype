using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour {

    //[SerializeField] Main main;
    [SerializeField] Light LightMain;
    float LightIntensityOld;
    float LightRangeOld;

    float Timer;
    float Timer2;
    bool LightningInProgress = false;
    int MaxIntensityRange;

	// Use this for initialization
	void Start () {
        LightIntensityOld = LightMain.intensity;
        //if (main != null)
        //{
            Timer = Random.Range(8, 15);
            StartCoroutine(LightningStart(Timer));
            MaxIntensityRange = 5;
        //}
        //else
        //{
        //    LightRangeOld = LightMain.range;
        //    MaxIntensityRange = 100;
        //}
	}
	
	// Update is called once per frame
	void Update () {
		if (LightningInProgress)
        {
            if (Timer2 > Time.realtimeSinceStartup)
            {
                LightMain.intensity = LightIntensityOld + (Timer2 - Time.realtimeSinceStartup) * Random.Range(1, MaxIntensityRange);
            }
            else
            {
                //if (main != null)
                //{
                    Timer = Random.Range(5, 15);
                    StartCoroutine(LightningStart(Timer));
                //}
                //else
                //    LightMain.range = LightRangeOld;

                LightningInProgress = false;
                LightMain.intensity = LightIntensityOld;
            }
        }
	}

    IEnumerator LightningStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        //if (main.World != 3)
        //    Destroy(this);

        float intensity = Random.Range(1, 4);
        Timer2 = Time.realtimeSinceStartup + intensity;
        LightningInProgress = true;
        AudioController.audioCntInst.Play_Thunder();
    }

    public void ManualLightningTrigger(int  maxIntensiti)
    {
        //LightIntensityOld = LightMain.intensity;
        float intensity = Random.Range(0.6f, 1.7f);
        MaxIntensityRange = Random.Range(maxIntensiti / 2, maxIntensiti);
        LightMain.range = LightRangeOld * 2;
        Timer2 = Time.realtimeSinceStartup + intensity;
        LightningInProgress = true;
    }

    private void OnDisable()
    {
        LightMain.intensity = LightIntensityOld;
    }

    private void OnEnable()
    {
        LightIntensityOld = LightMain.intensity;

        Timer = Random.Range(0.3f, 0.5f);
        StartCoroutine(LightningStart(Timer));
    }
}
