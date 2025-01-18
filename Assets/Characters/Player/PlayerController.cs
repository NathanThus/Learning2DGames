using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NathanThus.Learning2DGames.Characters.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField]
        private Rigidbody _playerBody;

        [SerializeField, Range(0, 10)]
        private float _speed;

        #endregion

        #region Fields

        private PlayerControls _inputActions;
        private InputAction _movement;

        #endregion

        #region Unity Methods

        private void Start()
        {
            _inputActions = new();
            _movement = _inputActions.PlayerActions.Movement;
            _movement.Enable();
        }

        private void OnEnable()
        {
            _movement.Enable();
        }

        private void OnDisable()
        {
            _movement.Disable();
        }

        // TODO: See if this can be improved.
        // I'll do this in the physics loop to make it as framerate agnostic as I can.
        private void FixedUpdate()
        {
            HandleMovement();
        }

        #endregion

        #region Private

        private void HandleMovement()
        {
            Vector2 movementDirection = _movement.ReadValue<Vector2>();

            if(movementDirection == Vector2.zero) return;
            
            if(_playerBody.velocity.sqrMagnitude >= _speed) return;

            movementDirection *= _speed;
            _playerBody.AddForce(movementDirection);
        }

        #endregion
    }
}