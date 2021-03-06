﻿using System;
using System.IO;
using System.Workflow.Activities;
using Composite.C1Console.Actions;
using Composite.C1Console.Elements;
using Composite.C1Console.Workflow;
using Composite.Core.IO;


namespace Composite.Plugins.Elements.ElementProviders.WebsiteFileElementProvider
{
    [AllowPersistingWorkflow(WorkflowPersistingType.Idle)]
    public sealed partial class AddNewWebsiteFolderWorkflow : Composite.C1Console.Workflow.Activities.FormsWorkflow
    {
        public AddNewWebsiteFolderWorkflow()
        {
            InitializeComponent();
        }



        private string GetCurrentPath()
        {
            if (this.EntityToken is WebsiteFileElementProviderRootEntityToken)
            {
                string rootPath = (string)ElementFacade.GetData(new ElementProviderHandle(this.EntityToken.Source), "RootPath");

                return rootPath;
            }
            else if (this.EntityToken is WebsiteFileElementProviderEntityToken)
            {
                return (this.EntityToken as WebsiteFileElementProviderEntityToken).Path;
            }
            else
            {
                throw new NotImplementedException();
            }
        }



        private void FolderExists(object sender, ConditionalEventArgs e)
        {
            string currentPath = GetCurrentPath();
            string newFolderName = this.GetBinding<string>("NewFolderName");

            e.Result = C1Directory.Exists(Path.Combine(currentPath, newFolderName));
        }



        private void initializeAddNewfolderCodeActivity_ExecuteCode(object sender, EventArgs e)
        {
            this.Bindings.Add("NewFolderName", "");
        }



        private void finalizeCodeActivity_ExecuteCode(object sender, EventArgs e)
        {
            string currentPath = GetCurrentPath();
            string newFolderName = this.GetBinding<string>("NewFolderName");

            string newFolderPath = Path.Combine(currentPath, newFolderName);

            C1Directory.CreateDirectory(newFolderPath);

            SpecificTreeRefresher specificTreeRefresher = this.CreateSpecificTreeRefresher();
            specificTreeRefresher.PostRefreshMesseges(this.EntityToken);

            if (this.EntityToken is WebsiteFileElementProviderEntityToken)
            {
                WebsiteFileElementProviderEntityToken folderToken = (WebsiteFileElementProviderEntityToken)this.EntityToken;
                var newFileToken = new WebsiteFileElementProviderEntityToken(folderToken.ProviderName, newFolderPath, folderToken.RootPath);
                SelectElement(newFileToken);
            }

        }



        private void finalizeCodeActivity_ShowError_ExecuteCode(object sender, EventArgs e)
        {
            this.ShowFieldMessage("NewFolderName", "${Composite.Plugins.WebsiteFileElementProvider, AddNewFolder.Error.FolderExist}");
        }
    }
}
