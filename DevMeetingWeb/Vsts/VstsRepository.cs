using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Microsoft.VisualStudio.Services.Profile;
using Microsoft.VisualStudio.Services.Profile.Client;
using Microsoft.TeamFoundation.Core.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevMeetingWeb.Vsts
{
    public class VstsRepository
    {
        private readonly string _project;
        private readonly VssConnection _connection;

    public VstsRepository(string vstsAccount, string project, string personalAccessToken)
        {
            if (String.IsNullOrEmpty(vstsAccount) || String.IsNullOrEmpty(project) || String.IsNullOrEmpty(personalAccessToken))
                throw new ArgumentNullException("VstsRepository.ctor(null)");

            var accountUri = new Uri(vstsAccount);
            var connection = new VssConnection(accountUri, new VssBasicCredential(string.Empty, personalAccessToken));

            this._project = project;
            this._connection = connection;
        }

        T GetClient<T>()
            where T : VssHttpClientBase
        {
            return _connection.GetClient<T>();
        }

        public async Task CreateAsync()
        {
            var client = GetClient<WorkItemTrackingHttpClient>();

            var patchDocument = new JsonPatchDocument();

            patchDocument.Add(
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.Title",
                    Value = "jenkins vs VSTS"
                }
            );
            patchDocument.Add(
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.AssignedTo",
                    Value = "fcatae@microsoft.com"
                }
            );


            string workType = "Bug";
            var t = await client.CreateWorkItemAsync(patchDocument, _project, workType);
        }
        
        public async Task<WorkItem> GetItemAsync(int id)
        {
            var client = GetClient<WorkItemTrackingHttpClient>();

            var t = await client.GetWorkItemAsync(id);

            return t;
        }

        public async Task AddItemRelationAsync(int parentId)
        {
            var parent = await GetItemAsync(parentId);
            string parentUrl = ((ReferenceLink)parent.Links.Links["self"]).Href;

            var client = GetClient<WorkItemTrackingHttpClient>();

            var patchDocument = new JsonPatchDocument();

            patchDocument.Add(
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.Title",
                    Value = "Linked task"
                }
            );
            patchDocument.Add(
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/relations/-",
                    Value = new
                    {
                        rel = "System.LinkTypes.Hierarchy-Reverse",
                        url = parentUrl
                    }
                }
            );

            string workType = "Task";
            var t = await client.CreateWorkItemAsync(patchDocument, _project, workType);
        }

        public async Task UpdateItemAsync(int id)
        {
            var client = GetClient<WorkItemTrackingHttpClient>();

            var patchDocument = new JsonPatchDocument();

            patchDocument.Add(
                //new JsonPatchOperation()
                //{
                //    Operation = Operation.Add,
                //    Path = "/fields/System.Title",
                //    Value = "Updated task"
                //}
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.State",
                    Value = "Closed"
                }
            );

            var t = await client.UpdateWorkItemAsync(patchDocument, id);
        }
        
        public async Task GetTagsAsync()
        {
            var projClient = GetClient<ProjectHttpClient>();
            var projId = (await projClient.GetProject(_project)).Id;

            var client = GetClient<TaggingHttpClient>();

            var tags = await client.GetTagsAsync(projId);
        }

        public async Task<VsTaskItem[]> ListChildItemsAsync(int id, IEnumerable<string> extraFields)
        {
            var client = GetClient<WorkItemTrackingHttpClient>();

            // 1. Query all child tasks and return Ids
            var queryChild = new Wiql
            {
                Query = $"SELECT [System.Id] FROM WorkItemLinks" +
                          $"  WHERE ([System.Links.LinkType] = 'System.LinkTypes.Hierarchy-Forward') " +
                          $"  AND ([Source.Id] = {id})" +
                          $"  ORDER BY [System.Id]"
            };

            var results = await client.QueryByWiqlAsync(queryChild, _project);

            var taskIds = results.WorkItemRelations
                                .Where(w => w.Rel != null)
                                .Select(w => w.Target.Id);

            // 2. Retrieve information for all workitems found
            var childItems = await client.GetWorkItemsAsync(taskIds, extraFields);

            // 3. Return the object list
            return childItems.Select(t => new VsTaskItem
            {
                Id = (int)t.Id,
                Fields = t.Fields
            }).ToArray();
        }
    }
}
