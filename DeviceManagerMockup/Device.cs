using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManagerMockup
{
    public enum DeviceInterfaceType
    {
        USB,
        LAN,
        BLE
    }

    public abstract class Device
    {
        public string? Product { get; set; }
        public string? Address { get; set; }
        public string? FirmwareVersion { get; set; }
        public DeviceInterfaceType? InterfaceType { get; set; }

        public async Task Update(
    IProgress<int>? progressReporter,
    CancellationToken? cancellationToken)
        {
            // Simulate update process
            for (int i = 0; i < 100; i++)
            {
                // Check for cancellation
                cancellationToken?.ThrowIfCancellationRequested();

                // Report progress
                progressReporter?.Report(i);

                // Simulate some work
                await Task.Delay(10); // Simulating time during scan
            }

            progressReporter?.Report(100);
        }
    }

    public class USBDevice : Device 
    { 

    }

    public class LANDevice : Device
    {

    }

    public class BLEDevice : Device
    {

    }
}
