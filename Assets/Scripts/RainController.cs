using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RainController : MonoBehaviour
{
    public Slider healthBar;
    public float currentHealth;
    public bool isRain = false;
    public bool isFunctionCall = false;

    [Header("Randomize Weather")]
    //[SerializeField] private float tickFrequency = 1f;
    //private static int currentTick = 0;
    //public static int CurrentTick => currentTick;
    //[SerializeField] private static float currentGameTime;

    [Header("Weather VFX")]
    [SerializeField] GameObject rainPS;
    [SerializeField] GameObject rainTrigger;

    [Header("Timer")]
    public Text timeCount;
    public float startTimeCount;

    // Start is called before the first frame update
    void Start()
    {
        rainPS.SetActive(false);
        rainTrigger.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Rain
        RainControll();

        //Random Weather
        if(isFunctionCall == false)
        {
            StartCoroutine(randomWeather());
        }

        //Timer
        if (startTimeCount >= 0)
        {
            startTimeCount += Time.deltaTime;
        }
        timeCount.text = startTimeCount.ToString();
    }

    public void SetMaxHealth(float health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    public void SetHealth(float health)
    {
        healthBar.value += health;
    }

    IEnumerator randomWeather()
    {
        int rainTime = Random.Range(5, 10);
        int longRain = Random.Range(5, 10);

        isFunctionCall = true;

        yield return new WaitForSeconds(rainTime);
        isRain = true;
        rainPS.SetActive(true);
        rainTrigger.SetActive(true);
        yield return new WaitForSeconds(longRain);
        isRain = false;
        rainPS.SetActive(false);
        rainTrigger.SetActive(false);

        isFunctionCall = false;
    }

    void RainControll()
    {
        if (Input.GetKeyDown(KeyCode.Q) && isRain == false)
        {
            rainPS.SetActive(true);
            rainTrigger.SetActive(true);
            Debug.Log("hujan");
            isRain = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && isRain == true)
        {
            rainPS.SetActive(false);
            rainTrigger.SetActive(false);
            Debug.Log("berhenti");
            isRain = false;
        }
    }
}
