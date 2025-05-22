namespace Ether.Net.Entities
{
    /// <summary>
    /// Represents geolocation information retrieved for a given IP address using an external API.
    /// </summary>
    /// <param name="Query">The original IP address queried.</param>
    /// <param name="Status">The status of the query (e.g., success or fail).</param>
    /// <param name="Country">The country name of the IP address.</param>
    /// <param name="CountryCode">The ISO country code.</param>
    /// <param name="Region">The region code.</param>
    /// <param name="RegionName">The full name of the region.</param>
    /// <param name="City">The city name.</param>
    /// <param name="Zip">The postal code.</param>
    /// <param name="Lat">The latitude of the IP location.</param>
    /// <param name="Lon">The longitude of the IP location.</param>
    /// <param name="Timezone">The timezone name of the location.</param>
    /// <param name="Isp">The internet service provider name.</param>
    /// <param name="Org">The organization that owns the IP address.</param>
    /// <param name="As">The autonomous system (AS) name.</param>
    public readonly record struct IpGeolocation(
        string Query,
        string Status,
        string Country,
        string CountryCode,
        string Region,
        string RegionName,
        string City,
        string Zip,
        double Lat,
        double Lon,
        string Timezone,
        string Isp,
        string Org,
        string As)
    {
        /// <summary>
        /// Returns a human-readable string representation of the geolocation data.
        /// </summary>
        public override string ToString()
        {
            return $"[Query: {Query}; Status: {Status}; Country: {Country}; CountryCode: {CountryCode}; Region: {Region}; RegionName: {RegionName}; City: {City}; Zip: {Zip}; Lat: {Lat}; Lon: {Lon}; Timezone: {Timezone}; Isp: {Isp}; Org: {Org}; As: {As}]";
        }
    }
}
