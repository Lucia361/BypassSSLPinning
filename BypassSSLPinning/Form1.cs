using Microsoft.Win32;
using SharpAdbClient;
using SharpAdbClient.DeviceCommands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace BypassSSLPinning
{
    public partial class Form1 : Form
    {
        private readonly AdbServer server = new AdbServer();

        private List<string> AppList = new List<string>();

        // Constants
        const string APKTOOL = "apktool_2.6.1.jar";
        const string APK_SIGNER = "uber-apk-signer-1.2.1.jar";

        public Form1()
        {
            InitializeComponent();

            // Setup ADB
            server.StartServer(@"./adb.exe", false);

            this.LogsBox.AppendText(server.GetStatus().ToString() + "\n");

            var client = new AdbClient();
            var devices = client.GetDevices();

            if (devices.Count == 0)
            {
                this.DeviceBox.Items.Add("No devices found.");
            }
            else
            {
                foreach (var device in devices)
                {
                    this.DeviceBox.Items.Add($"{device.Model} ({device.Serial})");
                }
            }

            this.DeviceBox.SelectedIndex = 0;

            // Check if Java is installed
            var (javaInstalled, javaVersion) = Utils.IsJavaInstalled();
            if (!javaInstalled)
            {
                MessageBox.Show("Java is not installed. Please install Java and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
            else
            {
                this.LogsBox.AppendText($"Java version: {javaVersion}\n");
            }
        }

        private void HandleReloadDevices(object sender, EventArgs e)
        {
            var client = new AdbClient();
            var devices = client.GetDevices();


            this.DeviceBox.Items.Clear();
            if (devices.Count == 0)
            {
                this.DeviceBox.Items.Add("No devices found.");
                MessageBox.Show("No devices found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                foreach (var device in devices)
                {
                    this.DeviceBox.Items.Add($"{device.Model} ({device.Serial})");
                }
                this.LogsBox.AppendText("Devices list reloaded.\n");
            }

            this.DeviceBox.SelectedIndex = 0;
        }

        private void ReloadCacheList(object sender, EventArgs e)
        {
            if (DeviceBox.Text == "No devices found.")
            {
                return;
            }

            var client = new AdbClient();
            var device = client.GetDevices()[DeviceBox.SelectedIndex];

            this.AppList.Clear();

            PackageManager manager = new PackageManager(client, device);
            foreach (var package in manager.Packages)
            {
                if (!package.Value.Contains("/system"))
                {
                    this.AppList.Add(package.Key);
                }

            }
        }

        private void HandleSearch(object sender, EventArgs e)
        {
            foreach (var app in this.AppList)
            {
                if (app.Contains(this.SearchBox.Text))
                {
                    this.SelectedAppBox.Text = app;
                }
            }
        }
        private void HandlePatch(object sender, EventArgs e)
        {
            if (DeviceBox.Text == "No devices found.")
            {
                MessageBox.Show("No devices found. Please connect a device and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var packageId = this.SelectedAppBox.Text;

            if (packageId == "")
            {
                MessageBox.Show("Please select an app first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var client = new AdbClient();
            var device = client.GetDevices()[DeviceBox.SelectedIndex];
            PackageManager manager = new PackageManager(client, device);

            var wd = Directory.GetCurrentDirectory();
            var pkgPath = Path.Combine(wd, packageId);
            var patchPath = Path.Combine(wd, packageId + "_patched");
            this.LogsBox.AppendText($"Package path: {pkgPath}\n");

            // Clear old files
            if (Directory.Exists(pkgPath)) { Directory.Delete(pkgPath, true); }
            if (Directory.Exists(patchPath)) { Directory.Delete(patchPath, true); }

            System.IO.Directory.CreateDirectory(pkgPath);
            System.IO.Directory.CreateDirectory(patchPath);

            // Pull APK
            this.LogsBox.AppendText("Pulling APK...\n");
            Utils.PullPackage(client, device, packageId);

            var listFiles = Directory.GetFiles(pkgPath);
            foreach (var file in listFiles)
            {
                var fileName = file.Split('\\').Last().Split('.').First();
                var apkPath = Path.Combine(pkgPath, file);
                var unpackedApkPath = Path.Combine(patchPath, fileName);
                var packedApkPath = Path.Combine(patchPath, fileName + ".repack.apk");
                var signedApkPath = Path.Combine(patchPath, fileName + ".repack-aligned-debugSigned.apk");
                var patchedApkPath = Path.Combine(patchPath, fileName + "_patched.apk");

                // Unpack APK
                this.LogsBox.AppendText($"Unpacking APK: {apkPath}\n");
                if (fileName == "base")
                {
                    Utils.RunCommand($"java -jar {APKTOOL} d {apkPath} -o {unpackedApkPath} -s");
                }
                else
                {
                    Utils.RunCommand($"java -jar {APKTOOL} d {apkPath} -o {unpackedApkPath} -s -r");
                }

                // Disable SSL Pinning
                this.LogsBox.AppendText($"Disabling SSL Pinning...\n");
                if (fileName == "base")
                {
                    Utils.PatchManifest(unpackedApkPath);
                    Utils.AddNetworkSecurityConfig(unpackedApkPath);
                }

                // Repack APK
                this.LogsBox.AppendText($"Repacking APK...\n");
                Utils.RunCommand($"java -jar {APKTOOL} b {unpackedApkPath} -o {packedApkPath} --use-aapt2");

                // Sign APK
                this.LogsBox.AppendText($"Signing APK...\n");
                Utils.RunCommand($"java -jar {APK_SIGNER} -a {packedApkPath}");

                // Clean up
                File.Delete(packedApkPath);
                Directory.Delete(unpackedApkPath, true);

                File.Move(signedApkPath, patchedApkPath);

                // Uninstall old APK
                this.LogsBox.AppendText("Uninstalling old APK...\n");
                manager.UninstallPackage(packageId);

                var listFiles2 = Directory.GetFiles(patchPath);
                foreach (var file2 in listFiles2)
                {
                    var fileName2 = file2.Split('\\').Last().Split('.').First();
                    var apkPath2 = Path.Combine(patchPath, file2);

                    // Install new APK
                    this.LogsBox.AppendText($"Installing new APK... {fileName2}\n");
                    manager.InstallPackage(apkPath2, true);
                }

                this.LogsBox.AppendText("Done!\n");
            }
        }
    }

}
