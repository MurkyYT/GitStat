using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitStat
{
    internal class Author
    {
        [JsonProperty("login")]
        public string login;
        [JsonProperty("id")]
        public long id;
        [JsonProperty("node_id")]
        public string nodeID;
        [JsonProperty("avatar_url")]
        public string avatarURL;
        [JsonProperty("gravatar_id")]
        public string gravatarID;
        [JsonProperty("url")]
        public string URL;
        [JsonProperty("html_url")]
        public string HTMLURL;
        [JsonProperty("followers_url")]
        public string followersURL;
        [JsonProperty("following_url")]
        public string followingURL;
        [JsonProperty("gists_url")]
        public string gistsURL;
        [JsonProperty("starred_url")]
        public string starredURL;
        [JsonProperty("subscriptions_url")]
        public string subscriptionsURL;
        [JsonProperty("organizations_url")]
        public string organizationsURL;
        [JsonProperty("repos_url")]
        public string reposURL;
        [JsonProperty("events_url")]
        public string eventsURL;
        [JsonProperty("received_events_url")]
        public string receivedEventsURL;
        [JsonProperty("type")]
        public string type;
        [JsonProperty("site_admin")]
        public bool siteAdmin;
    }
}
