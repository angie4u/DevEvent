﻿using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace DevEvent.Mobile.Controllers
{
    [Authorize]
    public class UpdateTagsController : ApiController
    {
        private NotificationHubClient hubClient;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

            // Get the Mobile App settings.
            MobileAppSettingsDictionary settings = this.Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();

            // Get the Notification Hubs credentials for the Mobile App.
            string notificationHubName = settings.NotificationHubName;
            string notificationHubConnection = settings
                .Connections[MobileAppSettingsKeys.NotificationHubConnectionString]
                .ConnectionString;

            // Create the notification hub client.
            hubClient = NotificationHubClient.CreateClientFromConnectionString(notificationHubConnection, notificationHubName);
        }

        // GET api/UpdateTags/Id
        [HttpGet]
        public async Task<List<string>> GetTagsByInstallationId(string Id)
        {
            try
            {
                // Return the installation for the specific ID.
                var installation = await hubClient.GetInstallationAsync(Id);
                return installation.Tags as List<string>;
            }
            catch (MessagingException ex)
            {
                throw ex;
            }
        }

        // POST api/UpdateTags/Id
        
        [HttpPost]
        public async Task<HttpResponseMessage> AddTagsToInstallation(string Id)
        {
            // Get the tags to update from the body of the request.
            var message = await this.Request.Content.ReadAsStringAsync();

            // Validate the submitted tags.
            if (string.IsNullOrEmpty(message) || message.Contains("sid:"))
            {
                // We can't trust users to submit their own user IDs.
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            // Verify that the tags are a valid JSON array.
            var tags = JArray.Parse(message);

            // Define a collection of PartialUpdateOperations. Note that 
            // only one '/tags' path is permitted in a given collection.
            var updates = new List<PartialUpdateOperation>();

            // Add a update operation for the tag.
            updates.Add(new PartialUpdateOperation
            {
                Operation = UpdateOperationType.Add,
                Path = "/tags",
                Value = tags.ToString()
            });

            try
            {
                // Add the requested tag to the installation.
                await hubClient.PatchInstallationAsync(Id, updates);

                // Return success status.
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (MessagingException)
            {
                // When an error occurs, return a failure status.
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
