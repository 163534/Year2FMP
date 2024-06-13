using InventorySytem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField]
        private int _size = 8;

        [SerializeField]
        private List<InventorySlot> _slots;

        private void OnValidate()
        {
            AdjustSize();   
        }

        private void AdjustSize()
        {
            // this means the same as if (_slots == null){ _slots = new List<InventorySlot>();} //

            _slots ??= new List<InventorySlot>();

            if(_slots.Count > _size) _slots.RemoveRange(_size, _slots.Count - _size);
            //_slots.Count = 12, _size = 8, Remove Items stating at pos 8, 12 - 8 = 4 //

            if(_slots.Count < _size) _slots.AddRange(new InventorySlot[_size - _slots.Count]);
            // _slots.Count = 3, _size = 8, add to the list 8 - 3 = 5 items //
        }
        public bool IsFull()
        {
            return _slots.Count(slot => slot.HasItem) >= _size; 
        }
        //public bool Something(InventorySlot slot) => slot.HasItem;

        public bool CanAcceptItem(ItemStack itemStack)
        {
            var slotWithStackableItem = FindSlot(itemStack.Item, true);
            return !IsFull() || slotWithStackableItem != null;
        }
        private InventorySlot FindSlot(ItemDefinition item, bool onlyStackable = false)
        {
            return _slots.FirstOrDefault(slot => slot.Item == item &&
            item.IsStackable ||
            !onlyStackable);
        }

        public ItemStack AddItem(ItemStack itemStack)
        {
            var relevantSlot = FindSlot(itemStack.Item, true);
            if (IsFull() && relevantSlot == null)
            {
                throw new InventoryException(InventoryOperation.Add, "Inventory is Full");
            }

            if(relevantSlot != null)
            {
                relevantSlot.NumberOfItems += itemStack.NumberOfItems;
            }
            else
            {
                relevantSlot = _slots.First(slot => slot.HasItem);
                relevantSlot.State = itemStack;
            }

            return relevantSlot.State;
        }
    }
}
