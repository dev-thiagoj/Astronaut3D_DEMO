using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public float timeToDestroy = 1f;

    public int damageAmount = 1;
    public float speed = 50;

    private void Awake()
    {   
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var damageable = collision.transform.GetComponent<IDamageable>();

        if (damageable != null) damageable.Damage(damageAmount);

        Destroy(gameObject);
    }
}
