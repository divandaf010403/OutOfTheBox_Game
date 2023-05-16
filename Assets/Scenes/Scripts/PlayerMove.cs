using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    public CharacterController controller;
    public float speed = 12f;
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
    public RainController rain;
    bool isRain = false;
    public GameObject setRain;
    public GameObject playerRainTrigger;
    public Image centerDot;

    // Start is called before the first frame update
    void Start()
    {
        GameObject go = GameObject.Find("Center Dot");
        centerDot = go.GetComponent<Image>();
        Physics.queriesHitBackfaces = true;
        setRain.SetActive(false);
        centerDot.gameObject.SetActive(true);
        playerRainTrigger.SetActive(false);
        pickItemText.gameObject.SetActive(false);
        textNotification.gameObject.SetActive(false);
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

        Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;

        controller.Move(move * speed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        //Rain
        RainControll();

        //Take Item
        PickItem();

        //Read Note
        ReadNote();
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("Kehujanan");
        StartCoroutine(TimeWet(1));
    }

    void RainControll()
    {
        if (Input.GetKeyDown(KeyCode.Q) && isRain == false)
        {
            setRain.SetActive(true);
            playerRainTrigger.SetActive(true);
            Debug.Log("hujan");
            isRain = true;
            //TakeWet(10);
        }
        else if (Input.GetKeyDown(KeyCode.Q) && isRain == true)
        {
            setRain.SetActive(false);
            playerRainTrigger.SetActive(false);
            Debug.Log("berhenti");
            isRain = false;
            //TakeWet(0);
        }
    }

    void TakeWet(float wet)
    {
        currentWet = wet;
        rain.SetHealth(currentWet);
        //Mathf.SmoothDamp(rain.SetHealth(currentWet), minWet, ref currentWet, 100 * Time.deltaTime);
    }

    public void PickItem()
    {
        if (Physics.Raycast(CameraPos.position, CameraPos.forward, out pickHit, MaxPickDistance, PickLayer))
        {
            pickItemText.text = "Interact \"E\"";
            pickItemText.gameObject.SetActive(true);
            centerDot.gameObject.SetActive(false);
            pickItemText.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
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
            centerDot.gameObject.SetActive(true);
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
                noteItemText.text = "Interact \"E\"";
                noteItemText.gameObject.SetActive(true);
                noteItemText.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
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
        currentWet = wet;
        rain.SetHealth(currentWet);
        yield return new WaitForSeconds(5);
    }

    void HighLightCrosshair(bool on)
    {
        if (on)
        {
            centerDot.color = Color.red;
        }
        else
        {
            centerDot.color = Color.white;
        }
    }
}
