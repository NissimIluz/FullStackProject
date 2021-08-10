using System;

namespace InfraAttributes
{
    public class RegisterAttribute : Attribute
    {
        public RegisterAttribute(Type type)
        {
            InterfaceType = type;
        }
        public Type InterfaceType { get; private set; }
    }
}