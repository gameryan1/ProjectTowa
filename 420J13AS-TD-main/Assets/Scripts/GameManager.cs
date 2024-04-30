using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


// Aujourd'hui
// 1) Pratique: objet qui visite une liste de destinations
// 2) Vague d'ennemis qui suit le chemin le plus court
// 3) Détecter un chemin bloqué

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameTilePrefab;
    [SerializeField] GameObject[] enemyPrefab;
    GameTile[,] gameTiles;
    private GameTile spawnTile;
    public static GameManager instance;
     int ColCount = 20;
     int RowCount = 10;
    private int currentLevel = 1;
    public int startX = 0;
    public int startY = 2;

    public int endX = 20;
    public int endY = 9;
    [Header ("Wave Config")]
    [SerializeField] private int baseEnemy = 8;
    [SerializeField] private float secondeEntreEnnemy = 0.5f;
    [SerializeField] private float restAfterWave = 6f;
    [SerializeField] private float difficulty = 0.75f;

    public static UnityEvent ennemyDestroy = new UnityEvent();

    public int currentWave = 1;
    private float lastSpawn;
    public int ennemyEnVie;
    public int ennemyASpawn;
   public bool EnVague = false;

    
    public GameTile TargetTile { get; internal set; }
    List<GameTile> pathToGoal = new List<GameTile>();

    public void LevelChange(int level)
    {
        if (currentLevel != level && level == 1)
        {
            ChangeToLvl1();
        }
        if (currentLevel != level && level == 2)
        {
            ChangeToLvl2();
        }
        
    }

    public void UpDifficulty()
    {
        difficulty += 0.05f;
    }
    
    private void OnenemyDestroy()
    {
        ennemyEnVie--;
    }
    private IEnumerator WaveSart()
    {
        yield return new WaitForSeconds(restAfterWave);

        EnVague = true;
        ennemyASpawn = EnemyParVague();
      
    }
    private void EndWave()
    {
        EnVague = false;
        lastSpawn = 0;
        Player.player.GetXP();
        currentWave++;
       StartCoroutine(WaveSart());
    }

    private int EnemyParVague()
    {
        return Mathf.RoundToInt(baseEnemy* Mathf.Pow(currentWave,difficulty));
    }

    private void Awake()
    {
        instance = this;
        ennemyDestroy.AddListener(OnenemyDestroy);
        gameTiles = new GameTile[ColCount, RowCount];

        for (int x = 0; x < ColCount; x++)
        {
            for (int y = 0; y < RowCount; y++)
            {
                var spawnPosition = new Vector3(x, y, 0);
                var tile = Instantiate(gameTilePrefab, spawnPosition, Quaternion.identity);
                gameTiles[x, y] = tile.GetComponent<GameTile>();
                gameTiles[x, y].GM = this;
                gameTiles[x, y].X = x;
                gameTiles[x, y].Y = y;

                if ((x + y) % 2 == 0)
                {
                    gameTiles[x, y].TurnGrey();
                }
            }
        }

        
       
        for(int y = 0; y <= 8; y++)
        {
            gameTiles[3, y].SetWall();
        }
        for(int y = 9; y>= 1 ; y--)
        {
            gameTiles[11,y].SetWall();
        }
    }

    public void Start()
    {
        foreach (var cas in gameTiles)
        {
            if (cas.X == startX && cas.Y == startY)
            {
                spawnTile = cas;
            }

            if ( cas.X == endX && cas.Y == endY)
            {
                TargetTile = cas;
            }
        }
        spawnTile.SetEnemySpawn();
        var path = Pathfinding(spawnTile, TargetTile);
        var tile = TargetTile;
        while (tile != null)
        {
            pathToGoal.Add(tile);
            tile.SetPath(true);
            tile = path[tile];
        }
        StartCoroutine( WaveSart());
        
    }
    private void ChangeToLvl1()
    {

        StopAllCoroutines();
        for (int x = 0; x < ColCount; x++)
        {
            for (int y = 0; y < RowCount; y++)
            {
                Destroy(gameTiles[x, y].gameObject);

            }
        }
        ColCount = 20;
        RowCount = 10;
        gameTiles = new GameTile[ColCount, RowCount];

        for (int x = 0; x < ColCount; x++)
        {
            for (int y = 0; y < RowCount; y++)
            {
                var spawnPosition = new Vector3(x, y, 0);
                var cile = Instantiate(gameTilePrefab, spawnPosition, Quaternion.identity);
                gameTiles[x, y] = cile.GetComponent<GameTile>();
                gameTiles[x, y].GM = this;
                gameTiles[x, y].X = x;
                gameTiles[x, y].Y = y;

                if ((x + y) % 2 == 0)
                {
                    gameTiles[x, y].TurnGrey();
                }
            }
            foreach (var cas in gameTiles)
            {
                if (cas.X == startX && cas.Y == startY)
                {
                    spawnTile = cas;
                }

                if (cas.X == endX && cas.Y == endY)
                {
                    TargetTile = cas;
                }
            }
            spawnTile.SetEnemySpawn();
            var path = Pathfinding(spawnTile, TargetTile);
            var tile = TargetTile;
            while (tile != null)
            {
                pathToGoal.Add(tile);
                tile.SetPath(true);
                tile = path[tile];
            }
            StartCoroutine(WaveSart());

        }



        for (int y = 0; y <= 8; y++)
        {
            gameTiles[3, y].SetWall();
        }
        for (int y = 9; y >= 1; y--)
        {
            gameTiles[11, y].SetWall();
        }
        currentLevel = 1;
    }
    private void ChangeToLvl2()
    {
        StopAllCoroutines();
        for (int x = 0; x < ColCount; x++)
        {
            for (int y = 0; y < RowCount; y++)
            {
                Destroy(gameTiles[x, y].gameObject);

            }
        }
        ColCount = 10;
        RowCount = 10;
        gameTiles = new GameTile[ColCount, RowCount];

        for (int x = 0; x < ColCount; x++)
        {
            for (int y = 0; y < RowCount; y++)
            {
                var spawnPosition = new Vector3(x, y, 0);
                var cile = Instantiate(gameTilePrefab, spawnPosition, Quaternion.identity);
                gameTiles[x, y] = cile.GetComponent<GameTile>();
                gameTiles[x, y].GM = this;
                gameTiles[x, y].X = x;
                gameTiles[x, y].Y = y;

                if ((x + y) % 2 == 0)
                {
                    gameTiles[x, y].TurnGrey();
                }
            }
        }



        for (int x = 0; x <= 8; x++)
        {
            gameTiles[x, 6].SetWall();
        }
        for (int x = 9; x >= 1; x--)
        {
            gameTiles[x, 8].SetWall();
        }
        currentLevel = 2;

         startX = 4;
        startY = 0;

         endX = 4;
        endY = 9;

        foreach (var cas in gameTiles)
        {
            if (cas.X == startX && cas.Y == startY)
            {
                spawnTile = cas;
            }

            if (cas.X == endX && cas.Y == endY)
            {
                TargetTile = cas;
            }
        }
        spawnTile.SetEnemySpawn();
        var path = Pathfinding(spawnTile, TargetTile);
        var tile = TargetTile;
        while (tile != null)
        {
            pathToGoal.Add(tile);
            tile.SetPath(true);
            tile = path[tile];
        }
        StartCoroutine(WaveSart());


    }

    private void Update()
    {
        if (!EnVague) return;

        lastSpawn += Time.deltaTime;

        if (lastSpawn >= (1f / secondeEntreEnnemy) && ennemyASpawn > 0)
        {
            SpawnE();
            lastSpawn = 0f;
        }
        if (ennemyASpawn ==0 && ennemyEnVie == 0) EndWave();
        
           
        
        /*
        if (Input.GetKeyDown(KeyCode.Space) && TargetTile != null) 
        {
            foreach (var t in gameTiles)
            {
                t.SetPath(false);
            }

            var path = Pathfinding(spawnTile, TargetTile);
            var tile = TargetTile;

            while (tile != null)
            {
                pathToGoal.Add(tile);
                tile.SetPath(true);
                tile = path[tile];
            }
            StartCoroutine(SpawnEnemyCoroutine());
        } */
    }

    private Dictionary<GameTile, GameTile> Pathfinding(GameTile sourceTile, GameTile targetTile)
    {
        // distance minimal de la tuile a la source
        var dist = new Dictionary<GameTile, int>();

        // tuile precedente qui mene au chemin le plus court
        var prev = new Dictionary<GameTile, GameTile>();

        // liste des tuiles restante
        var Q = new List<GameTile>();

//3      for each vertex v in Graph.Vertices:
        foreach (var v in gameTiles)
        {
//4          dist[v] ← INFINITY
            dist.Add(v, 9999);

//5          prev[v] ← UNDEFINED
            prev.Add(v, null);

//6          add v to Q
            Q.Add(v);
        }

//7      dist[source] ← 0
        dist[sourceTile] = 0;

//8
//9      while Q is not empty:
        while (Q.Count > 0)
        {
//10          u ← vertex in Q with min dist[u]
            GameTile u = null;
            int minDistance = int.MaxValue;

            foreach (var v in Q)
            {
                if (dist[v] < minDistance)
                {
                    minDistance = dist[v];
                    u = v;
                }
            }

//11          remove u from Q
            Q.Remove(u);
            //12
            //13          for each neighbor v of u still in Q:
            foreach (var v in FindNeighbor(u))
            {
                if (!Q.Contains(v) || v.IsBlocked)
                {
                    continue;
                }

//14              alt ← dist[u] + Graph.Edges(u, v)
                int alt = dist[u] + 1;

//15              if alt < dist[v]:
                if (alt < dist[v])
                {
//16                  dist[v] ← alt
                    dist[v] = alt;
//17                  prev[v] ← u
                    prev[v] = u;
                }
            }
        }

//19      return dist[], prev[]
        return prev;
    }

    private List<GameTile> FindNeighbor(GameTile u)
    {
        var result = new List<GameTile>();

        if (u.X - 1 >= 0)
            result.Add(gameTiles[u.X - 1, u.Y]);
        if (u.X + 1 < ColCount)
            result.Add(gameTiles[u.X + 1, u.Y]);
        if (u.Y - 1 >= 0)
            result.Add(gameTiles[u.X, u.Y - 1]);
        if (u.Y + 1 < RowCount)
            result.Add(gameTiles[u.X, u.Y + 1]);

        return result;
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while (ennemyASpawn > 0)
        {
           // for (int i = 0; i < 5; i++)
           // {
                yield return new WaitForSeconds(secondeEntreEnnemy);
                var enemy = Instantiate(enemyPrefab[Random.Range(0, currentWave % 5)], spawnTile.transform.position, Quaternion.identity);
                enemy.GetComponent<Enemy>().SetPath(pathToGoal);
                ennemyASpawn--;
            //}
            yield return new WaitForSeconds(2f);
        }
    }
    
    public void SpawnE()
    {
        var enemy = Instantiate(enemyPrefab[Random.Range(0, currentWave % 5)], spawnTile.transform.position, Quaternion.identity);
        enemy.GetComponent<Enemy>().SetPath(pathToGoal);
        ennemyASpawn--;
        ennemyEnVie++;
    }
}
