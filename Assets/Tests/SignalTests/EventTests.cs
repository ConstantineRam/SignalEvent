using NUnit.Framework;
using System.Threading.Tasks;

public class EventTests
{

    [Test]
    public void EventTests_EventFires()
    {
        int testresult = 0;
        Event<int> ev = new Event<int>();

        ev.Listen((i) => { testresult = i; });
        ev.Fire(999);

        Assert.IsTrue(testresult == 999);

    }

    [Test]
    [Description("Race condition test.")]
    public void EventTests_Parcalls ( )
    {
        
        for (int attempt = 0; attempt < 5; attempt++)
        {
            int testResult = 0;
            Event<int> ev = new Event<int>();
            ev.Listen(( i ) => { testResult += i; }); 
            
            try
            {
                Parallel.For(0, 10, ( i ) => { ev.Fire(1); });
            }
            finally
            {
                Assert.IsTrue(testResult == 10);
            }
        }

    }

}
