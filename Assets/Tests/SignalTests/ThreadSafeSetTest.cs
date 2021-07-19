using NUnit.Framework;
using System.Threading.Tasks;

public class ThreadSafeSetTest
{
    [Test]
    public void ThreadSafe_ImlementsFunctionality_Contains ( )
    {
        ThreadSafeSet<int> safeSet = new ThreadSafeSet<int>();
        safeSet.Add(999);

        Assert.IsTrue(safeSet.Contains(999));

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

        safeSet.RemoveWhere(elem => elem.Equals(2));

        Assert.IsFalse(safeSet.Contains(2));
    }

    [Test]
    public void ThreadSafe_ClearsSelf ( )
    {
        ThreadSafeSet<int> safeSet = new ThreadSafeSet<int>();
        safeSet.Add(111);
        safeSet.Add(222);
        safeSet.Add(4444);
        safeSet.Add(0);

        safeSet.Clear();

        Assert.IsFalse(safeSet.Contains(111) || safeSet.Contains(222) || safeSet.Contains(4444) || safeSet.Contains(0));
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
