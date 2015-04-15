using System;
using Common.Logging;
using Newtonsoft.Json;


namespace Hepsi.CommandProcessor.Handlers
{
    /// <summary>
    /// Class RequestLoggingHandler.
    /// Logs a request to a <see cref="IHandleRequests"/> handler using the Common.Logging logger registered with the <see cref="CommandProcessor"/>
    /// The log shows the original <see cref="IRequest"/> properties as well as the timer handling.
    /// </summary>
    /// <typeparam name="TRequest">The type of the t request.</typeparam>
    public class RequestLoggingHandler<TRequest> : RequestHandler<TRequest> where TRequest : class, IRequest
    {
        private HandlerTiming timing;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestLoggingHandler{TRequest}"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public RequestLoggingHandler(ILog logger)
            : base(logger)
        { }

        /// <summary>
        /// Initializes from attribute parameters.
        /// </summary>
        /// <param name="initializerList">The initializer list.</param>
        public override void InitializeFromAttributeParams(params object[] initializerList)
        {
            timing = (HandlerTiming)initializerList[0];
        }

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>TRequest.</returns>
        public override TRequest Handle(TRequest command)
        {
            LogCommand(command);
            return base.Handle(command);
        }

        private void LogCommand(TRequest request)
        {
            logger.Info(m => m("Logging handler pipeline call. Pipeline timing {0} target, for {1} with values of {2} at: {3}", timing.ToString(), typeof(TRequest), JsonConvert.SerializeObject(request), DateTime.UtcNow));
        }
    }
}
