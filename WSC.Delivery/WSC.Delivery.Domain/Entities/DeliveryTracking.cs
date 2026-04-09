using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Delivery.Domain.Entities
{
    public class DeliveryTracking
    {
        public int TrackingId { get; set; }
        public int DeliveryId { get; set; }
        public DeliveryStatus Status { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
