using UnityEngine;

public class RayToTheGround : MonoBehaviour
{
    public Vector3 currPos;
    public float distToGround;
    public float spaceToGround = .1f;
    public Collider collider;
    public Rigidbody myRb;

    private ItemRotate _itemRotate;

    private void Awake()
    {
        if (myRb == null) myRb = GetComponent<Rigidbody>();
        if (collider == null) collider = GetComponent<Collider>();
        if (_itemRotate == null) _itemRotate = GetComponent<ItemRotate>();

        if (collider != null) distToGround = collider.bounds.extents.y;
    }

    private void Update()
    {
        currPos.y = collider.transform.position.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            currPos.y = collider.transform.position.y;
            _itemRotate.canRotate = true;
            Destroy(myRb);
        }
    }
}
