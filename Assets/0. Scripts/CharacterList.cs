using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterList", menuName = "ScriptableObjs/CharacterList")]

public class CharacterList : ScriptableObject
{
    [System.Serializable]
    public struct CharacterProperties
    {
        public string name;
        public int id;
        public PlayerScriptableObject scriptableObject;
        public GameObject prefab;
    }

    public List<CharacterProperties> allCharacterList;
}
