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
        public string Name => _name;
        public bool IsStackable => _isStackable;
        public Sprite InGameSprite => _inGameSprite;
        public Sprite UiSprite => _uiSprite;
    }
}