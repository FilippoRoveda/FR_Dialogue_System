using UnityEngine;

namespace Game
{
    using Characters.Runtime;

    public class PlayerComponent : Singleton<PlayerComponent>, ICharacterComponent
    {      
        [SerializeField] private Collider2D _collider;
        [SerializeField] private CharacterSO _playerData;

        [SerializeField] private MovementComponent _movementComponent;
        [SerializeField] private bool canMove = true;

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
