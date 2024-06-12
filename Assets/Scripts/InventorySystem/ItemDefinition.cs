using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(menuName = "Inventory/Item Definition", fileName = "New Item Definition")]
    public class ItemDefinition : ScriptableObject
    {
        // fields //
        [SerializeField]
        private string _name;

        [SerializeField]
        private bool _isStackable;

        [SerializeField]
        private Sprite _inGameSprite;

        [SerializeField]
        private Sprite _uiSprite;

        // properties //
        // => just means it returns whatever is next to it.
        public string name => _name;
        public bool isStackable => _isStackable;
        public Sprite inGameSprite => _inGameSprite;
        public Sprite uiSprite => _uiSprite;
    }
}