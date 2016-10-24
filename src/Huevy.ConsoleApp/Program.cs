﻿using Huevy.Lib.ColorSource;
using Huevy.Lib.Controllers;
using Huevy.Lib.Utilities;
using Huevy.Lib.Utilities.BitmapDisplay;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Huevy.ConsoleApp
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            DoWorkLiveCapture().Wait();

            Console.WriteLine("Press ANY key to exit");
            Console.ReadKey(true);
        }



        private static async Task DoWorkLiveCapture()
        {
            var source = new LiveCaptureColorSource();
            while (true)
            {
                var scene = source.DetectScene();
                BitmapDisplayForm.Instance.LoadScene(scene);
                await Task.Delay(50);
            };
        }

        private static async Task DoWorkScreenshots()
        {
            while (true)
            {
                var screenshot = Screenshot.TakeSmall();
                BitmapDisplayForm.Instance.LoadBitmap(screenshot);
                await Task.Delay(700);
            };
        }

        private static async Task DoWork()
        {
            var controller = new ConnectionController();
            Console.WriteLine("Locating bridges...");

            var bridges = (await controller.FindBridges()).ToList();
            if (bridges.Count == 0)
            {
                Console.WriteLine("No bridge found on the network. Exiting...");
                return;
            }
            var bridgeIp = bridges[0];
            Console.WriteLine("Found bridge at " + bridgeIp);

            bool connected;
            do
            {
                Console.WriteLine("Connecting to bridge...");
                connected = await controller.OpenConnection(bridgeIp);
                if (!connected)
                {
                    Console.WriteLine("Can not connect to the bridge. Press the LINK button on the bridge and press any key to retry.");
                    Console.ReadKey(true);
                }
            } while (!connected);


        }
    }
}
