﻿namespace Wss3ContentRecovery.Models
{
    public class SettingsModel
    {
        public string Query = @"
                select ad.SiteId, ad.Id, ad.DirName, ad.LeafName, ads.Content, s.HostHeader
                from AllDocs ad, AllDocStreams ads, Sites s
                where ad.SiteId = ads.SiteId
                    and ad.Id = ads.Id
                    and s.Id = ad.SiteId
                    and ads.Content IS NOT NULL
                    and ad.DirName IS NOT NULL
                order by DirName";

        public int BufferSize { get; set; }
        public int CommandTimeout { get; set; }
        public int ConnectionTimeout { get; set; }
        public string Database { get; set; }
        public string Server { get; set; }
        public bool WhatIf { get; set; }
    }
}
