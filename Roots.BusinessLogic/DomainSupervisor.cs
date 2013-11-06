using Roots.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Roots.BusinessLogic
{
    [Export]
    public class DomainSupervisor
    {
        private IUnitOfWorkFactory factory;

        private IDictionary<Type, ISet<IMutationEventListener>> mutatorListeners = new Dictionary<Type, ISet<IMutationEventListener>>();

        [ImportingConstructor]
        public DomainSupervisor([Import]IUnitOfWorkFactory factory)
        {
            this.factory = factory;
        }

        public void Apply(Mutator mutator)
        {
            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                try
                {
                    mutator.Mutate(new RepositoryAccessor(uow));
                    uow.Commit();
                    NotifyMutatorListeners(mutator);
                }
                catch 
                {
                    uow.Rollback();
                }
            }
        }



        public TResult Apply<TResult>(Selector<TResult> selector)
        {
            using (var uow = factory.CreateNew(IsolationLevel.ReadItsOwnWrite))
            {
                var result = selector.Select(new ReadOnlyRepositoryAccessor(uow));
                uow.Rollback();
                return result;
            }
        }

        public void Apply<TMutator>(MutationEventListener<TMutator> mutatorListener) where TMutator : Mutator
        {
            AddMutatorListener(mutatorListener);
        }

        private void AddMutatorListener<TMutator>(MutationEventListener<TMutator> mutatorListener) where TMutator : Mutator
        {
            var type = typeof(TMutator);
            if (!mutatorListeners.ContainsKey(type)) mutatorListeners[type] = new HashSet<IMutationEventListener>();
            mutatorListeners[type].Add(mutatorListener);
        }

        private void NotifyMutatorListeners(Mutator mutator)
        {
            var type = mutator.GetType();
            ISet<IMutationEventListener> listeners;
            if (!mutatorListeners.TryGetValue(type, out listeners)) return;
            foreach (var listener in listeners)
            {
                listener.OnMutation(mutator);
            }
        }
    }
}
