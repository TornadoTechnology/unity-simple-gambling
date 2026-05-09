using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data.Objects.Ranks;
using UnityEngine;

namespace Data.Loader
{
    public static class RankLoader
    {
        public static List<Rank> Load()
        {
            var config = LoadRankConfig();
            return CreateRanks(config);
        }

        private static RankConfigList LoadRankConfig()
        {
            var path = GetStreamingPath("ranks.json");
            var json = File.ReadAllText(path);
            return JsonUtility.FromJson<RankConfigList>(json);
        }

        private static List<Rank> CreateRanks(RankConfigList config) =>
            config.Ranks.Select(CreateRank).ToList();

        private static Rank CreateRank(RankData data) => new(data.Id, data.Min, data.Max);

        private static string GetStreamingPath(string relativePath) => Path.Combine(Root.Path, relativePath);
    }
}
