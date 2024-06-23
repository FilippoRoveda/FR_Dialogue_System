using UnityEngine;

namespace DS.Gameplay
{
    public class DialogueTalkZone : MonoBehaviour
    {
        [SerializeField] private GameObject speechBubble;
        [SerializeField] private KeyCode talkKey = KeyCode.E;

        #region Unity callbacks
        private void Update()
        {
            if(Input.GetKeyDown(talkKey) && speechBubble.activeSelf)
            {

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
