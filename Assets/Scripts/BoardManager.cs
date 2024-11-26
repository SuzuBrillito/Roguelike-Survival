using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{

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
        GenerateFood();
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
            CellData data = m_BoardData[coord.x, coord.y];
            
            FoodObject newFood = Instantiate(FoodPrefab);
            newFood.transform.position = CellToWorld(coord);
            data.ContainObject = newFood;
            
        }
    }
}
