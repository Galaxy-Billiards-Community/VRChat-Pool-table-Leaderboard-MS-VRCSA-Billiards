using UdonSharp;
using UnityEngine;
using VRC.Economy;

namespace VRC.Examples.CreatorEconomy
{
    public class OpenListing : UdonSharpBehaviour
    {
        public string listingId;

        public void OpenListingPage()
        {
            if (!string.IsNullOrEmpty(listingId))
            {
                Store.OpenGroupListing(listingId);
            }
            else
            {
                Debug.LogError("You need to set a listing id in the inspector for OpenListing to work.");
            }
        }
    }
}