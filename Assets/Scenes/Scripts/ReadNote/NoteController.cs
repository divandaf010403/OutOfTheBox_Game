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

    private FirstPersonCamera _firstPersonCam;

    // Start is called before the first frame update
    void Start()
    {
        noteCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (isOpen)
        //{
        //    if(Input.GetKeyDown(KeyCode.E))
        //    {
        //        DisableNote();
        //    }
        //}
    }

    public void ShowNote(CharacterController player)
    {
        if (isOpen)
        {
            noteCanvas.SetActive(false);
            player.enabled = true;
            isOpen = false;
            //_firstPersonCam.enabled = true;
            GameObject.Find("Main Camera").GetComponent<FirstPersonCamera>().enabled = true;
        }
        else
        {
            noteTextArea.text = noteText;
            noteCanvas.SetActive(true);
            noteCanvas.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            openEvent.Invoke();
            player.enabled = false;
            isOpen = true;
            //if(_firstPersonCam != null)
            //{
            //    _firstPersonCam.enabled = false;
            //}
            GameObject.Find("Main Camera").GetComponent<FirstPersonCamera>().enabled = false;
        }
    }

    //void DisableNote()
    //{
        
    //}

    //void DisablePlayer(bool disable)
    //{
    //    player.enabled = !disable;
    //}
}
