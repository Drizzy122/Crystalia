using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    private PlayerInput.OnAttackActions onAttack;
    private PlayerMotor motor;
    
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        onAttack = playerInput.OnAttack;

        motor = GetComponent<PlayerMotor>();
        

        onFoot.Sprint.performed += ctx => motor.Sprint();
        onAttack.Attack.performed += ctx => motor.Attack();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
        onAttack.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
        onAttack.Disable();
    }
}
