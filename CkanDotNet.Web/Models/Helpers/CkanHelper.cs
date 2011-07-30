﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CkanDotNet.Api;
using CkanDotNet.Web.Models.Helpers;
using CkanDotNet.Api.Model;
using System.Web.Caching;

namespace CkanDotNet.Web.Models
{
    public static class CkanHelper
    {
        public static CkanClient GetClient()
        {
            CkanClient client = new CkanClient(SettingsHelper.GetRepository());
            return client;
        }

        /// <summary>
        /// Get all packages from the CKAN group.
        /// TODO: Implement common caching proxy for cachable requests.
        /// </summary>
        /// <returns></returns>
        public static List<Package> GetAllPackages()
        {
            List<Package> packages = null;

            if (HttpRuntime.Cache["Packages"] != null)
            {
                packages = (List<Package>)HttpRuntime.Cache.Get("Packages");
            }
            else
            {
                // Create the CKAN search parameters
                var searchParameters = new PackageSearchParameters();
                searchParameters.Groups.Add(SettingsHelper.GetGroup());

                // Collect the results
                PackageSearchResponse<Package> response = CkanHelper.GetClient().SearchPackages<Package>(searchParameters);
                packages = response.Results;

                HttpRuntime.Cache.Insert(
                    "Packages",
                    packages,
                    null,
                    Cache.NoAbsoluteExpiration,
                    new TimeSpan(1, 0, 0),
                    CacheItemPriority.Default,
                    null
                );
            }

            return packages;
        }

        /// <summary>
        /// Get a license by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static License GetLicenseById(string id)
        {
            License license = null;

            List<License> licenses = GetLicenses();
            foreach (var licenseEntry in licenses)
            {
                if (licenseEntry.Id == id)
                {
                    license = licenseEntry;
                    break;
                }
            }
            return license;
        }

        /// <summary>
        /// Get the licenses from the repository.  Caches
        /// the licenses for 1 hour as these don't update frequently.
        /// TODO: Implement common caching proxy for cachable requests.
        /// </summary>
        /// <returns></returns>
        private static List<License> GetLicenses()
        {
            List<License> licenses = null;

            if (HttpRuntime.Cache["Licenses"] != null)
            {
                licenses = (List<License>)HttpRuntime.Cache.Get("Licenses");
            }
            else
            {
                licenses = CkanHelper.GetClient().GetLicenses();

                HttpRuntime.Cache.Insert(
                    "License",
                    licenses,
                    null,
                    Cache.NoAbsoluteExpiration,
                    new TimeSpan(1, 0, 0),
                    CacheItemPriority.Default,
                    null
                );
            }

            return licenses;
        }

    }
}