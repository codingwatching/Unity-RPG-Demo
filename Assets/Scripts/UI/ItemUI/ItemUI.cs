﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Manager;
using Items;

namespace UI
{
    [System.Serializable]
    public abstract class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
        IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        Transform originParent = null;
        public Item item { get; set; }

        void Update()
        {
            GetComponent<Image>().fillAmount =
                item.cdTimer < item.itemConfig.cd ? item.cdTimer / item.itemConfig.cd : 1;
        }

        Transform CheckSlotType(GameObject obj)
        {
            if (obj == null)
                return originParent;
            ItemSlot originSlot = originParent.GetComponent<ItemSlot>();
            ItemSlot targetSlot = obj.GetComponent<ItemSlot>();
            if (targetSlot == null)
            {
                ItemUI target = obj.GetComponent<ItemUI>();
                if (target == null)
                    return originParent;
                else
                {
                    targetSlot = target.transform.parent.GetComponent<ItemSlot>();
                    if (originSlot.slotType == targetSlot.slotType)
                        return SwapItemUI(targetSlot, target);
                    else if (targetSlot.slotType == SlotType.ACTION)
                    {
                        if (targetSlot.itemUI.item.itemConfig.itemType == ItemType.Skill)
                            return originParent;
                        if (item.containerType == ContainerType.Equipment)
                        {
                            if (item.itemConfig.itemType != targetSlot.itemUI.item.itemConfig.itemType)
                                return originParent;
                            GameManager.Instance.player.DetachEquipment(item as Equipment, ContainerType.Action);
                            GameManager.Instance.player.AttachEquipment(targetSlot.itemUI.item as Equipment);
                        }

                        return SwapItemUI(targetSlot, target);
                    }
                    else if (targetSlot.slotType == SlotType.BAG)
                    {
                        if (item.itemConfig.itemType == ItemType.Skill)
                            return originParent;
                        if (item.containerType == ContainerType.Equipment)
                        {
                            if (item.itemConfig.itemType != targetSlot.itemUI.item.itemConfig.itemType)
                                return originParent;
                            GameManager.Instance.player.DetachEquipment(item as Equipment, ContainerType.Bag);
                            GameManager.Instance.player.AttachEquipment(targetSlot.itemUI.item as Equipment);
                        }

                        return SwapItemUI(targetSlot, target);
                    }
                    else
                    {
                        if (item.itemConfig.itemType != targetSlot.itemType)
                            return originParent;
                        if (item.containerType == ContainerType.Equipment)
                        {
                            GameManager.Instance.player.DetachEquipment(item as Equipment,
                                targetSlot.itemUI.item.containerType);
                            GameManager.Instance.player.AttachEquipment(targetSlot.itemUI.item as Equipment);
                        }
                        else
                        {
                            GameManager.Instance.player.DetachEquipment(targetSlot.itemUI.item as Equipment,
                                item.containerType);
                            GameManager.Instance.player.AttachEquipment(item as Equipment);
                        }

                        return SwapItemUI(targetSlot, target);
                    }
                }
            }
            else
            {
                if (originSlot.slotType == targetSlot.slotType)
                    return targetSlot.transform;
                else if (targetSlot.slotType == SlotType.ACTION)
                {
                    if (item.containerType == ContainerType.Equipment)
                        GameManager.Instance.player.DetachEquipment(item as Equipment, ContainerType.Action);
                    return targetSlot.transform;
                }
                else if (targetSlot.slotType == SlotType.BAG)
                {
                    if (item.itemConfig.itemType == ItemType.Skill)
                        return originParent;
                    if (item.containerType == ContainerType.Equipment)
                        GameManager.Instance.player.DetachEquipment(item as Equipment, ContainerType.Bag);
                    return targetSlot.transform;
                }
                else
                {
                    if (item.itemConfig.itemType != targetSlot.itemType)
                        return originParent;
                    GameManager.Instance.player.AttachEquipment(item as Equipment);
                    return targetSlot.transform;
                }
            }
        }

        Transform SwapItemUI(ItemSlot targetSlot, ItemUI target)
        {
            Transform temp = targetSlot.transform;
            target.transform.SetParent(originParent);
            target.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            return temp;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            originParent = transform.parent.parent;
            eventData.pointerDrag.transform.SetParent(UIManager.Instance.transform);
            GetComponent<Image>().raycastTarget = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            originParent.GetComponent<ItemSlot>().itemUI = null;
            eventData.pointerDrag.transform.SetParent(CheckSlotType(eventData.pointerEnter).GetComponent<ItemSlot>()
                .icons);
            transform.parent.parent.GetComponent<ItemSlot>().itemUI = this;
            GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            GetComponent<Image>().raycastTarget = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!GameManager.Instance.player.isDead && eventData.pointerId == -2)
            {
                item.Use(GameManager.Instance.player);
                UIManager.Instance.attributePanel.UpdatePanel();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Vector3 position = eventData.pointerEnter.transform.position;
            Rect tipRect = UIManager.Instance.tipPanel.transform.GetComponent<RectTransform>().rect;
            Rect itemRect = GetComponent<RectTransform>().rect;
            if (position.x > tipRect.width)
            {
                if (position.y < tipRect.height)
                    UIManager.Instance.tipPanel.transform.position =
                        position + new Vector3(-itemRect.width, tipRect.height / 2, 0);
                else
                    UIManager.Instance.tipPanel.transform.position =
                        position + new Vector3(-itemRect.width, -tipRect.height / 2, 0);
            }
            else
            {
                if (position.y < tipRect.height)
                    UIManager.Instance.tipPanel.transform.position = position +
                                                                     new Vector3(itemRect.width / 2 + tipRect.width,
                                                                         tipRect.height / 2, 0);
                else
                    UIManager.Instance.tipPanel.transform.position = position +
                                                                     new Vector3(itemRect.width / 2 + tipRect.width,
                                                                         -tipRect.height / 2, 0);
            }

            UIManager.Instance.tipPanel.Draw(item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UIManager.Instance.tipPanel.Erase();
        }
    }
}