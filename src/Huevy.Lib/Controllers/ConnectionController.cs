using Huetiful.Lib;
using Huevy.Lib.IO;
using Q42.HueApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Huevy.Lib.Controllers
{
    public sealed class ConnectionController
    {
        private HueClient _hueClient;

        internal HueClient Client { get { return _hueClient; } }

        public async Task<IEnumerable<string>> FindBridges()
        {
            var locator = new Q42.HueApi.HttpBridgeLocator();
            return await locator.LocateBridgesAsync(TimeSpan.FromSeconds(4));
        }

        private string RetrieveToken(string macAddress)
        {

            var config = AssemblyConfig.Load();

            string token;
            if (config.ApiKeys.TryGetValue(macAddress, out token))
                return token;
            return null;
        }

        private void StoreToken(string macAddress, string token)
        {
            var config = AssemblyConfig.Load();
            if (config.ApiKeys.ContainsKey(macAddress))
                config.ApiKeys[macAddress] = token;
            else
                config.ApiKeys.Add(macAddress, token);
            config.Save();
        }

        private async Task<bool> TryRegisterApplication(LocalHueClient hueClient, string macAddress)
        {
            try
            {
                var token = await hueClient.RegisterAsync("Huevy", Environment.MachineName);
                hueClient.Initialize(token);
                StoreToken(macAddress, token);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Link button not pressed")
                    return false;
                throw;
            }
            return true;
        }

        public async Task<bool> OpenConnection(string ipAddress)
        {
            if (_hueClient == null)
            {
                var macAddress = await Task.Factory.StartNew(() => MacUtility.Resolve(ipAddress));
                if (macAddress == null)
                {
                    Trace.WriteLine("Cannot find the MAC address of the bridge using an ARP request.");
                    return false;
                }

                var hueClient = new LocalHueClient(ipAddress);
                var token = RetrieveToken(macAddress);

                // Already a token for this bridge
                if (token != null)
                {
                    Trace.WriteLine("Token found for the bridge.");
                    hueClient.Initialize(token);
                }
                else
                {
                    Trace.WriteLine("No token for the bridge. Trying to register the application...");
                    if (!await TryRegisterApplication(hueClient, macAddress))
                        return false;
                }

                Bridge bridge = null;
                bool isUnautorized, registerAttemptFailed = false;

                Trace.WriteLine("Connecting to bridge...");
                do
                {
                    try
                    {
                        bridge = await hueClient.GetBridgeAsync();
                        isUnautorized = false;
                    }
                    catch (Exception ex)
                    {
                        // Unauthorized, try a register
                        if (ex.Message == "unauthorized user")
                            isUnautorized = true;
                        else throw;
                    }

                    if (isUnautorized)
                    {
                        if (registerAttemptFailed)
                        {
                            Trace.WriteLine("Application is unauthorized to send commands to the bridge. Registration failed.");
                            return false;
                        }

                        Trace.WriteLine("Application is unauthorized to send commands to the bridge. Trying to register it once...");
                        if (!await TryRegisterApplication(hueClient, macAddress))
                            return false;

                        registerAttemptFailed = true;
                    }
                } while (isUnautorized);

                _hueClient = hueClient;

                Trace.WriteLine("Connected");
            }

            return true;
        }

        public void CloseConnection()
        {
            _hueClient = null;
        }
    }

}
