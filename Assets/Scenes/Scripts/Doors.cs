using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Doors : OnItems
{
    public bool isOpen = false;
    [SerializeField] private bool isRotatingDoor = true;
    [SerializeField] private float speed = 1f;

    [Header("DoorAction ")]
    [SerializeField] public Text UseText;
    [SerializeField] public float MaxUseDistance = 2f;
    [SerializeField] public LayerMask UseLayer;

    [Header("Rotation Configs")]
    [SerializeField] private float RotationAmount = 90f;
    [SerializeField] private float forwardDirection = 0;

    private Vector3 StartRotation;
    private Vector3 forward;

    private Coroutine AnimationCorountine;

    public void Start()
    {
        UseText.gameObject.SetActive(false);
    }

    //public void Update()
    //{
    //    //Door Open
    //    if (Physics.Raycast(CameraPos.position, CameraPos.forward, out RaycastHit hit, MaxUseDistance, UseLayer)
    //        && hit.collider.TryGetComponent<Doors>(out Doors doors))
    //    {
    //        if (doors.isOpen == false)
    //        {
    //            UseText.text = "Open \"E\"";
    //            UseText.gameObject.SetActive(true);
    //            UseText.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
    //            //UseText.transform.rotation = Quaternion.LookRotation((hit.point - CameraPos.position).normalized);
    //            if (Input.GetKeyDown(KeyCode.E))
    //            {
    //                OnUse();
    //            }
    //        }
    //        else
    //        {
    //            UseText.gameObject.SetActive(false);
    //        }

    //    }
    //    else
    //    {
    //        UseText.gameObject.SetActive(false);
    //    }
    //}

    private void Update()
    {
        if (isOpen == false)
        {
            //UseText.text = "Open \"E\"";
            //UseText.gameObject.SetActive(true);
            //UseText.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            //UseText.transform.rotation = Quaternion.LookRotation((hit.point - CameraPos.position).normalized);
            Open(transform.position);
        }
        //else
        //{
        //    UseText.gameObject.SetActive(false);
        //}
    }

    //public void Awake()
    //{
    //    StartRotation = transform.rotation.eulerAngles;
    //    forward = transform.right;
    //}

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

    public override void OnUseItem()
    {
        Debug.Log("Pintu Terbuka");
        removeRequire();
    }

    public override void OnInteract()
    {
        isOpen = !isOpen;
    }

    //public void OnUse()
    //{
    //    if (Physics.Raycast(CameraPos.position, CameraPos.forward, out RaycastHit hit, MaxUseDistance, UseLayer))
    //    {
    //        if (hit.collider.TryGetComponent<Doors>(out Doors doors))
    //        {
    //            if (doors.isOpen == false)
    //            {
    //                Debug.Log("Pintu Terbuka");
    //                removeRequire();
    //                doors.Open(-transform.position);
    //            }
    //        }
    //    }
    //}

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