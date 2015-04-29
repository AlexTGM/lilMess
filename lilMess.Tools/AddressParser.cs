namespace lilMess.Tools
{
    using System.Text.RegularExpressions;

    public struct Address
    {
        public string Ip;

        public int Port;
    }

    public static class AddressParser
    {
        private const string IpV4Pattern = @"(?<ipAddress>(?<octet1>\d{1,3})\.(?<octet2>\d{1,3})\.(?<octet3>\d{1,3})\.(?<octet4>\d{1,3})):(?<port>\d+)";

        private static readonly Regex IpV4Matcher = new Regex(IpV4Pattern);

        public static bool IsValidEndPoint(string hostPort, ref Address address)
        {
            var matchIpV4 = IpV4Matcher.Match(hostPort);

            if (!matchIpV4.Success) return false;

            for (var i = 2; i < 6; i++) { if (int.Parse(matchIpV4.Groups[i].Value) > 255) return false; }

            address.Ip = matchIpV4.Groups["ipAddress"].Value;
            address.Port = int.Parse(matchIpV4.Groups["port"].Value);

            return true;
        }
    }
}