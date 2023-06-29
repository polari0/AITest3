using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// I hate that I made this script but it is dirty and easy solution to problem I don't want to tackle at the moment 
/// Basically when you change the oriantation sometimes the player character clips out of the map. I could fix this with changing the speed at which the platform moves 
/// Doing that how ever breaks Navmesh completely so I dont want to do it. 
/// </summary>
public class PlayerPosFix : MonoBehaviour
{
    [SerializeField]
    private GameObject playerRef;
    // Update is called once per frame
    void Update()
    {
        if(playerRef.transform.position.y < -9)
        {
            playerRef.transform.position = (new Vector3(playerRef.transform.position.x, 9, playerRef.transform.position.z));
        }
    }
}
