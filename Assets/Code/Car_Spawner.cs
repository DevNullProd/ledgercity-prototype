using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Spawner : MonoBehaviour
{
    [SerializeField] GameObject CarHolder = null;
    public GameObject[] WayPoints = null;

    int CarsToSpavn = 3;
    int CarsSpawned = 0;
    float CarSpawn_Timer;

    // Start is called before the first frame update
    void Start()
    {
        CarSpawn_Timer = (Time.realtimeSinceStartup + Random.Range(8f, 15f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup > CarSpawn_Timer)
        {
            if (CarsSpawned < CarsToSpavn)
            {
                SpawnCar();
                CarSpawn_Timer = (Time.realtimeSinceStartup + Random.Range(2f, 15f));
            }
            else
                this.enabled = false;
        }
    }

    void SpawnCar()
    {
        GameObject car = Instantiate(CarHolder);
        car.transform.SetParent(this.gameObject.transform);
        car.transform.localPosition = Vector3.zero;
        car.transform.localScale = car.transform.localScale * 0.7f;
        car.transform.eulerAngles = new Vector3(0, -90, 0);
        car.GetComponent<Car_Script>().carSpawner = this;
        car.GetComponent<Car_Script>().SetLights(GameManager.gameMngInst.IsNight);
        GameManager.gameMngInst.gameObject.GetComponent<Populate>().Car_List.Add(car);
        CarsSpawned++;
    }
}
