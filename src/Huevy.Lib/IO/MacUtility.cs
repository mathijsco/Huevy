using System;
using System.Net;
using System.Runtime.InteropServices;

namespace Huevy.Lib.IO
{
    internal static class MacUtility
    {
        /// <summary>
        /// Get the MAC address for a specified IP address
        /// </summary>
        /// <param name="ipAddress">The IP address to get the MAC address for.</param>
        /// <returns>MAC address, or null if it cannot be resolved.</returns>
        public static string Resolve(string ipAddress)
        {
            var dst = IPAddress.Parse(ipAddress);

            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;

            if (SendARP(BitConverter.ToInt32(dst.GetAddressBytes(), 0), 0, macAddr, ref macAddrLen) != 0)
                return null;

            string[] str = new string[(int)macAddrLen];
            for (int i = 0; i < macAddrLen; i++)
                str[i] = macAddr[i].ToString("X2");

            return string.Join(":", str);
        }

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        private static extern int SendARP(int destIp, int srcIP, byte[] macAddr, ref uint physicalAddrLen);
    }
}
