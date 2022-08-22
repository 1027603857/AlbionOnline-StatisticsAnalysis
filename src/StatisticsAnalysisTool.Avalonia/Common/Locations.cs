using System;
using System.Collections.Generic;

namespace StatisticsAnalysisTool.Avalonia.Common
{
    public static class Locations
    {
        public static readonly Dictionary<Location, string> ParameterNames = new()
        {
            { Location.Thetford, "Thetford" },
            { Location.SwampCross, "Swamp Cross" },
            { Location.Lymhurst, "Lymhurst" },
            { Location.ForestCross, "Forest Cross" },
            { Location.Bridgewatch, "Bridgewatch" },
            { Location.SteppeCross, "Steppe Cross" },
            { Location.HighlandCross, "Highland Cross" },
            { Location.BlackMarket, "Black Market" },
            { Location.Martlock, "Martlock" },
            { Location.Caerleon, "Caerleon" },
            { Location.FortSterling, "Fort Sterling" },
            { Location.MountainCross, "Mountain Cross" },
            { Location.ArthursRest, "Arthurs Rest" },
            { Location.MerlynsRest, "Merlyns Rest" },
            { Location.MorganasRest, "Morganas Rest" }
        };

        public static string GetParameterName(Location location)
        {
            return ParameterNames.TryGetValue(location, out var name) ? name : string.Empty;
        }

        public static Location GetLocationByIndex(string index)
        {
            if (index.Equals("@BLACK_MARKET"))
            {
                return Location.BlackMarket;
            }

            if (index.Equals("3013-Auction2"))
            {
                return Location.Caerleon;
            }

            return Enum.TryParse(index, true, out Location location) ? location : Location.Unknown;
        }

        public static List<Location> GetLocationsListByArea(bool blackZoneOutposts, bool villages, bool cities, bool blackMarket, bool withPortalCities)
        {
            var locationAreas = new List<LocationArea>();

            if (villages)
            {
                locationAreas.Add(LocationArea.Villages);
            }

            if (blackZoneOutposts)
            {
                locationAreas.Add(LocationArea.BlackZone);
            }

            if (cities)
            {
                locationAreas.Add(LocationArea.Cities);
            }

            if (blackMarket)
            {
                locationAreas.Add(LocationArea.BlackMarket);
            }

            var locations = new List<Location>();

            foreach (var area in locationAreas)
                switch (area)
                {
                    case LocationArea.BlackMarket:
                        locations.Add(Location.BlackMarket);
                        break;

                    case LocationArea.BlackZone:
                        locations.Add(Location.ArthursRest);
                        locations.Add(Location.MerlynsRest);
                        locations.Add(Location.MorganasRest);
                        break;

                    case LocationArea.Villages:
                        locations.Add(Location.SwampCross);
                        locations.Add(Location.ForestCross);
                        locations.Add(Location.SteppeCross);
                        locations.Add(Location.HighlandCross);
                        locations.Add(Location.MountainCross);
                        break;

                    case LocationArea.Cities:
                        locations.Add(Location.Thetford);
                        locations.Add(Location.Lymhurst);
                        locations.Add(Location.Bridgewatch);
                        locations.Add(Location.Martlock);
                        locations.Add(Location.FortSterling);
                        locations.Add(Location.Caerleon);
                        if (withPortalCities)
                        {
                            locations.Add(Location.ThetfordPortal);
                            locations.Add(Location.LymhurstPortal);
                            locations.Add(Location.BridgewatchPortal);
                            locations.Add(Location.MartlockPortal);
                            locations.Add(Location.FortSterlingPortal);
                        }

                        break;
                }

            return locations;
        }

        public static Location GetLocationByLocationNameOrId(string location)
        {
            return location switch
            {
                "Thetford" => Location.Thetford,
                "Lymhurst" => Location.Lymhurst,
                "Bridgewatch" => Location.Bridgewatch,
                "Martlock" => Location.Martlock,
                "Fort Sterling" => Location.FortSterling,
                "0301" or "Thetford Portal" => Location.ThetfordPortal,
                "1301" or "Lymhurst Portal" => Location.LymhurstPortal,
                "2301" or "Bridgewatch Portal" => Location.BridgewatchPortal,
                "3301" or "Martlock Portal" => Location.MartlockPortal,
                "4301" or "Fort Sterling Portal" => Location.FortSterlingPortal,
                "Caerleon" => Location.Caerleon,
                "Swamp Cross" => Location.SwampCross,
                "Forest Cross" => Location.ForestCross,
                "Steppe Cross" => Location.SteppeCross,
                "Highland Cross" => Location.HighlandCross,
                "Mountain Cross" => Location.MountainCross,
                "Arthurs Rest" => Location.ArthursRest,
                "Merlyns Rest" => Location.MerlynsRest,
                "Morganas Rest" => Location.MorganasRest,
                "Black Market" => Location.BlackMarket,
                _ => Location.Unknown,
            };
        }

        // TODO: Rework for avalonia, put it direct into a xaml file
        //public static SolidColorPaint GetLocationBrush(Location location, bool transparent)
        //{
        //    if (location == Location.Unknown)
        //    {
        //        return new SolidColorPaint
        //        {
        //            Color = new SKColor(0, 0, 0, 0)
        //        };
        //    }

        //    try
        //    {
        //        if (transparent)
        //        {
        //            var scbt = (SolidColorBrush)Application.Current.Resources[$"SolidColorBrush.City.{location}.Transparent"];
        //            return new SolidColorPaint
        //            {
        //                Color = new SKColor(scbt.Color.R, scbt.Color.G, scbt.Color.B, scbt.Color.A)
        //            };
        //        }

        //        var scb = (SolidColorBrush)Application.Current.Resources[$"SolidColorBrush.City.{location}"];
        //        return new SolidColorPaint
        //        {
        //            Color = new SKColor(scb.Color.R, scb.Color.G, scb.Color.B, scb.Color.A)
        //        };
        //    }
        //    catch
        //    {
        //        return new SolidColorPaint
        //        {
        //            Color = new SKColor(0, 0, 0, 0)
        //        };
        //    }
        //}

        //public static Color GetLocationColor(Location location)
        //{
        //    try
        //    {
        //        return (Color)Application.Current?.Resources[$"Color.City.{location}"];
        //    }
        //    catch
        //    {
        //        return (Color)Application.Current?.Resources["Color.City.Default"];
        //    }
        //}
    }

    public enum Location
    {
        Unknown = 0000,
        SwampCross = 0004,
        Thetford = 0007,
        ThetfordPortal = 0301,
        Lymhurst = 1002,
        LymhurstPortal = 1301,
        ForestCross = 1006,
        SteppeCross = 2002,
        Bridgewatch = 2004,
        BridgewatchPortal = 2301,
        HighlandCross = 3002,
        BlackMarket = 3003,
        Caerleon = 3005,
        Martlock = 3008,
        MartlockPortal = 3301,
        FortSterling = 4002,
        FortSterlingPortal = 4301,
        MountainCross = 4006,
        ArthursRest = 4300,
        MerlynsRest = 1012,
        MorganasRest = 0008
    }

    public enum LocationArea
    {
        BlackZone,
        Villages,
        Cities,
        BlackMarket
    }
}