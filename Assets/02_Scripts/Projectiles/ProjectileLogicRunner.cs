using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogicRunner : MonoBehaviour
{
    public ProjectileLogicBase Logic { set; private get; }
    private ProjectileInstanceContext context;
    public ProjectileInstanceContext Context => context;
    public ProjectileInstanceInitializationData InitData { set; private get; }

    private void Awake()
    {
        context =
            new ProjectileInstanceContext
            {
                obj = gameObject,
                anim = gameObject.GetComponent<Animator>(),
                sr = gameObject.GetComponent<SpriteRenderer>(),
                cc = gameObject.GetComponent<CircleCollider2D>(),
                rb = gameObject.GetComponent<Rigidbody2D>(),
                timer = 0f
            };
    }

    private void OnEnable()
    {
        Logic?.CallbackAtOnEnable(ref context, InitData);
    }

    //private void OnDisable()
    //{
    //    logic?.CallbackAtOnDisable(Context);
    //}

    private void OnDrawGizmos()
    {
        Logic?.CallbackOnDrawGizmos(ref context);
    }

    private void FixedUpdate()
    {
        Logic?.CallbackAtFixedUpdate(ref context);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Logic?.CallbackAtOnCollisionEnter2D(ref context, collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Logic?.CallbackAtOnCollisionStay2D(ref context, collision);
    }
}
