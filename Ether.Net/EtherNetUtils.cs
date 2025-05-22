using Ether.Net.Entities;
using System.Net;
using System.Net.Http.Json;

namespace Ether.Net
{
    /// <summary>
    /// Provides utility methods for resolving hostnames and retrieving geolocation data for IP addresses.
    /// </summary>
    public class EtherNetUtils
    {
        private readonly HttpClient _httpClient = new();

        /// <summary>
        /// Attempts to resolve a DNS hostname for the specified IP address.
        /// </summary>
        /// <param name="address">The <see cref="IPAddress"/> to resolve.</param>
        /// <param name="hostName">The resolved hostname if successful; otherwise, <c>null</c>.</param>
        /// <returns>
        /// <c>true</c> if the hostname was successfully resolved; otherwise, <c>false</c>.
        /// </returns>
        public bool TryResolveHostname(IPAddress address, out string? hostName)
        {
            try
            {
                hostName = Dns.GetHostEntry(address).HostName;
                return true;
            }
            catch
            {
                hostName = null;
                return false;
            }
        }

        /// <summary>
        /// Retrieves geolocation information for a specified IP address using the ip-api.com service.
        /// </summary>
        /// <param name="address">The <see cref="IPAddress"/> to look up.</param>
        /// <param name="cancellationToken">An optional cancellation token to cancel the request.</param>
        /// <returns>
        /// A task representing the asynchronous operation, with a result of type <see cref="IpGeolocation"/>
        /// containing the resolved geolocation data.
        /// </returns>
        /// <remarks>
        /// This method sends an HTTP GET request to the public API at http://ip-api.com.
        /// Be mindful of request limits for free usage.
        /// </remarks>
        public async Task<IpGeolocation> GetIpGeolocationAsync(IPAddress address, CancellationToken cancellationToken = default)
        {
            string ip = address.ToString();
            return await ProcessGetIpGeolocationAsync(ip, cancellationToken);
        }

        /// <summary>
        /// Retrieves geolocation information for a specified IP address using the ip-api.com service.
        /// </summary>
        /// <param name="address">The IP address to look up.</param>
        /// <param name="cancellationToken">An optional cancellation token to cancel the request.</param>
        /// <returns>
        /// A task representing the asynchronous operation, with a result of type <see cref="IpGeolocation"/>
        /// containing the resolved geolocation data.
        /// </returns>
        /// <remarks>
        /// This method sends an HTTP GET request to the public API at http://ip-api.com.
        /// Be mindful of request limits for free usage.
        /// </remarks>
        public async Task<IpGeolocation> GetIpGeolocationAsync(string address, CancellationToken cancellationToken = default)
        {
            return await ProcessGetIpGeolocationAsync(address, cancellationToken);
        }

        private async Task<IpGeolocation> ProcessGetIpGeolocationAsync(string address, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://ip-api.com/json/{address}", cancellationToken);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<IpGeolocation>(cancellationToken: cancellationToken);

                throw new HttpRequestException(response.ReasonPhrase, null, response.StatusCode);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
