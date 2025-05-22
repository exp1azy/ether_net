using Ether.Net.Entities;
using Ether.Net.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using SharpPcap;

namespace Ether.Net.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> to facilitate dependency injection setup for packet capturing.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers an <see cref="IPacketCaptureProvider"/> service with the specified capture device and optional capture options.
        /// 
        /// This method adds the provider as a singleton to the dependency injection container,
        /// allowing the capture device and options to be created via factory functions.
        /// </summary>
        /// <param name="services">The service collection to add the provider to.</param>
        /// <param name="deviceFactory">A factory function to create the <see cref="ICaptureDevice"/> instance.</param>
        /// <param name="optionsFactory">
        /// An optional factory function to create <see cref="CaptureOptions"/> for the provider.
        /// If omitted, default options will be used.
        /// </param>
        /// <returns>The original <see cref="IServiceCollection"/> to enable method chaining.</returns>
        public static IServiceCollection AddPacketCaptureProvider(this IServiceCollection services, Func<IServiceProvider, ICaptureDevice> deviceFactory, Func<IServiceProvider, CaptureOptions>? optionsFactory = null)
        {
            services.AddSingleton<IPacketCaptureProvider, PacketCaptureProvider>(sp =>
            {
                var device = deviceFactory(sp);
                var options = optionsFactory?.Invoke(sp);
                return new PacketCaptureProvider(device, options);
            });

            return services;
        }
    }
}
