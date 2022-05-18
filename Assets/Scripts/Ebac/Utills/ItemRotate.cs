using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    public float speed = 1f;
    public bool canRotate;
    //public GameObject gameObject;

    private void Update()
    {
        if (canRotate) StartRotate();
    }

    public void StartRotate()
    {
        transform.Rotate(Vector3.up * speed * 100 * Time.deltaTime);
    }
}
