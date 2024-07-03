namespace My.CrossCuttingConcerns.Locks;

public interface IDistributedLockScope : IDisposable
{
    bool StillHoldingLock();
}
