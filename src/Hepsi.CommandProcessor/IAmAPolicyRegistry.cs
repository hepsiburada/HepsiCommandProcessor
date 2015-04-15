using Polly;

namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Interface IAmAPolicyRegistry
    /// We use <a href="">Polly</a> policies to provide Quality of Service. 
    /// By default we provide them for a <a href="http://parlab.eecs.berkeley.edu/wiki/_media/patterns/taskqueue.pdf">Task Queue</a> and require you to register policies with
    /// the policy names of:
    /// <list type="bullet">
    /// <item>CommandProcessor.RETRYPOLICY</item>
    /// <item>CommandProcessor.CIRCUTIBREAKER</item>
    /// </list>
    /// to respectively determine retry attempts for putting onto and popping off the queue and for breaking the circuit if we cannot
    /// You can register additional policies (or reuse these) to provide QoS for individual handlers. The <see cref="UsePolicyAttribute"/> and <see cref="ExceptionPolicyandler"/>
    /// provide an easy way to do this using the policies that you add to this registry
    /// The default implementation of <see cref="PolicyRegistry"/> is suitable for most purposes and the interface is provided for testing.0vjy
    /// </summary>
    public interface IAmAPolicyRegistry
    {
        /// <summary>
        /// Gets the specified policy name.
        /// </summary>
        /// <param name="policyName">Name of the policy.</param>
        /// <returns>Policy.</returns>
        Policy Get(string policyName);
    }
}
