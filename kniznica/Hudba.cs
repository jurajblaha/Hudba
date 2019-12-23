using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;

namespace kniznica
{
    public class Hudba
    {
        public static List<SongItem> GetSongTitlesByArtist()
        {
            var client = new RestClient("https://genius.p.rapidapi.com/search?q=Kendrick%20Lamar");
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "genius.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "251044c33cmshf7dc9e167389a36p14c6d9jsn647f11e8fe83");
            IRestResponse response = client.Execute(request);
            var sreader = new StringReader(response.Content);
            var reader = new JsonTextReader(sreader);

            var result = JsonSerializer.Create().Deserialize(reader) as Newtonsoft.Json.Linq.JObject;
            List<SongItem> hits = new List<SongItem>();
            var jsonHits = result.SelectToken("response.hits").Children();

            foreach (var jsonHit in jsonHits)
            {
                SongItem song = new SongItem();
                song.id = jsonHit.SelectToken("result.id").ToObject<int>();
                song.artist = jsonHit.SelectToken("result.primary_artist.name").ToString();
                song.title = jsonHit.SelectToken("result.title").ToString();
                song.lyrics = jsonHit.SelectToken("result.url").ToString();

                hits.Add(song);
            }

            return hits;
        }

        public static List<SongItem> Search(String str)
        {
            var client = new RestClient($"https://genius.p.rapidapi.com/search?q={WebUtility.UrlEncode(str)}");
            
                        RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "genius.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "251044c33cmshf7dc9e167389a36p14c6d9jsn647f11e8fe83");
            IRestResponse response = client.Execute(request);
            var sreader = new StringReader(response.Content);
            var reader = new JsonTextReader(sreader);

            var result = JsonSerializer.Create().Deserialize(reader) as Newtonsoft.Json.Linq.JObject;
            List<SongItem> hits = new List<SongItem>();
            var jsonHits = result.SelectToken("response.hits").Children();

            foreach (var jsonHit in jsonHits)
            {
                SongItem song = new SongItem();
                song.id = jsonHit.SelectToken("result.id").ToObject<int>();
                song.artist = jsonHit.SelectToken("result.primary_artist.name").ToString();
                song.title = jsonHit.SelectToken("result.title").ToString();
                song.lyrics = jsonHit.SelectToken("result.url").ToString();

                hits.Add(song);
            }

            return hits;
        }

        public static bool InsertIntoDB(List<SongItem> hits)
        {
            try
            {
                HudbaEntities entities = new HudbaEntities();
                foreach (SongItem hit in hits)
                {
                    Song song = new Song();
                    song.Artist = hit.artist;
                    song.Id = hit.id;
                    song.Lyrics = hit.lyrics;
                    song.Title = hit.title;

                    entities.Songs.Add(song); ;
                }

                entities.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool DeleteDBtable()
        {
            HudbaEntities entities = new HudbaEntities();
            entities.Database.ExecuteSqlCommand("Delete FROM Songs");

            entities.SaveChanges();

            return true;
        }
    }
}
