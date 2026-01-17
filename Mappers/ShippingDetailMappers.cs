using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolCBackEnd.Dtos.ShippingDetail;
using CoolCBackEnd.Models;

namespace CoolCBackEnd.Mappers
{
    public static class ShippingDetailMappers
{
    public static ShippingDetailDto ToShippingDto(this ShippingDetail shippingDetail)
    {
        return new ShippingDetailDto
        {
            ShippingDetailId = shippingDetail.ShippingDetailId,
            OrderId = shippingDetail.OrderId,
            AddressId = shippingDetail.AddressId,
            ShippingStatus = shippingDetail.ShippingStatus,
            TrackingNumber = shippingDetail.TrackingNumber,
            EstimatedDelivery = shippingDetail.EstimatedDelivery
        };
    }

    public static ShippingDetail ToEntity(this CreateShippingDetailDto createDto)
    {
        return new ShippingDetail
        {
            OrderId = createDto.OrderId,
            AddressId = createDto.AddressId,
            ShippingStatus = createDto.ShippingStatus,
            TrackingNumber = createDto.TrackingNumber,
            EstimatedDelivery = createDto.EstimatedDelivery
        };
    }

    public static void ApplyUpdates(this ShippingDetail shippingDetail, UpdateShippingDetailDto updateDto)
    {
        if (!string.IsNullOrEmpty(updateDto.ShippingStatus))
            shippingDetail.ShippingStatus = updateDto.ShippingStatus;
        if (!string.IsNullOrEmpty(updateDto.TrackingNumber))
            shippingDetail.TrackingNumber = updateDto.TrackingNumber;
        if (updateDto.EstimatedDelivery.HasValue)
            shippingDetail.EstimatedDelivery = updateDto.EstimatedDelivery.Value;
    }
}

}