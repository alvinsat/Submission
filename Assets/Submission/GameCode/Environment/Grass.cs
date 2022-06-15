using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour, IDamageable
{
    public ParticleSystem leafParticle;

    Animator anim;


    void Start() {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (transform.position.x - col.transform.position.x > 0) 
        {
            anim.Play("MovingGrassL");
        }
        else 
        {
            anim.Play("MovingGrassR");
        }
    }

    public void ApplyDamage(float damage)
    {
        Instantiate(leafParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void ApplyDamage(float damage, Vector3 position)
    {
        // Interface not implemented
    }
}