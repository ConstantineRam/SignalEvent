using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Threading.Tasks;

public class ThreadSafeSetTest
{
    [Test]
    public void ThreadSafe_ImlementsFunctionality_Contains()
    {
        ThreadSafeSet<int> safeSet = new ThreadSafeSet<int>();
        safeSet.Add(999);

        Assert.IsTrue (safeSet.Contains(999) );

    }

    [Test]
    public void ThreadSafe_ReturnsFalseIfNotContains ( )
    {
        ThreadSafeSet<int> safeSet = new ThreadSafeSet<int>();
        
        Assert.IsFalse(safeSet.Contains(888));

    }

    [Test]
    public void ThreadSafe_RemovesWhere ( )
    {
        ThreadSafeSet<int> safeSet = new ThreadSafeSet<int>();
        safeSet.Add(1);
        safeSet.Add(2);
        safeSet.Add(3);

        safeSet.RemoveWhere( elem => elem.Equals (2));

        Assert.IsFalse(safeSet.Contains(2));
    }

    [Test]
    [Description("Race condition test. Take it with a grain of salt, however this test was effectively failing before lock was added.")]
    public void ThreadSafe_AddsValuesInParallelSafely ( )
    {
        for (int attempt = 0; attempt < 5; attempt++)
        {
            ThreadSafeSet<int> safeSet = new ThreadSafeSet<int>();
            try
            {
                Parallel.For(0, 10, ( i ) => { safeSet.Add(i); });
            }
            finally
            {
                for (int i = 0; i < 10; i++)
                    Assert.IsTrue(safeSet.Contains(i));
            }
        }
        
    }

}
