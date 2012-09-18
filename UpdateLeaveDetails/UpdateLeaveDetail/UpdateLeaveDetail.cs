using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.Data;

namespace UpdateLeaveDetails.UpdateLeaveDetail
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class UpdateLeaveDetail : SPItemEventReceiver
    {
        /// <summary>
        /// An item was updated.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            base.ItemUpdated(properties);
           
                SPListItem item = properties.ListItem;
                var createdByLeaveRequest = new SPFieldUserValue(properties.Web,item["Created By"].ToString());
                
                if (item["Status"].ToString() == "Approved")

                {
                    SPSite myDestinationSite = new SPSite(properties.WebUrl);
                    SPWeb myDestinationWeb = myDestinationSite.OpenWeb();
                    SPList myDestinationList = myDestinationWeb.Lists["LeaveDays"];
                    SPListItem myDestinationListItem = myDestinationList.Items[0];                
                    
                    var EmpNameLeavedays = new SPFieldUserValue(properties.Web, myDestinationListItem["Employee Name"].ToString());
                    myDestinationListItem["PaidLeaveBalance"] = item["Dup Paid Leave Balance"];//SourceEmpId;
                    myDestinationListItem["PaidLeaveUtilized"] = item["Dup Paidleaveutilize"];// SourceEmpName;
                    myDestinationWeb.AllowUnsafeUpdates = true;
                    if(createdByLeaveRequest.User.ID  == EmpNameLeavedays.User.ID)
                   {
                    myDestinationListItem.Update();
                    myDestinationWeb.AllowUnsafeUpdates = false;
                   }


                }

            
        }


    }
}
