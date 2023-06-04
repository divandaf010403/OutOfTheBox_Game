using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    public CharacterController controller;
    public Animator animator;
    public float speed = 5f;
    public float runSpeed = 15f;
    public float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    RaycastHit pickHit;
    bool isGrounded;
    private NoteController _noteController;
    private Doors2 doorOpen;

    [Header("Wet Controll")]
    public float minWet = 0;
    public float currentWet;
    private bool isRainDamage = false;
    private bool isNotWet = false;

    [Header("ItemAction ")]
    [SerializeField] public Transform CameraPos;
    [SerializeField] public Text pickItemText;
    [SerializeField] public Text noteItemText;
    [SerializeField] public float MaxPickDistance = 2f;
    [SerializeField] public LayerMask PickLayer;

    [Header("Inventory")]
    public Inventory inventory;
    public Text textNotification;

    [Header("Read Note")]
    //[SerializeField] private KeyCode interactKey;
    [SerializeField] public LayerMask ReadLayer;

    [Header("")]
    private RainController rain;
    public Image centerDot;

    // Start is called before the first frame update
    void Start()
    {
        GameObject go = GameObject.Find("Center Dot");
        rain = gameObject.GetComponent<RainController>();
        centerDot = go.GetComponent<Image>();
        animator = GetComponent<Animator>();
        Physics.queriesHitBackfaces = true;
        centerDot.gameObject.SetActive(true);
        pickItemText.gameObject.SetActive(false);
        textNotification.gameObject.SetActive(false);
        gameObject.GetComponent<RainController>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool isRunning = animator.GetBool("isRunning");
        bool walkingPress = Input.GetKey(KeyCode.W);
        bool runPress = Input.GetKey(KeyCode.LeftShift);

        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;

        if(!isRunning && walkingPress)
        {
            speed = 5f;
            //Debug.Log("Jalan");
            animator.SetBool("isRunning", true);
        }
        if (walkingPress && runPress)
        {
            speed = runSpeed;
            //Debug.Log("Lariiiii");
            animator.SetBool("isRunning", true);
        }
        if(isRunning && !walkingPress && !runPress)
        {
            animator.SetBool("isRunning", false);
        }

        controller.Move(move * speed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        if (isRainDamage == false && isNotWet == false)
        {
            Debug.Log("Kering");
            StartCoroutine(TimeDry(-2));
        }

        //Take Item
        PickItem();

        //Read Note
        ReadNote();

        //Starting Random Rain
        if (GameObject.FindGameObjectsWithTag("RainStart").Length == 0)
        {
            GameObject.Find("MC").GetComponent<RainController>().enabled = true;
            Debug.Log("Random Cuaca Dimulai");
        }

    }

    void OnParticleCollision(GameObject other)
    {
        if(rain != null)
        {
            if (rain.isRain == true)
            {
                if (isRainDamage == false)
                {
                    Debug.Log("Kehujanan");
                    StartCoroutine(TimeWet(5));
                }
            }
        }
    }

    public void PickItem()
    {
        if (Physics.Raycast(CameraPos.position, CameraPos.forward, out pickHit, MaxPickDistance, PickLayer))
        {
            pickItemText.text = "Interact \"E\"";
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
                noteItemText.text = "Read \"E\"";
                noteItemText.gameObject.SetActive(true);
                noteItemText.transform.position = new Vector3(Screen.width * 0.65f, Screen.height * 0.5f, 0);
                centerDot.gameObject.SetActive(false);
            }
            else
            {
                clearNote();
                noteItemText.gameObject.SetActive(false);
                centerDot.gameObject.SetActive(true);
            }
        }
        else
        {
            clearNote();
            noteItemText.gameObject.SetActive(false);
            centerDot.gameObject.SetActive(true);
        }

        if (_noteController != null)
        {
            if(Input.GetKeyDown(KeyCode.E)) {
                _noteController.ShowNote(controller);
            }
        }
    }

    void clearNote()
    {
        if (_noteController != null)
        {
            HighLightCrosshair(false);
            _noteController = null;
        }
    }

    IEnumerator UINotification(OnItems onItems)
    {
        textNotification.gameObject.SetActive(true);
        textNotification.text = "Cari Kunci Untuk Membuka Pintu";
        yield return new WaitForSeconds(5);
        textNotification.gameObject.SetActive(false);
    }

    IEnumerator UIUseKey(OnItems onItems)
    {
        textNotification.gameObject.SetActive(true);
        textNotification.text = "Pintu Terbuka";
        yield return new WaitForSeconds(5);
        textNotification.gameObject.SetActive(false);
    }

    IEnumerator TimeWet(int wet)
    {
        isRainDamage = true;

        yield return new WaitForSeconds(1);
        currentWet = wet;
        rain.SetHealth(currentWet);

        isRainDamage = false;
    }

    IEnumerator TimeDry(int dry)
    {
        isNotWet = true;

        yield return new WaitForSeconds(3);
        currentWet = dry;
        rain.SetHealth(currentWet);

        isNotWet = false;
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
