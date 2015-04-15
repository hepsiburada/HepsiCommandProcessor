using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Logging;
using Hepsi.CommandProcessor.Attributes;


namespace Hepsi.CommandProcessor.Handlers
{
    /// <summary>
    /// Class TimeoutPolicyHandler.
    /// The handler is injected into the pipeline if the <see cref="TimeoutPolicyAttribute"/>
    /// </summary>
    /// <typeparam name="TRequest">The type of the t request.</typeparam>
    public class TimeoutPolicyHandler<TRequest> : RequestHandler<TRequest> where TRequest : class, IRequest
    {
        /// <summary>
        /// The context holds a timeout cancellation token with this key, that can be used by handlers to cancel an operation
        /// and kill the thread which manages the timeout
        /// </summary>
        public const string CONTEXT_BAG_TIMEOUT_CANCELLATION_TOKEN = "TimeoutCancellationToken";
        private int milliseconds;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeoutPolicyHandler{TRequest}"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public TimeoutPolicyHandler(ILog logger)
            : base(logger)
        { }

        /// <summary>
        /// Initializes from attribute parameters.
        /// </summary>
        /// <param name="initializerList">The initializer list.</param>
        public override void InitializeFromAttributeParams(params object[] initializerList)
        {
            milliseconds = (int)initializerList[0];
        }

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>TRequest.</returns>
        public override TRequest Handle(TRequest command)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var task =
                TimeoutAfter(
                    Task<TRequest>.Factory.StartNew(() =>
                    {
                        //we already cancelled the task
                        cancellationToken.ThrowIfCancellationRequested();
                        //allow the handlers that can timeout to grab the cancellation token
                        Context.Bag[CONTEXT_BAG_TIMEOUT_CANCELLATION_TOKEN] = cancellationToken;
                        return base.Handle(command);
                    },
                    cancellationToken,
                    TaskCreationOptions.PreferFairness,
                    TaskScheduler.Current
                    ),
                    milliseconds,
                    cancellationTokenSource);

            task.Wait();

            return task.Result;
        }

        private static Task<TRequest> TimeoutAfter(Task<TRequest> task, int millisecondsTimeout, CancellationTokenSource cancellationTokenSource)
        {
            // Short-circuit #1: infinite timeout or task already completed
            if (task.IsCompleted || (millisecondsTimeout == Timeout.Infinite))
            {
                // Either the task has already completed or timeout will never occur.
                // No proxy necessary.
                return task;
            }

            // tcs.Task will be returned as a proxy to the caller
            var tcs = new TaskCompletionSource<TRequest>();

            // Short-circuit #2: zero timeout
            if (millisecondsTimeout == 0)
            {
                //signal cancellation to tasks that have run out of time - its up to them to try and abort
                cancellationTokenSource.Cancel();

                // We've already timed out.
                tcs.SetException(new TimeoutException());
                return tcs.Task;
            }

            // Set up a timer to complete after the specified timeout period
            var timer = new Timer(state =>
            {
                // Recover your state information
                var myTcs = (TaskCompletionSource<TRequest>)state;

                //signal cancellation to tasks that have run out of time - its up to them to try and abort
                cancellationTokenSource.Cancel();

                // Fault our proxy with a TimeoutException
                myTcs.TrySetException(new TimeoutException());
            },
                tcs,
                millisecondsTimeout,
                Timeout.Infinite
            );

            // Wire up the logic for what happens when source task completes
            task.ContinueWith((antecedent, state) =>
            {
                // Recover our state data
                var tuple = (Tuple<Timer, TaskCompletionSource<TRequest>>)state;

                // Cancel the Timer
                tuple.Item1.Dispose();

                // Marshal results to proxy
                MarshalTaskResults(antecedent, tuple.Item2);
            },
                Tuple.Create(timer, tcs),
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default
            );

            return tcs.Task;
        }

        private static void MarshalTaskResults(Task<TRequest> source, TaskCompletionSource<TRequest> proxy)
        {
            switch (source.Status)
            {
                case TaskStatus.Faulted:
                    if (source.Exception != null)
                    {
                        proxy.TrySetException(source.Exception);
                    }
                    break;
                case TaskStatus.Canceled:
                    proxy.TrySetCanceled();
                    break;
                case TaskStatus.RanToCompletion:
                    proxy.TrySetResult(source.Result);
                    break;
            }
        }
    }
}
