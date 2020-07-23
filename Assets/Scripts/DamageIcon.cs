using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIcon : MonoBehaviour
{
    public Sprite[] Sprites;
    public SpriteRenderer SR;
    public GameObject DamageDestroyEffect;

    public float lifeTime;

    private void Start()
    {
        Invoke("Destruction", lifeTime);
    }

    public void Setup(int damage)
    {
        SR.sprite = Sprites[damage];
    }

    public void Destruction()
    {
        Instantiate(DamageDestroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
