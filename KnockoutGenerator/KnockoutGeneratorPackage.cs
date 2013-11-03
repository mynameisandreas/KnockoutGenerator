using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using AndreasGustafsson.KnockoutGenerator.Model;
using AndreasGustafsson.KnockoutGenerator.Utilities;
using EnvDTE;
using ICSharpCode.NRefactory;
using KnockoutGenerator.Core;
using KnockoutGenerator.Core.Contracts;
using KnockoutGenerator.Core.Extensions;
using KnockoutGenerator.Core.Models;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

namespace AndreasGustafsson.KnockoutGenerator
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideAutoLoad("{f1536ef8-92ec-443c-9ed7-fdadf150da82}")]
    [Guid(GuidList.guidKnockoutGeneratorPkgString)]
    public sealed class KnockoutGeneratorPackage : Package
    {
        private IVsUIShell Shell;
        private IVsUIShell _schell
        {
            get { return Shell ?? (Shell = (IVsUIShell)GetService(typeof(SVsUIShell))); }
        }

        private EnvDTE80.DTE2 DTE;
        private EnvDTE80.DTE2 _dte
        {
            get { return DTE ?? (DTE = GetGlobalService(typeof(DTE)) as EnvDTE80.DTE2); }
        }

        private ICodeGenerator _codeGenerator;

        protected override void Initialize()
        {
            var container = Bootstraper.Start();
            _codeGenerator = container.Resolve<ICodeGenerator>();

            base.Initialize();

            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                var menuCommandId = new CommandID(GuidList.guidKnockoutGeneratorCmdSet, (int)PkgCmdIDList.cmdidMyCommand);
                var menuCodeWindowId = new CommandID(GuidList.guidKnockoutGeneratorCodeWindowCmdSet, (int)PkgCmdIDList.cmdidMyCommand);

                var codeWindowCommand = new OleMenuCommand(MenuItemCallbackCodeWindow, menuCodeWindowId);
                codeWindowCommand.BeforeQueryStatus += codeWindowCommand_BeforeQueryStatus;

                var command = new OleMenuCommand(MenuItemCallback, menuCommandId);
                command.BeforeQueryStatus += CommandBeforeQueryStatus;

                mcs.AddCommand(command);
                mcs.AddCommand(codeWindowCommand);
            }

        }

        static void codeWindowCommand_BeforeQueryStatus(object sender, EventArgs e)
        {
            HideOrShowMenuCommand(sender);
        }

        static void CommandBeforeQueryStatus(object sender, EventArgs e)
        {
            HideOrShowMenuCommand(sender);
        }

        private static void HideOrShowMenuCommand(object sender)
        {
            var menuCommand = sender as OleMenuCommand;
            if (menuCommand == null) return;

            var value = GetFileFromRightClick();

            if (value == null) return;

            menuCommand.Visible = (value != null && (value.ToString().EndsWith(".cs") || value.ToString().EndsWith(".vb")));
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            var selectedFiles = GetSelectedFiles();
            if (!selectedFiles.Any()) return;

            var projItem = selectedFiles.FirstOrDefault();
            OpenOptionsWindow(projItem);
        }

        private void MenuItemCallbackCodeWindow(object sender, EventArgs e)
        {
            var item = _dte.ActiveDocument.ProjectItem;
            bool snippetFromSelection = false;

            try
            {
                dynamic selection = item.Document.Selection;
                snippetFromSelection = !string.IsNullOrEmpty(selection.Text);
            }
            catch (Exception)
            {

            }
            
            OpenOptionsWindow(item, snippetFromSelection);
        }

        private void OpenOptionsWindow(ProjectItem item, bool snippetFromClipboard = false)
        {
            var file = GenerateJsFileFromProjectItem(item, snippetFromClipboard);

            if (file == null) return;

            var window = new OptionsWindow(file);
            window.ShowDialog();
        }

        private IEnumerable<ProjectItem> GetSelectedFiles()
        {
            var projectItems = new List<ProjectItem>();

            var uih = _dte.ToolWindows.SolutionExplorer;
            var selectedItems = (Array)uih.SelectedItems;
            if (null != selectedItems)
            {
                foreach (UIHierarchyItem selItem in selectedItems)
                {
                    var prjItem = selItem.Object as ProjectItem;
                    projectItems.Add(prjItem);
                }
            }

            return projectItems;
        }

        private ViewModel GenerateJsFileFromProjectItem(ProjectItem item, bool fromCodeSnippet)
        {
            var fileExtension = Path.GetExtension(item.GetFullPath());
            var language = fileExtension.Equals(".cs") ? SupportedLanguage.CSharp : SupportedLanguage.VBNet;

            if (!item.Saved)
                item.Save();

            try
            {
                JsFile jsFile;

                if (fromCodeSnippet)
                {
                    dynamic comObject = item.Document.Selection;
                    jsFile = _codeGenerator.GetJsFileFromCodeSnippet(comObject.Text, language);
                }
                else
                {
                    jsFile = _codeGenerator.GetJsFileFromCodeFile(item.GetFullPath(), language);
                }

                return ViewModelExtensions.MapViewModelFromJsFile(jsFile);
            }
            catch (Exception)
            {
                _schell.Toast("Error parsing file");
            }

            return null;
        }

        private static object GetFileFromRightClick()
        {
            IntPtr hierarchyPtr, selectionContainerPtr;
            uint projectItemId;
            IVsMultiItemSelect mis;
            var monitorSelection = (IVsMonitorSelection)Package.GetGlobalService(typeof(SVsShellMonitorSelection));
            monitorSelection.GetCurrentSelection(out hierarchyPtr, out projectItemId, out mis, out selectionContainerPtr);

            var hierarchy = Marshal.GetTypedObjectForIUnknown(hierarchyPtr, typeof(IVsHierarchy)) as IVsHierarchy;
            if (hierarchy == null) return null;

            object value;
            hierarchy.GetProperty(projectItemId, (int)__VSHPROPID.VSHPROPID_Name, out value);

            return value;
        }
    }
}
