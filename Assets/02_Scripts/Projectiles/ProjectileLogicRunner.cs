using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(10)]
[RequireComponent(typeof(ProjectileCollisionDamageBehaviour))]
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
                timer = 0f,
                hitCount = 0,
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Logic?.CallbackAtOnTriggerEnter2D(ref context, collider);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        Logic?.CallbackAtOnTriggerStay2D(ref context, collider);
    }
}
