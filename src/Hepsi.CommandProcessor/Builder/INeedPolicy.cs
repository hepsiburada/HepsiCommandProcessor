namespace Hepsi.CommandProcessor.Builder
{
    /// <summary>
    /// Interface INeedPolicy{CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
    /// </summary>
    public interface INeedPolicy
    {
        /// <summary>
        /// Policieses the specified policy registry.
        /// </summary>
        /// <param name="policyRegistry">The policy registry.</param>
        /// <returns>INeedLogging.</returns>
        INeedLogging Policies(IAmAPolicyRegistry policyRegistry);
        /// <summary>
        /// Noes the policy.
        /// </summary>
        /// <returns>INeedLogging.</returns>
        INeedLogging NoPolicy();
    }
}
