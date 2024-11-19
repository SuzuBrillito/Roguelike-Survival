using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{

    public class CellData
    {
        public bool pasable;
    }

    private CellData[,] m_BoardData;

    private Tilemap m_Tilemap;
     

    public int Height;
    public int Width;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;

    private Grid m_Grid;
    public PlayerManager playerCont;

    // Start is called before the first frame update
    public void Init()
    {

        m_Tilemap = GetComponentInChildren<Tilemap>(); //busca el componente tilemap

        m_Grid = GetComponentInChildren<Grid>();

        m_BoardData = new CellData[Height, Width];

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
                }
            }
        }

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
