using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GitStat
{
    internal class Program
    {
        static string GetWebInfo(string url)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.UserAgent = "[any words that is more than 5 characters]";
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string webInfo = reader.ReadToEnd();
                    return webInfo;
                }
            }
            catch { return ""; }
        }
        static void Main(string[] args)
        {
            CLIMain(args);
            return;
        }

        private static void CLIMain(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Usage: GitStat <repo-author> <repo-name> [-s] [-C <path>] [-S <path>]\n\nHelp:\n" +
                        "[-C <path>] - Compare current repo release status to an older one which is saved in a json file\n" +
                        "[-S <path>] - Save current repo release status to a file\n" +
                        "[-s] - Silent run, without writing to console except errors");
                    return;
                }
                bool silent = HasSilent(args);
                string url = $"https://api.github.com/repos/{args[0]}/{args[1]}/releases";
                string data = GetWebInfo(url);
                if (data == "")
                {
                    Console.WriteLine("Couldn't find the specified repo!");
                    return;
                }
                Release[] releases = JsonConvert.DeserializeObject<Release[]>(data);
                if (!silent)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    ulong totalDownloads = 0;
                    for (int i = 0; i < releases.Length; i++)
                    {
                        Release release = releases[i];
                        ulong totalReleaseDownloads = 0;
                        release.assets.ToList().ForEach(x => totalReleaseDownloads += x.downloadCount);
                        stringBuilder.AppendLine($"Release {release.name}, tag: {release.tagName}");
                        stringBuilder.AppendLine("-------------");
                        stringBuilder.AppendLine($"Release info:");
                        stringBuilder.AppendLine($"Published on: {release.publishedAt}");
                        stringBuilder.AppendLine($"Release Author: {release.author.login}");
                        stringBuilder.AppendLine($"Downloads: {totalReleaseDownloads}");
                        stringBuilder.AppendLine($"Download info:");
                        foreach (Asset item in release.assets)
                            stringBuilder.AppendLine($"{item.name}: {item.downloadCount}");
                        totalDownloads += totalReleaseDownloads;
                        stringBuilder.AppendLine("-------------");
                    }
                    Console.WriteLine($"{args[0]} - {args[1]}\n" +
                        $"Total downloads: {totalDownloads}");
                    Console.WriteLine(stringBuilder.ToString());
                }
                CompareFile(args, releases);
                SaveFile(args, data);
            }
            catch (Exception ex) { Console.WriteLine($"A critical error occured!\n {ex}"); Console.ReadKey(); }
        }

        private static void CompareFile(string[] args, Release[] releases)
        {
            for (int i = 2; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg == "-C" && i < args.Length)
                {
                    try
                    {
                        //TODO: actually compare
                        Release[] compareReleases = JsonConvert.DeserializeObject<Release[]>(File.ReadAllText(args[i + 1]));
                        ulong[,] downloadsReleases = new ulong[2, compareReleases.Length];
                        ulong[] totalDownloadsReleases = new ulong[2];
                        int compareIndex = compareReleases.Length - 1;
                        int originalIndex = compareReleases.Length - 1;
                        if (releases.Length > compareReleases.Length)
                        {
                            originalIndex += releases.Length - compareReleases.Length;
                            for (int j = 0; j < releases.Length - compareReleases.Length; j++)
                            {
                                Release release = releases[j];
                                Console.WriteLine($"Release '{release.name}' is new");
                                ulong newReleasesCount = 0;
                                release.assets.ToList().ForEach(x => newReleasesCount += x.downloadCount);
                                Console.WriteLine($"Download count: {newReleasesCount}");
                                totalDownloadsReleases[0] += newReleasesCount;
                            }
                        }
                        while (originalIndex >= 0 && compareIndex >= 0)
                        {
                            try
                            {
                                ulong newReleasesCount = 0;
                                ulong oldReleasesCount = 0;
                                releases[originalIndex].assets.ToList().ForEach(x => newReleasesCount += x.downloadCount);
                                compareReleases[compareIndex].assets.ToList().ForEach(x => oldReleasesCount += x.downloadCount);
                                downloadsReleases[0, compareIndex] = newReleasesCount;
                                downloadsReleases[1, compareIndex] = oldReleasesCount;
                                totalDownloadsReleases[0] += newReleasesCount;
                                totalDownloadsReleases[1] += oldReleasesCount;
                                originalIndex--;
                                compareIndex--;
                            }
                            catch (IndexOutOfRangeException)
                            {
                                originalIndex--;
                                Console.WriteLine($"Release '{compareReleases[compareReleases.Length - compareIndex - 1].name}' was removed");
                            }
                        }
                        if (totalDownloadsReleases[1] != totalDownloadsReleases[0])
                            Console.WriteLine($"Overall download count changed! {totalDownloadsReleases[1]} -> {totalDownloadsReleases[0]}");
                        else
                            Console.WriteLine($"Overall download count {totalDownloadsReleases[1]}");
                        bool changed = false;
                        for (int j = compareReleases.Length - 1; j >= 0; j--)
                        {
                            if (downloadsReleases[0,j] > downloadsReleases[1,j])
                            {
                                changed = true;
                                string releaseName = compareReleases[j].name;
                                Console.WriteLine($"Release '{releaseName}' has updated downloads from {downloadsReleases[1, j]} to {downloadsReleases[0, j]}");
                            }
                        }
                        if(!changed)
                            Console.WriteLine("Nothing has changed!");
                    }
                    catch (Exception ex) { Console.WriteLine("An error occured when comparing the file, check if the path is correct!\n"+ex); }
                }
            }
        }

        private static bool HasSilent(string[] args)
        {
            for (int i = 2; i < args.Length; i++)
            {
                if (args[i] == "-s")
                    return true;
            }
            return false;
        }

        private static void SaveFile(string[] args, string data)
        {
            for (int i = 2; i < args.Length; i++)
            {
                string arg = args[i];
                if(arg == "-S" && i < args.Length)
                {
                    try
                    {
                        File.WriteAllText(args[i + 1], data);
                        Console.WriteLine($"Succesfully saved data to {args[i + 1]}");
                    }
                    catch { Console.WriteLine("An error occured when saving the file, check if the path is correct!"); }
                }
            }
        }
    }
}
