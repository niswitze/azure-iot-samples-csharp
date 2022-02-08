using CommandLine;

namespace Microsoft.Azure.Devices.Client.Samples
{
    /// <summary>
    /// Parameters for the application.
    /// </summary>
    internal class Parameters
    {

        [Option(
            't',
            "TransportType",
            Default = TransportType.Mqtt,
            Required = false,
            HelpText = "The transport to use to communicate with the IoT Hub. Possible values include Mqtt, Mqtt_WebSocket_Only, Mqtt_Tcp_Only, Amqp, Amqp_WebSocket_Only, Amqp_Tcp_only, and Http1.")]
        public TransportType TransportType { get; set; }

        [Option(
            'x',
            "CertificatePath",
            Required = true,
            HelpText = "Relative file path to pfx file.")]
        public string CertificatePath { get; set; }

        [Option(
            'p',
            "CertificatePassword",
            Required = true,
            HelpText = "Pasword to read certificate data.")]
        public string CertificatePassword { get; set; }

        [Option(
            'd',
            "DeviceId",
            Required = true,
            HelpText = "Device ID in IoT Hub.")]
        public string DeviceId { get; set; }


        [Option(
            'i',
            "IoTHubHostName",
            Required = true,
            HelpText = "IoT Hub host name.")]
        public string IoTHubHostName { get; set; }


    }
}
