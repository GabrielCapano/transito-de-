using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Model.Models._Address;
using Model.Models._Alert;
using Model.Models._CEP;
using Model.Models._CET;
using Model.Models._Document;
using Model.Models._Email;
using Model.Models._Language;
using Model.Models._Log;
using Model.Models._Master;
using Model.Models._User;

namespace Model
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
            : base("Name=GlobalConnection")
        {
             
        }

        #region CET

        public DbSet<Ocorrencia> Ocorrencias { get; set; }
        public DbSet<CodigoOcorrencia> CodigoOcorrencias { get; set; }
        public DbSet<TipoOcorrencia> TipoOcorrencias { get; set; }
        public DbSet<Lentidao> Lentidoes { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<LentidaoConsolidado> LentidaoConsolidados { get; set; }
        public DbSet<Status> Status { get; set; }

        #endregion

        #region alert
        public DbSet<Alert> Alerts { get; set; }
        #endregion

        #region CEP
        public DbSet<Logradouro> Logradouros { get; set; }
        public DbSet<LogradouroBairro> LogradouroBairros { get; set; }
        public DbSet<LogradouroEstado> LogradouroEstados { get; set; }
        public DbSet<LogradouroMunicipio> LogradouroMunicipios { get; set; }
        public DbSet<LogradouroMunicipioCep> LogradouroMunicipioCeps { get; set; }
        public DbSet<LogradouroPais> LogradouroPaises { get; set; }
        public DbSet<LogradouroPrefixo> LogradouroPrefixos { get; set; }
        public DbSet<LogradouroTipo> LogradouroTipos { get; set; } 

        #endregion

        #region log

        public DbSet<TableLog> Logs { get; set; } 

        #endregion


        public DbSet<Master> Masters { get; set; }


        public DbSet<Document> Documents { get; set; }


        


        #region tabelas relacionadas ao módulo de email

        public DbSet<EmailList> EmailLists { get; set; }
        public DbSet<EmailConfiguration> EmailConfigurations { get; set; }
        public DbSet<Template> Templates { get; set; }

        #endregion



        


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder
                .Entity<Address>()
                .MapToStoredProcedures();

            modelBuilder
                .Entity<Step>()
                .MapToStoredProcedures();

        }

        #region Tabelas relacionadas as Linguagens
        public DbSet<Language> Languages { get; set; }
        public DbSet<LanguageKey> LanguageKeys { get; set; }
        #endregion

        

        

        #region tabelas relacionadas aos documentos
        //public DbSet<Document> ClientsDocuments { get; set; }
        #endregion

        #region Tabelas relacionadas ao usuário
        public DbSet<User> Users { get; set; }
        public DbSet<AuthLevel> AuthLevels { get; set; }
        #endregion

        #region Tabelas relacionadas ao endereço

        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<StreetType> StreetTypes { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Address> Addresses { get; set; }

        #endregion


        #region bulkInsert

        private readonly List<String> NiceTypes = new List<string> { "System.String" }; 

        public void BulkInsertAll<T>(T[] entities) where T : class
        {
            var conn = (SqlConnection)Database.Connection;

            conn.Open();

            Type t = typeof(T);
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            var workspace = objectContext.MetadataWorkspace;
            var mappings = GetMappings(workspace, objectContext.DefaultContainerName, typeof(T).Name);

            var tableName = GetTableName<T>();
            var bulkCopy = new SqlBulkCopy(conn) { DestinationTableName = tableName };

            var properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToArray();

            var table = new DataTable();
            var props = properties;
            foreach (var property in props)
            {
                Type propertyType = property.PropertyType;

                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = Nullable.GetUnderlyingType(propertyType);
                }
                if ((!propertyType.IsInterface && !propertyType.IsClass) || NiceTypes.Contains(propertyType.FullName))
                {
                    table.Columns.Add(new DataColumn(property.Name, propertyType));
                    var clrPropertyName = property.Name;
                    var tableColumnName = mappings[property.Name];
                    bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(clrPropertyName, tableColumnName));
                }
            }

            foreach (var entity in entities)
            {
                var e = entity;
                table.Rows.Add(properties.Where(a => (!a.PropertyType.IsInterface && !a.PropertyType.IsClass) || NiceTypes.Contains(a.PropertyType.FullName)).Select(property => GetPropertyValue(property.GetValue(e, null))).ToArray());
            }

            bulkCopy.BulkCopyTimeout = 5 * 60;
            bulkCopy.WriteToServer(table);

            conn.Close();
        }

        private string GetTableName<T>() where T : class
        {
            var dbSet = Set<T>();
            var sql = dbSet.ToString();
            var regex = new Regex(@"FROM (?<table>.*) AS");
            var match = regex.Match(sql);
            return match.Groups["table"].Value;
        }

        private object GetPropertyValue(object o)
        {
            if (o == null)
                return DBNull.Value;
            return o;
        }

        private Dictionary<string, string> GetMappings(MetadataWorkspace workspace, string containerName, string entityName)
        {
            var mappings = new Dictionary<string, string>();
            var storageMapping = workspace.GetItem<GlobalItem>(containerName, DataSpace.CSSpace);
            dynamic entitySetMaps = storageMapping.GetType().InvokeMember(
                "EntitySetMaps",
                BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance,
                null, storageMapping, null);

            foreach (var entitySetMap in entitySetMaps)
            {
                var typeMappings = GetArrayList("EntityTypeMappings", entitySetMap);
                dynamic typeMapping = typeMappings[0];
                dynamic types = GetArrayList("Types", typeMapping);

                if (types[0].Name == entityName)
                {
                    var fragments = GetArrayList("MappingFragments", typeMapping);
                    var fragment = fragments[0];
                    var properties = GetArrayList("AllProperties", fragment);
                    foreach (var property in properties)
                    {
                        var edmProperty = GetProperty("EdmProperty", property);
                        var columnProperty = GetProperty("ColumnProperty", property);
                        mappings.Add(edmProperty.Name, columnProperty.Name);
                    }
                }
            }

            return mappings;
        }

        private ArrayList GetArrayList(string property, object instance)
        {
            var type = instance.GetType();
            var objects = (IEnumerable)type.InvokeMember(
                property,
                BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance, null, instance, null);
            var list = new ArrayList();
            foreach (var o in objects)
            {
                list.Add(o);
            }
            return list;
        }

        private dynamic GetProperty(string property, object instance)
        {
            var type = instance.GetType();
            return type.InvokeMember(property, BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance, null, instance, null);
        }
        #endregion
    }
}