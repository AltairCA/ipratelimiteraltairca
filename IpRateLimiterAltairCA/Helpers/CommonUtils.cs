using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using IpRateLimiter.AspNetCore.AltairCA.Models;
using Microsoft.AspNetCore.Http;

namespace IpRateLimiter.AspNetCore.AltairCA.Helpers
{
    internal static class CommonUtils
    {
        public static string GetClientIP(IpRateLimitOptions settings, IHttpContextAccessor httpContext)
        {
            string ip = string.Empty;
            if (string.IsNullOrWhiteSpace(settings.RealIpHeader))
            {
                ip = httpContext.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            else
            {
                ip = httpContext.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == settings.RealIpHeader).Value;
            }
            return ip;
        }
        public static string GetKey(string clientIp, string path,string timeSpan)
        {
            using (var algorithm = SHA512.Create()) //or MD5 SHA256 etc.
            {
                var hashedBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(clientIp,path, timeSpan)));

                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
        /// <summary>
        /// Source <see cref="https://stackoverflow.com/questions/1499269/how-to-check-if-an-ip-address-is-within-a-particular-subnet"/>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="subnetMask"></param>
        /// <returns></returns>
        public static bool IsInSubnet(this IPAddress address, string subnetMask)
        {
            var slashIdx = subnetMask.IndexOf("/");
            if (!subnetMask.Contains("/"))
            { // We only handle netmasks in format "IP/PrefixLength".
                throw new NotSupportedException("Only SubNetMasks with a given prefix length are supported.");
            }

            // First parse the address of the netmask before the prefix length.
            var maskAddress = IPAddress.Parse(subnetMask.Substring(0, slashIdx));

            if (maskAddress.AddressFamily != address.AddressFamily)
            { // We got something like an IPV4-Address for an IPv6-Mask. This is not valid.
                return false;
            }

            // Now find out how long the prefix is.
            int maskLength = int.Parse(subnetMask.Substring(slashIdx + 1));

            if (maskAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                // Convert the mask address to an unsigned integer.
                var maskAddressBits = BitConverter.ToUInt32(maskAddress.GetAddressBytes().Reverse().ToArray(),0);

                // And convert the IpAddress to an unsigned integer.
                var ipAdressBits = BitConverter.ToUInt32(address.GetAddressBytes().Reverse().ToArray(),0);

                // Get the mask/network address as unsigned integer.
                uint mask = uint.MaxValue << (32 - maskLength);

                // https://stackoverflow.com/a/1499284/3085985
                // Bitwise AND mask and MaskAddress, this should be the same as mask and IpAddress
                // as the end of the mask is 0000 which leads to both addresses to end with 0000
                // and to start with the prefix.
                return (maskAddressBits & mask) == (ipAdressBits & mask);
            }

            if (maskAddress.AddressFamily == AddressFamily.InterNetworkV6)
            {
                // Convert the mask address to a BitArray.
                var maskAddressBits = new BitArray(maskAddress.GetAddressBytes());

                // And convert the IpAddress to a BitArray.
                var ipAdressBits = new BitArray(address.GetAddressBytes());

                if (maskAddressBits.Length != ipAdressBits.Length)
                {
                    throw new ArgumentException("Length of IP Address and Subnet Mask do not match.");
                }

                // Compare the prefix bits.
                for (int i = 0; i < maskLength; i++)
                {
                    if (ipAdressBits[i] != maskAddressBits[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            throw new NotSupportedException("Only InterNetworkV6 or InterNetwork address families are supported.");
        }
    }
}
