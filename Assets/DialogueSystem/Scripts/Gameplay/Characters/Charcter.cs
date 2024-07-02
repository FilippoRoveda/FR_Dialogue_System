using DS.Runtime.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS.Runtime.Gameplay
{
    public class Charcter : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D _rigidbody;
        [SerializeField] protected Collider2D _collider;
        [SerializeField] protected DialogueTalkZone _dialogueTalkZone;

        [SerializeField] protected CharacterSO character;

        #region Unity callbacks
        private void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            if (_rigidbody == null)
            {
                gameObject.AddComponent<Rigidbody2D>();
            }
            _collider = GetComponent<Collider2D>();
            if (_collider == null)
            {
                gameObject.AddComponent<Collider2D>();
            }       
            _dialogueTalkZone = GetComponent<DialogueTalkZone>();
            if (_dialogueTalkZone == null)
            {
                gameObject.AddComponent<DialogueTalkZone>();
            }
        }
        private void Update()
        {

        }
        #endregion
    }
}
