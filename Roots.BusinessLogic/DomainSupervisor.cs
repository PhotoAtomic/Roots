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
        private IDictionary<Type, ISet<IExceptionEventListener>> exceptionListeners = new Dictionary<Type, ISet<IExceptionEventListener>>();

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
                catch(Exception exception)
                {
                    uow.Rollback();
                    NotifyExceptionListeners(exception);
                    throw exception;
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

        public void Apply<TException>(ExceptionEventListener<TException> exceptionListener) where TException : Exception
        {
            AddExceptionListener(exceptionListener);
        }

        private void AddMutatorListener<TMutator>(MutationEventListener<TMutator> mutatorListener) where TMutator : Mutator
        {
            var type = typeof(TMutator);
            if (!mutatorListeners.ContainsKey(type)) mutatorListeners[type] = new HashSet<IMutationEventListener>();
            mutatorListeners[type].Add(mutatorListener);
        }

        private void AddExceptionListener<TException>(ExceptionEventListener<TException> exceptionListener) where TException : Exception
        {
            var type = typeof(TException);
            if (!mutatorListeners.ContainsKey(type)) exceptionListeners[type] = new HashSet<IExceptionEventListener>();
            exceptionListeners[type].Add(exceptionListener);
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

        private void NotifyExceptionListeners(Exception exception)
        {
            var type = exception.GetType();
            foreach (var key in exceptionListeners.Keys.Where(k => type.IsAssignableFrom(k)))
            {
                ISet<IExceptionEventListener> listeners;
                if (!exceptionListeners.TryGetValue(key, out listeners)) return;
                foreach (var listener in listeners)
                {
                    listener.OnException(exception);
                }
            }
        }
    }
}
