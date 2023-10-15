using Newtonsoft.Json;
using System;

namespace GitStat
{
    internal class Asset
    {
        [JsonProperty("url")]
        public string URL;
        [JsonProperty("id")]
        public long id;
        [JsonProperty("uploader")]
        public Author uploader;
        [JsonProperty("node_id")]
        public string nodeID;
        [JsonProperty("name")]
        public string name;
        [JsonProperty("label")]
        public string label;
        [JsonProperty("content_type")]
        public string contentType;
        [JsonProperty("state")]
        public string state;
        [JsonProperty("size")]
        public ulong size;
        [JsonProperty("download_count")]
        public ulong downloadCount;
        [JsonProperty("created_at")]
        public DateTime createdAt;
        [JsonProperty("published_at")]
        public DateTime publishedAt;
        [JsonProperty("browser_download_url")]
        public string browserDownloadURL;
    }
}