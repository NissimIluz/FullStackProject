using InfraAttributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

namespace DynamicLoaderServiceImp
{
    public class LoadServices
    {
        string _path;
        public LoadServices(string servicesLocation)
        {
            _path = servicesLocation;
        }

        public void Load(IServiceCollection servise)
        {
            var files = Directory.GetFiles(_path, "*.dll");
            foreach (var file in files)
            {
                var assembly = Assembly.LoadFrom(file);
                foreach (var type in assembly.GetTypes())
                {
                    var policyAttribute = type.GetCustomAttribute<PolicyAttribute>();
                    var registerAttribute = type.GetCustomAttribute<RegisterAttribute>();
                    if (registerAttribute != null)
                    {
                        var x = registerAttribute.InterfaceType;
                        if (policyAttribute != null && policyAttribute.Policy == Policy.Singleton)
                        {
                            servise.AddSingleton(registerAttribute.InterfaceType, type);
                        }
                        else
                            servise.AddTransient(registerAttribute.InterfaceType, type);
                    }
                }
            }
        }
    }
}