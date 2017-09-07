using ASTRA.EMSG.Business.Models;
using System.Collections.Generic;
using System;
using ASTRA.EMSG.Business.Models.GIS;
using ASTRA.EMSG.Common.DataTransferObjects;
using ASTRA.EMSG.Common;

namespace ASTRA.EMSG.Business.Services.PackageService
{
    public class ImportResult
    {
        public ImportResult()
        {
            Errors = new List<string>();
            InspektionsRouten = new List<Guid>();
        }

        public List<string> Errors { get; set; }
        public List<Guid> InspektionsRouten { get; set; }
        public DTOContainer dtocontainer { get; set; }
        public ClientPackageDescriptor descriptor { get; set; }
    }
}