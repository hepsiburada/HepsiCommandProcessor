using System.Collections;
using System.Collections.Generic;
using Hepsi.CommandProcessor.Attributes;
using Polly;

namespace Hepsi.CommandProcessor
{
    /// <summary>
    /// Class PolicyRegistry
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
    /// This is a default implementation of <see cref="IAmAPolicyRegistry"/> and is suitable for most purposes excluding testing
    /// </summary>
    public class PolicyRegistry : IAmAPolicyRegistry, IEnumerable<KeyValuePair<string, Policy>>
    {
        private readonly Dictionary<string, Policy> policies = new Dictionary<string, Policy>();

        /// <summary>
        /// Gets the specified policy name.
        /// </summary>
        /// <param name="policyName">Name of the policy.</param>
        /// <returns>Policy.</returns>
        public Policy Get(string policyName)
        {
            return policies.ContainsKey(policyName) ? policies[policyName] : null;
        }

        /// <summary>
        /// Adds the specified policy name.
        /// </summary>
        /// <param name="policyName">Name of the policy.</param>
        /// <param name="policy">The policy.</param>
        public void Add(string policyName, Policy policy)
        {
            policies.Add(policyName, policy);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<string, Policy>> GetEnumerator()
        {
            return policies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
