using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class Slowmo : MonoBehaviour
{
    [Header("Modificateur")]
    [SerializeField] LayerMask layerMask;
    [SerializeField] float range;
    [SerializeField] float tps = 0.2f;
    [SerializeField] float freezetime = 2f;
    
    
    
    private float tempPourNextTir;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tempPourNextTir += Time.deltaTime;

        if (tempPourNextTir  >= 1f/tps)
        {
            Freeze();
            tempPourNextTir = 0f;
        }
    }

    private void Freeze()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, range, (Vector2)transform.position, range, layerMask);

       for(int i = 0; i < hit.Length; i++)
        {
            Enemy enemy = hit[i].transform.GetComponent<Enemy>();
            if (enemy == null) return;

            enemy.ChangerSpeed(0.5f);
            StartCoroutine(ResetSpeed(enemy));

        }
    }
    private IEnumerator ResetSpeed(Enemy en)
    {
        yield return new WaitForSeconds(freezetime);
        en.ResetSpeed();
    }
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, transform.forward, range);
    }

}
