using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PhoneLocator.Models
{
    public class Location
    {
        public Location()
        {
            this.Updated = DateTime.Now;
            this.User = string.Empty;
            this.Latitude = 0;
            this.Longitude = 0;
            this.Speed = 0;
            this.Heading = 0;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; private set; }
        public DateTime? Updated { get; set; }
        public string User { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Speed { get; set; }
        public decimal? Heading { get; set; }
    }
}