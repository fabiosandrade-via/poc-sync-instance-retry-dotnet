﻿using System.Net;

namespace poc_sync_spot_service_api.Model
{
    public class SpotInstanceModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        //public IList<string> Logs { get; set; }
        //public SpotInstanceModel()
        //{
        //    Logs = new List<string>();
        //}
        //public int GeneralThreshold { get; set; }
    }
}
