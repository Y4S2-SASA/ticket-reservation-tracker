﻿namespace TRT.Domain.Constants
{
    public static class AuthorizedRoles
    {
        public const string BackOffice = "Back Office";
        public const string TravelAgent = "Travel Agent";
        public const string Traveler = "Traveler";
        public const string TravelAgentAndTraveler = "Travel Agent,Traveler";
        public const string AllAuthorizedUser = "Back Office,Travel Agent,Traveler";
    }
}
