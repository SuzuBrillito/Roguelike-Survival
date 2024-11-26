using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : CellObject
{
    public int FoodValue = 5;

    public override void PlayerEntered()
    {
        Destroy(gameObject);

        //suma mas comida
        GameManager.Instance.FoodChange(FoodValue);
        Debug.Log("Subio la comidaa");
    }

}
