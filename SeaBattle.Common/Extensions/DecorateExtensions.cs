using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace SeaBattle.Common.Extensions
{
    public static class DecorateExtensions
    {
        /// <summary>
        /// Replace all services of type TService in the collection of services with a wrappers of type TDecorator. 
        /// <para>
        /// The wrapper can take a constructor reference to an object of type TService. 
        /// The reference will point to the original service of type TService. For more information see pattern 'Decorator'
        /// </para>
        /// </summary>
        /// <typeparam name="TService">The service type.</typeparam>
        /// <typeparam name="TDecorator">The decorator type.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection Decorate<TService, TDecorator>(this IServiceCollection services)
            where TDecorator : TService
        {
            var serviceType = typeof(TService);
            var decoratorType = typeof(TDecorator);
            var descriptors = services.Where(x => x.ServiceType == serviceType).ToArray();

            if (descriptors.Any())
            {
                foreach (ServiceDescriptor serviceDescriptor in descriptors)
                {
                    var index = services.IndexOf(serviceDescriptor);
                    var decoratorDescriptor = CreateDecoratorDescriptor(serviceDescriptor, decoratorType);

                    services.Insert(index, decoratorDescriptor);
                    services.Remove(serviceDescriptor);
                }

                return services;
            }
            else
            {
                throw new InvalidOperationException("No registered types found that can be used for decorator.");
            }
        }

        private static ServiceDescriptor CreateDecoratorDescriptor(ServiceDescriptor serviceDescriptor, Type decoratorType)
        {
            Func<IServiceProvider, object> implementationFactory = CreateFactory(serviceDescriptor, decoratorType);

            return ServiceDescriptor.Describe
            (
                serviceType: serviceDescriptor.ServiceType,
                implementationFactory: implementationFactory,
                lifetime: serviceDescriptor.Lifetime
            );
        }

        private static Func<IServiceProvider, object> CreateFactory(ServiceDescriptor serviceDescriptor, Type decoratorType)
        {
            ConstructorInfo[] ctors = decoratorType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            ConstructorInfo firstCtor = ctors.FirstOrDefault();
            if (firstCtor is null)
            {
                throw new InvalidOperationException($"Type '{decoratorType}' does not contain public constructor.");
            }

            ParameterInfo[] parameters = firstCtor.GetParameters().Where(p => p.ParameterType == serviceDescriptor.ServiceType).ToArray();

            if (parameters.Length > 0)
            {
                return provider => ActivatorUtilities.CreateInstance(provider, decoratorType, GetInstance(provider, serviceDescriptor));
            }

            return provider => ActivatorUtilities.CreateInstance(provider, decoratorType);
        }

        private static object GetInstance(IServiceProvider provider, ServiceDescriptor descriptor)
        {
            if (descriptor.ImplementationInstance != null)
            {
                return descriptor.ImplementationInstance;
            }
            else if (descriptor.ImplementationType != null)
            {
                return ActivatorUtilities.GetServiceOrCreateInstance(provider, descriptor.ImplementationType);
            }
            else
            {
                return descriptor.ImplementationFactory(provider);
            }
        }
    }
}
