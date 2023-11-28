using Assets.Scripts.Entity;
using System;

public class ModuleHolder<T> where T : Module
{
    public const int MODULE_STORAGE_CAPACITY = 3;

    private int accessPointer;
    private int insertionPointer;
    private int amount;
    private T[] storage;


    public ModuleHolder()
    {
        accessPointer = 0;
        insertionPointer = 0;
        amount = 0;
        storage = new T[MODULE_STORAGE_CAPACITY];
    }

    public void Insert(T module)
    {
        if (insertionPointer > MODULE_STORAGE_CAPACITY - 1)
            insertionPointer = 0;

        storage[insertionPointer] = module;
        amount++;
        amount = Math.Clamp(amount, 0, MODULE_STORAGE_CAPACITY);
        insertionPointer++;
    }

    public T Peek()
    {
        return storage[accessPointer];
    }
    public T Cycle()
    {
        accessPointer++;
        if (accessPointer > amount - 1)
            accessPointer = 0;

        return storage[accessPointer];
    }

    public T[] GetArray()
    {
        return storage;
    }


}