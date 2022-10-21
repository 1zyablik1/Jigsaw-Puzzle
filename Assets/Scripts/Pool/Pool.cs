using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T: MonoBehaviour 
{
    private T gameObject;
    private Transform container;
    private int poolCapacity;
    private List<T> poolObjects;

    public Pool(T gameObject, Transform container, int startCapacity = 10)
    {
        this.gameObject = gameObject;
        this.container = container;
        this.poolCapacity = startCapacity;

        CreatePool();
    }

    private void CreatePool()
    {
        poolObjects = new List<T>();
        
        for(int i = 0; i < poolCapacity; i++)
        {
            CreateElement();
        }
    }

    private T CreateElement(bool isActiveByDefault = false)
    {
        T createdObject = GameObject.Instantiate(gameObject, container.transform);
        createdObject.gameObject.SetActive(isActiveByDefault);

        poolObjects.Add(createdObject);

        return createdObject;
    }

    public bool TryGetElement(out T element)
    {
        foreach(T item in poolObjects)
        {
            if(!item.gameObject.activeSelf)
            {
                element = item;
                item.gameObject.SetActive(true);
                item.transform.SetParent(container.transform);
                return true;
            }
        }

        element = null;
        return false;
    }

    public T GetFreeElement()
    {
        if(TryGetElement(out var element))
        {
            return element;
        }

        return CreateElement();
    }

    public T GetFreeElement(Vector3 position)
    {
        var element = GetFreeElement();
        element.transform.position = position;

        return element;
    }

    public T GetFreeElement(Quaternion quaternion)
    {
        var element = GetFreeElement();
        element.transform.rotation = quaternion;

        return element;
    }

    public T GetFreeElement(Vector3 position, Quaternion quaternion)
    {
        var element = GetFreeElement();
        element.transform.position = position;
        element.transform.rotation = quaternion;

        return element;
    }

    public void ClearPool()
    {
        foreach(var item in poolObjects)
        {
            item.gameObject.SetActive(false);
            item.transform.position = Vector3.zero;
            item.transform.rotation = Quaternion.Euler(Vector3.zero);
            item.transform.localScale = Vector3.one;
        }
    }

    public List<T> GetPool()
    {
        return poolObjects;
    }
}
