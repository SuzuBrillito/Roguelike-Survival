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

    // Start is called before the first frame update
    private void Start()
    {

        m_Tilemap = GetComponentInChildren<Tilemap>();

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

      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
