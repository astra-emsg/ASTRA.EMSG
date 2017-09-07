using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using ASTRA.EMSG.Common.DataTransferObjects;

namespace ASTRA.EMSG.Mobile.Services
{
    public interface IDTOService
    {
        DataTransferObject GetDTOByID(Guid id);
        TDTO GetDTOByID<TDTO>(Guid id)
            where TDTO : class, IDataTransferObject;
        IEnumerable<TDTO> Get<TDTO>()
            where TDTO : class, IDataTransferObject;
        IEnumerable<DataTransferObject> Get();
        void CreateOrReplaceDTO(DataTransferObject datatransferobject);
        void LoadFile();
        void DeleteDTO(Guid id);
        void DeleteDTO(DataTransferObject datatransferobject);
        void saveFile(DTOContainer container, string path);
        void DeleteZustandsabschnitt(ZustandsabschnittGISDTO dto);
        IEnumerable<TDTO> GetAll<TDTO>()
        where TDTO : class, IDataTransferObject;
        void Clear();
    }
    public class DTOService : IDTOService
    {
        private readonly IClientConfigurationProvider clientConfigurationProvider;
        private readonly IMessageBoxService messageBoxService;
        private DTOContainer dtoContainer;

        public void Clear()
        {
            if (dtoContainer != null)
            {
                dtoContainer.DataTransferObjects.Clear();
            }
        }
        public DTOService(IClientConfigurationProvider clientConfigurationProvider,
            IMessageBoxService messageBoxService)
        {
            this.clientConfigurationProvider = clientConfigurationProvider;
            this.messageBoxService = messageBoxService;
        }

        public DataTransferObject GetDTOByID(Guid id)
        {
            return dtoContainer.DataTransferObjects.Where(d => !d.IsDeleted).Single(dto => dto.Id == id);
        }

        public TDTO GetDTOByID<TDTO>(Guid id)
            where TDTO : class, IDataTransferObject
        {
            return dtoContainer.DataTransferObjects.OfType<TDTO>().Where(d => !d.IsDeleted).Where(dto => dto.Id == id).SingleOrDefault() as TDTO;
        }

        public IEnumerable<TDTO> Get<TDTO>()
            where TDTO : class, IDataTransferObject
        {
            return (dtoContainer.DataTransferObjects.OfType<TDTO>().Where(d => !d.IsDeleted).Select(dto => dto as TDTO)).ToList();

        }

        public IEnumerable<TDTO> GetAll<TDTO>()
           where TDTO : class, IDataTransferObject
        {
            return (dtoContainer.DataTransferObjects.OfType<TDTO>().Select(dto => dto as TDTO)).ToList();
        }

        public IEnumerable<DataTransferObject> Get()
        {
            return dtoContainer.DataTransferObjects.Where(d => !d.IsDeleted).ToList();
        }

        public void CreateOrReplaceDTO(DataTransferObject datatransferobject)
        {
            if (datatransferobject.Id!= null && datatransferobject.Id != Guid.Empty)
            {
                datatransferobject.IsEdited = true;
                var dtos = dtoContainer.DataTransferObjects.Where(dto => dto.Id == datatransferobject.Id);
                if (dtos.Count() > 0)
                {
                    removeDTO(datatransferobject.Id);
                }
            }
            else
            {
                datatransferobject.Id = Guid.NewGuid();
                datatransferobject.IsAdded = true;
            }
            dtoContainer.DataTransferObjects.Add(datatransferobject);
            saveFile();
        }
        private void removeDTO(Guid id)
        {
            dtoContainer.DataTransferObjects.Remove(dtoContainer.DataTransferObjects.SingleOrDefault(dto => dto.Id == id));
        }
        public void DeleteDTO(Guid id)
        {
            dtoContainer.DataTransferObjects.SingleOrDefault(dto => dto.Id == id).IsDeleted = true;
            saveFile();
        }
        public void DeleteDTO(DataTransferObject datatransferobject)
        {
            DeleteDTO(datatransferobject.Id);
        }

        public void LoadFile()
        {
            Stream fs = new FileStream(getFilePath(), FileMode.Open);
            IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            dtoContainer = (DTOContainer)formatter.Deserialize(fs);
            fs.Close();
        }

        private void saveFile()
        {
            saveFile(getFilePath());
        }

        private void saveFile(string path)
        {
            saveFile(dtoContainer, path);
        }
        public void saveFile(DTOContainer container, string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            Stream fs = new FileStream(path, FileMode.CreateNew);
            IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(fs, container);
            fs.Seek(0, 0);
            fs.Flush();
            fs.Close();
        }

        private string getFilePath()
        {

            return Directory.GetFiles(clientConfigurationProvider.CurrentTemporaryFolder, "*.model",
                                          SearchOption.AllDirectories).Single();

        }

        public void DeleteZustandsabschnitt(ZustandsabschnittGISDTO dto)
        {
            var schadengruppeDtos = Get<SchadengruppeDTO>().Where(sg => sg.ZustandsabschnittId == dto.Id).ToList();
            var schadendetailDtos = Get<SchadendetailDTO>().Where(sg => sg.ZustandsabschnittId == dto.Id).ToList();
            foreach (var schadengruppeDto in schadengruppeDtos)
            {
                DeleteDTO(schadengruppeDto);
            }
            foreach (var schadendetail in schadendetailDtos)
            {
                DeleteDTO(schadendetail);
            }
            var strab = this.GetDTOByID<StrassenabschnittGISDTO>(dto.StrassenabschnittGIS);
            strab.ZustandsabschnittenId.Remove(strab.ZustandsabschnittenId.SingleOrDefault(z=> z.Equals(dto.Id)));
            //set the length of the zustandsabschnitt dto to 0
            //to prevent certain validation errors

            dto.Laenge = 0;
            DeleteDTO(dto);
        }
    }
}

