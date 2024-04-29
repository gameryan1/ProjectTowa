using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject visual;
    public static HashSet<Enemy> allEnemies = new HashSet<Enemy>();
    private Stack<GameTile> path = new Stack<GameTile>();
    [Header("Modificateur")]
    [SerializeField] int hp = 20;
    [SerializeField] float baseSpeed = 2;
    public float movementSpeed;
    [SerializeField] int argentGagner = 1;

    private void Awake()
    {
        allEnemies.Add(this);
        movementSpeed = baseSpeed;
    }

    internal void SetPath(List<GameTile> pathToGoal)
    {

        path.Clear();
        foreach (GameTile tile in pathToGoal)
        {
            path.Push(tile);
        }
    }

    void Update()
    {
        if (path.Count > 0)
        {
            Vector3 destPos = path.Peek().transform.position;
            transform.position = Vector3.MoveTowards(transform.position, destPos, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, destPos) < 0.01f)
            {
                path.Pop();
            }
        }
        else
        {
            Player.hp--;
            Die();
        }
    }

    private void Die()
    {
        allEnemies.Remove(this);
        Player.player.GagnerCash(argentGagner);
        GameManager.ennemyDestroy.Invoke();
        Destroy(gameObject);
    }

    

    internal void Attack(int dmg)
    {
        if (hp - dmg <= 0)
        {
            Die();
        }
        else
        {
            hp -= dmg;
           // visual.transform.localScale *= 0.9f;
        }
    }
    public void ChangerSpeed(float nextSpeed)
    {
        movementSpeed = nextSpeed;
    }
    public void ResetSpeed()
    {
        movementSpeed = baseSpeed;
    }
}
