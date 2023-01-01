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
    public Tilemap avoidTileMap;
    public Tilemap groundTileMap;
    public Tilemap presentTileMap;
    public RuleTile groundTile;
    public Tile dirtGroundTile;
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
        if (tpToPlayer)
            transform.position = playerTransform.position;

        SpawnPresents();
    }

    private void Update()
    {
        if (tpToPlayer)
            transform.position = playerTransform.position;
    }

    // runs when the player collects a present -> new presents can spawn
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

    // Spawns presents until until maximum presents are spawned or until maximum attempts reached (performance)
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
            newPresent.SpawnTile(out bool success);

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

    // gets a random position inside of given radius but outside of given avoid radius
    private Vector2Int GetRandomPosition(int xMin, int xMax, int yMin, int yMax)
    {
        Vector2Int randomPosition = new Vector2Int();

        Vector3Int pos = presentTileMap.WorldToCell(transform.position);

        // Initialize radius
        generateRandomPosition:
        Vector2Int minPos = new Vector2Int(pos.x - xMin, pos.y - yMin);
        Vector2Int maxPos = new Vector2Int(pos.x + xMax, pos.y + yMax);

        // Generate a random position inside of the given radius
        randomPosition.x = Random.Range(minPos.x, maxPos.x);
        randomPosition.y = Random.Range(minPos.y, maxPos.y);

        // initialize avoid radius
        Vector2Int minAvoidRadius = new Vector2Int(pos.x - avoidRadius, pos.y - avoidRadius);
        Vector2Int maxAvoidRadious = new Vector2Int(pos.x + avoidRadius, pos.y + avoidRadius);

        // make sure generated position isn't inside of avoid radius
        if ((randomPosition.x >= minAvoidRadius.x && randomPosition.x <= maxAvoidRadious.x) && 
            (randomPosition.y >= minAvoidRadius.y    && randomPosition.y <= maxAvoidRadious.y))
        {
            goto generateRandomPosition;
        }

        return randomPosition;
    }

    // CHECKS IF LOCATION FOR A PRESENT IS VALID
    bool IsValidPos(int x, int y)
    {
        bool isValid = false;

        if ((groundTileMap.GetTile(new Vector3Int(x, y - 1)) == groundTile || 
            groundTileMap.GetTile(new Vector3Int(x, y - 1)) == dirtGroundTile) &&
            avoidTileMap.GetTile(new Vector3Int(x, y)) == null &&
            groundTileMap.GetTile(new Vector3Int(x, y)) == null &&
            !presentLocations.Contains(new Vector2Int(x, y)) &&
            !presentLocations.Contains(new Vector2Int(x + 1, y)) && !presentLocations.Contains(new Vector2Int(x - 1, y)))
        {
            isValid = true;
        }
        else
        {
            isValid = false;
        }

        return isValid;
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

        public void SpawnTile(out bool success)
        {
            // check if given location is valid for a present
            if  (!instance.IsValidPos(x, y)) 
            { 
                success = false;
                return;
            }

            // spawn the present
            var newTile = Instantiate(prefabTile, grid.transform);
            newTile.transform.position = grid.CellToWorld(new Vector3Int(x, y)) + new Vector3(0.625f, 0.625f);
            
            // successfully spawned present
            success = true;
        }
    }
}
