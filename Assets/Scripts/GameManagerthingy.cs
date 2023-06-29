using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is in controll of the game stat aka 
/// is it active? 
/// Which status screen to display? 
/// </summary>
public class GameManagerthingy : MonoBehaviour
{
    /// <summary>
    ///Status of the game if it is active everything is good this should only change if and when player wins the game or loses the game by getting chauth. 
    ///The fuck is that typo right there. 
    /// </summary>
    public static bool gameActive = true;

    [SerializeField]
    private bool testBool;

    private void Awake()
    {
        Debug.Log(gameActive);
    }

    public static void ResetScene()
    {
        gameActive = false;
        Debug.Log(gameActive);
    }
    private void Update()
    {
    }
}
