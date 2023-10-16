using System;

public class ModuleHolder
{
    public const int MODULE_STORAGE_CAPACITY = 3;

    private int accessPointer;
    private int insertionPointer;
    private Module2[] storage;


    public ModuleHolder()
    {
        accessPointer = 0;
        insertionPointer = 0;
        storage = new Module2[MODULE_STORAGE_CAPACITY];
    }

    public void Insert(Module2 module)
    {
        if (insertionPointer > MODULE_STORAGE_CAPACITY - 1)
            insertionPointer = 0;

        storage[insertionPointer] = module;
        insertionPointer++;
    }

    public Module2 Peek()
    {
        return storage[accessPointer];
    }

    public Module2 Cycle()
    {
        accessPointer++;
        if (accessPointer >= insertionPointer)
            accessPointer = 0;

        return storage[accessPointer];
    }


}