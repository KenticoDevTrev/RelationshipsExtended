using CMS.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationshipsExtended
{
    public static class Functions
    {
        /// <summary>
        /// Returns formatted username in format: username. 
        /// Allows you to customize how the usernames will look like throughout the admin UI. 
        /// </summary>
        /// <param name="username">Source user name</param>   
        /// <param name="isLiveSite">Indicates if returned username should be displayed on live site</param>
        public static string GetFormattedUserName(string username, bool isLiveSite)
        {
            return GetFormattedUserName(username, null, null, isLiveSite);
        }


        /// <summary>
        /// Returns formatted username in format: fullname (username). 
        /// Allows you to customize how the usernames will look like throughout the admin UI. 
        /// </summary>
        /// <param name="username">Source user name</param>
        /// <param name="fullname">Source full name</param>
        /// <param name="isLiveSite">Indicates if returned username should be displayed on live site</param>
        public static string GetFormattedUserName(string username, string fullname = null, bool isLiveSite = false)
        {
            return GetFormattedUserName(username, fullname, null, isLiveSite);
        }


        /// <summary>
        /// Returns formatted username in format: fullname (nickname) if nicname specified otherwise fullname (username). 
        /// Allows you to customize how the usernames will look like throughout the friends and messaging modules UI. 
        /// </summary>
        /// <param name="username">Source user name</param>
        /// <param name="fullname">Source full name</param>
        /// <param name="nickname">Source nick name</param>
        /// <param name="isLiveSite">Indicates if returned username should be displayed on live site</param>
        public static string GetFormattedUserName(string username, string fullname, string nickname, bool isLiveSite = false)
        {
            return UserInfoProvider.GetFormattedUserName(username, fullname, nickname, isLiveSite);
        }
    }
}
