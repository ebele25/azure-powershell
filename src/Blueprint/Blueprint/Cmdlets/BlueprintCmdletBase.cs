﻿using Microsoft.Azure.Commands.Blueprint.Common;
using Microsoft.Azure.Commands.Common.Authentication;
using Microsoft.Azure.Commands.Common.Authentication.Abstractions;
using Microsoft.Azure.Commands.ResourceManager.Common;
using Microsoft.Azure.Management.ManagementGroups;
using Microsoft.Azure.Internal.Subscriptions;
using System;
using Microsoft.Rest;
using Microsoft.Rest.Azure;
using Microsoft.Azure.PowerShell.Cmdlets.Blueprint.Properties;
using Microsoft.Azure.Commands.Blueprint.Models;
using Microsoft.Azure.Management.Blueprint.Models;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Microsoft.Azure.PowerShell.Cmdlets.Blueprint.Common;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Internal.ResourceManager.Version2018_05_01;
using Microsoft.Azure.Graph.RBAC.Version1_6;
using Microsoft.Azure.Graph.RBAC.Version1_6.Models;
using Microsoft.Azure.Management.Authorization.Version2015_07_01;
using Microsoft.Azure.Management.Authorization.Version2015_07_01.Models;

namespace Microsoft.Azure.Commands.Blueprint.Cmdlets
{
    public class BlueprintCmdletBase : AzureRMCmdlet
    {
        #region Properties
        /// <summary>
        /// The blueprint client.
        /// </summary>
        private IBlueprintClient blueprintClient;

        public IBlueprintClient BlueprintClient
        {
            get
            {
                return blueprintClient = blueprintClient ?? new BlueprintClient(DefaultProfile.DefaultContext);
            }
            set => blueprintClient = value;
        }

        /// <summary>
        /// The blueprint client with delegating handler. The delegating handler is needed to get blueprint versions info.
        /// </summary>
        private IBlueprintClient blueprintClientWithVersion;

        public IBlueprintClient BlueprintClientWithVersion
        {
            get
            {
                return blueprintClientWithVersion = blueprintClientWithVersion ?? new BlueprintClient(DefaultProfile.DefaultContext.Environment.GetEndpointAsUri(AzureEnvironment.Endpoint.ResourceManager),
                                                        ClientCredentials,
                                                        new DelegatingHandler[]
                                                        {
                                                            new ApiExpandHandler()
                                                        });
            }
        }

        /// <summary>
        /// Service client credentials client to hold credentials
        /// </summary>
        private ServiceClientCredentials clientCredentials;
        public ServiceClientCredentials ClientCredentials
        {
            get
            {
                return clientCredentials = clientCredentials ?? AzureSession.Instance.AuthenticationFactory.GetServiceClientCredentials(DefaultProfile.DefaultContext,
                                               AzureEnvironment.Endpoint.ResourceManager);

            }

            set => clientCredentials = value;
        }

        /// <summary>
        /// Graph RBAC client
        /// </summary>
        private IGraphRbacManagementClient graphRbacManagementClient;

        public IGraphRbacManagementClient GraphRbacManagementClient
        {
            get
            {

                graphRbacManagementClient = graphRbacManagementClient ?? AzureSession.Instance.ClientFactory.CreateArmClient<GraphRbacManagementClient>(DefaultProfile.DefaultContext, AzureEnvironment.Endpoint.Graph);

                graphRbacManagementClient.TenantID = DefaultProfile.DefaultContext.Tenant.Id.ToString();

                return graphRbacManagementClient;
            }
            set => graphRbacManagementClient = value;
        }

        /// <summary>
        /// Authorization client
        /// </summary>
        private IAuthorizationManagementClient authorizationManagementClient;

        public IAuthorizationManagementClient AuthorizationManagementClient
        {
            get
            {
                return authorizationManagementClient = authorizationManagementClient ?? AzureSession.Instance.ClientFactory.CreateArmClient<AuthorizationManagementClient>(DefaultProfile.DefaultContext, AzureEnvironment.Endpoint.ResourceManager);
 
            }
            set => authorizationManagementClient = value;
        }

        /// <summary>
        /// ARM client
        /// </summary>
        private IResourceManagementClient resourceManagerClient;
        public IResourceManagementClient ResourceManagerClient
        { 
            get
            {
                return resourceManagerClient = resourceManagerClient ?? new ResourceManagementClient(
                                                   DefaultProfile.DefaultContext.Environment.GetEndpointAsUri(AzureEnvironment.Endpoint.ResourceManager),
                                                   ClientCredentials);
            }
            set => resourceManagerClient = value;
        }

        #endregion Properties

        #region Cmdlet Overrides
        protected override void WriteExceptionError(Exception ex)
        {
            var aggEx = ex as AggregateException;

            if (aggEx != null && aggEx.InnerExceptions != null)
            {
                foreach (var e in aggEx.Flatten().InnerExceptions)
                {
                    WriteExceptionError(e);
                }

                return;
            }

            throw ex;
        }
        #endregion Cmdlet Overrides

        #region Protected Methods
        /// <summary>
        /// Check if an assignment with the same name exists
        /// </summary>
        /// <param name="scope"> Subscription scope</param>
        /// <param name="name"> Assignment name</param>
        /// <param name="invertCheck"> Inverts the check logic from check for exists to check for not-exits </param>
        protected void CheckIfAssignmentExist(string scope, string name, bool invertCheck)
        {
            PSBlueprintAssignment existingAssignment = null;

            try
            {
                existingAssignment = BlueprintClient.GetBlueprintAssignment(scope, name);
            }
            catch (Exception ex)
            {
                if (ex is CloudException cex && cex.Response.StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    throw new CloudException(cex.Message);
                }
            }

            if (invertCheck)
            {
                if (existingAssignment == null)
                {
                    throw new Exception(string.Format(Resources.AssignmentNotExist, name, scope));
                }
            }
            else
            {
                if (existingAssignment != null)
                {
                    throw new Exception(string.Format(Resources.AssignmentExists, name, scope));
                }
            }
        }

        /// <summary>
        /// Creates an assignment object to be submitted
        /// </summary>
        /// <param name="identityType"></param>
        /// <param name="bpLocation"></param>
        /// <param name="blueprintId"></param>
        /// <param name="lockMode"></param>
        /// <param name="Parameters"></param>
        /// <param name="ResourceGroups"></param>
        /// <returns></returns>
        protected Assignment CreateAssignmentObject(string identityType, string bpLocation, string blueprintId, PSLockMode? lockMode, Hashtable Parameters, Hashtable ResourceGroups)
        {
            var localAssignment = new Assignment
            {
                Identity = new ManagedServiceIdentity { Type = identityType },
                Location = bpLocation,
                BlueprintId = blueprintId,
                Locks = new AssignmentLockSettings { Mode = lockMode == null ? PSLockMode.None.ToString() : lockMode.ToString() },
                Parameters = new Dictionary<string, ParameterValueBase>(),
                ResourceGroups = new Dictionary<string, ResourceGroupValue>()
            };

            if (Parameters != null)
            {
                foreach (var key in Parameters.Keys)
                {
                    var value = new ParameterValue(Parameters[key], null);   
                    localAssignment.Parameters.Add(key.ToString(), value);
                }   
            }

            if (ResourceGroups != null)
            {
                foreach (var key in ResourceGroups.Keys)
                {
                    var kvp = ResourceGroups[key] as Hashtable;
                    string name = null;
                    string location = null;

                    foreach (var k in kvp.Keys)
                    {
                        var rgKey = k.ToString();

                        if (string.Equals(rgKey, "name", StringComparison.InvariantCultureIgnoreCase))
                        {
                            name = kvp[k].ToString();
                        }

                        if (string.Equals(rgKey, "location", StringComparison.InvariantCultureIgnoreCase))
                        {
                            location = kvp[k].ToString();
                        }
                    }

                    var rgv = new ResourceGroupValue(name, location);
                    localAssignment.ResourceGroups.Add(key.ToString(), rgv);
                }
            }

            return localAssignment;
        }

        /// <summary>
        /// Register Blueprint RP with a subscription.
        /// </summary>
        /// <param name="subscriptionId"> SubscriptionId passed from the cmdlet</param>
        protected void RegisterBlueprintRp(string subscriptionId)
        {
            ResourceManagerClient.SubscriptionId = subscriptionId;
            ResourceManagerClient.Providers.Register(BlueprintConstants.BlueprintProviderNamespace);
        }

        /// <summary>
        /// Get Blueprint SPN for this tenant
        /// </summary>
        /// <returns></returns>
        protected ServicePrincipal GetBlueprintSpn()
        {
            var odataQuery = new Rest.Azure.OData.ODataQuery<ServicePrincipal>(s => s.ServicePrincipalNames.Contains(BlueprintConstants.AzureBlueprintAppId));
            var servicePrincipal = GraphRbacManagementClient.ServicePrincipals.List(odataQuery.ToString())
                .FirstOrDefault();

            return servicePrincipal;
        }

        /// <summary>
        /// Assign owner role to Blueprint RP (so that we can do deployments)
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="servicePrincipal"></param>
        protected void AssignOwnerPermission(string subscriptionId, ServicePrincipal servicePrincipal)
        {
            string scope = string.Format(BlueprintConstants.SubscriptionScope, subscriptionId);

            var filter = new Rest.Azure.OData.ODataQuery<RoleAssignmentFilter>();
            filter.SetFilter(a => a.AssignedTo(servicePrincipal.ObjectId));

            var roleAssignmentList = AuthorizationManagementClient.RoleAssignments.ListForScopeAsync(scope, filter).GetAwaiter().GetResult();

            var roleAssignment = roleAssignmentList?
                .Where(ra => ra.Id.EndsWith(BlueprintConstants.OwnerRoleDefinitionId))
                .FirstOrDefault();

            if (roleAssignment != null) return;

            var roleAssignmentParams = new RoleAssignmentProperties(
                roleDefinitionId: BlueprintConstants.OwnerRoleDefinitionId, principalId: servicePrincipal.ObjectId);

            try
            {
                AuthorizationManagementClient.RoleAssignments.CreateAsync(scope: scope, 
                    roleAssignmentName: Guid.NewGuid().ToString(), 
                    parameters: new RoleAssignmentCreateParameters(roleAssignmentParams))
                    .GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                // ignore if it already exists
                if (ex is CloudException cex && cex.Response.StatusCode != System.Net.HttpStatusCode.Conflict)
                {
                    throw new CloudException(cex.Message);
                }
            }
        }

        /// <summary>
        /// Get management group ancestors for a given subscription
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        protected List<string> GetManagementGroupAncestorsForSubscription(string subscriptionId)
        {
            List<string> managementGroupAncestors = new List<string>();

            if (subscriptionId != null)
            {
                string result = GetManagementGroupAncestorsAsync(subscriptionId).GetAwaiter().GetResult();
                var resultJObjects = JObject.Parse(result);
                var managementGroupAncestorsObjects = resultJObjects["managementGroupAncestors"].Children().ToList();

                foreach (var mgObject in managementGroupAncestorsObjects)
                {
                    managementGroupAncestors.Add(mgObject.ToString());
                }

                if (managementGroupAncestors.Count > 0)
                {
                    // Remove the last item which is the tenantId.
                    int lastIndex = managementGroupAncestors.Count() - 1;
                    managementGroupAncestors.RemoveAt(lastIndex);
                }
            }

            return managementGroupAncestors;
        }

        /// <summary>
        /// Task for get management group ancestors
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        private async Task<string> GetManagementGroupAncestorsAsync(string subscriptionId)
        {
            var url = string.Format(BlueprintConstants.MgAncestorsRequestUrlTemplate, subscriptionId);
            var httpRequest = new HttpRequestMessage();
            httpRequest.Method = new HttpMethod("GET");
            httpRequest.RequestUri = new Uri(url);

            HttpResponseMessage httpResponse = null;
            string responseContent = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    await ClientCredentials.ProcessHttpRequestAsync(httpRequest, new CancellationToken(false)).ConfigureAwait(false);
                    httpResponse = await client.SendAsync(httpRequest, new CancellationToken(false));
                    responseContent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                return responseContent;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        protected string FormatManagementGroupAncestorScope(string mg) => string.Format(BlueprintConstants.ManagementGroupScope, mg);

        #endregion Protected Methods
    }
}
