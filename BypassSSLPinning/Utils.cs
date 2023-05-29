using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace BypassSSLPinning
{
    internal class Utils
    {

        public static string RunCommand(string command)
        {
            string output;

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = "/c " + command;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();

                string stdout = process.StandardOutput.ReadToEnd().Trim();
                string stderr = process.StandardError.ReadToEnd().Trim();

                output = stdout + stderr;

                process.WaitForExit();
                process.Close();
            }
            catch (Exception ex)
            {
                output = "Error: " + ex.Message;
            }

            return output;
        }

        public static (bool, string) IsJavaInstalled()
        {
            var output = RunCommand("java -version");
            var line = output.Split('\n')[0];
            MatchCollection matches = Regex.Matches(line, @"(\d+\.)?(\d+\.)?(\*|\d+)");

            if (matches.Count > 0)
            {
                return (true, matches[0].Value);
            }
            else
            {
                return (false, "");
            }
        }

        public static void PullPackage(AdbClient client, DeviceData device, string packageName)
        {
            var receiver = new ConsoleOutputReceiver();

            client.ExecuteRemoteCommand("pm path " + packageName, device, receiver);
            var apks = receiver.ToString().Trim().Split('\n');

            foreach (var apk in apks)
            {
                var apkPath = apk.Split(':')[1];
                var apkName = apkPath.Split('/').Last().Trim();
                var outPath = "./" + packageName + '/' + apkName;
                RunCommand("adb.exe pull " + apkPath + " " + outPath);
            }
        }

        public static void PatchManifest(string unpackedApkPath)
        {
            string manifestPath = Path.Combine(unpackedApkPath, "AndroidManifest.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(manifestPath);
            XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
            nsManager.AddNamespace("android", "http://schemas.android.com/apk/res/android");

            XmlNode applicationNode = doc.SelectSingleNode("//manifest/application");

            if (applicationNode.Attributes["android:networkSecurityConfig"] == null)
            {
                // Add networkSecurityConfig attribute
                XmlAttribute networkSecurityConfigAttr = doc.CreateAttribute("android", "networkSecurityConfig", "http://schemas.android.com/apk/res/android");
                networkSecurityConfigAttr.Value = "@xml/network_security_config";
                applicationNode.Attributes.Append(networkSecurityConfigAttr);

                using (StreamWriter writer = new StreamWriter(manifestPath))
                {
                    doc.Save(writer);
                }
            }
        }

        public static void AddNetworkSecurityConfig(string unpackedApkPath)
        {
            string networkSecurityConfigPath = Path.Combine(unpackedApkPath, "res", "xml", "network_security_config.xml");

            using (StreamWriter writer = new StreamWriter(networkSecurityConfigPath))
            {
                writer.Write(@"<?xml version=""1.0"" encoding=""utf-8""?>
<network-security-config>
    <debug-overrides>
        <trust-anchors>
            <certificates src=""user"" />
        </trust-anchors>
    </debug-overrides>
    <base-config cleartextTrafficPermitted=""true"">
        <trust-anchors>
            <certificates src=""system"" />
            <certificates src=""user"" />
        </trust-anchors>
    </base-config>
</network-security-config>");
            }
        }
    }
}
