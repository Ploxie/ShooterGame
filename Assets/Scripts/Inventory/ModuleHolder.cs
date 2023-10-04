using System;

public class ModuleHolder
{
    public const int MODULE_STORAGE_CAPACITY = 3;

    private int accessPointer;
    private int insertionPointer;
    private Module[] storage;


    public ModuleHolder()
    {
        accessPointer = 0;
        insertionPointer = 0;
        storage = new Module[MODULE_STORAGE_CAPACITY];
    }

    public void Insert(Module module)
    {
        if (insertionPointer > MODULE_STORAGE_CAPACITY - 1)
            insertionPointer = 0;

        storage[insertionPointer] = module;
        insertionPointer++;
    }

    public void Pop()
    {
        storage[accessPointer] = null;
        insertionPointer--;
        accessPointer--;

        if (accessPointer < 0)
            accessPointer = 0;

        if (insertionPointer < 0)
            insertionPointer = 0;
    }

    public Module Peek()
    {
        return storage[accessPointer];
    }

    public Module Cycle()
    {
        accessPointer++;
        if (accessPointer >= insertionPointer)
            accessPointer = 0;

        return storage[accessPointer];
    }


}