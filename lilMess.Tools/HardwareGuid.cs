namespace lilMess.Tools
{
    using System;
    using System.Management;
    using System.Security.Cryptography;
    using System.Text;

    public static class HardwareGuid
    {
        public static string GetUniqueHardwareId()
        {
            var sb = new StringBuilder();

            var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");

            foreach (var queryObj in searcher.Get())
            {
                sb.Append(queryObj["NumberOfCores"]);
                sb.Append(queryObj["ProcessorId"]);
                sb.Append(queryObj["Name"]);
                sb.Append(queryObj["SocketDesignation"]);
            }

            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");

            foreach (var queryObj in searcher.Get())
            {
                sb.Append(queryObj["Manufacturer"]);
                sb.Append(queryObj["Name"]);
                sb.Append(queryObj["Version"]);
            }

            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

            foreach (var queryObj in searcher.Get())
            {
                sb.Append(queryObj["Product"]);
            }

            var bytes = Encoding.ASCII.GetBytes(sb.ToString());
            var hash = new SHA256Managed().ComputeHash(bytes);

            return BitConverter.ToString(hash);
        }
    }
}