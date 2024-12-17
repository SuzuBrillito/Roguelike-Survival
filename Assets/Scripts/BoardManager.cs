using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class BoardManager : MonoBehaviour
{
    public WallObject WallPrefab;
    public WallObject2 Wall2Prefab;
    public ExistCellObject ExitPrefab;
    public EnemyObject EnemyPrefab;

    public class CellData
    {
        public bool pasable;
        public CellObject ContainObject;
    }

    private CellData[,] m_BoardData;

    private Tilemap m_Tilemap;
     

    public int Height;
    public int Width;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;

    private Grid m_Grid;
    public PlayerManager playerCont;

    public FoodObject FoodPrefab;
    public FoodObjectBig BigFoodPrefab;

    private List<Vector2Int> m_EmptyCells;

    // Start is called before the first frame update
    public void Init()
    {

        m_Tilemap = GetComponentInChildren<Tilemap>(); //busca el componente tilemap

        m_Grid = GetComponentInChildren<Grid>();

        m_BoardData = new CellData[Height, Width];
        m_EmptyCells = new List<Vector2Int>();

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                m_BoardData[x,y] = new CellData();

                if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                {
                    Tile wallTile = WallTiles[Random.Range(0, WallTiles.Length)];
                    m_Tilemap.SetTile(new Vector3Int(x, y, 0), wallTile);
                    m_BoardData[x,y].pasable = false;
                }
                else
                {
                    Tile groundTile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                    m_Tilemap.SetTile(new Vector3Int(x, y, 0), groundTile);
                    m_BoardData[x, y].pasable = true;

                    m_EmptyCells.Add(new Vector2Int(x,y));
                }
            }
        }
        m_EmptyCells.Remove(new Vector2Int(1,1));
        Vector2Int endCoord = new Vector2Int(Width - 2, Height - 2);
        AddObject(Instantiate(ExitPrefab), endCoord);
        m_EmptyCells.Remove(endCoord);
        GenerateObstacles();
        GenerateObstacles2();
        GenerateEnemies();
        GenerateFood();
        GenerateFood2();

        //llama a spawn y pasale la info. El primer elemento es este mismo script, y el segundo es la casilla (1,1)


    }

    public Vector3 CellToWorld (Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public CellData GetCellData (Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= Width || cellIndex.y < 0 || cellIndex.y >= Height)
        {
            return null;
        }

        return m_BoardData[cellIndex.x,cellIndex.y];
    }

    //funcion/metodo que diga cuantos objetos de comida voy a spawnear, que por cada uno vaya buscando el escenario
    //dentro de los muros y que lo spawnee en una casilla aleatoria, si esa casilla esta vacia y es pasable
    void GenerateFood()
    {
        int foodCount = 5;
        for (int i = 0; i < foodCount; i++)
        {
            int randomIndex = Random.Range(0, m_EmptyCells.Count);
            Vector2Int coord = m_EmptyCells[randomIndex];

            m_EmptyCells.RemoveAt(randomIndex);
            FoodObject newFood = Instantiate(FoodPrefab);
            AddObject(newFood, coord);
            
        }
    }

    void GenerateFood2()
    {
        int foodCount = 2;
        for (int i = 0; i < foodCount; i++)
        {
            int randomIndex = Random.Range(0, m_EmptyCells.Count);
            Vector2Int coord = m_EmptyCells[randomIndex];

            m_EmptyCells.RemoveAt(randomIndex);
            FoodObjectBig newFood = Instantiate(BigFoodPrefab);
            AddObject(newFood, coord);

        }
    }

    //metodo generar paredes
    //genera un num aleatorio de obstaculos entre 5 y 10, en casillas vacias del escenario, que no sean vacias
    //ocuparse por un obstaculo
    //que el obstaculo sea uno aleatorio de una lista

    void GenerateObstacles()
    {
        int obstacleCount = Random.Range(3,7);
        for (int i = 0; i < obstacleCount; i++)
        {
            int randomIndex = Random.Range(0, m_EmptyCells.Count);
            Vector2Int coord = m_EmptyCells[randomIndex];

            m_EmptyCells.RemoveAt(randomIndex);
            WallObject newObstacle = Instantiate(WallPrefab);
            AddObject(newObstacle, coord);

        }
    }

    void GenerateObstacles2()
    {
        int obstacleCount2 = Random.Range(2, 5);
        for (int i = 0; i < obstacleCount2; i++)
        {
            int randomIndex = Random.Range(0, m_EmptyCells.Count);
            Vector2Int coord = m_EmptyCells[randomIndex];

            m_EmptyCells.RemoveAt(randomIndex);
            WallObject2 newObstacle2 = Instantiate(Wall2Prefab);
            AddObject(newObstacle2, coord);

        }
    }

    void GenerateEnemies()
    {

        int enemiesCount = Random.Range(2, 5);
        for (int i = 0;i < enemiesCount; i++)
        {
            int randomIndex = Random.Range(0, m_EmptyCells.Count);
            Vector2Int coord = m_EmptyCells[randomIndex];
            m_EmptyCells.RemoveAt(randomIndex);
            EnemyObject newEnemy = Instantiate(EnemyPrefab);
            AddObject(newEnemy, coord);
        }

        
    }

    

    //nueva funcion AddObject, queremos que necesite un cell object y una coord, qie coja la coord, mueva el objeto a
    // ese lugar, que cambie el objeto contenido por el nuevo, y que inicialice el metodo del objeto con esa coordenada
    void AddObject(CellObject obj, Vector2Int coord)
    {
        CellData data = m_BoardData[coord.x, coord.y];
        obj.transform.position = CellToWorld(coord);
        data.ContainObject = obj;
        obj.Init(coord);
    }

    public void SetCellTile(Vector2Int cellIndex, Tile tile)
    {
        m_Tilemap.SetTile(new Vector3Int(cellIndex.x, cellIndex.y, 0), tile); 
    }

    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return m_Tilemap.GetTile<Tile>(new Vector3Int (cellIndex.x, cellIndex.y, 0));
    }

    public void Limpiar()
    {
        if (m_BoardData == null) return;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                var cellData = m_BoardData[x, y];

                if (cellData.ContainObject != null)
                {
                    Destroy(cellData.ContainObject.gameObject);
                }

                SetCellTile(new Vector2Int(x, y), null);

                
            }
        }
    }
}
