using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    public GameObject objectiveTextObject;
    public Text objectiveText;
    [SerializeField] public string description;
    [SerializeField] public bool complete;
    //public GameObject objectiveComplete;

    // Start is called before the first frame update
    void Start()
    {
        complete = false;
        objectiveText = objectiveText.GetComponent<Text>();
        objectiveTextObject.SetActive(false);
        objectiveText.gameObject.SetActive(false);
        //objectiveComplete.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            objectiveTextObject.SetActive(true);
            objectiveText.gameObject.SetActive(true);
            objectiveText.text = description.ToString();
            //objectiveComplete.SetActive(true);
            Destroy(gameObject);
        }
    }

}
