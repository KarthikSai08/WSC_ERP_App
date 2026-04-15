using System;
using System.Collections.Generic;
using System.Text;
using WSC.Delivery.Domain.Enums;

namespace WSC.Dashboard.Application.Dtos
{
    public class TrackingDisplayDto
    {
        public int TrackingId { get; set; }
        public int DeliveryId { get; set; }
        public DeliveryStatus Status { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }
    }
}
