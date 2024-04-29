using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Madificateur")]
    [SerializeField] float speed = 2;
    [SerializeField] int dmg = 1;

    private Transform cible;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void AssignTarget(Transform target)
    {
        if (target == null) return;   
        cible = target;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (cible == null) Destroy(this.gameObject);
        Vector2 direction = (cible.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"{collision.gameObject.name}");
        Enemy e = collision.gameObject.GetComponent<Enemy>();
        if (e == null) { return; }
        e.Attack(dmg);
        
        Destroy(gameObject);
    }
}
