using ALListViewer.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ALListViewer.Repository
{
    class ALRepository
    {
        private List<Ship> _fullShipList;
        //sub list for basic filter
        private List<Ship> _filterList;
        //sub sub list for stats
        private List<Ship> _filterStatList;

        public async Task<List<Ship>> GetShips()
        {
            if (_fullShipList != null)
                return _fullShipList;

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
                    _fullShipList = new List<Ship>();
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
                        _fullShipList.Add(ship);
                    }
                    _fullShipList = _fullShipList.OrderBy(x => x.Id).ToList();

                    //local file
                    //...

                    _filterList = _fullShipList;
                    return _fullShipList;

                }
                catch(Exception)
                {
                    return null;
                }
            }
        }

        public List<Ship> SearchShip(string name)
        {
            List<Ship> sortedList = new List<Ship>();
            string lowerCaseShip, lowerCaseName = name.ToLower();
            foreach (Ship ship in _filterStatList)
            {
                lowerCaseShip = ship.Name.ToLower();
                if (lowerCaseShip.Contains(lowerCaseName))
                    sortedList.Add(ship);
            }
            return sortedList;
        }
    }
}