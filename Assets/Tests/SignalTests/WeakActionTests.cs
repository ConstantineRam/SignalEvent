using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class WeakActionTests
{
    private string TestAction (int i ) => $"Meow  {i} !";
    [Test]
    public void WeakAction_DataAssignesData()
    {
        WeakReference owner = new WeakReference(this);
        WeakAction<int> weakAction = new WeakAction<int>((x) => TestAction(x), owner);

        Assert.IsTrue(weakAction.IsAlive());
    }

}
