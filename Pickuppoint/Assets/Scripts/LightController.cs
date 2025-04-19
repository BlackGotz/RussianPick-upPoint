using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class LightController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _lamps;
    [SerializeField] private TimeManager time;
    [SerializeField] private Light sun;
    [SerializeField] private float startRotation = 30f;
    [SerializeField] private float endRotation = 150f;
    public float morning;
    public float night = 21f;
    public float day = 12f;
    public float evening = 17f;
    public float minIntensity = 0.2f;
    public float maxIntensity = 1f;
    private float yRotation;
    private float xRotation;
    private float timeonday;
    private float deltaT;

    // Start is called before the first frame update
    void Start()
    {
        sun.intensity = minIntensity;
        yRotation = -30;
        transform.rotation = Quaternion.Euler(startRotation,-30,0);
        timeonday = 9f;
        morning = time.startTime/60;
        night = time.endTime/60;
        deltaT = time.timeIncrement;
        xRotation = startRotation;
    }

    // Update is called once per frame
    void Update()
    {
        timeonday = time.currentTime / 60;
        if (timeonday >= day && timeonday < evening)
        {
            if (_lamps[0].activeInHierarchy)
            {
                foreach (var l in _lamps)
                {
                    l.SetActive(false);
                }
            }
            
        }
        else if(timeonday >= evening)
        {
            if (!_lamps[0].activeInHierarchy)
            {
                foreach (var l in _lamps)
                {
                    l.SetActive(true);
                }
            }
            sun.intensity = Mathf.MoveTowards(sun.intensity, minIntensity, (maxIntensity - minIntensity) / ((night - evening) / deltaT) / 60 * Time.deltaTime);
        }
        else
        {
            sun.intensity = Mathf.MoveTowards(sun.intensity, maxIntensity, (maxIntensity-minIntensity)/((day-morning) / deltaT) / 60 * Time.deltaTime);
        }
        xRotation = Mathf.MoveTowards(xRotation, endRotation, (endRotation - startRotation) / ((night - morning ) / deltaT) / 60*Time.deltaTime);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
