using UnityEngine;

namespace DS.Runtime.Gameplay
{
    using ScriptableObjects;
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;

        [SerializeField] private MovementComponent _movementComponent;
        [SerializeField] private bool _movementEnabled = true;

        [SerializeField] private CharacterSO _characterData;
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
            _movementComponent = GetComponent<MovementComponent>();
            if (_collider == null)
            {
                gameObject.AddComponent<MovementComponent>();
            }
            _movementComponent.Initialize();
        }
        private void Update()
        {
            if (_movementEnabled) 
            {
                _movementComponent.UpdateMovement();
            }
        }
        #endregion

    }
}