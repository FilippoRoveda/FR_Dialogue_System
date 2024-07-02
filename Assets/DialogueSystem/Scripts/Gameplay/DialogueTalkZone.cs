using UnityEngine;
using UnityEngine.Events;

namespace DS.Runtime.Gameplay
{
    public class DialogueTalkZone : MonoBehaviour
    {
        [SerializeField] private GameObject speechBubble;
        [SerializeField] private KeyCode talkKey = KeyCode.E;

        public UnityEvent TalkButtonPressed;

        #region Unity callbacks
        private void Awake()
        {
            TalkButtonPressed = new UnityEvent();
        }
        private void Update()
        {
            if(Input.GetKeyDown(talkKey) && speechBubble.activeSelf)
            {
                speechBubble.SetActive(false);
                TalkButtonPressed?.Invoke();
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Player")
            {
                speechBubble.SetActive(true);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                speechBubble.SetActive(false);
            }
        }
        #endregion
    }
}
