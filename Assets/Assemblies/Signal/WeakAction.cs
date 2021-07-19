using System;
using System.Reflection;
using UnityEngine;

internal sealed class WeakAction<T>
{
    private readonly WeakReference owner;
    private readonly MethodInfo method;
    private readonly object target;

    internal WeakAction (in Action<T> assignedAction, in object assignedOwner = default )
    {
        this.owner = new WeakReference(assignedOwner ?? assignedAction.Target);
        this.method = assignedAction.Method;
        this.target = assignedAction.Target;
    }

    internal void Invoke (in T t )
    {
        if (this.IsActiveAsMonoBeh())
            this.method.Invoke(this.target, new object[] { t });
    }
    internal bool IsAlive ( ) => this.owner.IsAlive && (this.owner != null && this.owner.Target != null)
                                && this.IsAliveAsMonoBeh();
    internal bool IsOwnedBy (in object owner ) => this.owner.Target == owner;
    private bool IsAliveAsMonoBeh ( ) => !(this.target is MonoBehaviour mono) || mono != null && mono.gameObject != null;
    private bool IsActiveAsMonoBeh ( ) => !(this.target is MonoBehaviour mono) || mono.gameObject.activeInHierarchy;
}

