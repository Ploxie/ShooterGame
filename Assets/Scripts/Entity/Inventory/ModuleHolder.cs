using Assets.Scripts.Entity;
using System;

public class ModuleHolder<T> where T : Module
{
    public const int MODULE_STORAGE_CAPACITY = 3;

    private int accessPointer;
    private int insertionPointer;
    private T[] storage;


    public ModuleHolder()
    {
        accessPointer = 0;
        insertionPointer = 0;
        storage = new T[MODULE_STORAGE_CAPACITY];
    }

    public void Insert(T module)
    {
        if (insertionPointer > MODULE_STORAGE_CAPACITY - 1)
            insertionPointer = 0;

        storage[insertionPointer] = module;
        insertionPointer++;
    }

    public T Peek()
    {
        return storage[accessPointer];
    }
    public T Peek(int i)
    {
        return storage[i];
    }

    public T Cycle()
    {
        accessPointer++;
        if (accessPointer >= insertionPointer)
            accessPointer = 0;

        return storage[accessPointer];
    }

    public T[] GetArray()
    {
        return storage;
    }


}