﻿namespace ScrumPm.AspNetCore.Common.Configuration
{
    public class DbConnectionOptions
    {
        public ConnectionStrings ConnectionStrings { get; set; }

        public DbConnectionOptions()
        {
            ConnectionStrings = new ConnectionStrings();
        }
    }
}
