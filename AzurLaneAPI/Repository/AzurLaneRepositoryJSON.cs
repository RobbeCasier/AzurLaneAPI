using AzurLaneAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzurLaneAPI.Repository
{
    class AzurLaneRepositoryJSON: AzurLaneRepositoryBASE
    {
        public override async Task<List<Ship>> GetShips()
        {
            if (_fullListShips != null)
                return _fullListShips;

            var assembly = Assembly.GetExecutingAssembly();

            var resourceName = "AzurLaneAPI.Resources.Data.AzurLaneShips.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    try
                    {
                        var json = await reader.ReadToEndAsync();
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
}
