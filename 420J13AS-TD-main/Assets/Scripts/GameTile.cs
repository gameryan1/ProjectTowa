using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameTile : MonoBehaviour, IPointerEnterHandler,
    IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] SpriteRenderer hoverRenderer;
   [SerializeField] SpriteRenderer turretRenderer;
    [SerializeField] SpriteRenderer spawnRenderer;
    [SerializeField] SpriteRenderer wallRenderer;
    private LineRenderer lineRenderer;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool canAttack = true;
    private GameObject tower;

    public GameManager GM { get; internal set; }
    public int X { get; internal set; }
    public int Y { get; internal set; }
    public bool IsBlocked { get; private set; }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.SetPosition(0, transform.position);
        spriteRenderer = GetComponent<SpriteRenderer>();
        turretRenderer.enabled = false;
        wallRenderer.enabled = false;
        originalColor = spriteRenderer.color;
    }

    private void Update()
    {/*
        if (turretRenderer.enabled && canAttack)
        {
            Enemy target = null;
            foreach (var enemy in Enemy.allEnemies)
            {
                if (Vector3.Distance(transform.position, enemy.transform.position) < 2)
                {
                    target = enemy;
                    break;
                }
            }

            if (target != null)
            {
                StartCoroutine(AttackCoroutine(target));
            }
        } */
    }

    IEnumerator AttackCoroutine(Enemy target)
    {
      // target.GetComponent<Enemy>().Attack();
      canAttack = false;
        lineRenderer.SetPosition(1, target.transform.position);
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.2f);
        lineRenderer.enabled = false;
        yield return new WaitForSeconds(1.0f);
        canAttack = true;
    }

    internal void TurnGrey()
    {
        spriteRenderer.color = Color.gray;
        originalColor = spriteRenderer.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverRenderer.enabled = true;
       // GM.TargetTile = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverRenderer.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (tower != null) return;
        if (!TouConstructeur.instance.Canbuild()) return;
        if (TouConstructeur.instance.tourSelec == 4 && spriteRenderer.color != Color.yellow)
        {
            Player.player.DepenseCash(5);
            SetWall();
            return;
        }
        if (!IsBlocked) return;
        Shoptower temptower = TouConstructeur.instance.PrendreTourSelectionner();
        Player.player.DepenseCash(temptower.cout);
        tower = Instantiate(temptower.prefab,transform.position,Quaternion.identity);
       // turretRenderer.enabled = !turretRenderer.enabled;
       // IsBlocked = turretRenderer.enabled;
    }

    internal void SetEnemySpawn()
    {
        spawnRenderer.enabled = true;
    }

    internal void SetPath(bool isPath)
    {
        spriteRenderer.color = isPath ? Color.yellow : originalColor;
    }

    internal void SetWall()
    {
        wallRenderer.enabled = true;
        IsBlocked = true;
    }

    private void OnDestroy()
    {
        if (tower != null)
        Destroy(tower.gameObject);
    }
}
