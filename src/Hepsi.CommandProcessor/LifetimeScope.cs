using System.Collections.Generic;
using Common.Logging;
using Hepsi.CommandProcessor.Extensions;

namespace Hepsi.CommandProcessor
{
    internal class LifetimeScope : IAmALifetime
    {
        private readonly IAmAHandlerFactory handlerFactory;
        private readonly ILog logger;
        private readonly List<IHandleRequests> trackedObjects = new List<IHandleRequests>();

        public LifetimeScope(IAmAHandlerFactory handlerFactory, ILog logger = null)
        {
            this.handlerFactory = handlerFactory;
            this.logger = logger;
        }

        public int TrackedItemCount
        {
            get { return trackedObjects.Count; }
        }

        public void Add(IHandleRequests instance)
        {
            trackedObjects.Add(instance);

            if (logger != null)
            {
                logger.Debug(m => m("Tracking instance {0} of type {1}", instance.GetHashCode(), instance.GetType()));
            }
        }

        public void Dispose()
        {
            trackedObjects.Each((trackedItem) =>
            {
                //free disposableitems
                handlerFactory.Release(trackedItem);

                if (logger != null)
                {
                    logger.Debug(m => m("Releasing handler instance {0} of type {1}", trackedItem.GetHashCode(), trackedItem.GetType()));
                }
            });

            //clear our tracking
            trackedObjects.Clear();
        }
    }
}
