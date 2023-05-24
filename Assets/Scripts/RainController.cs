using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RainController : MonoBehaviour
{
    public Slider healthBar;
    public float currentHealth;
    public bool isRain = false;

    [Header("Randomize Weather")]
    [SerializeField] private float tickFrequency = 1f;
    private static int currentTick = 0;
    private int currentWeatherTick = 0;
    public static int CurrentTick => currentTick;
    private float lastTickTime = 0;
    [SerializeField] private static float currentGameTime;

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
        if (isRain)
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

        yield return new WaitForSeconds(rainTime);
        isRain = true;
        rainPS.SetActive(true);
        rainTrigger.SetActive(true);

        if (isRain == false)
        {
            isRain = true;
            rainPS.SetActive(true);
            rainTrigger.SetActive(true);
            Debug.Log("hujan");
            yield return new WaitForSeconds(rainTime);
        }
        else
        {
            isRain = false;
            rainPS.SetActive(false);
            rainTrigger.SetActive(false);
            Debug.Log("berhenti");
            yield return new WaitForSeconds(rainTime);
        }
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
