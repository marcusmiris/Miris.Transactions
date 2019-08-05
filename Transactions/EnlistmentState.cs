namespace Miris.Transactions
{
    public enum EnlistmentState
    {
        VolatileEnlistmentActive,
        VolatileEnlistmentPreparing ,
        VolatileEnlistmentPrepared,
        VolatileEnlistmentSPC,
        VolatileEnlistmentPreparingAborting,
        VolatileEnlistmentAborting,
        VolatileEnlistmentCommitting,
        VolatileEnlistmentInDoubt,
        VolatileEnlistmentEnded,
        VolatileEnlistmentDone,
    }
}
