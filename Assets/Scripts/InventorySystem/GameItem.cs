using UnityEngine;

namespace InventorySystem
{
    public class GameItem : MonoBehaviour
    {
        [SerializeField]
        private ItemStack _stack;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private void OnValidate()
        {
            SetupGameObject();
        }
        private void SetupGameObject()
        {
            if (_stack.item == null) return;
            //Set game sprite
            SetGameSprite();
            // Adjust number of items
            AdjustNumberOfItems();
            //Update game object name
            UpdateGameObjectName();
        }

        private void SetGameSprite()
        {
            _spriteRenderer.sprite = _stack.item.inGameSprite;
        }
        private void UpdateGameObjectName()
        {
            //ItemName (numberOfItem)/(ns)
            string name = _stack.item.name;
            string number = _stack.isStackable ? _stack.NumberOfItems.ToString() : "ns";
            gameObject.name = $"{name} ({number})";
        }

        private void AdjustNumberOfItems()
        {
           _stack.NumberOfItems = _stack.NumberOfItems;
            //print(_stack.NumberOfItems);
        }
        //Should destroy object
        public ItemStack Pick()
        {
            Destroy(gameObject);
            return _stack;
        }
    }

}
