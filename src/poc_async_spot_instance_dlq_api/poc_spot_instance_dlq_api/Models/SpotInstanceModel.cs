﻿using System.Net;

namespace poc_async_spot_instance_dlq_api.Models
{
    public class SpotInstanceModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public IList<string> Logs { get; set; }
        public SpotInstanceModel()
        {
            Logs = new List<string>();
        }
    }
}
