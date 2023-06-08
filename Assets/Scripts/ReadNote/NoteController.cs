using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NoteController : MonoBehaviour
{

    //public CharacterController player;

    [Header("UI Text")]
    [SerializeField] private GameObject noteCanvas;
    [SerializeField] Text noteTextArea;

    [Space(10)]
    [SerializeField] [TextArea] private string noteText;

    [Space(10)]
    [SerializeField] private UnityEvent openEvent;
    private bool isOpen = false;

    [Space(10)]
    public bool itemSpawn = false;
    public Rigidbody prefabItem;
    public Transform spawnPos;

    private FirstPersonCamera _firstPersonCam;

    // Start is called before the first frame update
    void Start()
    {
        noteCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowNote(CharacterController player)
    {
        if (isOpen)
        {
            noteCanvas.SetActive(false);
            player.enabled = true;
            isOpen = false;
            GameObject.Find("Main Camera").GetComponent<FirstPersonCamera>().enabled = true;
            if(itemSpawn == false)
            {
                spawningItem();
            }
        }
        else
        {
            noteTextArea.text = noteText;
            noteCanvas.SetActive(true);
            noteCanvas.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            openEvent.Invoke();
            player.enabled = false;
            isOpen = true;
            GameObject.Find("Main Camera").GetComponent<FirstPersonCamera>().enabled = false;
        }
    }

    void spawningItem()
    {
        if (itemSpawn == false && prefabItem != null)
        {
            Rigidbody rb;
            rb = Instantiate(prefabItem, spawnPos.position, spawnPos.rotation) as Rigidbody;
            itemSpawn = true;
        }
    }
}
