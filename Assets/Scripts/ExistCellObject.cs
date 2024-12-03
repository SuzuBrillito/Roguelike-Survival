using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class ExistCellObject : CellObject
{
    //crear una casilla de salida que cambie el tile correcto y que reaccione al entrar el jugador generando un nuevo nivel (debug.log)

    //crear un tile con el sprite de la salida
    public Tile EndTile;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        GameManager.Instance.boardManager.SetCellTile(cell, EndTile);
    }

    public override void PlayerEntered()
    {

        GameManager.Instance.NuevoNivel();
        Debug.Log("has entrao");

    }

    //asignar el script al prefab
    //darle el prefab como ref al board manager
    //usar la posicion "altura max -2, anchura max -2"

    //crear tablero nuevo


}
