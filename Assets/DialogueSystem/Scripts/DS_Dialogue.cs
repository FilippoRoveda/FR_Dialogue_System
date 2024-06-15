using UnityEngine;

namespace DS
{
    using ScriptableObjects;
    public class DS_Dialogue : MonoBehaviour
    {
        //Dialogue SO
        [SerializeField] private DS_DialogueContainerSO dialogueContainer;
        [SerializeField] private DS_DialogueGroupSO dialogueGroup;
        [SerializeField] private DS_DialogueSO dialogue;

        //Filters
        [SerializeField] private bool groupedDialogues;
        [SerializeField] private bool startingDialoguesOnly;

        //Indexes
        [SerializeField] private int selectedGroupIndex;
        [SerializeField] private int selectedDialogueIndex;
    }
}
