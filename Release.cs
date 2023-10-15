using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitStat
{
    internal class Release
    {
        [JsonProperty("url")]
        public string URL;
        [JsonProperty("assets_url")]
        public string AssetsURL;
        [JsonProperty("upload_url")]
        public string UploadURL;
        [JsonProperty("html_url")]
        public string HTMLURL;
        [JsonProperty("id")]
        public long id;
        [JsonProperty("author")]
        public Author author;
        [JsonProperty("node_id")]
        public string nodeID;
        [JsonProperty("tag_name")]
        public string tagName;
        [JsonProperty("target_commitish")]
        public string targetCommitish;
        [JsonProperty("name")]
        public string name;
        [JsonProperty("draft")]
        public bool draft;
        [JsonProperty("prerelease")]
        public bool prerelease;
        [JsonProperty("created_at")]
        public DateTime createdAt;
        [JsonProperty("published_at")]
        public DateTime publishedAt;
        [JsonProperty("assets")]
        public Asset[] assets;
        [JsonProperty("tarball_url")]
        public string tarballURL;
        [JsonProperty("zipball_url")]
        public string zipballURL;
        [JsonProperty("body")]
        public string body;
    }
}
