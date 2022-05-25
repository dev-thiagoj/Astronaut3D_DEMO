using UnityEngine;

public class ItemRotate : MonoBehaviour
{
    public float speed = 1f;
    public bool canRotate;

    private void Update()
    {
        if (canRotate) StartRotate();
    }

    public void StartRotate()
    {
        transform.Rotate(100 * speed * Time.deltaTime * Vector3.up);
    }
}
