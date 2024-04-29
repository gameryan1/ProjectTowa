using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    Transform cible;
    [SerializeField] LayerMask ennemyLayer;
    
    [Header("Modificateur")]
    [SerializeField] float range;
    [SerializeField] float tps = 1f;
    [SerializeField] GameObject ballePrefab;
    public bool canshoot;
    private float tempPourNextTir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canshoot)
        {
            if (cible == null)
            { TrouveCible(); }

            if (!InRange()) { cible = null; }
            else
            {
                tempPourNextTir += Time.deltaTime;
                if (tempPourNextTir >= 1f / tps)
                {
                    Shoot();
                    tempPourNextTir = 0f;
                }
            }
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(ballePrefab,transform.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.AssignTarget(cible);
    }

    private void TrouveCible()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, range, (Vector2)transform.position, range, ennemyLayer);

        if (hit.Length >= 0)
        {
                cible = hit[0].transform;
           
        }
    }

    private bool InRange()
    {
        return Vector2.Distance(cible.position, transform.position) <= range;
    }
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position,transform.forward, range);
    }
}
