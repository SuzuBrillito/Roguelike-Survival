using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public BoardManager boardManager;
    public PlayerManager playerManager;
    public TurnManager turnManager { get; private set; }

    private int m_comida = 100;

    public UIDocument UIDoc;
    private Label m_FoodLabel;

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

        turnManager = new TurnManager();
        turnManager.OnTick += OnTurnHappen;
        //Debug.Log("Comida actual: " + m_comida);

        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        

        boardManager.Init(); //crea mapa
        playerManager.Spawn(boardManager, new Vector2Int(1,1));
        
    }

    public void OnTurnHappen()
    {
        m_comida -= 1;
        m_FoodLabel.text = "Comida: " + m_comida;
    }
}
