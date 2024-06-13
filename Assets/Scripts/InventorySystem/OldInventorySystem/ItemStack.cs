using System;
using UnityEngine;

namespace InventorySystem
{
    [Serializable]
    public class ItemStack
    {
        public static readonly ItemStack Empty = new ItemStack(null,0);

        [SerializeField]
        private ItemDefinition _item;

        [SerializeField]
        private int _numberOfItems;

        // Checks to see if the item is not null (if we have the item) and if it's stackable, if it is than isStackable = true;
        public bool IsStackable => _item != null && _item.IsStackable;
        public ItemDefinition Item => _item;
        public int NumberOfItems
        {
            get => _numberOfItems;
            set
            {
                //checks to see if the value is smaller than zero, if not, keep the value as what it was. 
                // makes sure the value (number of items) is either positive or zero.
                value = value < 0 ? 0 : value;
                //checks to see if the item is stackable, if it is then we keep the old value, if not than we set the value to 1
                _numberOfItems = IsStackable ? value : 1;
            }
        }

        public ItemStack(ItemDefinition item , int numberOfItems)
        {
            _item = item;
            NumberOfItems = numberOfItems;
        }
        public ItemStack() { }

    }

}
