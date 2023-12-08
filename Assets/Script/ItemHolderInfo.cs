using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolderInfo : MonoBehaviour
{
    [SerializeField]
    ItemStorage[] items;

    public ItemStorage[] Items { get { return items; } set { items = value; } }

    [SerializeField]
    ToolSetInfo toolSetInfo;

    public int SearchItemNum(int index)
    {
        return items[index].ItemNum;
    }

    public bool PlusItem(int index,int current_index, int num)
    {
        items[index] += num;
        ChangeItemNumUI(current_index);
        return true;
    }

    public bool MinusItem(int index,int current_index, int num)
    {
        if (items[index].ItemNum >= num)
        {
            items[index] -= num;
            ChangeItemNumUI(current_index);
            return true;
        }
        else
        {
            Debug.Log("ë´ÇÁÇ»Ç¢");
            return false;
        }
    }

    public void ChangeItemNumUI(int index)
    {
        if (this.gameObject.tag.Equals("Player"))
        {
            toolSetInfo.SetToolNumText((items[index].ItemNum).ToString("D3"));
        }
    }

    
}

[System.SerializableAttribute]
public class ItemStorage
{
    [SerializeField]
    private Item it;
    [SerializeField]
    private int item_num = 0;

    public Item It { get { return it; } set { it = value; } }
    public int ItemNum { get { return item_num; } set { item_num = value; } }

    public static ItemStorage operator + (ItemStorage itemStorage,int num)
    {
        ItemStorage new_itemStorage = new ItemStorage();
        new_itemStorage.it = itemStorage.it;
        new_itemStorage.item_num = itemStorage.item_num + num;

        return new_itemStorage;
    }

    public static ItemStorage operator +( int num, ItemStorage itemStorage)
    {
        ItemStorage new_itemStorage = new ItemStorage();
        new_itemStorage.it = itemStorage.it;
        new_itemStorage.item_num = itemStorage.item_num + num;

        return new_itemStorage;
    }

    public void minusItem()
    {
        try
        {
            item_num--;
        }catch
        {
            Debug.Log("ïâÇÃêîÇ…Ç»ÇÈ");
        }
    }

    public static ItemStorage operator -(ItemStorage itemStorage, int num)
    {
        ItemStorage new_itemStorage = new ItemStorage();

        try
        {
            new_itemStorage.it = itemStorage.it;
            new_itemStorage.item_num = itemStorage.item_num - num;

        }
        catch
        {
            Debug.Log("ïâÇÃêîÇ…Ç»ÇÈ");
        }
        
        return new_itemStorage;
    }
}
