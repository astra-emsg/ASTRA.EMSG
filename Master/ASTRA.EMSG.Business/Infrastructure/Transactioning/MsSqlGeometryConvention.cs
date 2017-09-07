using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using GeoAPI.Geometries;
using NHibernate.Spatial.Type;

namespace ASTRA.EMSG.Business.Infrastructure.Transactioning
{
    public class MsSqlGeometryConvention : IUserTypeConvention
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
        {
            criteria.Expect(x => typeof(IGeometry).IsAssignableFrom(x.Property.PropertyType));
        }

        public void Apply(IPropertyInstance target)
        {
            target.CustomType(typeof(MsSql2008GeometryType));
            target.CustomSqlType("GEOMETRY");
        }
    }
}