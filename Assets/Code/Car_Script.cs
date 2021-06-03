using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Script : MonoBehaviour
{
    public Car_Spawner carSpawner = null;
    [SerializeField] GameObject[] Cars_Ref = null;
    [SerializeField] GameObject CarLights = null;
    [SerializeField] GameObject CarCamera = null;
    [SerializeField] GameObject DebugText3D = null;
    GameObject PreviousWayPoint;
    GameObject Car;
    Vector3 CarStartPos;

    Vector3 StartPos;
    float Speed = 40f;
    float SpeedCorrection = 1f;
    bool CarCreated = false;

    int NextWayPoint = 0;
    float WayPointDistanceBoorder = 4f;

    // Start is called before the first frame update
    void Start()
    {
        int CarNo = Random.Range(0, Cars_Ref.Length);
        CarCamera.SetActive(false);

        StartPos = transform.position;

        Car = Instantiate(Cars_Ref[CarNo]);
        Car.transform.SetParent(this.gameObject.transform);
        Car.transform.localPosition = new Vector3(0, 0, 1.17f);
        CarStartPos = Car.transform.localPosition;
        Car.transform.localScale = Vector3.one * 3;
        CarCreated = true;

        PreviousWayPoint = carSpawner.WayPoints[carSpawner.WayPoints.Length - 1];

        transform.rotation = Quaternion.Euler(carSpawner.WayPoints[NextWayPoint].transform.localEulerAngles + new Vector3(0, 90, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (CarCreated)
        {
            //transform.position += Vector3.forward * Time.deltaTime * Speed;

            //if (transform.position.z > 1700f)
            //    transform.position = StartPos;

            CarDrive();
        }
    }

    //Slow downs near corner
    float speedCor = 0.5f;
    float distMultiplier = 3f;
    float speedMin = 18f;
    float angleCorrection = 0;
    float carRotLerpSpeed_Inverse = 30f;
    Vector3 carPosCorrection = Vector3.zero;

    void CarDrive()
    {
        transform.position += (carSpawner.WayPoints[NextWayPoint].transform.position -  transform.transform.position).normalized * Time.deltaTime * (Speed - SpeedCorrection);
        //transform.position = Vector3.LerpUnclamped(transform.position, carSpawner.WayPoints[NextWayPoint].transform.position, Time.deltaTime * (Speed * SpeedCorrection));
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler (carSpawner.WayPoints[NextWayPoint].transform.localEulerAngles + new Vector3(0, 90 /*+ angleCorrection*/, 0))
            , Time.deltaTime * (Speed - SpeedCorrection) / 12f);

        Car.transform.localPosition = Vector3.Lerp(Car.transform.localPosition, CarStartPos + carPosCorrection, 5f * Time.deltaTime);
        Car.transform.localRotation = Quaternion.Lerp(Car.transform.localRotation, Quaternion.Euler( new Vector3(0, 90 + angleCorrection, 0))
            , Time.deltaTime * (Speed - SpeedCorrection) / carRotLerpSpeed_Inverse);


        if (Vector3.Distance(transform.position, carSpawner.WayPoints[NextWayPoint].transform.position) < WayPointDistanceBoorder)
        {
            PreviousWayPoint = carSpawner.WayPoints[NextWayPoint];
            NextWayPoint++;
        }

        if (NextWayPoint >= carSpawner.WayPoints.Length)
            NextWayPoint = 0;

        //Slow downs near corner
        carRotLerpSpeed_Inverse = 10f;
        float distNext = Vector3.Distance(transform.position, carSpawner.WayPoints[NextWayPoint].transform.position);
        float distPrev = Vector3.Distance(transform.position, PreviousWayPoint.transform.position);
        if (distNext < distPrev)
        {
            if (distNext < (Speed * distMultiplier * speedCor))
            {
                SpeedCorrection = ((Speed * distMultiplier * speedCor) - distNext) * 0.5f;

                if (distNext < WayPointDistanceBoorder * 2f)
                {
                    angleCorrection = 160f;// distNext * 40f;
                    carRotLerpSpeed_Inverse = 25f;
                }
                else
                    angleCorrection = 0;
            }
            else
            {
                SpeedCorrection = 0f;
                angleCorrection = 0f;
            }

            if (distNext < WayPointDistanceBoorder * 2f)
                carPosCorrection = new Vector3(0, 0, -3f);
            else
                carPosCorrection = Vector3.zero;
        }
        else
        {
            if (distPrev < (Speed * distMultiplier * speedCor))
            {
                SpeedCorrection = ((Speed * distMultiplier * speedCor) - distPrev) * 0.5f;

                if (distPrev < WayPointDistanceBoorder * 2f)
                {
                    angleCorrection = 80f;// distPrev * 40f;
                    carRotLerpSpeed_Inverse = 30f;
                }
                else
                    angleCorrection = 0;
            }
            else
            {
                SpeedCorrection = 0f;
                angleCorrection = 0f;
            }

            carPosCorrection = Vector3.zero;
        }

        if ((Speed - SpeedCorrection) < speedMin)
            SpeedCorrection = Speed - speedMin;

        //if (CarCamera.activeInHierarchy)
        //    DebugText3D.GetComponent<TextMesh>().text = (Speed - SpeedCorrection).ToString("#.##") + "  > distNext = " + distNext.ToString("#.##") + "  > distPrev = " + distPrev.ToString("#.##")
        //         + "  > angleCirection = " + Car.transform.localEulerAngles.ToString("#.##");
    }

    public void SetLights(bool ligtsOn)
    {
        CarLights.SetActive(ligtsOn);
    }

    public void Set_Car_Camera_Active(bool isActive)
    {
        CarCamera.SetActive(isActive);
    }
}