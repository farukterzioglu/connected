using System;
using System.Collections.Generic;
using System.Linq;
using Connected.DAL.Core;
using Connected.DAL.Core.Configuration.Model;

namespace Connected.DAL.Configuration.Repositories.Fake
{
    public class GenericConfigurationRepositoryFake<T> : GenericRepositoryBase<T> where T : EntityBase, new()
    {
        private static readonly List<AdapterBasicDTO> Adapters;
        private static readonly List<AdapterTypeDIMDTO> AdapterTypes;

        #region Fake DbContext
        private List<AdapterBasicDTO> GetAllAdapters(int? adapterType = null)
        {
            return adapterType == null ?
                Adapters :
                Adapters.Where(x => x.AdapterTypeId == adapterType).ToList();
        }
        private AdapterBasicDTO InsertAdapter(AdapterBasicDTO adapterBasicDTO)
        {
            adapterBasicDTO.Id = Adapters.Count;

            Adapters.Add(adapterBasicDTO);
            return adapterBasicDTO;
        }
        private AdapterBasicDTO UpdateAdapter(AdapterBasicDTO adapterBasicDTO)
        {
            var adapter = Adapters.FirstOrDefault(x => x.Id == adapterBasicDTO.Id);
            if (adapter == null) return null;

            Adapters.Remove(adapter);
            Adapters.Add(adapterBasicDTO);

            return adapterBasicDTO;
        }
        private List<AdapterTypeDIMDTO> GetAdapterTypes(int? typeId = null)
        {
            if (typeId != null)
                return AdapterTypes.Where(x => x.Id == typeId).ToList();
            else
                return AdapterTypes;
        }
        private AdapterTypeDIMDTO InsertAdapterType(AdapterTypeDIMDTO adapterTypeDIMDTO)
        {
            adapterTypeDIMDTO.Id = AdapterTypes.Count;

            AdapterTypes.Add(adapterTypeDIMDTO);
            return adapterTypeDIMDTO;
        }
        private AdapterTypeDIMDTO UpdateAdapterType(AdapterTypeDIMDTO adapterTypeDIMDTO)
        {
            var adapterType = AdapterTypes.FirstOrDefault(x => x.Id == adapterTypeDIMDTO.Id);
            if (adapterType == null) return null;

            AdapterTypes.Remove(adapterType);

            adapterTypeDIMDTO.CreationDate = adapterType.CreationDate;
            adapterTypeDIMDTO.ModifiedDate = adapterTypeDIMDTO.ModifiedDate;

            AdapterTypes.Add(adapterTypeDIMDTO);

            return adapterTypeDIMDTO;
        }
        private List<MessageTypeDTO> GetAllMessageTypes()
        {
            return new List<MessageTypeDTO>()
            {
                new MessageTypeDTO(){ MessageType1 = "Fake Message Type"}
            };
        }
        #endregion
        static GenericConfigurationRepositoryFake()
        {
            AdapterTypes = new List<AdapterTypeDIMDTO>()
            {
                new AdapterTypeDIMDTO()
                {
                    Id = 1,
                    AdapterType = "Send Adapter",
                    CreationDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                },
                new AdapterTypeDIMDTO()
                {
                    Id = 2,
                    AdapterType = "Receive Adapter",
                    CreationDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                }
            };

            Adapters = new List<AdapterBasicDTO>()
            {
                new AdapterBasicDTO()
                {
                    Id = 1,
                    AdapterName = "Mocked Send Adapter",
                    // ReSharper disable once PossibleNullReferenceException
                    AdapterTypeId = AdapterTypes.FirstOrDefault(x => x.AdapterType == "Send Adapter").Id,
                    RegistrationDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                },
                new AdapterBasicDTO()
                {
                    Id = 2,
                    AdapterName = "Mocked Receive Adapter",
                    // ReSharper disable once PossibleNullReferenceException
                    AdapterTypeId = AdapterTypes.FirstOrDefault(x => x.AdapterType == "Receive Adapter").Id,
                    RegistrationDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                }
            };
        }

        public GenericConfigurationRepositoryFake(UnitOfWorkContext unitOfWorkContext) : base(unitOfWorkContext)
        {
        }

        public override T OnInsert(T obj)
        {
            Type type = typeof(T);

            if (type == typeof(AdapterBasicDTO))
            {
                var adapter = InsertAdapter(obj as AdapterBasicDTO);
                return adapter as T;
            }

            if (type == typeof(AdapterTypeDIMDTO))
            {
                var typeDB = InsertAdapterType(obj as AdapterTypeDIMDTO);
                return typeDB as T;
            }

            //TODO : Add other DTOs
            return null;
        }

        public override void OnUpdate(T obj)
        {
            Type type = typeof(T);

            if (type == typeof(AdapterBasicDTO))
            {
                UpdateAdapter(obj as AdapterBasicDTO);
            }

            if (type == typeof(AdapterTypeDIMDTO))
            {
                UpdateAdapterType(obj as AdapterTypeDIMDTO);
            }

            //TODO : Add other DTOs
        }

        public override void OnDelete(T obj)
        {
            throw new System.NotImplementedException();
        }

        public override T OnGetById(int id)
        {
            Type type = typeof (T);

            if (type == typeof (AdapterBasicDTO))
            {
                var adapter = GetAllAdapters().FirstOrDefault(x => x.Id == id) as T;
                return adapter;
            }
            else if (type == typeof (AdapterTypeDIMDTO))
            {
                var typeDIM = GetAdapterTypes().FirstOrDefault( x=> x.Id == id) as T;
                return typeDIM;
            }
            else
                return null;
        }

        public override IEnumerable<T> OnGetAll()
        {
            Type type = typeof (T);
         
            if (type == typeof (AdapterBasicDTO))
            {
                var adapters = GetAllAdapters();
                return adapters as IEnumerable<T>;
            }
            
            if (type == typeof (AdapterTypeDIMDTO))
            {
                var types = GetAdapterTypes();
                return types as IEnumerable<T>;
            }
            
            //TODO : Add other DTOs
            return null;
        }
    }
}