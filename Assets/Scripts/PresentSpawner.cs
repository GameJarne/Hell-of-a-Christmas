using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PresentSpawner : MonoBehaviour
{
    public static PresentSpawner instance;

    List<Vector2Int> presentLocations = new List<Vector2Int>();

    public bool tpToPlayer = true;
    public Transform playerTransform;

    [Header("Radius")]
    public int radius = 50;
    public int avoidRadius = 5;

    [Header("Spawning Config")]
    public int minPresents = 3;
    public int maxPresents = 20;
    public int maxSpawnAttemps = 50;

    [HideInInspector] public int currentPresentsAmount = 0;
    int currentSpawnAttempts = 0;

    [Header("Tilemap Things")]
    public Tilemap groundTileMap;
    public Tilemap presentTileMap;
    public RuleTile groundTile;
    [SerializeField] List<GameObject> presentTileTypes = new List<GameObject>();

    bool spawningPresents = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        SpawnPresents();
    }

    private void Update()
    {
        if (tpToPlayer)
            transform.position = playerTransform.position;
    }

    public void OnPresentCollected(Vector3 location)
    {
        currentPresentsAmount--;
        currentSpawnAttempts = 0;

        Vector3Int cellLoc3 = presentTileMap.WorldToCell(location);
        var cellLoc = new Vector2Int(cellLoc3.x, cellLoc3.y);
        if (presentLocations.Contains(cellLoc))
            presentLocations.Remove(cellLoc);
        
        if (!spawningPresents) { SpawnPresents(); }
    }

    void SpawnPresents()
    {
        if (currentPresentsAmount >= maxPresents || currentSpawnAttempts >= maxSpawnAttemps) { return; }

        spawningPresents = true;

        while ((currentPresentsAmount < maxPresents && currentSpawnAttempts < maxSpawnAttemps) || currentPresentsAmount < minPresents)
        {
            // get a random position for the present inside of the spawning radius
            Vector2Int position = GetRandomPosition(radius, radius, radius, radius);

            // initialize spawning present
            GameObject presentTile;
            int random = Random.Range(0, presentTileTypes.Count);
            presentTile = presentTileTypes[random];

            var newPresent = new PresentTile(position.x, position.y, presentTile, presentTileMap);

            // spawn present
            bool success = false;
            if (IsValidPos(groundTileMap, groundTile, position.x, position.y))
            {
                newPresent.SpawnTile(groundTileMap, groundTile, out success);
            }

            // check if successful and debug
            if (success) 
            { 
                Debug.Log($"SPAWNED NEW PRESENT AT: {position.x} {position.y}");
                presentLocations.Add(position);

                currentPresentsAmount++;
            }

            currentSpawnAttempts++;
        }

        spawningPresents = false;
    }

    private Vector2Int GetRandomPosition(int xMin, int xMax, int yMin, int yMax)
    {
        Vector2Int randomPosition = new Vector2Int();

        Vector3Int pos = presentTileMap.WorldToCell(transform.position);

        generateRandomPosition:
        Vector2Int minPos = new Vector2Int(pos.x - xMin, pos.y - yMin);
        Vector2Int maxPos = new Vector2Int(pos.x + xMax, pos.y + yMax);

        randomPosition.x = Random.Range(minPos.x, maxPos.x);
        randomPosition.y = Random.Range(minPos.y, maxPos.y);

        Vector2Int minAvoidRadius = new Vector2Int(pos.x - avoidRadius, pos.y - avoidRadius);
        Vector2Int maxAvoidRadious = new Vector2Int(pos.x + avoidRadius, pos.y + avoidRadius);

        if ((randomPosition.x >= minAvoidRadius.x && randomPosition.x <= maxAvoidRadious.x) && 
            (randomPosition.y >= minAvoidRadius.y    && randomPosition.y <= maxAvoidRadious.y))
        {
            goto generateRandomPosition;
        }

        return randomPosition;
    }

    // CHECKS IF LOCATION IS VALID FOR A PRESENT TO BE SPAWNED AT
    bool IsValidPos(Tilemap groundTileMap, TileBase groundTile, int x, int y)
    {
        if (groundTileMap.GetTile(new Vector3Int(x, y - 1)) == groundTile &&
            groundTileMap.GetTile(new Vector3Int(x, y)) == null &&
            !presentLocations.Contains(new Vector2Int(x, y)) && 
            !presentLocations.Contains(new Vector2Int(x + 1, y)) && !presentLocations.Contains(new Vector2Int(x - 1, y)))
        {
            return true;
        }

        return false;
    }

    [System.Serializable]
    public class PresentTile
    {
        [HideInInspector] public GameObject prefabTile;
        [Space]
        [HideInInspector] public Tilemap grid;
        public int x;
        public int y;

        public PresentTile(int x, int y, GameObject prefabTile, Tilemap grid)
        {
            this.x = x;
            this.y = y;
            this.prefabTile = prefabTile;
            this.grid = grid;
        }
        public PresentTile() { }

        // OUTS A SUCCESS BOOLEAN -> true: success ; false: failure
        public void SpawnTile(Tilemap groundTilemap, TileBase groundTile, out bool success)
        {
            // check if given location is valid for a present
            if  (!IsValidPos(groundTilemap, groundTile)) 
            { 
                success = false;
                return;
            }

            // spawn the present
            var newTile = Instantiate(prefabTile, grid.transform);
            newTile.transform.position = grid.CellToWorld(new Vector3Int(x, y)) + new Vector3(0.625f, 0.625f);
            
            success = true;
        }
        // DOES NOT OUT A SUCCESS BOOLEAN
        public void SpawnTile(Tilemap groundTilemap, TileBase groundTile)
        {
            if (!IsValidPos(groundTilemap, groundTile))
            {
                return;
            }

            var newTile = Instantiate(prefabTile, grid.transform);
            newTile.transform.position = grid.CellToWorld(new Vector3Int(x, y)) + new Vector3(0.625f, 0.625f);
        }

        // CHECKS IF LOCATION IS VALID FOR A PRESENT TO BE SPAWNED AT
        bool IsValidPos(Tilemap groundTileMap, TileBase groundTile)
        {
            bool isValid = false;

            if (groundTileMap.GetTile(new Vector3Int(x, y - 1)) == groundTile &&
                Equals(groundTileMap.GetTile(new Vector3Int(x, y)), null))
            {
                isValid = true;
            }
            
            return isValid;
        }
    }
}
