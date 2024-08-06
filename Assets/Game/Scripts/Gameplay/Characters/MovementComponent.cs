using System.Collections;
using UnityEngine;

namespace Game
{
    public class MovementComponent : MonoBehaviour
    {
        [SerializeField] protected float movementSpeed = 1.5f;
        [SerializeField] private LayerMask collisionLayer;
 
        private Animator animator;

        private Vector3 targetPos;
        private bool isMoving = false;

        private void OnEnable()
        {
            animator = GetComponentInChildren<Animator>();
        }
        public virtual void UpdateMovement()
        {
            if (isMoving == false)
            {
                float horizontalInput = Input.GetAxisRaw("Horizontal");
                float verticalInput = Input.GetAxisRaw("Vertical");

                if (horizontalInput != 0.0f) verticalInput = 0.0f;

                if (horizontalInput != 0.0f | verticalInput != 0.0f)
                {
                    animator.SetFloat("MoveX", horizontalInput);
                    animator.SetFloat("MoveY", verticalInput);

                    targetPos = transform.position;
                    targetPos.x += horizontalInput;
                    targetPos.y += verticalInput;

                    if(IsWalkable(targetPos) == true)
                    {
                        StartCoroutine(MovementRoutine());
                    }
                }
            }
            animator.SetBool("IsMoving", isMoving);
        }
        private IEnumerator MovementRoutine()
        {
            if (isMoving == false)
            {
                isMoving = true;
                while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);
                    yield return null;
                }
                transform.position = targetPos;
                isMoving = false;
            }
            else yield return null;
        }
        private bool IsWalkable(Vector3 targetPos)
        {
            if(Physics2D.OverlapCircle(targetPos, 0.5f, collisionLayer) != null)
            {
                Debug.Log($"Colliding with {Physics2D.OverlapCircle(targetPos, 0.5f, collisionLayer).gameObject.name}");
                return false;
            }
            return true;
        }
    }
}
