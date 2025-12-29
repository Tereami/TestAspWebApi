using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceId;

namespace TestAspWebApiClientApp
{
    public static class MachineIdGenerator
    {
        public static string Get()
        {
            string deviceId = new DeviceIdBuilder()
                .AddMachineName()
                .AddUserName()
                .AddMacAddress()
                .AddOsVersion()
                .ToString();
            return deviceId;
        }
    }
}
