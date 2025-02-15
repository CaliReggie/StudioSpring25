using System;
using UnityEngine;
using System.Collections.Generic;
using Player;

namespace Scripts
{
    public class PlayerLoader : MonoBehaviour
    {
        // Player stat scriptable object
        // Default value is Pink player
        public PlayerStats curPlayerStats = new PlayerStats(
            heroName: "Pink",
            jmpLimit: 2,
            walkSpeed: 7,
            jmpHeight: 7,
            djmpHeight: 7,
            jmpGravScale: 0.75f,
            animOverride: null);
        
        // Event for OnLoaded
        public Action OnLoaded;
        
        // Flag for when the player is successfully loaded
        public bool Loaded { get; private set; }
        
        // Local current character variables
        public GameObject curCharacter { get; private set; }
        public int curCharacterID { get; private set; }
        
        // Dictionary of the added GameObjects
        private Dictionary<int, GameObject> characterDictionary = new Dictionary<int, GameObject>();

        [Header("Character List")] public CharacterList characterList;
        
        // List of player from ScriptableObject list
        private List<CharacterList.CharacterProperties> characterListLocal;
        
        void Start()
        {
            GetCharacterList(characterList);
            LoadCharacterfromList(characterListLocal);
            GetCurrentCharacter(0);
        }
        
        /// <summary>
        /// Load Character from the list
        /// </summary>
        /// <param name="charList">List of character</param>
        void LoadCharacterfromList(List<CharacterList.CharacterProperties> charList)
        {
            GameObject curCharacterObject;

            // Loop through and add all of them to the Player as Disabled
            foreach (var character in charList)
            {
                curCharacterObject = Instantiate(character.prefab, transform.position, Quaternion.identity);
                curCharacterObject.SetActive(false);
                
                // Add character into Dictionary for management
                characterDictionary.Add(character.id, curCharacterObject);
                
                // Add the character to the player as a child
                curCharacterObject.transform.parent = transform;
            }
        }
        
        /// <summary>
        /// Copy stats from PlayerScriptableObject and copy it locally
        /// </summary>
        /// <param name="character">Character to copy from</param>
        void FetchCharacterStats(PlayerScriptableObject character)
        {
            if (!character)
            {
                Debug.LogError("PlayerLoader: No character detected, falling back to Pink");
                return;
            }

            curPlayerStats = character.PlayerStats;
            
            Debug.Log($"Player Loader: Loaded {curPlayerStats.heroName}");
        }
        
        /// <summary>
        /// Get the character list from character script
        /// Character List is a scriptable object that contains all the characters
        /// It is set in the editor
        /// </summary>
        /// <param name="characterList">Character List to get from</param>
        private void GetCharacterList(CharacterList characterList)
        {
            if (!characterList)
            {
                Debug.LogError("[FATAL] PlayerLoader: Character list doesn't exist!!");
                return;
            }
            characterListLocal = characterList.allCharacterList;
        }
        
        /// <summary>
        /// Get the character based on the ID and set it as the current character
        /// </summary>
        /// <param name="ID">ID of the character to get from</param>
        private void GetCurrentCharacter(int ID)
        {
            // Fetch character
            CharacterList.CharacterProperties curCharacterProps = characterListLocal[ID];
            
            // Apply the stats
            FetchCharacterStats(curCharacterProps.scriptableObject);
            
            // Set selected character as active
            characterDictionary[ID].SetActive(true);
            curCharacter = characterDictionary[ID];
            curCharacterID = ID;
            
            // Invoke subscribed function after successfully loaded
            OnLoaded?.Invoke();
            Loaded = true;
        }
        
        /// <summary>
        /// Switch character based on the ID
        /// </summary>
        /// <param name="ID">Character to switch to</param>
        public void ChangeCharacter(int ID)
        {
            // Remove and disable the old character
            characterDictionary[curCharacterID].SetActive(false);
            
            // Get the requested character and apply properties
            GetCurrentCharacter(ID);
            
            // Clean up
        }
    }
}

