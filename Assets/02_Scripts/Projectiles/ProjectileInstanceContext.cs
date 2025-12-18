using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ProjectileInstanceContext
{
    public GameObject obj;
    public Animator anim;
    public SpriteRenderer sr;
    public CircleCollider2D cc; // TODO: use generic collider
    public Rigidbody2D rb;
    public float timer;
}
