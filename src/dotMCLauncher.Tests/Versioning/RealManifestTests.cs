using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using dotMCLauncher.Versioning;
using NUnit.Framework;

namespace dotMCLauncher.Tests.Versioning
{
    [TestFixtureSource(typeof(RealManifestFixtureData), "Fixtures")]
    public class RealManifestTests
    {
        private string Platform { get; }

        private string VersionId { get; }


        private VersionManifest VersionManifest { get; }

        public RealManifestTests(string version, string platform)
        {
            Platform = platform;
            VersionId = version;

            VersionManifest = VersionManifest.Parse(File.ReadAllText(TestContext.CurrentContext.TestDirectory + $@"\Versioning\Data\Versions\{VersionId}.json"));
        }

        [TestCase("x86")]
        [TestCase("x64")]
        [TestCase(null)]
        public void GettingLibraries(string architecture)
        {
            RuleConditions conditions = new RuleConditions {
                OsConditions = new OsConditions {
                    Architecture = architecture, Name = Platform
                }
            };

            List<Library> list = VersionManifest.Libraries.Where(lib =>
                lib.IsAllowed(conditions) || lib.IsAllowed(conditions) && lib.IsNativesFor(Platform)).ToList();

            foreach (Library library in list) {

                Console.WriteLine($"{library.Name} IsNatives: {library.IsNatives}");
                foreach (DownloadEntry downloadsEntry in library.GetDownloadsEntries(string.Empty, Platform, architecture == "x64")) {
                    Console.WriteLine($" Url: {downloadsEntry.Url}");
                    Console.WriteLine($" Path: {downloadsEntry.Path}");
                }
            }
        }

        [TestCase("x86")]
        [TestCase("x64")]
        [TestCase(null)]
        public void BuildingArguments(string architecture)
        {
            RuleConditions conditions = new RuleConditions {
                FeatureConditions = new FeatureConditions {
                    IsForDemoUser = true
                },
                OsConditions = new OsConditions {
                    Architecture = architecture, Name = Platform
                }
            };

            if (VersionManifest.Type == VersionManifestType.V2) {
                string jvmArguments = VersionManifest.BuildArgumentsByGroup(ArgumentsGroupType.JVM, null,
                    conditions);
                string gameArguments = VersionManifest.BuildArgumentsByGroup(ArgumentsGroupType.GAME, null,
                    conditions);

                Console.WriteLine("JVM:");
                Console.WriteLine(jvmArguments);

                Console.WriteLine();

                Console.WriteLine("GAME:");
                Console.WriteLine(gameArguments);
            } else {
                Console.WriteLine(VersionManifest.ArgumentsCollection.ToString(null));
            }
        }
    }

    public class RealManifestFixtureData
    {
        // ReSharper disable once UnusedMember.Global
        public static IEnumerable Fixtures
        {
            get {
                string[] versions = {
                    "1.14_Pre-Release_2", "1.13.2", "1.12.2", "1.11.2", "1.8.9", "1.7.10", "1.2.5", "1.0"
                };

                string[] operatingSystems = {
                    "windows", "linux", "osx", null
                };

                foreach (string version in versions) {
                    foreach (string operatingSystem in operatingSystems) {
                        yield return new TestFixtureData(version, operatingSystem) {
                            TestName = $"{version.Replace('.', '_')}. {operatingSystem ?? "null"}"
                        };
                    }
                }
            }
        }
    }
}
