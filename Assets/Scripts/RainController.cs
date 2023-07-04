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
    public bool isPetir = false;
    public Text conditionNow;

    [Header("Weather VFX")]
    [SerializeField] public GameObject rainPS;
    [SerializeField] public GameObject rainTrigger;

    public AudioSource audioHujan;
    public AudioSource audioPetir;

    public bool afterRain = false;

    // Start is called before the first frame update
    void Start()
    {
        rainPS.SetActive(false);
        rainTrigger.SetActive(false);
        conditionNow.text = "Sunny Weather";
    }

    // Update is called once per frame
    void FixedUpdate()
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
        int rainTime = Random.Range(40, 50);
        int longRain = Random.Range(30, 40);
        int petir = 6;

        isFunctionCall = true;

        yield return new WaitForSeconds(rainTime);
        isPetir = true;
        audioPetir.Play();
        yield return new WaitForSeconds(petir);
        isPetir = false;
        audioPetir.Stop();
        isRain = true;
        rainPS.SetActive(true);
        rainTrigger.SetActive(true);
        audioHujan.Play();
        conditionNow.text = "Rainy Weather";
        yield return new WaitForSeconds(longRain);
        yield return FadeOutAudioSource(audioHujan, 3f);
        audioHujan.Stop();
        isRain = false;
        rainPS.SetActive(false);
        rainTrigger.SetActive(false);
        conditionNow.text = "Sunny Weather";
        afterRain = false;

        isFunctionCall = false;
    }

    IEnumerator FadeOutAudioSource(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;
        float currentTime = 0;

        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, currentTime / fadeTime);
            yield return null;
        }

        audioSource.Stop();
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
