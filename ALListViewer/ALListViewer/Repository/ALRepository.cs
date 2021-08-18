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

            string endpointShip = "https://raw.githubusercontent.com/AzurAPI/azurapi-js-setup/master/ships.json";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var responseShip = await client.GetAsync(endpointShip);

                    if (!responseShip.IsSuccessStatusCode)
                        throw new HttpRequestException(responseShip.ReasonPhrase);

                    string jsonShip = await responseShip.Content.ReadAsStringAsync();

                    JToken jObjShip = JsonConvert.DeserializeObject<JToken>(jsonShip);
                    _fullShipList = new List<Ship>();
                    foreach (JToken data in jObjShip)
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

                    _filterStatList = _filterList = _fullShipList;
                    return _fullShipList;

                }
                catch(Exception)
                {
                    return null;
                }
            }
        }

        public async Task GetVoices()
        {
            if (_fullShipList == null)
                return;

            string endpointVoice = "https://raw.githubusercontent.com/AzurAPI/azurapi-js-setup/master/voice_lines.json";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var responseVoice = await client.GetAsync(endpointVoice);

                    if (!responseVoice.IsSuccessStatusCode)
                        throw new HttpRequestException(responseVoice.ReasonPhrase);

                    string jsonVoice = await responseVoice.Content.ReadAsStringAsync();

                    JObject id = JsonConvert.DeserializeObject<JObject>(jsonVoice);
                    foreach (JProperty prop in id.Properties())
                    {
                        Ship ship = _fullShipList.Find(x => x.Id == prop.Name);
                        foreach (JProperty propSkin in prop.Children<JObject>().Properties())
                        {
                            foreach (Skin skin in ship.Skins)
                            {
                                if (skin.Name == propSkin.Name)
                                {
                                    skin.Lines = new List<VoiceLine>();
                                    JToken lines = propSkin.Children().ElementAt(0);
                                    foreach (JToken line in lines)
                                    {
                                        VoiceLine newLine = line.ToObject<VoiceLine>();
                                        skin.Lines.Add(newLine);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        public List<Ship> GetCommonShips()
        {
            List<Ship> fullList = _fullShipList;

            List<Ship> sortedList = new List<Ship>();

            foreach (Ship ship in fullList)
            {
                int id;
                if (int.TryParse(ship.Id, out id))
                    if (id < 1000)
                        sortedList.Add(ship);
            }
            _filterStatList = _filterList = sortedList;
            return _filterList;
        }

        public List<Ship> GetCollabsShips()
        {
            List<Ship> fullList = _fullShipList;

            List<Ship> sortedList = new List<Ship>();

            foreach (Ship ship in fullList)
            {
                int id;
                if (ship.Id.Contains("Collab"))
                    sortedList.Add(ship);
                else if (int.TryParse(ship.Id, out id))
                    if (id > 10000 && id < 30000)
                        sortedList.Add(ship);
            }
            _filterStatList = _filterList = sortedList;
            return _filterList;
        }

        public List<Ship> GetPRShips()
        {
            List<Ship> fullList = _fullShipList;

            List<Ship> sortedList = new List<Ship>();

            foreach (Ship ship in fullList)
            {
                if (ship.Id.Contains("Plan"))
                    sortedList.Add(ship);
            }
            _filterStatList = _filterList = sortedList;
            return _filterList;
        }

        public List<Ship> GetMETAShips()
        {
            List<Ship> fullList = _fullShipList;

            List<Ship> sortedList = new List<Ship>();

            foreach (Ship ship in fullList)
            {
                int id;
                if (int.TryParse(ship.Id, out id))
                    if (id > 30000)
                        sortedList.Add(ship);
            }
            _filterStatList = _filterList = sortedList;
            return _filterList;
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