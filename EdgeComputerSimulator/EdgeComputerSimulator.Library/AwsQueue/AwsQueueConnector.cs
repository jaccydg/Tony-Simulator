using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Amazon.Runtime;

namespace EdgeComputerSimulator.Library.AwsQueue
{
    public static class AwsQueueConnector
    {
        public static AWSCredentials LoadAWSCredentials()
        {
            var credentialsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".aws", "credentials");
            var lines = File.ReadAllLines(credentialsFilePath);

            var profileName = "default";
            var accessKeyId = string.Empty;
            var secretAccessKey = string.Empty;

            var isInDefaultProfile = false;
            foreach (var line in lines)
            {
                if (line.Trim().Equals($"[{profileName}]", StringComparison.OrdinalIgnoreCase))
                {
                    isInDefaultProfile = true;
                    continue;
                }

                if (isInDefaultProfile)
                {
                    if (line.Trim().StartsWith("["))
                    {
                        break; // End of default profile section
                    }

                    var keyValue = line.Split(new[] { '=' }, 2);
                    if (keyValue.Length == 2)
                    {
                        var key = keyValue[0].Trim();
                        var value = keyValue[1].Trim();
                        if (key.Equals("aws_access_key_id", StringComparison.OrdinalIgnoreCase))
                        {
                            accessKeyId = value;
                        }
                        else if (key.Equals("aws_secret_access_key", StringComparison.OrdinalIgnoreCase))
                        {
                            secretAccessKey = value;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(accessKeyId) || string.IsNullOrEmpty(secretAccessKey))
            {
                throw new InvalidOperationException("AWS credentials not found in the specified profile.");
            }

            return new BasicAWSCredentials(accessKeyId, secretAccessKey);
        }
    }

}
