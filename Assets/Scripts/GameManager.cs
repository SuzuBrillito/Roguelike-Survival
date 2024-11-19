using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public BoardManager boardManager;
    public PlayerManager playerManager;
    public TurnManager m_turnManager { get; private set; }

    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {

        m_turnManager = new TurnManager();
        boardManager.Init(); //crea mapa
        playerManager.Spawn(boardManager, new Vector2Int(1,1));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
