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
        [Header("Required Components")]
        [SerializeField]
        private Rigidbody2D _playerBody;

        [SerializeField]
        private Transform _localTransform;

        [SerializeField]
        private GroundCheck _groundCheck;

        [Header("Settings")]

        [SerializeField, Range(0, 30)]
        private float _walkingSpeed = 10;

        [SerializeField, Range(0, 32)]
        private float _jumpForce = 12;

        [SerializeField]
        private LayerMask _jumpAble;

        [SerializeField, Range(0, 2f)]
        private float _maxJumpHeightCutoff = 0.5f;

        #endregion

        #region Fields

        private PlayerControls _inputActions;
        private InputAction _movement;
        [SerializeField]
        private bool _canJump = true;

        #endregion

        #region Unity Methods

        private void Start()
        {
            _inputActions = new();
            _movement = _inputActions.PlayerActions.Movement;
            _movement.Enable();
            _groundCheck.OnGroundContact += HandleOnGroundContact;
        }

        private void OnEnable()
        {
            if (_movement == null) return; // On intial startup, can be null.
            _movement.Enable();
            _groundCheck.OnGroundContact += HandleOnGroundContact;
        }

        private void OnDisable()
        {
            _movement.Disable();
            _groundCheck.OnGroundContact -= HandleOnGroundContact;
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

            if (movementDirection == Vector2.zero) return;
            if (_playerBody.velocity.sqrMagnitude >= _walkingSpeed) return;

            var walkForce = new Vector2(movementDirection.x * _walkingSpeed, 0);
            _playerBody.AddForce(walkForce, ForceMode2D.Force);

            if (movementDirection.y <= 0 || !_canJump) return;

            RaycastHit2D hit = Physics2D.Raycast(_localTransform.position, Vector2.down, _maxJumpHeightCutoff, _jumpAble);
            if (hit.distance < _maxJumpHeightCutoff) PerformJump(movementDirection);
        }

        private void PerformJump(Vector2 movementDirection)
        {
            var jumpForce = new Vector2(0, movementDirection.y * _jumpForce);
            _playerBody.AddForce(jumpForce, ForceMode2D.Impulse);
            _canJump = false;
        }

        #endregion

        #region Event Handlers

        private void HandleOnGroundContact()
        {
            _canJump = true;
        }


        #endregion
    }
}