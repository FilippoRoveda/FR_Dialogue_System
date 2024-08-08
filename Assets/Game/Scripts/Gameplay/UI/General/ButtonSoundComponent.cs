using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    [AddComponentMenu("Game/UI/Button/ButtonSoundComponent")]

    [RequireComponent(typeof(Button))]
    [RequireComponent (typeof(AudioSource))]
    public class ButtonSoundComponent : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {

        [SerializeField]
        private AudioClip hoveredSfx;
        [SerializeField]
        private AudioClip pressedSfx;

        public Button button { private set; get; }
        public AudioSource audioSource { private set; get; }

        #region UnityCallbacks

        private void Awake()
        {
            button = GetComponent<Button>();
            if (!button)
            {
                Debug.LogWarning("Button for this custom button sound not founded!");
            }
            audioSource = GetComponent<AudioSource>();
            if (!audioSource)
            {
                Debug.LogWarning("AudioSource for this custom button sound not founded!");
            }
        }

        #endregion

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (button.interactable == false) return;

            if (hoveredSfx != null)
            {
                audioSource.PlayOneShot(hoveredSfx);
            }
        }

        public  void OnPointerClick(PointerEventData eventData)
        {
            if (button.interactable == false) return;

            if (pressedSfx != null)
            {
                audioSource.PlayOneShot(pressedSfx);
            }
        }
    }
}
