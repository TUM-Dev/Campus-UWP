using System.Threading;
using Shared.Classes;
using Shared.Classes.Threading;
using Storage.Classes.Contexts;

namespace Storage.Classes.Models
{
    public abstract class AbstractModel: AbstractDataTemplate
    {
        //--------------------------------------------------------Attributes:-----------------------------------------------------------------\\
        #region --Attributes--
        protected readonly SemaphoreSlim LOCK_SEMA = new SemaphoreSlim(1);

        #endregion
        //--------------------------------------------------------Constructor:----------------------------------------------------------------\\
        #region --Constructors--


        #endregion
        //--------------------------------------------------------Set-, Get- Methods:---------------------------------------------------------\\
        #region --Set-, Get- Methods--


        #endregion
        //--------------------------------------------------------Misc Methods:---------------------------------------------------------------\\
        #region --Misc Methods (Public)--
        /// <summary>
        /// Updates the current model in the appropriate <see cref="AbstractDbContext"/>.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Adds the current model to the appropriate <see cref="AbstractDbContext"/>.
        /// </summary>
        public abstract void Add();

        /// <summary>
        /// Returns a new locked <see cref="SemaLock"/>.
        /// </summary>
        public SemaLock NewSemaLock()
        {
            SemaLock l = new SemaLock(LOCK_SEMA);
            l.Wait();
            return l;
        }

        #endregion

        #region --Misc Methods (Private)--


        #endregion

        #region --Misc Methods (Protected)--


        #endregion
        //--------------------------------------------------------Events:---------------------------------------------------------------------\\
        #region --Events--


        #endregion
    }
}
