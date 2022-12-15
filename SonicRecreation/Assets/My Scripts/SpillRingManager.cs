using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpillRingManager : MonoBehaviour
{
    public GameObject spillRing;
    public GameObject spillRingSpawn;
    public PlayerMovement player;
    public int max;
    public bool ringSpill = false;
    public static int counter;


    void Start()
    {
        counter = 0;
        max = 0;
    }

    void Update()
    {
        if (ringSpill == true)
        {
            max = player.ringCount;
            if (max > 30)
            {
                max = 30;
            }
            if (counter <= max)
            {
                Instantiate(spillRing, spillRingSpawn.transform.position,spillRingSpawn.transform.rotation);
                counter += 1;
            }
            if (counter > max)
            {
                max = 0;
                counter = 0;
                ringSpill = false;
            }
        }
    }
}
