using SharpPcap;

namespace Ether.Net
{
    /// <summary>
    /// Provides utilities for discovering network capture devices available on the host system.
    /// </summary>
    public static class DeviceObserver
    {
        /// <summary>
        /// Retrieves a list of all available network capture devices on the local machine.
        /// </summary>
        /// <returns>
        /// A list of <see cref="ICaptureDevice"/> instances representing the available network interfaces
        /// that support packet capturing.
        /// </returns>
        /// <remarks>
        /// This method internally uses SharpPcap's <see cref="CaptureDeviceList.Instance"/> to enumerate devices.
        /// It casts the result to a common interface for easier abstraction and testability.
        /// </remarks>
        public static IList<ICaptureDevice> GetAvailableDevices()
        {
            return CaptureDeviceList.Instance.Cast<ICaptureDevice>().ToList();
        }
    }
}