using Roots.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roots.Identification
{
    public class Store
    {

        private IAsyncUnitOfWork asyncUow;
        public Store(IAsyncUnitOfWork uow)
        {
            this.asyncUow = uow;
        }

        public async Task<T> Commit<T>(Func<IAsyncUnitOfWork, Task<T>> action)
        {
            try
            {
                var result = await action(asyncUow);                
                await asyncUow.CommitAsync();
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async void Commit<T>(Func<IAsyncUnitOfWork, Task> action)
        {
            try
            {
                await action(asyncUow);
                await asyncUow.CommitAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
