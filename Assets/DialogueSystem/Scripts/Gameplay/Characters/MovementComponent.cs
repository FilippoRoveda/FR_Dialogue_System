using UnityEngine;

namespace DS.Runtime.Gameplay
{
    public class MovementComponent : MonoBehaviour
    {
        [SerializeField] protected float speed = 5f;
        protected Rigidbody2D rb;

        public virtual void Initialize()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public virtual void UpdateMovement()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
        }
    }
}
