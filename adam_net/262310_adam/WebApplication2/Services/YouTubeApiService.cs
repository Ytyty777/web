using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using WebApplication2.Models;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace WebApplication2.Services
{
    public class YouTubeApiService
    {
        private readonly string _key;

        public YouTubeApiService(IConfiguration cfg)
        {
            _key = cfg["YouTubeApiKey"] ?? throw new ArgumentNullException("Missing YouTube API key.");
        }

        public async Task<List<Video>> SearchVideosAsync(string phrase)
        {
            var yt = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = _key,
                ApplicationName = "MyYouTubeApp"
            });

            var call = yt.Search.List("snippet");
            call.Q = phrase;
            call.MaxResults = 5;

            var result = await call.ExecuteAsync();
            var output = new List<Video>();

            foreach (var item in result.Items)
            {
                if (!string.IsNullOrEmpty(item?.Id?.VideoId))
                {
                    output.Add(new Video
                    {
                        vID = item.Id.VideoId,
                        Title = item.Snippet.Title,
                        cID = item.Snippet.ChannelId,
                        data_m = item.Snippet.PublishedAtDateTimeOffset?.UtcDateTime ?? DateTime.UtcNow
                    });
                }
            }

            return output;
        }
    }
}
