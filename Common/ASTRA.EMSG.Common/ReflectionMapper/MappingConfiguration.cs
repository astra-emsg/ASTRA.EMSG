using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Common.ReflectionMapper.Interfaces;

namespace ASTRA.EMSG.Common.ReflectionMapper
{
    public abstract class MappingConfiguration : IMappingConfiguration
    {
        private readonly Dictionary<Type, List<IMapper>> _translatorsBySourceType = new Dictionary<Type, List<IMapper>>();

        public IMapper GetTranslator(Type sourceType, Type destinationType)
        {
            List<IMapper> translatorsForSourceType;
            if (!_translatorsBySourceType.TryGetValue(sourceType, out translatorsForSourceType))
                return null;

            return translatorsForSourceType.SingleOrDefault(
                translator => destinationType.IsAssignableFrom(translator.DestinationType));

        }

        protected void Register(IMapper mapper)
        {
            List<IMapper> translatorsForSourceType;
            if (!_translatorsBySourceType.TryGetValue(mapper.SourceType, out translatorsForSourceType))
            {
                translatorsForSourceType = new List<IMapper>();
                _translatorsBySourceType.Add(mapper.SourceType, translatorsForSourceType);
            }

            if (translatorsForSourceType.Exists(
                registeredTranslator =>
                registeredTranslator.DestinationType.IsAssignableFrom(mapper.DestinationType) ||
                mapper.DestinationType.IsAssignableFrom(registeredTranslator.DestinationType)))
            {
                throw new ArgumentException(
                    "A mapper with the same, a derived or base destination type has already been registered.");
            }

            translatorsForSourceType.Add(mapper);
        }

        protected void Register(Func<IMapper> mapper)
        {
            Register(mapper.Invoke);
        }
    }
}