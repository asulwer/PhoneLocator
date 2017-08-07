using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace PhoneTracker
{
    class Data
    {
        public Plugin.Geolocator.Abstractions.Position Position { get; set; }
    }

    public partial class MainPage : ContentPage
	{
        public string Username { get; set; }

        public MainPage()
		{
			InitializeComponent();
            BindingContext = this;

            MessagingCenter.Subscribe<Data>(this, "Position", async (sender) => {
                string site = "http://phonelocator.azurewebsites.net/api/locations";
                //string site = "http://localhost:60373/api/locations";

                var json = JsonConvert.SerializeObject(new
                {
                    Latitude = (!double.IsNaN(sender.Position.Latitude)) ? sender.Position.Latitude : 0,
                    Longitude = (!double.IsNaN(sender.Position.Longitude)) ? sender.Position.Longitude : 0,
                    Speed = (!double.IsNaN(sender.Position.Speed)) ? sender.Position.Speed : 0,
                    Heading = (!double.IsNaN(sender.Position.Heading)) ? sender.Position.Heading : 0,
                    User = Username
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage responseMessage = await client.PostAsync(site, content);
                    responseMessage.EnsureSuccessStatusCode();
                }
            });
        }

        protected void Start_OnClicked(object sender, EventArgs args)
        {
            MessagingCenter.Send<MainPage>(this, "Start");
        }
        protected void Stop_OnClicked(object sender, EventArgs args)
        {
            MessagingCenter.Send<MainPage>(this, "Stop");
        }
    }
}
