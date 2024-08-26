using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace CoolCBackEnd.Models
{
    public class Address
    {
        [Key]
        public int AddressId {get; set;}
        public string? AddressLine1 {get; set;}
        public string? AddressLine2 {get; set;}
        public string? City {get; set;}
        public string? State {get; set;}
        public string? Country {get; set;}
        public string? PostalCode {get; set;}
    }
}