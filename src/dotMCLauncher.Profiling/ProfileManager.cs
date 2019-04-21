using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Profiling
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptOut)]
    public class ProfileManager : JsonSerializable, IEnumerable<LauncherProfile>, IDictionary<string, LauncherProfile>
    {
        /// <summary>
        /// Last used profile's id. 
        /// </summary>
        public string SelectedProfile { get; set; }

        [JsonProperty]
        private IList<KeyValuePair<string, LauncherProfile>> Profiles { get; set; }

        [JsonProperty("profiles")]
        private Dictionary<string, LauncherProfile> _profiles
        {
            get {
                if (Profiles == null || !Profiles.Any()) {
                    return null;
                }

                return Profiles.ToDictionary(pair => pair.Key, pair => pair.Value);
            }
            set => Profiles = value.ToList();
        }

        /// <summary>
        /// Launcher settings. 
        /// </summary>
        public Dictionary<string, object> Settings { get; set; }

        /// <summary>
        /// Launcher version. 
        /// </summary>
        public LauncherVersion LauncherVersion { get; set; }

        /// <summary>
        /// Authentication database. 
        /// </summary>
        public Dictionary<string, AuthenticationEntry> AuthenticationDatabase { get; set; }

        /// <summary>
        /// Last used entry from authentication database. 
        /// </summary>
        public SelectedUser SelectedUser { get; set; }

        /// <summary>
        /// Analytics token. I don't like analytics. Why do u even need this field?
        /// </summary>
        public string AnalyticsToken { get; set; }

        /// <summary>
        /// Analytics failcount. Mojang are watching us!
        /// </summary>
        public int AnalyticsFailcount { get; set; }

        /// <summary>
        /// Client token.
        /// </summary>
        public string ClientToken { get; set; }

        public void Clear()
        {
            Profiles.Clear();
        }

        public bool Contains(KeyValuePair<string, LauncherProfile> item)
        {
            return Profiles.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, LauncherProfile>[] array, int arrayIndex)
        {
            Profiles.CopyTo(array, arrayIndex);
        }

        public int Count => Profiles.Count;
        public bool IsReadOnly => Profiles.IsReadOnly;
        public ICollection<string> Keys => Profiles.ToDictionary(entry => entry.Key, entry => entry.Value).Keys;
        public ICollection<LauncherProfile> Values => Profiles.ToDictionary(entry => entry.Key, entry => entry.Value).Values;

        public void Add(KeyValuePair<string, LauncherProfile> item)
        {
            Profiles.Add(item);
        }

        public void Add(LauncherProfile profile)
        {
            do {
                profile.Id = Guid.NewGuid().ToString();
            } while (string.IsNullOrWhiteSpace(profile.Id) || ContainsKey(profile.Id));

            Add(new KeyValuePair<string, LauncherProfile>(profile.Id, profile));
        }

        public void Add(string id, LauncherProfile profile)
        {
            if (string.IsNullOrWhiteSpace(id)) {
                throw new ArgumentNullException(nameof(id));
            }

            if (ContainsKey(id)) {
                throw new ArgumentException($"Profile with id '{id}' already exists.");
            }

            profile.Id = id;
            Add(new KeyValuePair<string, LauncherProfile>(profile.Id, profile));
        }

        public bool ContainsKey(string key)
        {
            return Keys.Contains(key);
        }

        public bool Remove(KeyValuePair<string, LauncherProfile> item)
        {
            return Profiles.Remove(item);
        }

        public bool Remove(LauncherProfile launcherProfile)
        {
            return Remove(launcherProfile.Id);
        }

        public bool Remove(string id)
        {
            return Remove(Profiles.FirstOrDefault(entry => entry.Key == id));
        }

        public bool TryGetValue(string key, out LauncherProfile value)
        {
            try {
                value = Profiles.First(entry => entry.Key == key).Value;
                return true;
            }
            catch {
                value = null;
                return false;
            }
        }

        public LauncherProfile this[string id]
        {
            get { return Values.FirstOrDefault(entry => entry.Id == id); }
            set {
                Profiles.Remove(Profiles.First(entry => entry.Key == id));
                Add(id, value);
            }
        }

        public void ChangeProfileId(string id, string newId)
        {
            Dictionary<string, LauncherProfile> newProfiles = new Dictionary<string, LauncherProfile>();
            foreach (LauncherProfile profile in Values) {
                if (profile.Id == id) {
                    profile.Id = newId;
                }

                newProfiles.Add(profile.Id, profile);
            }

            Profiles = newProfiles.ToList();
        }

        public static ProfileManager Parse(string rawJsonProfileList)
        {
            return JsonConvert.DeserializeObject<ProfileManager>(rawJsonProfileList);
        }

        private void AssociateIds()
        {
            foreach (KeyValuePair<string, LauncherProfile> pair in Profiles) {
                pair.Value.Id = pair.Key;
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            AssociateIds();
        }

        IEnumerator<KeyValuePair<string, LauncherProfile>> IEnumerable<KeyValuePair<string, LauncherProfile>>.GetEnumerator()
        {
            return Profiles.GetEnumerator();
        }

        public IEnumerator<LauncherProfile> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
