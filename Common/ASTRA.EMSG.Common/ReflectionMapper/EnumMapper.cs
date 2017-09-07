using System;
using System.Collections.Generic;
using System.Linq;
using ASTRA.EMSG.Common.ReflectionMapper.Interfaces;

namespace ASTRA.EMSG.Common.ReflectionMapper
{
    public class EnumMapper<TSource, TDestination> : IMapper
        where TSource : struct
        where TDestination : struct
    {
        private Dictionary<TSource, TDestination> valueMappings = new Dictionary<TSource, TDestination>();

        public EnumMapper()
        {
            Initialize();
        }

        object IMapper.Translate(object source, IMappingEngine engine)
        {
            return Translate((TSource)source);
        }

        private void Initialize()
        {
            var destinationValues = Enum.GetValues(DestinationType);

            foreach (TSource sourceValue in Enum.GetValues(SourceType))
            {
                var sourceName = Enum.GetName(SourceType, sourceValue);

                var destinationValueCandidates =
                    ((IEnumerable<TDestination>)destinationValues).Where(
                        value => Enum.GetName(DestinationType, value) == sourceName);

                if (destinationValueCandidates.Count() > 0)
                    valueMappings.Add(sourceValue, destinationValueCandidates.Single());
            }
        }

        public TDestination Translate(TSource source)
        {
            try
            {
                return valueMappings[source];
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("{0} => {1} ({2})", typeof(TSource).FullName, typeof(TDestination).FullName, source), e);
            }
        }

        public Type SourceType
        {
            get { return typeof(TSource); }
        }

        public Type DestinationType
        {
            get { return typeof(TDestination); }
        }

        public void Translate(object source, object destination, IMappingEngine engine)
        {
            throw new NotImplementedException();
        }

        public void Map(TSource source, TDestination destination)
        {
            valueMappings[source] = destination;
        }
    }
}