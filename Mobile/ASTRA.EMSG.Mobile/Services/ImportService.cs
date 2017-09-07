using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using ASTRA.EMSG.Localization;
using ASTRA.EMSG.Common.Mobile.Enums;
using ASTRA.EMSG.Common.Mobile;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Map.Services;
using ASTRA.EMSG.Common.Mobile.Utils;
using ASTRA.EMSG.Common;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using ASTRA.EMSG.Common.Mobile.Logging;
using ASTRA.EMSG.Mobile.BusinessExceptions;
using System.Windows;
using ASTRA.EMSG.Mobile.Views.Windows;
using System.Windows.Input;

namespace ASTRA.EMSG.Mobile.Services
{
    public interface IImportService
    {
        void Import();
        
    }
    public class ImportService : IImportService
    {
        private readonly IFileDialogService fileDialogService;
        private readonly IPackageService packageService;
       


        public ImportService(IFileDialogService fileDialogService, 
            IPackageService packageService)
        {
            Console.WriteLine(Environment.CurrentDirectory);
            this.fileDialogService = fileDialogService;
            this.packageService = packageService;
        }

        public void Import()
        {
           fileDialogService.ShowImportDialog(packageService.Import);
        }
    }
}
