using AzurLaneAPI.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace AzurLaneAPI.Repository
{
    class AzurLaneRepositoryWEB : AzurLaneRepositoryBASE
    {
        public override async Task<List<Ship>> GetShips()
        {
            if (_fullListShips != null)
                return _fullListShips;
            string endpoint = "https://raw.githubusercontent.com/AzurAPI/azurapi-js-setup/master/ships.json";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(endpoint);

                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException(response.ReasonPhrase);

                    string json = await response.Content.ReadAsStringAsync();

                    JToken jObj = JsonConvert.DeserializeObject<JToken>(json);
                    _fullListShips = new List<Ship>();
                    foreach (JToken data in jObj)
                    {
                        Ship ship = data.ToObject<Ship>();
                        ship.Name = (string)data.SelectToken("names.en");
                        ship.Stars = (string)data.SelectToken("stars.stars");
                        ship.ConstructionTime = (string)data.SelectToken("construction.constructionTime");
                        for (int i = 0; i < ship.Skills.Length; i++)
                        {
                            ship.Skills[i].Name = (string)data.SelectToken($"skills[{i}].names.en");
                        }
                        _fullListShips.Add(ship);
                    }
                    _fullListShips = _fullListShips.OrderBy(x => x.Id).ToList();

                    _filterList = _fullListShips;

                    json = File.ReadAllText(@"D:\AzurLaneAPIList.json");
                    return _fullListShips;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
