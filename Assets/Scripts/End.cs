using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public delegate void EndAction();
    public static event EndAction OnEnd;

    public Animator anim;
    public int stage = 0;

    public void EndGame()
    {
        stage++;
        anim.SetInteger("lightStage", stage);

        // has functions from Player.cs, EndUI.cs, Enemy.cs, PickupSpawn.cs
        if (stage >= 4)
            OnEnd?.Invoke();
    }
}
