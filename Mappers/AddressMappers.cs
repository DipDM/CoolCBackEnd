using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.Address;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Mappers
{
    public static class AddressMappers
    {
        public static AddressDto ToAddressDto(this Address address)
        {
            return new AddressDto
            {
                AddressId = address.AddressId,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                State = address.State,
                Country = address.Country,
                PostalCode = address.PostalCode
            };
        }

        public static Address ToCreateFromAddress(this CreateAddressDto createaddressDto)
        {
            return new Address
            {
                City = createaddressDto.City,
                AddressLine1 = createaddressDto.AddressLine1,
                AddressLine2 = createaddressDto.AddressLine2,
                State = createaddressDto.State,
                Country = createaddressDto.Country,
                PostalCode = createaddressDto.PostalCode
            };
        }

        public static Address ToUpdateAddress(this UpdateAddressDto updateAddressDto)
        {
            return new Address
            {
                City = updateAddressDto.City,
                AddressLine1 = updateAddressDto.AddressLine1,
                AddressLine2 = updateAddressDto.AddressLine2,
                State = updateAddressDto.State,
                Country = updateAddressDto.Country,
                PostalCode = updateAddressDto.PostalCode,
            };
        }
    }
}