using System;
using System.Linq;
using System.Threading.Tasks;

using CMS.Membership;

using Kentico.Xperience.Admin.Base;
using Kentico.Xperience.Admin.Base.UIPages;

using XperienceCommunity.RelationshipsExtended.Web.Admin;

/* 
    This file demonstrates the use of page extenders. 

    UserListExtender extends the existing user listing page which is based on the listing template.
    The extender modifies which columns are displayed in the listing and adds new actions to the grid. 
 */
/*
[assembly: PageExtender(typeof(UserListExtender))]

namespace XperienceCommunity.RelationshipsExtended.Web.Admin
{
    internal class UserListExtender : PageExtender<UserList>
    {
        private readonly IUserInfoProvider provider;


        public UserListExtender(IUserInfoProvider provider)
        {
            this.provider = provider;
        }


        // Configures the listing template on which UserList is based.
        public override Task ConfigurePage()
        {
            var configuration = Page.PageConfiguration;

            // Removed the default full name column and replaces it with a custom one
            var fullNameColumn = configuration.ColumnConfigurations
                .FirstOrDefault(c => c.Name == "LastName");

            configuration.ColumnConfigurations
                .Remove(fullNameColumn);

            configuration.ColumnConfigurations
                .AddColumn("LastName", "Custom last name");

            // Adds a new action to each item in the listing
            configuration.TableActions
                .AddCommand("Reverse last name", nameof(ReverseName), Icons.ArrowsCrooked);

            // Adds a new action for the entire listing
            configuration.HeaderActions
                .AddLink<DashboardApplication>("Dashboard", Icons.LGrid22)
                .AddCommand("Reverse last name", nameof(ReverseNameBulk), Icons.ArrowsCrooked);

            return base.ConfigurePage();
        }


        [PageCommand]
        public async Task<ICommandResponse<RowActionResult>> ReverseName(int id)
        {
            var user = await provider.GetAsync(id);
            ReverseName(user);

            return ResponseFrom(new RowActionResult(reload: true))
                .AddSuccessMessage($"Reversed the name of {user.UserName}");
        }


        [PageCommand]
        public async Task<ICommandResponse> ReverseNameBulk()
        {
            var users = await provider.Get().GetEnumerableTypedResultAsync();
            foreach (var user in users)
            {
                ReverseName(user);
            }

            return Response().UseCommand("LoadData")
                .AddSuccessMessage("Reversed the names of all users");
        }


        private static void ReverseName(UserInfo user)
        {
            var lastNameChars = user?.LastName?.ToCharArray();

            if (lastNameChars is not null)
            {
                Array.Reverse(lastNameChars);
                user.LastName = new String(lastNameChars);

                user.Update();
            }
        }
    }
}
*/