using System;

/// <summary>
/// Interface for tiles to provide effects after they are destroyed. This is set up as a returned function because
/// if it was just a function itself, it could not be called after the object was destroyed (since it's, you know,
/// not there anymore).
/// </summary>
public interface IEffectOnDestroyed
{
    public Action GetInvokeAfterDestroyed();
}
