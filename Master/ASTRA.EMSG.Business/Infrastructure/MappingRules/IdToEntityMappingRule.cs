using System;
using System.Reflection;
using ASTRA.EMSG.Business.Entities.Common;
using ASTRA.EMSG.Business.Infrastructure.Transactioning;
using ASTRA.EMSG.Common;
using ASTRA.EMSG.Common.ReflectionMapper;

namespace ASTRA.EMSG.Business.Infrastructure.MappingRules
{
    public interface IIdToEntityMappingRule : IMappingRule
    {
    }

    public class IdToEntityMappingRule : IIdToEntityMappingRule
    {
        private readonly ITransactionScopeProvider transactionScopeProvider;

        public IdToEntityMappingRule(ITransactionScopeProvider transactionScopeProvider)
        {
            this.transactionScopeProvider = transactionScopeProvider;
        }

        public bool CanApply(PropertyInfo sourceProperty, PropertyInfo destinationProperty)
        {
            return typeof(IEntity).IsAssignableFrom(destinationProperty.PropertyType);
        }

        public void Apply<TSource, TDestination>(PropertyInfo sourceProperty, PropertyInfo destinationProperty, RuleDrivenReflectingMapper<TSource, TDestination> ruleDrivenReflectingMapper)
        {
            ruleDrivenReflectingMapper.SetValueFrom(destinationProperty, source => EntityToRefDtoTransformation(source, sourceProperty, destinationProperty));
        }

        private object EntityToRefDtoTransformation(object source, PropertyInfo sourceProperty, PropertyInfo destinationProperty)
        {
            var sourceEntityPropertyValue = sourceProperty.GetValue(source, new object[0]);
            if (sourceEntityPropertyValue == null)
                return null;

            var id = (Guid)sourceEntityPropertyValue;
            var result = transactionScopeProvider.CurrentTransactionScope.Session.Get(destinationProperty.PropertyType, id);
            if (result == null)
            {
                var modelInfo = "";
                var model = source as Model;
                if (model != null)
                {
                    modelInfo = string.Format("Source type: {0} id: {1}.", model.GetType().Name, model.Id);
                }
                throw new EntityNotFoundException(string.Format("Entity {0} with id: {1} was not found. When setting from {2}",
                    destinationProperty.PropertyType.Name, id, modelInfo));
            }
            return result;
        }
    }

    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message)
            : base(message)
        {
        }
    }
}