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
    public Text conditionNow;

    [Header("Weather VFX")]
    [SerializeField] public GameObject rainPS;
    [SerializeField] public GameObject rainTrigger;

    // Start is called before the first frame update
    void Start()
    {
        rainPS.SetActive(false);
        rainTrigger.SetActive(false);
        conditionNow.text = "Sunny Weather";
    }

    // Update is called once per frame
    void Update()
    {
        //Rain
        //RainControll();

        //Random Weather
        if(isFunctionCall == false)
        {
            StartCoroutine(randomWeather());
        }
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
        int rainTime = Random.Range(15, 20);
        int longRain = Random.Range(15, 20);

        isFunctionCall = true;

        yield return new WaitForSeconds(rainTime);
        isRain = true;
        rainPS.SetActive(true);
        rainTrigger.SetActive(true);
        conditionNow.text = "Rainy Weather";
        yield return new WaitForSeconds(longRain);
        isRain = false;
        rainPS.SetActive(false);
        rainTrigger.SetActive(false);
        conditionNow.text = "Sunny Weather";

        isFunctionCall = false;
    }

    //void RainControll()
    //{
    //    if (Input.GetKeyDown(KeyCode.Q) && isRain == false)
    //    {
    //        rainPS.SetActive(true);
    //        rainTrigger.SetActive(true);
    //        Debug.Log("hujan");
    //        isRain = true;
    //    }
    //    else if (Input.GetKeyDown(KeyCode.Q) && isRain == true)
    //    {
    //        rainPS.SetActive(false);
    //        rainTrigger.SetActive(false);
    //        Debug.Log("berhenti");
    //        isRain = false;
    //    }
    //}
}
