using System;
using System.Web.Mvc;
using System.IO;

namespace PerpetuumSoft.Knockout
{
    public abstract class KnockoutRegionContext<TModel> : KnockoutContext<TModel>, IDisposable
    {
        protected KnockoutRegionContext(ViewContext viewContext)
            : base(viewContext)
        {
            if (viewContext == null)
                throw new ArgumentNullException("viewContext");
            writer = viewContext.Writer;
            InStack = true;
        }

        public bool InStack { get; set; }

        private bool disposed;
        private readonly TextWriter writer;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposed)
                return;
            disposed = true;

            WriteEnd(writer);
            if (InStack)
                ContextStack.RemoveAt(ContextStack.Count - 1);
        }

        public abstract void WriteStart(TextWriter writer);
        protected abstract void WriteEnd(TextWriter writer);
    }
}
