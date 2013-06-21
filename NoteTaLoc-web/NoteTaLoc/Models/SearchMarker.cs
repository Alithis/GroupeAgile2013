using System;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace RateYoutRent.Models
{
    public class DetailsMarker
    {
        public DateTime Date { get; set; }

        public string UserName { get; set; }

        public double Score { get; set; }
    }

    public partial class SearchMarker
    {
        /// <summary>
        /// Id du marker
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Latitude du marker
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Longitude du marker
        /// </summary>
        public double Longitude { get; set; }

		/// <summary>
        /// Adresse
        /// </summary>
        public string Address { get; set; }
		
        /// <summary>
        /// Note moyenne
        /// </summary>
        public double Score { get; set; }

        public List<DetailsMarker> Details { get; set; }

    }

}