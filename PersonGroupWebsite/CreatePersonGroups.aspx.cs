using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PersonGroupWebsite
{
    public partial class CreatePersonGroups : System.Web.UI.Page
    {
        private readonly IFaceServiceClient faceServiceClient = new FaceServiceClient("00d7358854144900955ef88f7f0b190b", "https://westus.api.cognitive.microsoft.com/face/v1.0");

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                await PopulateDropdownList();
        }

        protected async Task CreateGroup()
        {
            try
            {
                await faceServiceClient.CreatePersonGroupAsync(newPersonGroupID.Text, newPersonGroupName.Text);
                txtMessage.Text = "Person Group '" + newPersonGroupID.Text + "' was successfully created!";
            }
            catch (FaceAPIException f)
            {
                txtMessage.Text = "Response status: " + f.ErrorMessage;
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message;
            }
        }

        protected async void btnCreateGroup_Click(object sender, EventArgs e)
        {
           await CreateGroup();
           ddlAllGroups.Items.Clear();
           await PopulateDropdownList();
        }

        private async Task PopulateDropdownList()
        {
            string[] groups;

            PersonGroup[] personGroups = await faceServiceClient.ListPersonGroupsAsync();

            groups = personGroups.Select(groupId => groupId.PersonGroupId).ToArray();

            //ddlAllGroups.DataSource = groups;
            for (int i=0; i< groups.Length; i++)
            {
                ddlAllGroups.Items.Insert(i, new ListItem(groups[i], i.ToString()));
            }
        }

        protected async Task DeleteGroup()
        {
                PersonGroup[] x = await faceServiceClient.ListPersonGroupsAsync();
                bool checkIfExists = false;

                string groupID = ddlAllGroups.SelectedItem.Text;
                for (int i = 0; i < x.Length; i++)
                {
                    if (x[i].PersonGroupId == groupID)
                    {
                        checkIfExists = true;
                    }
                }

                if (checkIfExists == true)
                {
                    await faceServiceClient.DeletePersonGroupAsync(groupID);
                    txtMessage2.Text = "Person Group with ID '" + groupID + "' was successfully deleted!";
                }
                else
                {
                    txtMessage2.Text = "Error couldn't delete the group!";
                }
        }

        protected async void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            await DeleteGroup();
            ddlAllGroups.Items.Clear();
            await PopulateDropdownList();
        }
    }
}