using UnityEngine;

namespace Game
{
    public class MovementComponent : MonoBehaviour
    {
        [SerializeField] protected float _speed = 5f;
        [SerializeField] protected Rigidbody2D _rigidbody;

        public virtual void Initialize()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            if (_rigidbody == null)
            {
                gameObject.AddComponent<Rigidbody2D>();
            }
        }

        public virtual void UpdateMovement()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            _rigidbody.velocity = new Vector2(horizontalInput * _speed, _rigidbody.velocity.y);
        }
    }
}
