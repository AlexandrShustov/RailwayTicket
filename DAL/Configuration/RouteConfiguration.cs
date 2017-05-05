﻿using System.Data.Entity.ModelConfiguration;
using Domain.Entities;

namespace DAL
{
    public class RouteConfiguration : EntityTypeConfiguration<Route>
    {
        internal RouteConfiguration()
        {
            ToTable("Route");

            HasKey(x => x.Id)
                .Property(x => x.Id)
                .IsRequired();

            HasMany(x => x.Stations);
        }
    }
}