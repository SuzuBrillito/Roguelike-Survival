using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObjectBig : CellObject
{
    public int FoodValue = 10;

    public override void PlayerEntered()
    {
        Destroy(gameObject);

        //suma mas comida
        GameManager.Instance.FoodChange(FoodValue);
        Debug.Log("Subio la comidaa");
    }
}
