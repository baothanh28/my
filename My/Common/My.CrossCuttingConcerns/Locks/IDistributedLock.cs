﻿namespace My.CrossCuttingConcerns.Locks;

public interface IDistributedLock
{
    IDistributedLockScope Acquire(string lockName);

    IDistributedLockScope TryAcquire(string lockName);
}
