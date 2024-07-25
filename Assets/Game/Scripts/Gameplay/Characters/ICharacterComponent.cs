using UnityEngine;

namespace Game
{
    using Characters.Runtime;
    interface ICharacterComponent
    {
        public Rigidbody2D Rigidbody { get; }
        public Collider2D Collider { get; }
        public CharacterSO Data { get; }

        void InitializeComponents();
    }
}
