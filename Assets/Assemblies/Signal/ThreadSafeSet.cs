using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public sealed class ThreadSafeSet<T>
{
    private readonly ReaderWriterLockSlim setLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
    private readonly HashSet<T> collection = new HashSet<T>();
    public void Add (in T value )
    {
        setLock.EnterWriteLock();
        try
        {
            this.collection.Add(value);
        }
        finally
        {
            if (setLock.IsWriteLockHeld)
                setLock.ExitWriteLock();
        }
        

    }

    public bool Contains ( in T value )
    {
        setLock.EnterReadLock();
        try
        {
            return this.collection.Contains(value);
        }
        finally
        {
            if (setLock.IsReadLockHeld)
                setLock.ExitReadLock();
        }
        
    }
    

    public void RemoveWhere ( in Predicate<T> match )
    {
        setLock.EnterWriteLock();
        try
        {
            collection.RemoveWhere(match);
        }
        finally
        {
            if (setLock.IsWriteLockHeld)
                setLock.ExitWriteLock();
        }
    }
}
