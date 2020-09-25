using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupUI : MonoBehaviour
{
    public int maxPickups;
    private int healthPickups = 0, speedPickups = 0, attackPickups = 0;

    public Image[] healthImgs, speedImgs, attackImgs;
    private void Start()
    {
        HealthPickups();
        SpeedPickups();
        AttackPickups();
    }

    void HealthPickups()
    {
        if (healthPickups > maxPickups)
            healthPickups = maxPickups;

        for (int i = 0; i < healthImgs.Length; i++)
        {
            if (i < healthPickups)
            {
                healthImgs[i].enabled = true;
            }
            else
            {
                healthImgs[i].enabled = false;
            }
        }
    }

    void SpeedPickups()
    {
        if (speedPickups > maxPickups)
            speedPickups = maxPickups;

        for (int i = 0; i < speedImgs.Length; i++)
        {
            if (i < speedPickups)
            {
                speedImgs[i].enabled = true;
            }
            else
            {
                speedImgs[i].enabled = false;
            }
        }
    }

    void AttackPickups()
    {
        if (attackPickups > maxPickups)
            attackPickups = maxPickups;

        for (int i = 0; i < attackImgs.Length; i++)
        {
            if (i < attackPickups)
            {
                attackImgs[i].enabled = true;
            }
            else
            {
                attackImgs[i].enabled = false;
            }
        }
    }

    public void UpdatePickups(int pickupID)
    { 
        // 0 for health, 1 for speed, 2 for attack
        if (pickupID == 0) {
            healthPickups++;
        } else if (pickupID == 1) {
            speedPickups++;
        } else if (pickupID == 2) {
            attackPickups++;
        }

        HealthPickups();
        SpeedPickups();
        AttackPickups();
    }
}
