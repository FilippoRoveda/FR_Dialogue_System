using UnityEngine;
using UnityEngine.Events;

namespace DS.Runtime.Gameplay
{
    public class TalkZone : MonoBehaviour
    {
        [SerializeField] private GameObject speechBubble;
        [SerializeField] private KeyCode talkKey = KeyCode.E;

        [SerializeField] private bool zoneEnabled;

        public UnityEvent TalkButtonPressed = new UnityEvent();

        #region Unity callbacks
        private void Awake()
        {
            zoneEnabled = true;
        }
        private void Update()
        {
            if(Input.GetKeyDown(talkKey) && speechBubble.activeSelf && zoneEnabled)
            {
                speechBubble.SetActive(false);
                TalkButtonPressed?.Invoke();
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Player" && zoneEnabled)
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

        public void EnableZone()
        {
            zoneEnabled = true;
        }
        public void DisableZone() 
        {
            zoneEnabled = false;
        }
    }
}
