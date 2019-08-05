#if NETSTANDARD
using System;
using System.Transactions;

namespace Miris.Transactions.Breacher.Proxies
{
    public class InternalEnlistmentProxy
        : BreacherProxy
    {

#region ' ctor '

        public InternalEnlistmentProxy(Func<object> _proxiedInstance)
            : base(_proxiedInstance)
        {
        }

        #endregion

        public PreparingEnlistment PreparingEnlistment
            => _ProxiedInstance.GetPrivateMemberValue<PreparingEnlistment>("PreparingEnlistment");

        public EnlistmentStateProxy State
        {
            get => new EnlistmentStateProxy(() => _ProxiedInstance.GetPrivateMemberValue("State"));
            set => _ProxiedInstance.SetPrivateMemberValue("State", value._ProxiedInstance);
        }

        public InternalTransactionProxy Transaction
            => new InternalTransactionProxy(() => _ProxiedInstance.GetPrivateMemberValue("Transaction"));

        public IEnlistmentNotification EnlistmentNotification
            => _ProxiedInstance.GetPrivateMemberValue<IEnlistmentNotification>("EnlistmentNotification");

        public IEnlistmentNotification TwoPhaseNotifications
        {
            get => _ProxiedInstance.GetPrivateMemberValue<IEnlistmentNotification>("_twoPhaseNotifications");
            set => _ProxiedInstance.SetPrivateMemberValue("_twoPhaseNotifications", value);
        }

        public EnlistmentStateProxy TwoPhaseState
            => new EnlistmentStateProxy(() => _ProxiedInstance.GetPrivateMemberValue("_twoPhaseState"));


    }
}
#endif