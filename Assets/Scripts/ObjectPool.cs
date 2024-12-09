// ObjectPool.cs
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject shortNotePrefab;
    private Queue<GameObject> shortNotePoolQueue;

    public GameObject longNotePrefab;
    private Queue<GameObject> longNotePoolQueue;

    void Awake()
    {
        // Initialization is handled dynamically
    }

    public void InitializePools(int shortNoteCount, int longNoteCount)
    {
        InitializeShortNotePool(shortNoteCount);
        InitializeLongNotePool(longNoteCount);
    }

    void InitializeShortNotePool(int count)
    {
        shortNotePoolQueue = new Queue<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(shortNotePrefab);
            obj.SetActive(false);
            shortNotePoolQueue.Enqueue(obj);
        }
    }

    void InitializeLongNotePool(int count)
    {
        longNotePoolQueue = new Queue<GameObject>();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(longNotePrefab);
            obj.SetActive(false);
            longNotePoolQueue.Enqueue(obj);
        }
    }

    public GameObject SpawnShortNote(Vector3 position, Quaternion rotation)
    {
        if (shortNotePoolQueue == null || shortNotePoolQueue.Count == 0)
        {
            GameObject obj = Instantiate(shortNotePrefab, position, rotation);
            IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
            pooledObj?.OnObjectSpawn();
            return obj;
        }

        GameObject objToSpawn = shortNotePoolQueue.Dequeue();
        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;
        IPooledObject pooledObjSpawn = objToSpawn.GetComponent<IPooledObject>();
        pooledObjSpawn?.OnObjectSpawn();
        shortNotePoolQueue.Enqueue(objToSpawn);
        return objToSpawn;
    }

    public GameObject SpawnLongNote(Vector3 position, Quaternion rotation)
    {
        if (longNotePoolQueue == null || longNotePoolQueue.Count == 0)
        {
            GameObject obj = Instantiate(longNotePrefab, position, rotation);
            IPooledObject pooledObj = obj.GetComponent<IPooledObject>();
            pooledObj?.OnObjectSpawn();
            return obj;
        }

        GameObject objToSpawn = longNotePoolQueue.Dequeue();
        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;
        IPooledObject pooledObjSpawn = objToSpawn.GetComponent<IPooledObject>();
        pooledObjSpawn?.OnObjectSpawn();
        longNotePoolQueue.Enqueue(objToSpawn);
        return objToSpawn;
    }
}
//github upload