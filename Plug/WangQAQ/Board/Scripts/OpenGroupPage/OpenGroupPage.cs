using UdonSharp;
using UnityEngine;
using VRC.Economy;

namespace VRC.Examples.CreatorEconomy
{
    public class OpenGroupPage : UdonSharpBehaviour
    {
        [Tooltip("The group id of the group you want to open. You can find the group id by opening the group on the website and copying the ID in the address bar of your browser." +
                 "For example: grp_a4f791af-a167-4c91-b849-2e37e37f509a")]
        public string groupId;

        [Tooltip("If true, will open the store page for the group instead of the group info page.")]
        public bool openToStorePage;

        public void OpenGroup()
        {
            if (string.IsNullOrEmpty(groupId))
            {
                Debug.LogError("You need to set a group id in the inspector for OpenGroupPage to work.");
                return;
            }

            if (openToStorePage)
            {
                Store.OpenGroupStorePage(groupId);
            }
            else
            {
                Store.OpenGroupPage(groupId);
            }
        }
    }
}