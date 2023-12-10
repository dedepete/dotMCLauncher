using System.Collections.Generic;

namespace dotMCLauncher.Versioning
{
    public class LibraryMavenName
    {
        public string FullName
        {
            get => $"{GroupId}:{ArtifactId}:{Version}{(Classifier != null ? $":{Classifier}" : string.Empty)}{(Type != null ? $"@{Type}" : string.Empty)}";
            set {
                string temp = value;
                if (temp.Contains("@")) {
                    string[] s = temp.Split('@');
                    if (!string.IsNullOrWhiteSpace(s[1])) {
                        Type = s[1];
                    }

                    temp = s[0];
                }

                string[] s2 = temp.Split(':');
                GroupId = s2[0];
                ArtifactId = s2[1];
                Version = s2[2];
                Classifier = s2.Length > 3 ? s2[3] : null;
            }
        }

        /*
         *
         * Considering Library name as GroupId:ArtifactId:Version[:Classifier][@Type]
         * 
         *              | net.neoforged.fancymodloader:earlydisplay:1.0.10@jar  | org.lwjgl:lwjgl-openal:3.3.2:natives-windows
         *
         * GroupId      | net.neoforged.fancymodloader                          | org.lwjgl
         *
         * ArtifactId   | earlydisplay                                          | lwjgl-openal
         *
         * Version      | 1.0.10                                                | 3.3.2
         *
         * Classifier   | Null                                                  | natives-windows
         *
         * Type         | jar (default)                                         | jar (default)
         *
         */

        public string GroupId { get; set; }
        public string ArtifactId { get; set; }
        public string Version { get; set; }
        public string Classifier { get; set; }
        public string Type { get; set; }

        private Dictionary<string, string> Natives { get; set; }

        public LibraryMavenName(string name)
        {
            FullName = name;
        }

        public override string ToString()
            => FullName;
    }
}
