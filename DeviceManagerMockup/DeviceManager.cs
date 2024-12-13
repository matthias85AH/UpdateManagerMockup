using DeviceManagerMockup;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace DeviceManagerMockup;

public static class DeviceManager
{
    private static Random rnd = new Random();

    public static List<Device> Devices = new List<Device>();

    private static string GenerateRandomMACaddress()
    {
        byte[] macAddr = new byte[6];
        rnd.NextBytes(macAddr);

        // Set the first byte to be even to ensure it's a locally administered address
        macAddr[0] = (byte)(macAddr[0] & (byte)0xfe);

        // Format the MAC address as a string
        return string.Join(":", macAddr.Select(b => b.ToString("X2")));
    }

    public static async Task ScanAsync(
        DeviceInterfaceType? deviceInterfaceType,
        IProgress<int>? progressReporter,
        CancellationToken? cancellationToken,
        Action<Device>? newDeviceFound)
    {
        Devices.Clear();

        // Simulate scanning a large collection of items
        for (int i = 0; i < 100; i++)
        {
            // Check for cancellation
            cancellationToken?.ThrowIfCancellationRequested();

            if (i % 20 == 0)
            {
                Device? device = null;
                // Simulate scanning an item and finding it
                switch (deviceInterfaceType)
                {
                    case DeviceInterfaceType.USB:
                        device = new USBDevice();
                        break;
                    case DeviceInterfaceType.LAN:
                        device = new LANDevice();
                        break;
                    case DeviceInterfaceType.BLE:
                        device = new BLEDevice();
                        break;
                    default:
                        device = new BLEDevice();
                        break;
                }

                if (device != null)
                {
                    device.Product = (new List<string>(){"MAUI", "MAILA"})[rnd.Next(2)];
                    device.Address = GenerateRandomMACaddress();
                    device.InterfaceType = deviceInterfaceType;
                    device.FirmwareVersion = $"{rnd.Next(4)}.{rnd.Next(4)}.{rnd.Next(4)}";

                    // Add device to Device List
                    Devices.Add(device);

                    // Report the found object to the observer
                    newDeviceFound?.Invoke(device);
                }
            }

            // Report progress
            progressReporter?.Report(i);

            // Simulate some work
            await Task.Delay(20); // Simulating time during scan
        }

        progressReporter?.Report(100);
    }
}
