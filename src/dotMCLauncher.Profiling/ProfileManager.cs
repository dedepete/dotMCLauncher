using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace dotMCLauncher.Profiling
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), MemberSerialization = MemberSerialization.OptIn)]
    public class ProfileManager : JsonSerializable, IEnumerable<LauncherProfile>, IDictionary<string, LauncherProfile>
    {
        public ProfileManager()
        {
            _profiles = new Dictionary<string, LauncherProfile>();
        }

        /// <summary>
        /// Last used profile's id. 
        /// </summary>
        [JsonProperty]
        public string SelectedProfile { get; set; }

        [JsonProperty("profiles")]
        private Dictionary<string, LauncherProfile> _profiles { get; set; }

        /// <summary>
        /// Launcher settings. 
        /// </summary>
        [JsonProperty]
        public Dictionary<string, object> Settings { get; set; }

        /// <summary>
        /// Launcher version. 
        /// </summary>
        [JsonProperty]
        public LauncherVersion LauncherVersion { get; set; }

        /// <summary>
        /// Authentication database. 
        /// </summary>
        [JsonProperty]
        public Dictionary<string, AuthenticationEntry> AuthenticationDatabase { get; set; }

        /// <summary>
        /// Last used entry from authentication database. 
        /// </summary>
        [JsonProperty]
        public SelectedUser SelectedUser { get; set; }

        /// <summary>
        /// Analytics token. I don't like analytics. Why do u even need this field?
        /// </summary>
        [JsonProperty]
        public string AnalyticsToken { get; set; }

        /// <summary>
        /// Analytics failcount. Mojang are watching us!
        /// </summary>
        [JsonProperty]
        public int AnalyticsFailcount { get; set; }

        /// <summary>
        /// Client token.
        /// </summary>
        [JsonProperty]
        public string ClientToken { get; set; }

        public void Clear()
            => _profiles.Clear();

        public bool Contains(KeyValuePair<string, LauncherProfile> item)
            => _profiles.Contains(item);

        public void CopyTo(KeyValuePair<string, LauncherProfile>[] array, int arrayIndex)
        {
            KeyValuePair<string, LauncherProfile>[] profilesArray = _profiles.ToArray();
            profilesArray.CopyTo(array, arrayIndex);
            _profiles = profilesArray.ToDictionary(key => key.Key, value => value.Value);
        }

        public int Count => _profiles.Count;
        public bool IsReadOnly => false;
        public ICollection<string> Keys => _profiles.Keys;
        public ICollection<LauncherProfile> Values => _profiles.Values;

        public void Add(KeyValuePair<string, LauncherProfile> item)
        {
            item.Value.Id = item.Key;
            Add(item.Key, item.Value);
        }

        public void Add(LauncherProfile profile)
        {
            do {
                profile.Id = Guid.NewGuid().ToString();
            } while (string.IsNullOrWhiteSpace(profile.Id) || ContainsKey(profile.Id));

            Add(profile.Id, profile);
        }

        public void Add(LauncherProfile profile, out string id)
        {
            do {
                profile.Id = Guid.NewGuid().ToString();
            } while (string.IsNullOrWhiteSpace(profile.Id) || ContainsKey(profile.Id));

            Add(profile.Id, profile);
            id = profile.Id;
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
            _profiles.Add(profile.Id, profile);
        }

        public bool ContainsKey(string id)
            => Keys.Contains(id);

        public bool Remove(KeyValuePair<string, LauncherProfile> pair)
            => _profiles.Remove(pair.Key);

        public bool Remove(LauncherProfile launcherProfile)
            => Remove(launcherProfile.Id);

        public bool Remove(string id)
            => Remove(_profiles.FirstOrDefault(entry => entry.Key == id));

        public bool TryGetValue(string id, out LauncherProfile value)
        {
            try {
                value = _profiles.First(entry => entry.Key == id).Value;
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
                Remove(id);
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

            _profiles = newProfiles;
        }

        public static ProfileManager Parse(string rawJsonProfileList)
            => JsonConvert.DeserializeObject<ProfileManager>(rawJsonProfileList);

        private void AssociateIds()
        {
            foreach (KeyValuePair<string, LauncherProfile> pair in _profiles) {
                pair.Value.Id = pair.Key;
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
            => AssociateIds();

        IEnumerator<KeyValuePair<string, LauncherProfile>> IEnumerable<KeyValuePair<string, LauncherProfile>>.GetEnumerator()
            => _profiles.GetEnumerator();

        public IEnumerator<LauncherProfile> GetEnumerator()
            => Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
