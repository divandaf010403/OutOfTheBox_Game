using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    public CharacterController controller;
    public Animator animator;
    public float walkSpeed = 5f;
    public bool isRunning = false;

    public float gravity = -9.8f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

        //public AudioSource walkingAudio;
        //public AudioSource runningAudio;

    Vector3 velocity;
    RaycastHit pickHit;
    bool isGrounded;
    private NoteController _noteController;
    private Doors2 doorOpen;

    [Header("Wet Controll")]
    public float minWet = 0;
    public float currentWet;
    public bool isRainFunc = false;
    public bool isRainDamage = false;
    private bool isNotWet = false;

    [Header("ItemAction ")]
    [SerializeField] public Transform CameraPos;
    [SerializeField] public Text pickItemText;
    [SerializeField] public Text noteItemText;
    [SerializeField] public float MaxPickDistance = 2f;
    [SerializeField] public LayerMask PickLayer;

    [Header("Inventory")]
    public Inventory inventory;
    public Text hintTitle;
    public Text textNotification;

    [Header("Read Note")]
    //[SerializeField] private KeyCode interactKey;
    [SerializeField] public LayerMask ReadLayer;

    [Header("Respawn")]
    public Vector3 respawnPoint;

    [Header("")]
    private RainController rain;
    public Image centerDot;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        GameObject go = GameObject.Find("Center Dot");
        rain = gameObject.GetComponent<RainController>();
        centerDot = go.GetComponent<Image>();
        animator = GetComponent<Animator>();

        Physics.queriesHitBackfaces = true;

        centerDot.gameObject.SetActive(true);

        pickItemText.gameObject.SetActive(false);
        hintTitle.gameObject.SetActive(false);
        textNotification.gameObject.SetActive(false);

        gameObject.GetComponent<RainController>().enabled = false;

        respawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the character is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.4f, groundMask);

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        float speed = isRunning ? walkSpeed+2 : walkSpeed;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.W))
        {
            isRunning = false;
            animator.SetBool("isRunning", true);
            //animator.SetBool("IsWalking", true);
            //runningAudio.Stop();
            //walkingAudio.pitch = 0.2f;
            //walkingAudio.Play();
        }

        if (Input.GetKey(KeyCode.W) && (Input.GetKey(KeyCode.LeftShift)))
        {
            isRunning = true;
            animator.SetBool("isRunning", true);
            //walkingAudio.Stop();
            //runningAudio.pitch = 0.5f;
            //runningAudio.Play();
        }

        //if (Input.GetKeyUp(KeyCode.LeftShift))
        //{
        //    isRunning = false;
        //    animator.SetBool("isRunning", true);
        //    //animator.SetBool("IsWalking", true);
        //}

        if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("isRunning", false);
                //walkingAudio.Stop();
                //runningAudio.Stop();
        }

        //Time for dry if character not interact with rain
        if (isNotWet == false)
        {
            if(rain.afterRain == false)
            {
                StartCoroutine(TimerCount(5));
                rain.afterRain = true;
            }
            StartCoroutine(TimeDry(-10));
        }

        //Die
        if (rain.healthBar.value == rain.healthBar.maxValue)
        {
            Debug.Log("You Die");
            transform.position = respawnPoint;
            rain.healthBar.value = rain.healthBar.minValue;
            rain.isFunctionCall = false;
            rain.isRain = false;
            rain.rainPS.SetActive(false);
            rain.rainTrigger.SetActive(false);
            rain.conditionNow.text = "Sunny Weather";
            rain.audioHujan.Stop();
            rain.isFunctionCall = false;
        }

        //Take Item
        PickItem();

        //Read Note
        ReadNote();

        //Starting Random Rain
        if (GameObject.FindGameObjectsWithTag("RainStart").Length == 0)
        {
            GameObject.Find("MC").GetComponent<RainController>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Checkpoint")
        {
            Debug.Log("Checkpoint lewat");
            respawnPoint = transform.position;
        }

        if (other.tag == "EndDemo")
        {
            SceneManager.LoadScene("DemoEnd");
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (rain != null)
        {
            if (rain.isRain == true)
            {
                if(!isRainFunc)
                {
                    isRainDamage = true;
                    Debug.Log("Kehujanan");
                    StartCoroutine(TimeWet(5));
                }
            }
        }
    }

    IEnumerator TimeWet(int wet)
    {
        isRainFunc = true;

        yield return new WaitForSeconds(1);
        currentWet = wet;
        rain.SetHealth(currentWet);

        isRainFunc = false;
        isRainDamage = false;
    }

    IEnumerator TimeDry(int dry)
    {
        isNotWet = true;

        if (!isRainDamage)
        {
            yield return new WaitForSeconds(1);
            currentWet = dry;
            rain.SetHealth(currentWet);
        }

        isNotWet = false;
    }

    IEnumerator TimerCount(int time)
    {
        yield return new WaitForSeconds(time);
    }

    public void PickItem()
    {
        if (Physics.Raycast(CameraPos.position, CameraPos.forward, out pickHit, MaxPickDistance, PickLayer))
        {
            pickItemText.text = "Interact (E)";
            pickItemText.gameObject.SetActive(true);
            pickItemText.transform.position = new Vector3(Screen.width * 0.65f, Screen.height * 0.5f, 0);
            OnItems onItems = pickHit.transform.gameObject.GetComponent<OnItems>();
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(onItems.requiereItem != null)
                {
                    if(inventory.searchItem(onItems.requiereItem))
                    {
                        DataItem searchedItem = inventory.searchItem(onItems.requiereItem);
                        onItems.OnUseItem();
                        StartCoroutine(UIUseKey(onItems));
                        inventory.RemoveItem(searchedItem);
                    }
                    else
                    {
                        StartCoroutine(UINotification(onItems));
                        Debug.Log(onItems.gameObject.name + " membutuhkan item " + onItems.requiereItem.items.m_name);
                    }
                }
                else
                {
                    onItems.OnInteract();
                }
                Debug.Log("Interaksi Objek: " + pickHit.transform.gameObject.name);
            }
        }
        else
        {
            pickItemText.gameObject.SetActive(false);
        }
    }

    public void ReadNote()
    {
        if (Physics.Raycast(CameraPos.position, CameraPos.forward, out pickHit, MaxPickDistance, ReadLayer))
        {
            var readableNote = pickHit.collider.GetComponent<NoteController>();
            if (readableNote != null)
            {
                _noteController = readableNote;
                //HighLightCrosshair(true);
                noteItemText.text = "Read (E)";
                noteItemText.gameObject.SetActive(true);
                noteItemText.transform.position = new Vector3(Screen.width * 0.65f, Screen.height * 0.5f, 0);
            }
            else
            {
                clearNote();
                noteItemText.gameObject.SetActive(false);
            }
        }
        else
        {
            clearNote();
            noteItemText.gameObject.SetActive(false);
        }

        if (_noteController != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _noteController.ShowNote(controller);
            }
        }
    }

    void clearNote()
    {
        if (_noteController != null)
        {
            //HighLightCrosshair(false);
            _noteController = null;
        }
    }

    IEnumerator UINotification(OnItems onItems)
    {
        hintTitle.gameObject.SetActive(true);
        textNotification.gameObject.SetActive(true);
        textNotification.text = "Find Key To Open The Door";
        yield return new WaitForSeconds(5);
        hintTitle.gameObject.SetActive(false);
        textNotification.gameObject.SetActive(false);
    }

    IEnumerator UIUseKey(OnItems onItems)
    {
        hintTitle.gameObject.SetActive(true);
        textNotification.gameObject.SetActive(true);
        textNotification.text = "Door Open";
        yield return new WaitForSeconds(5);
        hintTitle.gameObject.SetActive(false);
        textNotification.gameObject.SetActive(false);
    }

    void HighLightCrosshair(bool on)
    {
        if (on)
        {
            centerDot.color = Color.white;
        }
        else
        {
            centerDot.color = Color.white;
        }
    }
}
