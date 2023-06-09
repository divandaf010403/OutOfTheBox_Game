using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors2 : OnItems
{
    [Header("Rotation Configs")]
    [SerializeField] private float RotationAmount = 90f;
    [SerializeField] private float forwardDirection = 0;

    [SerializeField] private float speed;
    [SerializeField] Collider col;
    [SerializeField] private bool isRotatingDoor = true;

    public bool isOpen = false;
    private Quaternion newRotation;
    private Coroutine AnimationCorountine;

    public AudioSource doorSound;

    private Vector3 StartRotation;
    private Vector3 forward;

    public void Awake()
    {
        StartRotation = transform.rotation.eulerAngles;
        forward = transform.right;
    }

    private void Start()
    {
        doorSound = GetComponent<AudioSource>();
    }

    private void Update()
    {

    }

    public override void OnUseItem()
    {
        Debug.Log("Item Digunakan");
        removeRequire();
    }

    public override void OnInteract()
    {
        OnUse();
        //removeInteract();
    }

    public void OnUse()
    {
        if(isOpen == false)
        {
            Open(-transform.position);
            doorSound.Play();
            isOpen = true;
        }
    }

    public void Open(Vector3 PlayerPosition)
    {
        if (!isOpen)
        {
            if (AnimationCorountine != null)
            {
                StopCoroutine(AnimationCorountine);
            }

            if (isRotatingDoor)
            {
                float dot = Vector3.Dot(forward, (PlayerPosition - transform.position).normalized);
                Debug.Log($"Dot : {dot.ToString("N3")}");
                AnimationCorountine = StartCoroutine(DoRotationOpen(dot));
            }
        }
    }

    private IEnumerator DoRotationOpen(float ForwardAmount)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if (ForwardAmount >= forwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y - RotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y + RotationAmount, 0));
        }

        isOpen = true;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }
}
