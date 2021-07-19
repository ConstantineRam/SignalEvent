using System;
using System.Threading.Tasks;


public class Event<T>
{
    private readonly ThreadSafeSet<WeakAction<T>> listeners = new ThreadSafeSet<WeakAction<T>>();

    public void Fire (T t )
    {
        this.listeners.RemoveWhere(action => !action.IsAlive());
        this.listeners.ForEach(action => action.Invoke(t));
    }

    public void Listen (in Action<T> action, in object owner = null ) => this.listeners.Add(new WeakAction<T>(action, owner));
    public void Unsubscribe (object owner ) => this.listeners.RemoveWhere(weakAction => weakAction.IsOwnedBy(owner));
    public void Clear ( ) => this.listeners.Clear();
}