using Blazored.LocalStorage;
using EngineAnalyticsWebApp.Shared.Models.Engine;

namespace EngineAnalyticsWebApp.Shared.Services.Data
{
    public class AutomobileLocalStorageService(ILocalStorageService localStorage) : IAutomobileDataService
    {
        private readonly string localStorageAutoKey = "automobiles";

        public async Task AddAutomobile(Automobile automobile)
        {
            if (automobile != null)
            {
                // Get current collection and add new autommobile value to it
                var autos = await this.GetAutomobiles();
                autos = autos.Append(automobile);

                await localStorage.SetItemAsync(this.localStorageAutoKey, autos);
            }

        }

        public async Task<IEnumerable<Automobile>> GetAutomobiles()
        {
            IEnumerable<Automobile> automobiles = new List<Automobile>();
            var autos = await localStorage.GetItemAsync<IEnumerable<Automobile>>(this.localStorageAutoKey);
            return autos ?? automobiles;
        }
    }
}
