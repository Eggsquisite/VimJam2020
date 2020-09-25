using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public delegate void EndAction();
    public static event EndAction OnEnd; 

   

    public void EndGame()
    {
        // has functions from Player.cs, EndUI.cs, Enemy.cs, PickupSpawn.cs
        OnEnd();
    }
}
