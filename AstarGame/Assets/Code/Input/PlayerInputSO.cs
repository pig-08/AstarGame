using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "SO/Input/PlayerInputSO", order = 1)]
public class PlayerInputSO : ScriptableObject, Controller.IPlayerActions
{
    public Action<Vector3> OnClickPressedEvent;

    private Vector3 _mousePoint;
    private Controller _controller;

    private void OnEnable()
    {
        if(_controller == null)
        {
            _controller = new Controller();
            _controller.Player.SetCallbacks(this);
        }
        _controller.Player.Enable();
    }

    private void OnDisable()
    {
        _controller.Player.Disable();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        Vector2 point = Camera.main.ScreenToWorldPoint(_mousePoint);

        if(context.performed)
            OnClickPressedEvent?.Invoke(point);
    }

    public void OnMousePoint(InputAction.CallbackContext context)
    {
        _mousePoint = context.ReadValue<Vector2>();
    }
}
