using UnityEngine;

public class TurnManager
{
    public event System.Action OnTick;

    //algo que cuente los turnos
    private int m_turno = 1;

        //singleton

    //algo que modifique/cambie los turnos
    

    public void Tick()
    {
        m_turno += 1;
        OnTick?.Invoke();
        Debug.Log("Turno " + m_turno);
    }

}