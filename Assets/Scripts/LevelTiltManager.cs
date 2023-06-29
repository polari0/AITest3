using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelTiltManager : MonoBehaviour
{
    /// <summary>
    /// varialble to store reference to the level bottom based on this we also controll the character and the level tilt. 
    /// </summary>
    [SerializeField, Tooltip("This is the level bottom")]
    private GameObject level;
    // Update is called once per frame
    float smooth = 5f;
    float tiltAngle = 45.0f;

    private Vector3 originRotation = new Vector3(0f, 0f,0f);

    private Quaternion newRoationAngel;

    [SerializeField]
    private InputActionReference Up, Down, Left, Right;


    private void OnEnable()
    {
        Up.action.performed += OrientAreaUp;
        Down.action.performed += OrientAreaDown;
        Left.action.performed += OrientAreaLeft;
        Right.action.performed += OrientAreaRight;
    }
    private void OnDisable()
    {
        Up.action.performed -= OrientAreaUp;
        Down.action.performed -= OrientAreaDown;
        Left.action.performed -= OrientAreaLeft;
        Right.action.performed -= OrientAreaRight;
    }
    #region Orientation calls 
    private void OrientAreaRight(InputAction.CallbackContext obj)
    {
        newRoationAngel = Quaternion.Euler(-10, 0, 0);
        level.transform.rotation = Quaternion.Slerp(transform.rotation, newRoationAngel, smooth);
    }

    private void OrientAreaLeft(InputAction.CallbackContext obj)
    {
        newRoationAngel = Quaternion.Euler(10, 0, 0);
        level.transform.rotation = Quaternion.Slerp(transform.rotation, newRoationAngel, smooth);
    }

    private void OrientAreaDown(InputAction.CallbackContext obj)
    {
        newRoationAngel = Quaternion.Euler(0, 0, 10);
        level.transform.rotation = Quaternion.Slerp(transform.rotation, newRoationAngel, smooth);
    }
    private void OrientAreaUp(InputAction.CallbackContext obj)
    {
        newRoationAngel = Quaternion.Euler(0, 0, -10);
        level.transform.rotation = Quaternion.Slerp(transform.rotation, newRoationAngel, smooth);
    }
    #endregion

    void Update()
    {
    }
}
