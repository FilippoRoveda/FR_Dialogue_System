using UnityEngine;

namespace DS.Runtime.Gameplay
{
    using Runtime.ScriptableObjects;

    public class Charcter : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D _rigidbody;
        [SerializeField] protected Collider2D _collider;
        [SerializeField] protected CharacterSO _character;
        public CharacterSO Data { get { return _character; } }

        [SerializeField] protected TalkComponent _talkComponent;
        [SerializeField] protected bool canTalk;
        public bool CanTalk
        {
            get { return canTalk; }
            set
            {
                if (value == true)
                {
                    _talkComponent.EnableTalks();
                }
                else
                {
                    _talkComponent.DisableTalks();
                }
                canTalk = value;
            }
        }


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
                _rigidbody = gameObject.AddComponent<Rigidbody2D>();
            }
            _collider = GetComponent<Collider2D>();
            if (_collider == null)
            {
                _collider = gameObject.AddComponent<Collider2D>();
            }                 
            _talkComponent = GetComponentInChildren<TalkComponent>();
            if (_talkComponent == null) 
            {
                throw new System.Exception($"No talk component attached to {_character.Name} in scene.");
            }
            _talkComponent.Initialize(this);
        }


        #endregion
    }
}
