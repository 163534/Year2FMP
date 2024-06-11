using UnityEngine;

namespace InventorySystem
{
    public class ItemCollisionHandler : MonoBehaviour
    {
        private void OnCollisionEnter(Collision col)
        {
            if (!col.collider.TryGetComponent<GameItem>(out var gameItem))
            {
                return;
            }
            else
            {
                Debug.Log("Collided with an item");
                gameItem.Pick();
            }
            if(col.collider == true)
            print("Collided with an object");
        }
    }
}
