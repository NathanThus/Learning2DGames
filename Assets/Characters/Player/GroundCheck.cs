using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NathanThus.Learning2DGames.Characters.Player
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _jumpableSurfaces;
        public event Action OnGroundContact;

        private void OnTriggerEnter2D(Collider2D other)
        {
            
            if((other.gameObject.layer << 1 | _jumpableSurfaces.value) != 0)
            OnGroundContact?.Invoke();
        }

        private void OnDestroy()
        {
            OnGroundContact = null;
        }
    }
}
