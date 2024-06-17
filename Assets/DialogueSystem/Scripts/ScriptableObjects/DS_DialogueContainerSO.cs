using DS.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace DS.ScriptableObjects
{
    [System.Serializable]
    public class DS_DialogueContainerSO : ScriptableObject
    {
        [SerializeField] private string graphName;
        public string GraphName
        {
            get { return graphName; }
            set { graphName = value; }
        }
        [SerializeField] private SerializableDictionary<DS_DialogueGroupSO, List<DS_DialogueSO>> dialogueGroups;

        /// <summary>
        /// Dictionary containing as key a group scriptable object and as a value a list of owned dialogue scriptable objects.
        /// </summary>
        public SerializableDictionary<DS_DialogueGroupSO, List<DS_DialogueSO>> DialogueGroups
        {
            get { return dialogueGroups; }
            set { dialogueGroups = value; }
        }

        [SerializeField] private List<DS_DialogueSO> ungroupedDialogues;
        /// <summary>
        /// List of ungrouped dialogues scriptable objects.
        /// </summary>
        public List<DS_DialogueSO> UngroupedDialogues
        {
            get { return ungroupedDialogues; }
            set { ungroupedDialogues = value; }
        }

        public void Initialize(string filename)
        {
            GraphName = filename;
            DialogueGroups = new SerializableDictionary<DS_DialogueGroupSO, List<DS_DialogueSO>>();
            UngroupedDialogues = new List<DS_DialogueSO>();
        }
        /// <summary>
        /// Get all groups name saved in this container.
        /// </summary>
        /// <returns></returns>
        public List<string> GetGroupNames()
        {
            List<string> groupNames = new List<string>();
            foreach (DS_DialogueGroupSO dialogueGroup in DialogueGroups.Keys)
            {
                groupNames.Add(dialogueGroup.GroupName);
            }
            return groupNames;
        }

        /// <summary>
        /// Get all DS_DialogueSO grouped dialogues inside the selected DS_DialogueGroupSO if that is owned by the current Container.
        /// </summary>
        /// <param name="dialogueGroup">Reference to thr DS_DialogueGroupSO to look after.</param>
        /// <returns></returns>
        public List<string> GetGroupedDialogueNames(DS_DialogueGroupSO dialogueGroup, bool startingDialoguesOnly = false)
        {
            if (dialogueGroups.ContainsKey(dialogueGroup) == true)
            {
                List<DS_DialogueSO> groupedDialogues = DialogueGroups[dialogueGroup];
                List<string> groupedDialogueNames = new List<string>();
                foreach (DS_DialogueSO groupedDialogue in groupedDialogues)
                {
                    if(startingDialoguesOnly == true && groupedDialogue.IsStartingDialogue == false)
                    {
                        continue; 
                    }
                    groupedDialogueNames.Add(groupedDialogue.DialogueName);
                }
                return groupedDialogueNames;
            }
            else
            {
                Utilities.Logger.Error($"Group dictionary fot this Dialogue Container does not conatain DS_DialogueGroup Key {dialogueGroup} with name {dialogueGroup.GroupName}", Color.red);
                return null;
            }
        }

        /// <summary>
        /// Get all ungrouped DS_DialogueSO names contained in this container.
        /// </summary>
        /// <returns></returns>
        public List<string> GetUngroupedDialogueNames(bool startingDialoguesOnly = false)
        {
            List<string> ungroupedDialogueNames = new List<string>();
            foreach (DS_DialogueSO ungroupedDialogue in UngroupedDialogues)
            {
                if (startingDialoguesOnly == true && ungroupedDialogue.IsStartingDialogue == false)
                {
                    continue;
                }
                ungroupedDialogueNames.Add(ungroupedDialogue.DialogueName);
            }
            return ungroupedDialogueNames;
        }
    }
}
