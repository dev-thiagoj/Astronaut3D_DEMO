using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public TrailRenderer trailRenderer;
    public float timeToDestroy = 1f;

    public int damageAmount = 1;
    public float speed = 50;
    public GameObject projectile;

    [Header("Tags")]
    public List<string> tagsToHit;
    public string tagToIgnore;

    [Header("Particles")]
    public List<string> tagsToEnvironment;
    public ParticleSystem particleSystem;
    public int intParticles = 15;

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
        Destroy(gameObject, timeToDestroy);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        foreach (var t in tagsToHit)
        {
            if (collision.transform.CompareTag(t))
            {
                var damageable = collision.transform.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    Vector3 dir = collision.transform.position - transform.position;
                    dir = -dir.normalized;
                    dir.y = 0;

                    damageable.Damage(damageAmount, dir);

                    Destroy(gameObject);
                }

                break;
            }
        }

        foreach (var e in tagsToEnvironment)
        {
            if (collision.transform.CompareTag(e))
            {
                if (particleSystem != null)
                {
                    particleSystem.Play();
                }
            }
        }

        if (collision.transform.CompareTag(tagToIgnore)) return;

        if(meshRenderer != null) meshRenderer.enabled = false;
        if(trailRenderer != null) trailRenderer.enabled = false;
        Destroy(gameObject, 1f);
    }
}