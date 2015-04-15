using System;

namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Interface IAmAPipelineBuilder
    /// Builds the pipeline that handles an <see cref="IRequest"/>, with a target <see cref="IHandleRequests"/> and any orthogonal <see cref="IHandleRequests"/> for
    /// Quality of Service (qos)
    /// The default implementation is <see cref="PipelineBuilder{T}"/>
    /// </summary>
    /// <typeparam name="TRequest">The type of the t request.</typeparam>
    internal interface IAmAPipelineBuilder<TRequest> : IDisposable where TRequest : class, IRequest
    {
        /// <summary>
        /// Builds the specified request context.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <returns>Pipelines&lt;TRequest&gt;.</returns>
        Pipelines<TRequest> Build(IRequestContext requestContext);
    }
}
