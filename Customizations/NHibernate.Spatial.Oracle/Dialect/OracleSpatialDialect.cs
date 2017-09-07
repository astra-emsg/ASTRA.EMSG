// Copyright 2008 - Ricardo Stuven (rstuven@gmail.com)
//
// This file is part of NHibernate.Spatial.
// NHibernate.Spatial is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// NHibernate.Spatial is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with NHibernate.Spatial; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA 

using System;
using System.Globalization;
using System.Text;
using NHibernate.Dialect;
using NHibernate.Spatial.Dialect.Function;
using NHibernate.Spatial.Metadata;
using NHibernate.Spatial.Type;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;

namespace NHibernate.Spatial.Dialect
{
    /// <summary>
    /// 
    /// </summary>
    public class OracleSpatialDialect : Oracle10gDialect, ISpatialDialect
    {
        private static readonly IType geometryType = new CustomType(typeof(OracleGeometryType), null);
        private const string DialectPrefix = "SDO_";
        private const string GeometryColumnsViewName = "NHSP_GEOMETRY_COLUMNS";

        /// <summary>
        /// Initializes a new instance of the <see cref="OracleSpatialDialect"/> class.
        /// </summary>
        public OracleSpatialDialect()
        {
            SpatialDialect.LastInstantiated = this;
            RegisterBasicFunctions();
            RegisterFunctions();
        }

        #region Functions registration

        private void RegisterBasicFunctions()
        {
            // Relations
            RegisterSpatialFunction(SpatialRelation.Contains);
            RegisterSpatialFunction(SpatialRelation.CoveredBy);
            RegisterSpatialFunction(SpatialRelation.Covers);
            RegisterSpatialFunction(SpatialRelation.Crosses);
            RegisterSpatialFunction(SpatialRelation.Disjoint);
            RegisterSpatialFunction(SpatialRelation.Equals);
            RegisterSpatialFunction(SpatialRelation.Intersects);
            RegisterSpatialFunction(SpatialRelation.Overlaps);
            RegisterSpatialFunction(SpatialRelation.Touches);
            RegisterSpatialFunction(SpatialRelation.Within);

            // Analysis
            RegisterSpatialFunction(SpatialAnalysis.Buffer);
            RegisterSpatialFunction(SpatialAnalysis.ConvexHull);
            RegisterSpatialFunction(SpatialAnalysis.Difference);
            RegisterSpatialFunction(SpatialAnalysis.Distance);
            RegisterSpatialFunction(SpatialAnalysis.Intersection);
            RegisterSpatialFunction(SpatialAnalysis.SymDifference);
            RegisterSpatialFunction(SpatialAnalysis.Union);

            // Validations
            RegisterSpatialFunction(SpatialValidation.IsClosed);
            RegisterSpatialFunction(SpatialValidation.IsEmpty);
            RegisterSpatialFunction(SpatialValidation.IsRing);
            RegisterSpatialFunction(SpatialValidation.IsSimple);
            RegisterSpatialFunction(SpatialValidation.IsValid);
        }

        private void RegisterFunctions()
        {
            //TODO: what should i do with this is needed?
            //   RegisterConstantValue("TRUE", "1", NHibernateUtil.Boolean);
            //    RegisterConstantValue("FALSE", "0", NHibernateUtil.Boolean);

            RegisterSpatialFunction("Boundary");
            RegisterSpatialFunction("Centroid");
            RegisterSpatialFunction("EndPoint");
            RegisterSpatialFunction("Envelope");
            RegisterSpatialFunction("ExteriorRing");
            RegisterSpatialFunction("GeometryN", 2);
            RegisterSpatialFunction("InteriorRingN", 2);
            RegisterSpatialFunction("PointN", 2);
            RegisterSpatialFunction("PointOnSurface");
            RegisterSpatialFunction("Simplify", "Reduce", 2);
            RegisterSpatialFunction("StartPoint");
            RegisterSpatialFunction("Transform", 2);

            RegisterSpatialFunctionStatic("GeomCollFromText", 2);
            RegisterSpatialFunctionStatic("GeomCollFromWKB", 2);
            RegisterSpatialFunctionStatic("GeomFromText", 2);
            RegisterSpatialFunctionStatic("GeomFromWKB", 2);
            RegisterSpatialFunctionStatic("LineFromText", 2);
            RegisterSpatialFunctionStatic("LineFromWKB", 2);
            RegisterSpatialFunctionStatic("PointFromText", 2);
            RegisterSpatialFunctionStatic("PointFromWKB", 2);
            RegisterSpatialFunctionStatic("PolyFromText", 2);
            RegisterSpatialFunctionStatic("PolyFromWKB", 2);
            RegisterSpatialFunctionStatic("MLineFromText", 2);
            RegisterSpatialFunctionStatic("MLineFromWKB", 2);
            RegisterSpatialFunctionStatic("MPointFromText", 2);
            RegisterSpatialFunctionStatic("MPointFromWKB", 2);
            RegisterSpatialFunctionStatic("MPolyFromText", 2);
            RegisterSpatialFunctionStatic("MPolyFromWKB", 2);

            RegisterSpatialFunction("AsBinary", NHibernateUtil.Binary);

            RegisterSpatialFunction("AsText", NHibernateUtil.String);
            RegisterSpatialFunction("AsGML", NHibernateUtil.String);
            RegisterSpatialFunction("GeometryType", NHibernateUtil.String);

            RegisterSpatialFunction("Area", NHibernateUtil.Double);
            RegisterSpatialFunction("Length", NHibernateUtil.Double);
            RegisterSpatialFunctionProperty("X", NHibernateUtil.Double);
            RegisterSpatialFunctionProperty("Y", NHibernateUtil.Double);

            RegisterSpatialFunctionProperty("SRID", "STSrid", NHibernateUtil.Int32);
            RegisterSpatialFunction("Dimension", NHibernateUtil.Int32);
            RegisterSpatialFunction("NumGeometries", NHibernateUtil.Int32);
            RegisterSpatialFunction("NumInteriorRings", NHibernateUtil.Int32);
            RegisterSpatialFunction("NumPoints", NHibernateUtil.Int32);

            RegisterSpatialFunction("Relate", NHibernateUtil.Boolean, 3);
        }

        //private void RegisterConstantValue(string standardName, string value, IType returnedType)
        //{
        //    RegisterFunction(SpatialDialect.HqlPrefix + standardName, new ConstantValueFunction(value, returnedType));
        //}

        private void RegisterSpatialFunction(string standardName, string dialectName, IType returnedType, int allowedArgsCount)
        {
            RegisterFunction(SpatialDialect.HqlPrefix + standardName, new SpatialMethodSafeFunction(dialectName, returnedType, allowedArgsCount));
        }

        private void RegisterSpatialFunction(string standardName, string dialectName, IType returnedType)
        {
            RegisterSpatialFunction(standardName, dialectName, returnedType, 1);
        }

        private void RegisterSpatialFunction(string name, IType returnedType, int allowedArgsCount)
        {
            RegisterSpatialFunction(name, DialectPrefix + name, returnedType, allowedArgsCount);
        }

        private void RegisterSpatialFunction(string name, IType returnedType)
        {
            RegisterSpatialFunction(name, DialectPrefix + name, returnedType);
        }

        private void RegisterSpatialFunction(string name, int allowedArgsCount)
        {
            RegisterSpatialFunction(name, this.GeometryType, allowedArgsCount);
        }

        private void RegisterSpatialFunctionStatic(string name, int allowedArgsCount)
        {
            string standardName = name;
            string dialectName = DialectPrefix + name;
            IType returnedType = this.GeometryType;
            RegisterFunction(
                SpatialDialect.HqlPrefix + standardName,
                new SpatialStandardSafeFunction("geometry::" + dialectName, returnedType, allowedArgsCount)
            );
        }

        private void RegisterSpatialFunctionProperty(string name, IType returnedType)
        {
            RegisterSpatialFunctionProperty(name, DialectPrefix + name, returnedType);
        }

        private void RegisterSpatialFunctionProperty(string standardName, string dialectName, IType returnedType)
        {
            RegisterFunction(
                SpatialDialect.HqlPrefix + standardName,
                new SpatialPropertyFunction(dialectName, returnedType)
            );
        }

        private void RegisterSpatialFunction(string name)
        {
            RegisterSpatialFunction(name, this.GeometryType);
        }

        private void RegisterSpatialFunction(string standardName, string dialectName, int allowedArgsCount)
        {
            RegisterSpatialFunction(standardName, dialectName, this.GeometryType);
        }

        private void RegisterSpatialFunction(string standardName, string dialectName)
        {
            RegisterSpatialFunction(standardName, dialectName, this.GeometryType);
        }

        private void RegisterSpatialFunction(SpatialRelation relation)
        {
            RegisterFunction(SpatialDialect.HqlPrefix + relation.ToString(), new SpatialRelationFunction(this, relation));
        }

        private void RegisterSpatialFunction(SpatialValidation validation)
        {
            RegisterFunction(SpatialDialect.HqlPrefix + validation.ToString(), new SpatialValidationFunction(this, validation));
        }

        private void RegisterSpatialFunction(SpatialAnalysis analysis)
        {
            RegisterFunction(SpatialDialect.HqlPrefix + analysis.ToString(), new SpatialAnalysisFunction(this, analysis));
        }

        #endregion

        #region ISpatialDialect Members

        /// <summary>
        /// Gets the type of the geometry.
        /// </summary>
        /// <value>The type of the geometry.</value>
        public IType GeometryType
        {
            get { return geometryType; }
        }
    

        /// <summary>
        /// Creates the geometry user type.
        /// </summary>
        /// <returns></returns>
        public IGeometryUserType CreateGeometryUserType()
        {
            return new OracleGeometryType();
        }

        /// <summary>
        /// Gets the spatial transform string.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="srid">The srid.</param>
        /// <returns></returns>
        public SqlString GetSpatialTransformString(object geometry, int srid)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets the spatial aggregate string.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="aggregate">The aggregate.</param>
        /// <returns></returns>
        public SqlString GetSpatialAggregateString(object geometry, SpatialAggregate aggregate)
        {
            string aggregateFunction;
            switch (aggregate)
            {
                case SpatialAggregate.Collect:
                    throw new System.NotImplementedException("Collect");
                    
                case SpatialAggregate.Envelope:
                    aggregateFunction = "SDO_AGGR_MBR";
                    break;
                case SpatialAggregate.Intersection:
                    throw new System.NotImplementedException("Intersection");
                case SpatialAggregate.Union:
                    throw new System.NotImplementedException("Union");
                    
                default:
                    throw new ArgumentException("Invalid spatial aggregate argument");
            }
            return new SqlStringBuilder()
                .Add(aggregateFunction)
                .Add("(")
                .AddObject(geometry)
                .Add(")")
                .ToSqlString();
        }

        /// <summary>
        /// Gets the spatial analysis string.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="analysis">The analysis.</param>
        /// <param name="extraArgument">The extra argument.</param>
        /// <returns></returns>
        public SqlString GetSpatialAnalysisString(object geometry, SpatialAnalysis analysis, object extraArgument)
        {
            bool[] geomArgs;
            SqlStringBuilder sql = new SqlStringBuilder();
            sql.Add("MDSYS.");
            bool isGeomReturn = true;
            switch (analysis)
            {
                case SpatialAnalysis.Buffer:
                    sql.Add("OGC_BUFFER");
                    geomArgs = new bool[] { true, false };
                    break;
                case SpatialAnalysis.ConvexHull:
                    sql.Add("OGC_CONVEXHULL");
                    geomArgs = new bool[] { true};
                    break;
                case SpatialAnalysis.Difference:
                    sql.Add("OGC_DIFFERENCE");
                    geomArgs = new bool[] { true, true };
                    break;
                case SpatialAnalysis.Distance:
                    sql.Add("OGC_DISTANCE");
                    geomArgs = new bool[] { true, true };
                    isGeomReturn = false;
                    break;
                case SpatialAnalysis.Intersection:
                    sql.Add("OGC_INTERSECTION");
                    geomArgs = new bool[] { true, true };
                    break;
                case SpatialAnalysis.SymDifference:
                    sql.Add("OGC_SYMMETRICDIFFERENCE");
                    geomArgs = new bool[] { true, true };
                    break;
                case SpatialAnalysis.Union:
                    sql.Add("OGC_UNION");
                    geomArgs = new bool[] { true, true };
                    break;
                default:
                    throw new ArgumentException(
                            "Unknown SpatialAnalysisFunction ("
                                    + Enum.GetName(typeof(SpatialAnalysis), analysis) + ").");
            }



            sql.Add("(");
            if (geomArgs.Length > 0)
            {
                
                WrapInSTGeometry(geometry, sql);
            }
            if (geomArgs.Length > 1)
            {
                sql.Add(",");
                if (geomArgs[1])
                    WrapInSTGeometry(extraArgument, sql);
                else
                    sql.AddObject(extraArgument);
            }

            sql.Add(")");
            if (isGeomReturn)
                sql.Add(".geom");
            return sql.ToSqlString();
        }

        private void WrapInSTGeometry(object extraArgument, SqlStringBuilder sql)
        {
            sql.Add("MDSYS.ST_GEOMETRY(")
                .AddObject(extraArgument)
           .Add(")");
        }

        /// <summary>
        /// Gets the spatial validation string.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="validation">The validation.</param>
        /// <param name="criterion">if set to <c>true</c> [criterion].</param>
        /// <returns></returns>
        public SqlString GetSpatialValidationString(object geometry, SpatialValidation validation, bool criterion)
        {
            if (IsOGCStrict)
                return GetOGCSpatialValidationString(geometry, validation, criterion);
            else
                return GetNativeSpatialValidationString(geometry, validation, criterion);
        }

        public SqlString GetNativeSpatialValidationString(object geometry, SpatialValidation validation, bool criterion)
        {
            return new SqlStringBuilder()

                .Add(DialectPrefix)
                .Add(validation.ToString())
                .Add("(")
                .AddObject(geometry)
                .Add(")")
                .Add(criterion ? " = 1" : "")
                .ToSqlString();
        }

        public SqlString GetOGCSpatialValidationString(object geometry, SpatialValidation validation, bool criterion)
        {
            return new SqlStringBuilder()

                .Add("MDSYS.OGC_")
                .Add(validation.ToString())
                .Add("(")
                .Add("MDSYS.ST_GEOMETRY.FROM_SDO_GEOM(")
                .AddObject(geometry)
                .Add(")")
                .Add(")")
                .ToSqlString();
        }

        /// <summary>
        /// Gets the spatial relate string.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <param name="anotherGeometry">Another geometry.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="isStringPattern">if set to <c>true</c> [is string pattern].</param>
        /// <param name="criterion">if set to <c>true</c> [criterion].</param>
        /// <returns></returns>
        public SqlString GetSpatialRelateString(object geometry, object anotherGeometry, object pattern, bool isStringPattern, bool criterion)
        {
            SqlStringBuilder builder = new SqlStringBuilder();
            builder
                .AddObject(geometry)
                .Add(".STRelate(")
                .AddObject(anotherGeometry);
            if (pattern != null)
            {
                builder.Add(", ");
                if (isStringPattern)
                {
                    builder
                        .Add("'")
                        .Add((string)pattern)
                        .Add("'");
                }
                else
                {
                    builder.AddObject(pattern);
                }
            }
            return builder
                .Add(")")
                .ToSqlString();



        }

        private bool _isOGCStrict = true;
        public bool IsOGCStrict
        {
            get { return _isOGCStrict; }
            set { _isOGCStrict = value; }
        }

        private SqlString GetNativeSpatialRelateString(object geometry, object anotherGeometry, SpatialRelation realtion)
        {
            SqlStringBuilder sql = new SqlStringBuilder();
            string mask = "";
            bool negate = false;
            switch (realtion)
            {
                case SpatialRelation.Intersects:
                    mask = "ANYINTERACT"; // OGC Compliance verified
                    break;
                case SpatialRelation.Contains:
                    mask = "CONTAINS+COVERS";
                    break;
                case SpatialRelation.Crosses:
                    throw new NotImplementedException(
                            "Oracle Spatial does't have equivalent CROSSES relationship");
                case SpatialRelation.Disjoint:
                    mask = "ANYINTERACT";
                    negate = true;
                    break;
                case SpatialRelation.Equals:
                    mask = "EQUAL";
                    break;
                case SpatialRelation.Overlaps:
                    mask = "OVERLAPBDYDISJOINT+OVERLAPBDYINTERSECT";
                    break;
                case SpatialRelation.Touches:
                    mask = "TOUCH";
                    break;
                case SpatialRelation.Within:
                    mask = "INSIDE+COVEREDBY";
                    break;
                default:
                    throw new ApplicationException(
                            "Undefined SpatialRelation passed :" + realtion);
            }

            if (negate)
            {
                sql.Add("CASE WHEN ");
            }
            sql.Add("SDO_RELATE(")
                    .AddObject(geometry)
                    .Add(",")
                    .AddObject(anotherGeometry)
                    .Add(",'mask=" + mask + "') ");

            if (negate)
            {
                sql.Add(" = 'TRUE' THEN 'FALSE' ELSE 'TRUE' END");
            }
            return sql.ToSqlString();
        }

        public SqlString GetSpatialRelationString(object geometry, SpatialRelation relation, object anotherGeometry, bool criterion)
        {

            SqlStringBuilder sql =
                new SqlStringBuilder();
          //  sql.Add("((");
            SqlString str = IsOGCStrict
                                ? GetOGCSpatialRelateString(geometry, anotherGeometry, relation).Append(" = 'TRUE'")
                                : GetNativeSpatialRelateString(geometry, anotherGeometry, relation).Append(" = 'TRUE'");
            sql.Add(str);
           // sql.Add(") and (").AddObject(geometry).Add(" is not null").Add(")))");
            return sql.ToSqlString();
        }

        private SqlString GetOGCSpatialRelateString(object arg1, object arg2,
                                         SpatialRelation spatialRelation)
        {

            SqlStringBuilder sql = new SqlStringBuilder();
            sql.Add("MDSYS.");
            switch (spatialRelation)
            {
                case SpatialRelation.Intersects:
                    sql.Add("OGC_INTERSECTS");
                    break;
                case SpatialRelation.Contains:
                    sql.Add("OGC_CONTAINS");
                    break;
                case SpatialRelation.Crosses:
                    sql.Add("OGC_CROSS");
                    break;
                case SpatialRelation.Disjoint:
                    sql.Add("OGC_DISJOINT");
                    break;
                case SpatialRelation.Equals:
                    sql.Add("OGC_EQUALS");
                    break;
                case SpatialRelation.Overlaps:
                    sql.Add("OGC_OVERLAP");
                    break;
                case SpatialRelation.Touches:
                    sql.Add("OGC_TOUCH");
                    break;
                case SpatialRelation.Within:
                    sql.Add("OGC_WITHIN");
                    break;
                default:
                    throw new ArgumentException("Usupported SpatialRelation ("
                            + spatialRelation + ").");
            }
            sql.Add("(")
                .Add("MDSYS.ST_GEOMETRY.FROM_SDO_GEOM(")
                    .AddObject(arg1).Add("),").Add(
                    "MDSYS.ST_GEOMETRY.FROM_SDO_GEOM(").AddObject(arg2)
                    .Add(")").Add(")");
            return sql.ToSqlString();

        }

        public SqlString GetSpatialFilterString(string tableAlias, string geometryColumnName, string primaryKeyColumnName, string tableName, Parameter param)
        {
            return new SqlStringBuilder(6)
            	.Add("mdsys.sdo_filter(")
                .Add(tableAlias)
                .Add(".")
                .Add(geometryColumnName)
                .Add(", ")
                .Add(param)
                .Add(") = 'TRUE'")
                .ToSqlString();
        }

        /// <summary>
        /// Gets the spatial create string.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <returns></returns>
        public string GetSpatialCreateString(string schema)
        {
            return "";
        }

        /// <summary>
        /// Quotes the schema.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <returns></returns>
        private string QuoteSchema(string schema)
        {
            if (string.IsNullOrEmpty(schema))
            {
                return null;
            }
            else
            {
                return this.QuoteForSchemaName(schema) + StringHelper.Dot;
            }
        }

        /// <summary>
        /// Gets the spatial create string.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <param name="table">The table.</param>
        /// <param name="column">The column.</param>
        /// <param name="srid">The srid.</param>
        /// <param name="subtype">The subtype.</param>
        /// <returns></returns>
        public string GetSpatialCreateString(string schema, string table, string column, int srid, string subtype)
        {
            StringBuilder builder = new StringBuilder();

            string quotedSchema = this.QuoteSchema(schema);
            builder.Append("EXECUTE IMMEDIATE '");
            builder.AppendFormat("ALTER TABLE {0}{1} DROP COLUMN {2}"
                , quotedSchema
                , table
                , column
                );

            builder.Append("';");
            builder.Append("EXECUTE IMMEDIATE '");
            builder.AppendFormat("ALTER TABLE {0}{1} ADD {2} MDSYS.SDO_GEOMETRY"
                , quotedSchema
                , table
                , column
                , srid
                , subtype
                );
            builder.Append("';");
            return builder.ToString();
        }

        /// <summary>
        /// Gets the spatial drop string.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <returns></returns>
        public string GetSpatialDropString(string schema)
        {
            //TODO: DONT KNOW WHAT TO DO WITH THIS
            return "";
        }

        /// <summary>
        /// Gets the spatial drop string.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <param name="table">The table.</param>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public string GetSpatialDropString(string schema, string table, string column)
        {
            StringBuilder builder = new StringBuilder();

            string quotedSchema = null;
            if (!string.IsNullOrEmpty(schema))
            {
                quotedSchema = this.QuoteForSchemaName(schema) + StringHelper.Dot;
            }

            builder.AppendFormat("ALTER TABLE {0}{1} DROP COLUMN {2}"
                , quotedSchema
                , table
                , column
                );
            builder.Append(this.MultipleQueriesSeparator);
            return builder.ToString();
        }

        /// <summary>
        /// Gets a value indicating whether it supports spatial metadata.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if it supports spatial metadata; otherwise, <c>false</c>.
        /// </value>
        public bool SupportsSpatialMetadata(MetadataClass metadataClass)
        {
            return false;
        }

        public string MultipleQueriesSeparator
        {
            get { return ""; }
        }

        #endregion
    }
}
