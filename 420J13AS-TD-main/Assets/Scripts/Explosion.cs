using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Modificateur")]
    [SerializeField] LayerMask layerMask;
    [SerializeField] float range;
    [SerializeField] int exploDmg = 20;
    [SerializeField] bool Isennemy;
    // Start is called before the first frame update
    private void OnDestroy()
    {
        Explo___Sion();
    }
    private void Explo___Sion()
    {
        RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, range, (Vector2)transform.position, range, layerMask);

        for (int i = 0; i < hit.Length; i++)
        {
            if (!Isennemy)
            {
                Enemy enemy = hit[i].transform.GetComponent<Enemy>();
                if (enemy == null) return;
                enemy.Attack(exploDmg);
            }
            else
            {
                BaseTower tour = hit[i].transform.GetComponent<BaseTower>();
                if (tour == null) return;
                Destroy(tour.gameObject);
            }


        }
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, transform.forward, range);
    }
}
