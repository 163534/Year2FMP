using UnityEngine;

namespace InventorySystem
{
    public class GameItem : MonoBehaviour
    {
        [SerializeField]
        private ItemStack _stack;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        public ItemStack Stack => _stack;

        private void OnValidate()
        {
            SetupGameObject();
        }
        private void SetupGameObject()
        {
            if (_stack.Item == null) return;
            //Set game sprite
            SetGameSprite();
            // Adjust number of items
            AdjustNumberOfItems();
            //Update game object name
            UpdateGameObjectName();
        }

        private void SetGameSprite()
        {
            _spriteRenderer.sprite = _stack.Item.InGameSprite;
        }
        private void UpdateGameObjectName()
        {
            //ItemName (numberOfItem)/(ns)
            string name = _stack.Item.Name;
            string number = _stack.IsStackable ? _stack.NumberOfItems.ToString() : "ns";
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
            Debug.Log("Item Picked Up");
            Destroy(gameObject);
            return _stack;
        }
    }

}
