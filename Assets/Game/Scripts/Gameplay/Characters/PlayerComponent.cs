using UnityEngine;

namespace Game
{
    using Characters.Runtime;

    public class PlayerComponent : Singleton<PlayerComponent>, ICharacterComponent
    {      
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private CharacterSO _playerData;

        [SerializeField] private MovementComponent _movementComponent;
        [SerializeField] private bool canMove = true;


        public Rigidbody2D Rigidbody => _rigidbody;
        public Collider2D Collider => _collider;
        public CharacterSO Data => _playerData;




        #region Unity callbacks
        protected override void Awake()
        {
            base.Awake();
            InitializeComponents();
        }

        public void InitializeComponents()
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
            if (_movementComponent == null)
            {
                gameObject.AddComponent<MovementComponent>();
            }
            _movementComponent.Initialize();
        }
        private void Update()
        {
            if (canMove) 
            {
                _movementComponent.UpdateMovement();
            }
        }
        #endregion

    }
}
