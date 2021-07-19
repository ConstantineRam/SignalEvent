using System;
using System.Collections.Generic;
using System.Threading;

//NOTE: Its thread safe, not concurent.
internal sealed class ThreadSafeSet<T>
{
    private readonly ReaderWriterLockSlim setLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
    private readonly HashSet<T> collection = new HashSet<T>();

    ~ThreadSafeSet ( )
    {
        this.setLock?.Dispose();
    }

    internal void Add ( in T value )
    {
        this.setLock.EnterWriteLock();
        try
        {
            this.collection.Add(value);
        }
        finally
        {
            if (this.setLock.IsWriteLockHeld)
                this.setLock.ExitWriteLock();
        }
    }

    internal void ForEach ( in Action<T> action )
    {
        {
            this.setLock.EnterWriteLock();
            try
            {
                foreach (T t in collection)
                    action(t);
            }
            finally
            {
                if (this.setLock.IsWriteLockHeld)
                    this.setLock.ExitWriteLock();
            }
        }
    }

    internal bool Contains ( in T value )
    {
        setLock.EnterReadLock();
        try
        {
            return this.collection.Contains(value);
        }
        finally
        {
            if (this.setLock.IsReadLockHeld)
                this.setLock.ExitReadLock();
        }

    }


    internal void RemoveWhere ( in Predicate<T> match )
    {
        this.setLock.EnterWriteLock();
        try
        {
            this.collection.RemoveWhere(match);
        }
        finally
        {
            if (this.setLock.IsWriteLockHeld)
                this.setLock.ExitWriteLock();
        }
    }

    internal void Clear ( )
    {
        this.setLock.EnterWriteLock();
        try
        {
            this.collection.Clear();
        }
        finally
        {
            if (this.setLock.IsWriteLockHeld)
                this.setLock.ExitWriteLock();
        }
    }
}
