namespace lilMess.Tools
{
    using System.Net;
    using System.Text.RegularExpressions;

    public static class AddressParser
    {
        public static IPEndPoint ParseHostPort(string hostPort)
        {
            var hostPortMatch = new Regex(
                @"^(?<ip>(?:\[[\da-fA-F:]+\])|(?:\d{1,3}\.){3}\d{1,3})(?::(?<port>\d+))?$",
                RegexOptions.Compiled);

            var match = hostPortMatch.Match(hostPort);
            return !match.Success
                       ? null
                       : new IPEndPoint(
                             IPAddress.Parse(match.Groups["ip"].Value),
                             int.Parse(match.Groups["port"].Value));
        }
    }
}