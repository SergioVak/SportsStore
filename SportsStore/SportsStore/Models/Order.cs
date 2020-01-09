﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SportsStore.Models
{
    public class Order
    {
        [BindNever]
        public int OrderID { get; set; }

        [BindNever]
        public ICollection<CartLine> Lines { get; set; }

        [BindNever]
        public bool Shipped { get; set; }

        [Required(ErrorMessage = "Please enter а name")]
        //Введите имя
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the first address line")]
        //Введите первую строку адреса 
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }

        [Required(ErrorMessage = "Please enter а city name")]
        //Введите название города 
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter а state name")]
        //Введите название штата 
        public string State { get; set; }
        public string Zip { get; set; }

        [Required(ErrorMessage = "Please enter а country name")]
        // Введите название страны 
        public string Country { get; set; }
        public bool GiftWrap { get; set; }
    }
}