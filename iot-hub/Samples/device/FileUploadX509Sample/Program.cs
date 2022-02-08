// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommandLine;
using System;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace Microsoft.Azure.Devices.Client.Samples
{
    /// <summary>
    /// This sample requires an IoT Hub linked to a storage account container.
    /// Find instructions to configure a hub at <see href="https://docs.microsoft.com/en-us/azure/iot-hub/iot-hub-configure-file-upload"/>.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// A sample to illustrate how to upload files from a device using certificate authentication.
        /// </summary>
        /// <param name="args">
        /// Run with `--help` to see a list of required and optional parameters.
        /// </param>
        /// <returns></returns>
        public static async Task<int> Main(string[] args)
        {
            // Parse application parameters
            Parameters parameters = null;
            ParserResult<Parameters> result = Parser.Default.ParseArguments<Parameters>(args)
                .WithParsed(parsedParams =>
                {
                    parameters = parsedParams;
                })
                .WithNotParsed(errors =>
                {
                    Environment.Exit(1);
                });

            X509Certificate2 certificate = LoadProvisioningCertificate(parameters);

            var authMethod = new DeviceAuthenticationWithX509Certificate(parameters.DeviceId, certificate);

            Console.WriteLine($"Connecting to {parameters.IoTHubHostName} using Device ID {parameters.DeviceId}");

            using var deviceClient = DeviceClient.Create(parameters.IoTHubHostName, authMethod, parameters.TransportType);

            var sample = new FileUploadSample(deviceClient);
            await sample.RunSampleAsync();

            await deviceClient.CloseAsync();

            Console.WriteLine("Done.");
            return 0;
        }

        private static X509Certificate2 LoadProvisioningCertificate(Parameters parameters)
        {
            var certificateCollection = new X509Certificate2Collection();
            certificateCollection.Import(
                parameters.CertificatePath,
                parameters.CertificatePassword,
                X509KeyStorageFlags.UserKeySet);

            X509Certificate2 certificate = null;

            foreach (X509Certificate2 element in certificateCollection)
            {
                Console.WriteLine($"Found certificate: {element?.Thumbprint} {element?.Subject}; PrivateKey: {element?.HasPrivateKey}");
                if (certificate == null && element.HasPrivateKey)
                {
                    certificate = element;
                }
                else
                {
                    element.Dispose();
                }
            }

            if (certificate == null)
            {
                throw new FileNotFoundException($"{parameters.CertificatePath} did not contain any certificate with a private key.");
            }

            Console.WriteLine($"Using certificate {certificate.Thumbprint} {certificate.Subject}");

            return certificate;
        }
    }
}
