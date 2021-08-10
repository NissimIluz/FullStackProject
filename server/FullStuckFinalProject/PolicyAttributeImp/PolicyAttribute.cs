using System;

namespace InfraAttributes
{
    public enum Policy { Transient, Singleton };
    public class PolicyAttribute : Attribute
    {
        public PolicyAttribute(Policy policy)
        {
            Policy = policy;
        }
        public Policy Policy { get; private set; }
    }
}