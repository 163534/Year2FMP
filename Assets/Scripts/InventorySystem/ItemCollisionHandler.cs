using UnityEngine;

namespace InventorySystem
{
    public class ItemCollisionHandler : MonoBehaviour
    {
        private Inventory _inventory;

        private void Awake()
        {
            _inventory = GetComponent<Inventory>();
        }

        // checks to see if we touch any game objects //
        // Is apart of the inventory system //
        private void OnControllerColliderHit(ControllerColliderHit col)
        {
            if (!col.collider.TryGetComponent<GameItem>(out var gameItem))
            {
                return;
            }
            _inventory.AddItem(gameItem.Pick());

        }
    }
}
