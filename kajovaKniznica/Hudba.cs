using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace kajovaKniznica
{
    public class Hudba
    {
        
        public static List<Song> GetSongTitlesByArtist()
        {
            var client = new RestClient("https://genius.p.rapidapi.com/search?q=Kendrick%20Lamar");
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "genius.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "251044c33cmshf7dc9e167389a36p14c6d9jsn647f11e8fe83");
            IRestResponse response = client.Execute(request);
            var sreader = new StringReader(response.Content);
            var reader = new JsonTextReader(sreader);

            var result = JsonSerializer.Create().Deserialize(reader) as Newtonsoft.Json.Linq.JObject;
            List<Song> hits = new List<Song>();
            var jsonHits = result.SelectToken("response.hits").Children();

            foreach(var jsonHit in jsonHits)
            {
                Song song = new Song();
                song.id = jsonHit.SelectToken("result.id").ToObject<int>();
                song.artist = jsonHit.SelectToken("result.primary_artist.name").ToString();
                song.title = jsonHit.SelectToken("result.title").ToString();
                song.lyrics = jsonHit.SelectToken("result.url").ToString();

                hits.Add(song);
            }

            return hits;   
        }

        public static bool InsertIntoDB(List<Song> hits)
        {
            foreach (Song h in hits)
            {
                
            }

            throw new NotImplementedException();
        } 

    }
}
