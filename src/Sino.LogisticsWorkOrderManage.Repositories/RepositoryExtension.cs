using Sino.Domain.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Dapper;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Sino.LogisticsWorkOrderManage.Core;
using Sino.Domain.Entities;
using System.Linq.Expressions;
using System.ComponentModel;
using Sino;
using Sino.LogisticsWorkOrderManage.Core.IRepositories;
using Sino.Domain.Entities.Auditing;

namespace Sino.LogisticsWorkOrderManage.Repositories
{

    public static class ObjectExtension
    {
        /// <summary>
        /// 不为空
        /// </summary>
        /// <param name="obj">判断的对象</param>
        /// <returns></returns>
        public static bool IsNotNullAndEmpty(this object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }

    public static class RepositoryExtension
    {

        /// <summary>
        /// 验证字段数据是否已经存在于数据库
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="repository">继承于DapperRepositoryBase<TEntity, TPrimaryKey>的对象</param>
        /// <param name="colName">列名</param>
        /// <param name="datas">数据源</param>
        /// <param name="tableName">数据库表名</param>
        /// <returns></returns>
        public async static Task<IEnumerable<TBaseDataType>> ValidDataIsExistDB<TEntity, TPrimaryKey, TBaseDataType>(this IRepository<TEntity, TPrimaryKey> repository,
            string colName, IEnumerable<TBaseDataType> datas, string tableName = null)
             where TEntity : class, IEntity<TPrimaryKey>
        {
            if (datas?.Count() <= 0)
            {
                throw new Exception("");
            }

            string deleteWhere = string.Empty;
            StringBuilder sbSql = new StringBuilder();
            if (string.IsNullOrWhiteSpace(tableName))
            {
                var tableType = typeof(TEntity);
                var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
                tableName = tableAttr != null ? tableAttr.Name : tableType.Name;
                if (tableType.GetProperty("IsDeleted", BindingFlags.Public | BindingFlags.Instance) != null)
                {
                    deleteWhere = " IFNULL(IsDeleted,0) = 0 AND ";
                }
            }

            sbSql.Append($" SELECT {colName} FROM {tableName} WHERE {deleteWhere} {colName} IN @{colName}s ");
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{colName}s", datas);

            //真实的基本数据类型
            //var realDataType = datas.ToList()[0].GetType();

            var connProp = repository.GetType().GetProperty("ReadConnection", BindingFlags.Instance | BindingFlags.NonPublic);
            var connValue = connProp.GetValue(repository);
            var readConn = connValue as IDbConnection;
            using (readConn)
            {
                try
                {
                    return await readConn.QueryAsync<TBaseDataType>(sbSql.ToString(), parameters);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 验证字段数据是否已经存在于数据库
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="repository">继承于DapperRepositoryBase<TEntity, TPrimaryKey>的对象</param>
        /// <param name="conn">连接对象</param>
        /// <param name="trans">事务对象</param>
        /// <param name="colName">列名</param>
        /// <param name="datas">数据源</param>
        /// <param name="tableName">数据库表名</param>
        /// <returns></returns>
        public async static Task<IEnumerable<dynamic>> ValidDataIsExistDBRetDynamic<TEntity, TPrimaryKey, TBaseDataType>(this IRepository repository,
           IDbConnection conn, IDbTransaction trans, string colName, IEnumerable<TBaseDataType> datas, string tableName = null)
             where TEntity : class, IEntity<TPrimaryKey>
        {
            //if (datas?.Count() <= 0)
            //{
            //    throw new Exception("");
            //}

            string deleteWhere = string.Empty;
            StringBuilder sbSql = new StringBuilder();
            if (string.IsNullOrWhiteSpace(tableName))
            {
                var tableType = typeof(TEntity);
                var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
                tableName = tableAttr != null ? tableAttr.Name : tableType.Name;
                if (tableType.GetProperty("IsDeleted", BindingFlags.Public | BindingFlags.Instance) != null)
                {
                    deleteWhere = " IFNULL(IsDeleted,0) = 0 AND ";
                }
            }

            sbSql.Append($" SELECT {colName} FROM {tableName} WHERE {deleteWhere} {colName} IN @{colName}s ");
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{colName}s", datas);

            //真实的基本数据类型
            var realDataType = datas.ToList()[0].GetType();
            using (conn)
            {
                try
                {
                    return await conn.QueryAsync(realDataType, sbSql.ToString(), parameters, trans);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }



        public async static Task<IEnumerable<TEntity>> ValidDataIsExistDBRetEntitys<TEntity, TPrimaryKey, TBaseDataType>(this IRepository repository,
        IDbConnection conn, string colName, IEnumerable<TBaseDataType> datas, string tableName = null, IDbTransaction trans = null)
          where TEntity : class, IEntity<TPrimaryKey>
        {
            string deleteWhere = string.Empty;
            StringBuilder sbSql = new StringBuilder();
            if (string.IsNullOrWhiteSpace(tableName))
            {
                var tableType = typeof(TEntity);
                var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
                tableName = tableAttr != null ? tableAttr.Name : tableType.Name;
                if (tableType.GetInterface(nameof(IDeleted)) != null)
                {
                    deleteWhere = "  IFNULL(IsDeleted,0) = 0 AND ";
                }
            }

            sbSql.Append($" SELECT * FROM {tableName} WHERE {deleteWhere} {colName} IN @{colName}s ");
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{colName}s", datas);

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return await conn.QueryAsync<TEntity>(sbSql.ToString(), parameters, trans);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static Task<IEnumerable<TEntity>> ValidDataIsExistDBRetEntitys<TEntity, TPrimaryKey, TBaseDataType>(this IRepository repository,
        string colName, IEnumerable<TBaseDataType> datas, string tableName = null)
         where TEntity : class, IEntity<TPrimaryKey>
        {
            string deleteWhere = string.Empty;
            StringBuilder sbSql = new StringBuilder();
            if (string.IsNullOrWhiteSpace(tableName))
            {
                var tableType = typeof(TEntity);
                var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
                tableName = tableAttr != null ? tableAttr.Name : tableType.Name;
                if (tableType.GetInterface(nameof(IDeleted)) != null)
                {
                    deleteWhere = "  IFNULL(IsDeleted,0) = 0 AND ";
                }
            }

            sbSql.Append($" SELECT * FROM {tableName} WHERE {deleteWhere} {colName} IN @{colName}s ");
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{colName}s", datas);

            try
            {
                var connProp = repository.GetType().GetProperty("ReadConnection", BindingFlags.Instance | BindingFlags.NonPublic);
                var connValue = connProp.GetValue(repository);
                var conn = connValue as IDbConnection;
                using (conn)
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    return await conn.QueryAsync<TEntity>(sbSql.ToString(), parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async static Task<IEnumerable<TEntity>> GetEntitysNotContainDel<TEntity, TPrimaryKey, TBaseDataType>(this IRepository repository,
        string colName, IEnumerable<TBaseDataType> datas, string tableName = null)
         where TEntity : class, IEntity<TPrimaryKey>
        {
            string deleteWhere = string.Empty;
            StringBuilder sbSql = new StringBuilder();
            if (string.IsNullOrWhiteSpace(tableName))
            {
                var tableType = typeof(TEntity);
                var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
                tableName = tableAttr != null ? tableAttr.Name : tableType.Name;
                if (tableType.GetInterface(nameof(IOnlyDeleted)) != null)
                {
                    deleteWhere = "  IFNULL(IsDeleted,0) = 0 AND ";
                }
            }

            sbSql.Append($" SELECT * FROM {tableName} WHERE {deleteWhere} {colName} IN @{colName}s ");
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{colName}s", datas);

            try
            {
                var connProp = repository.GetType().GetProperty("ReadConnection", BindingFlags.Instance | BindingFlags.NonPublic);
                var connValue = connProp.GetValue(repository);
                var conn = connValue as IDbConnection;
                using (conn)
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    return await conn.QueryAsync<TEntity>(sbSql.ToString(), parameters);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async static Task<IEnumerable<TEntity>> ValidDataIsExistDBRetEntitys2<TEntity, TPrimaryKey, TBaseDataType>(this IRepository repository,
      IDbConnection conn, string colName, IEnumerable<TBaseDataType> datas, string tableName = null, IDbTransaction trans = null)
        where TEntity : class, IEntity<TPrimaryKey>
        {
            StringBuilder sbSql = new StringBuilder();
            if (string.IsNullOrWhiteSpace(tableName))
            {
                var tableType = typeof(TEntity);
                var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
                tableName = tableAttr != null ? tableAttr.Name : tableType.Name;
            }

            sbSql.Append($" SELECT * FROM {tableName} WHERE {colName} IN @{colName}s ");
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{colName}s", datas);

            //真实的基本数据类型
            var realDataType = datas.ToList()[0].GetType();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return await conn.QueryAsync<TEntity>(sbSql.ToString(), parameters, trans);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static Task<int> BulkUpdateAssignFields<TEntity, TPrimaryKey>(this IRepository repository,
       IDbConnection conn, IList<string> propNames, IList<TEntity> entities, string tableName = null, IDbTransaction trans = null)
         where TEntity : class, IEntity<TPrimaryKey>
        {
            var entityType = typeof(TEntity);
            if (string.IsNullOrWhiteSpace(tableName))
            {
                var tableType = typeof(TEntity);
                var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
                tableName = tableAttr != null ? tableAttr.Name : tableType.Name;
            }
            string propName = string.Empty;
            IList<string> fieldSqlList = new List<string>();
            var ids = entities.Select(t => GetMySqlDataVal(t.Id.GetType(), t.Id)).ToList();
            for (int i = 0; i < propNames.Count; i++)
            {
                propName = propNames[i];
                StringBuilder sbCase = new StringBuilder();
                sbCase.Append($" {propName} = CASE {nameof(IEntity<TPrimaryKey>.Id)} ");
                //获取属性
                var prop = entityType.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
                for (int j = 0; j < entities.Count; j++)
                {
                    var val = prop.GetValue(entities[j]);
                    sbCase.Append($" WHEN {ids[j]} THEN {GetMySqlDataVal(prop.PropertyType, val)} ");
                }
                sbCase.Append(" END ");
                fieldSqlList.Add(sbCase.ToString());
            }
            string sql = $" UPDATE {tableName} SET {string.Join(",", fieldSqlList)} WHERE {nameof(IEntity<TPrimaryKey>.Id)} IN ({string.Join(",", ids)}) ";
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return await conn.ExecuteAsync(sql, null, trans);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static Task<int> UpdateAssignFieldsById<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TEntity entity, IList<string> propNames, string tableName = null)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var entityType = typeof(TEntity);
            if (string.IsNullOrWhiteSpace(tableName))
            {
                var tableType = typeof(TEntity);
                var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
                tableName = tableAttr != null ? tableAttr.Name : tableType.Name;
            }

            var properties = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            StringBuilder sbSql = new StringBuilder();

            DynamicParameters parameters = new DynamicParameters();
            sbSql.Append($" UPDATE {tableName} SET ");

            IList<string> fields = new List<string>();

            foreach (var propName in propNames)
            {
                //判断属性是否存在
                if (properties.Count(t => t.Name.ToLower() == propName?.ToLower()) > 0)
                {
                    var prop = properties.First(t => t.Name.ToLower() == propName?.ToLower());
                    var propVal = prop.GetValue(entity);
                    if (propVal == null)
                        continue;

                    if (prop.Name != nameof(IEntity<TPrimaryKey>.Id))
                    {
                        fields.Add($"{prop.Name} = @{prop.Name} ");
                        parameters.Add($"@{prop.Name}", propVal);
                    }
                }
            }

            object id = null;
            if (properties.Count(t => t.Name == nameof(IEntity<TPrimaryKey>.Id)) > 0)
            {
                var idProp = properties.First(t => t.Name == nameof(IEntity<TPrimaryKey>.Id));
                var propVal = idProp.GetValue(entity);
                id = propVal;
            }

            if (id == null)
            {
                throw new SinoException("未提供主键Id值");
            }

            if (id != null && fields.Count() > 0)
            {
                sbSql.Append($" {string.Join(",", fields)} ");
                sbSql.Append($" WHERE {nameof(IEntity<TPrimaryKey>.Id)} = @{nameof(IEntity<TPrimaryKey>.Id)} ");
                parameters.Add($"@{nameof(IEntity<TPrimaryKey>.Id)}", id);
                //通过反射手段获取连接对象
                var connProp = repository.GetType().GetProperty("WriteConnection", BindingFlags.Instance | BindingFlags.NonPublic);
                if (connProp != null)
                {
                    var connValue = connProp.GetValue(repository);
                    if (connValue != null && connValue is IDbConnection)
                    {
                        var conn = connValue as IDbConnection;
                        using (conn)
                        {
                            int affectedRow = await conn.ExecuteAsync(sbSql.ToString(), parameters);
                            return affectedRow;
                        }
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 根据指定的某一个字段来更新指定的多个字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="repository"></param>
        /// <param name="conn"></param>
        /// <param name="wherePropName"></param>
        /// <param name="propNames"></param>
        /// <param name="entities"></param>
        /// <param name="tableName"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public async static Task<int> BulkUpdateAssignFields<TEntity, TPrimaryKey>(this IRepository repository,
      IDbConnection conn, string wherePropName, IList<string> propNames, IList<TEntity> entities, bool isFilterDeleted = true, string tableName = null, IDbTransaction trans = null)
        where TEntity : class, IEntity<TPrimaryKey>
        {
            var entityType = typeof(TEntity);
            if (string.IsNullOrWhiteSpace(tableName))
            {
                var tableType = typeof(TEntity);
                var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
                tableName = tableAttr != null ? tableAttr.Name : tableType.Name;
            }
            //where属性
            PropertyInfo whereProp = entityType.GetProperty(wherePropName);
            Expression typeConstantExp = Expression.Constant(whereProp.PropertyType);
            ParameterExpression paramExp = Expression.Parameter(entityType, "t");
            Expression propExp = Expression.Property(paramExp, whereProp);
            var methodInfo = typeof(RepositoryExtension).GetMethod(nameof(GetMySqlDataVal), BindingFlags.NonPublic | BindingFlags.Static);
            Expression callExp = Expression.Call(methodInfo, typeConstantExp, propExp);
            Expression<Func<TEntity, object>> lambda = Expression.Lambda<Func<TEntity, object>>(callExp, paramExp);

            string propName = string.Empty;
            IList<string> fieldSqlList = new List<string>();
            var ids = entities.Select(lambda.Compile()).ToList();
            for (int i = 0; i < propNames.Count; i++)
            {

                propName = propNames[i];

                StringBuilder sbCase = new StringBuilder();
                sbCase.Append($" {propName} = CASE {wherePropName} ");
                //获取属性
                var prop = entityType.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
                for (int j = 0; j < entities.Count; j++)
                {
                    var val = prop.GetValue(entities[j]);
                    sbCase.Append($" WHEN {ids[j]} THEN {GetMySqlDataVal(prop.PropertyType, val)} ");
                }
                sbCase.Append(" END ");
                fieldSqlList.Add(sbCase.ToString());
            }
            string sql = $" UPDATE {tableName} SET {string.Join(",", fieldSqlList)} WHERE  ";
            if (isFilterDeleted)
            {
                if (entityType.GetProperty("IsDeleted", BindingFlags.Public | BindingFlags.Instance) != null)
                {
                    sql += " IFNULL(IsDeleted,0) = 0 AND ";
                }
            }

            sql += $" {wherePropName} IN ({string.Join(",", ids)})";
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return await conn.ExecuteAsync(sql, null, trans);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新属性值不为空的对象集合通用方法
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="repository"></param>
        /// <param name="entities"></param>
        /// <param name="conn"></param>
        /// <param name="UpdateCols"></param>
        /// <param name="uniqueFieldName"></param>
        /// <param name="tableName"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public async static Task<int> BulkUpdateNotNullAssignFields<TEntity>(this IRepository repository, List<TEntity> entities, IDbConnection conn, List<string> UpdateCols, string uniqueFieldName = "Id", string tableName = "", IDbTransaction trans = null)
            where TEntity : class
        {
            var entityType = typeof(TEntity);
            if (string.IsNullOrWhiteSpace(tableName))
            {
                var tableType = typeof(TEntity);
                var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
                tableName = tableAttr != null ? tableAttr.Name : tableType.Name;
            }
            var properties = entityType.GetProperties();
            //获取每个属性对应的对象值不为空的对象集合
            Dictionary<PropertyInfo, List<TEntity>> dicPropEntities = new Dictionary<PropertyInfo, List<TEntity>>();
            if (UpdateCols?.Count > 0)
            {
                UpdateCols.ForEach(t =>
                {
                    if (properties.Count(x => string.Compare(t, x.Name, true) == 0) > 0 && string.Compare(t, uniqueFieldName, true) != 0)
                    {
                        var prop = properties.First(x => string.Compare(t, x.Name, true) == 0);
                        var tempList = entities.Where(x => x.GetType().GetProperty(prop.Name)?.GetValue(x) != null).ToList();
                        if (tempList.Count > 0)
                        {
                            dicPropEntities[prop] = tempList;
                        }
                    }
                });
            }
            else
            {
                properties.ToList().ForEach(t =>
                {
                    //排除唯一键属性
                    if (string.Compare(t.Name, uniqueFieldName, true) != 0)
                    {
                        var tempList = entities.Where(x => x.GetType().GetProperty(t.Name)?.GetValue(x) != null).ToList();
                        if (tempList.Count > 0)
                        {
                            dicPropEntities[t] = tempList;
                        }
                    }
                });
            }
            //唯一键属性
            var uniqueProp = properties.First(t => string.Compare(t.Name, uniqueFieldName, true) == 0);
            Expression typeConstantExp = Expression.Constant(uniqueProp.PropertyType);
            ParameterExpression paramExp = Expression.Parameter(entityType, "t");
            Expression propExp = Expression.Property(paramExp, uniqueProp);
            var methodInfo = typeof(RepositoryExtension).GetMethod(nameof(GetMySqlDataVal), BindingFlags.NonPublic | BindingFlags.Static);
            Expression callExp = Expression.Call(methodInfo, typeConstantExp, propExp);
            Expression<Func<TEntity, object>> lambda = Expression.Lambda<Func<TEntity, object>>(callExp, paramExp);

            List<string> totalSql = new List<string>();
            StringBuilder itemSql = new StringBuilder();
            //遍历每一个属性不为空的对象
            dicPropEntities.ToList().ForEach(t =>
            {
                itemSql.Append($" UPDATE {tableName} SET {t.Key.Name} = CASE {uniqueFieldName} ");
                t.Value.ForEach(i =>
                {
                    itemSql.Append($" WHEN {GetMySqlDataVal(uniqueProp.PropertyType, uniqueProp.GetValue(i))} THEN {GetMySqlDataVal(t.Key.PropertyType, t.Key.GetValue(i))} ");
                });
                itemSql.Append(" END ");
                var ids = t.Value.Select(lambda.Compile()).ToList();
                itemSql.Append($" WHERE {uniqueFieldName} IN ({string.Join(",", ids)}) ");
                totalSql.Add(itemSql.ToString());
            });

            try
            {
                if (totalSql.Count > 0)
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    return await conn.ExecuteAsync(string.Join("; ", totalSql), null, trans);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static Task<IEnumerable<TBaseDataType>> ValidDataIsExistDB<TEntity, TPrimaryKey, TBaseDataType>(this IRepository repository,
         IDbConnection conn, IDbTransaction trans, string colName, IEnumerable<TBaseDataType> datas, string tableName = null)
           where TEntity : class, IEntity<TPrimaryKey>
        {

            string deleteWhere = string.Empty;
            StringBuilder sbSql = new StringBuilder();
            if (string.IsNullOrWhiteSpace(tableName))
            {
                var tableType = typeof(TEntity);
                var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
                tableName = tableAttr != null ? tableAttr.Name : tableType.Name;
                if (tableType.GetProperty("IsDeleted", BindingFlags.Public | BindingFlags.Instance) != null)
                {
                    deleteWhere = " IFNULL(IsDeleted,0) = 0 AND ";
                }
            }

            sbSql.Append($" SELECT {colName} FROM {tableName} WHERE {deleteWhere} {colName} IN @{colName}s ");
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{colName}s", datas);

            //真实的基本数据类型
            var realDataType = datas.ToList()[0].GetType();
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return await conn.QueryAsync<TBaseDataType>(sbSql.ToString(), parameters, trans);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="TEntity">实体类型参数</typeparam>
        /// <typeparam name="TPrimaryKey">主键类型参数</typeparam>
        /// <param name="repository"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteAsync<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TPrimaryKey id, IDbTransaction transaction = null)
            where TEntity : class, IEntity<TPrimaryKey>, IDeleted
        {
            var tableType = typeof(TEntity);
            var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
            string tableName = tableAttr != null ? tableAttr.Name : tableType.Name;
            string strSql = $" UPDATE {tableName} SET {nameof(IDeleted.IsDeleted)} = 1 WHERE {nameof(IEntity<TPrimaryKey>.Id)} = @{nameof(IEntity<TPrimaryKey>.Id)} ";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{nameof(IEntity<TPrimaryKey>.Id)}", id);
            var connProp = repository.GetType().GetProperty("WriteConnection", BindingFlags.Instance | BindingFlags.NonPublic);
            var connValue = connProp.GetValue(repository);
            var conn = connValue as IDbConnection;
            using (conn)
            {
                int affectedRow = await conn.ExecuteAsync(strSql, parameters, transaction);
                return affectedRow > 0;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="repository"></param>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="deleteTime"></param>
        /// <param name="conn"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<int> DeleteAsync<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TPrimaryKey id, string userId, DateTime? deleteTime, IDbConnection conn, IDbTransaction transaction = null)
          where TEntity : class, IEntity<TPrimaryKey>, IDeleted
        {
            var tableType = typeof(TEntity);
            var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
            string tableName = tableAttr != null ? tableAttr.Name : tableType.Name;
            string strSql = $" UPDATE {tableName} SET {nameof(IDeleted.IsDeleted)} = 1 WHERE {nameof(IEntity<TPrimaryKey>.Id)} = @{nameof(IEntity<TPrimaryKey>.Id)} ";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{nameof(IEntity<TPrimaryKey>.Id)}", id);
            if (conn == null)
            {
                var connProp = repository.GetType().GetProperty("WriteConnection", BindingFlags.Instance | BindingFlags.NonPublic);
                var connValue = connProp.GetValue(repository);
                conn = connValue as IDbConnection;
            }
            using (conn)
            {
                int affectedRow = await conn.ExecuteAsync(strSql, parameters, transaction);
                return affectedRow;
            }
        }


        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="repository"></param>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="deleteTime"></param>
        /// <param name="conn"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<int> DeleteAsync<TEntity, TPrimaryKey>(this IRepository repository, IDbConnection conn, TPrimaryKey id, string userId, DateTime? deleteTime, IDbTransaction transaction = null)
          where TEntity : class, IEntity<TPrimaryKey>, IDeleted
        {
            var tableType = typeof(TEntity);
            var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
            string tableName = tableAttr != null ? tableAttr.Name : tableType.Name;
            string strSql = $" UPDATE {tableName} SET {nameof(IDeleted.IsDeleted)} = 1 WHERE {nameof(IEntity<TPrimaryKey>.Id)} = @{nameof(IEntity<TPrimaryKey>.Id)} ";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{nameof(IEntity<TPrimaryKey>.Id)}", id);
            using (conn)
            {
                int affectedRow = await conn.ExecuteAsync(strSql, parameters, transaction);
                return affectedRow;
            }
        }

        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="repository"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<bool> UpdateNotNullAsync<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TEntity entity, IDbTransaction transaction = null)
            where TEntity : class, IEntity<TPrimaryKey>
        {

            var type = typeof(TEntity);
            var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            string tableName = tableAttribute != null ? tableAttribute.Name : type.Name;

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            StringBuilder sbSql = new StringBuilder();

            DynamicParameters parameters = new DynamicParameters();
            sbSql.Append($" UPDATE {tableName} SET ");

            IList<string> fields = new List<string>();
            object id = null;
            Type idType = null;
            foreach (var prop in properties)
            {
                var propVal = prop.GetValue(entity);
                if (propVal == null)
                    continue;

                if (prop.Name != nameof(IEntity<TPrimaryKey>.Id))
                {
                    fields.Add($"{prop.Name} = @{prop.Name} ");
                    parameters.Add($"@{prop.Name}", propVal);
                }
                else
                {
                    idType = prop.PropertyType;
                    id = propVal;
                }
            }

            if (id != null && fields.Count() > 0)
            {
                sbSql.Append($" {string.Join(",", fields)} ");
                sbSql.Append($" WHERE {nameof(IEntity<TPrimaryKey>.Id)} = @{nameof(IEntity<TPrimaryKey>.Id)} ");
                parameters.Add($"@{nameof(IEntity<TPrimaryKey>.Id)}", id);
                //通过反射手段获取连接对象
                var connProp = repository.GetType().GetProperty("WriteConnection", BindingFlags.Instance | BindingFlags.NonPublic);
                if (connProp != null)
                {
                    var connValue = connProp.GetValue(repository);
                    if (connValue != null && connValue is IDbConnection)
                    {
                        var conn = connValue as IDbConnection;
                        using (conn)
                        {
                            int affectedRow = await conn.ExecuteAsync(sbSql.ToString(), parameters, transaction);
                            return affectedRow > 0;
                        }
                    }
                }
            }
            return false;
        }


        ///// <summary>
        ///// 更新实体对象
        ///// </summary>
        ///// <typeparam name="TEntity"></typeparam>
        ///// <typeparam name="TPrimaryKey"></typeparam>
        ///// <param name="repository"></param>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public static async Task<bool> UpdateNotNullAssignWhereFieldAsync<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TEntity entity, IDbTransaction transaction = null)
        //    where TEntity : class, IEntity<TPrimaryKey>
        //{

        //    var type = typeof(TEntity);
        //    var tableAttribute = type.GetCustomAttribute<TableAttribute>();
        //    string tableName = tableAttribute != null ? tableAttribute.Name : type.Name;

        //    var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        //    StringBuilder sbSql = new StringBuilder();

        //    DynamicParameters parameters = new DynamicParameters();
        //    sbSql.Append($" UPDATE {tableName} SET ");

        //    IList<string> fields = new List<string>();
        //    object id = null;
        //    Type idType = null;
        //    foreach (var prop in properties)
        //    {
        //        var propVal = prop.GetValue(entity);
        //        if (propVal == null)
        //            continue;

        //        if (prop.Name != nameof(IEntity<TPrimaryKey>.Id))
        //        {
        //            fields.Add($"{prop.Name} = @{prop.Name} ");
        //            parameters.Add($"@{prop.Name}", propVal);
        //        }
        //        else
        //        {
        //            idType = prop.PropertyType;
        //            id = propVal;
        //        }
        //    }

        //    if (id != null && fields.Count() > 0)
        //    {
        //        sbSql.Append($" {string.Join(",", fields)} ");
        //        sbSql.Append($" WHERE {nameof(IEntity<TPrimaryKey>.Id)} = @{nameof(IEntity<TPrimaryKey>.Id)} ");
        //        parameters.Add($"@{nameof(IEntity<TPrimaryKey>.Id)}", id);
        //        //通过反射手段获取连接对象
        //        var connProp = repository.GetType().GetProperty("WriteConnection", BindingFlags.Instance | BindingFlags.NonPublic);
        //        if (connProp != null)
        //        {
        //            var connValue = connProp.GetValue(repository);
        //            if (connValue != null && connValue is IDbConnection)
        //            {
        //                var conn = connValue as IDbConnection;
        //                using (conn)
        //                {
        //                    int affectedRow = await conn.ExecuteAsync(sbSql.ToString(), parameters, transaction);
        //                    return affectedRow > 0;
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}





        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="repository"></param>
        /// <param name="whereFieldName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<bool> UpdateNotNullAssignWhereFieldAsync<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TEntity entity, string whereFieldName, List<string> updateFields, IDbConnection conn = null, IDbTransaction transaction = null)
            where TEntity : class, IEntity<TPrimaryKey>
        {

            var type = typeof(TEntity);
            var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            string tableName = tableAttribute != null ? tableAttribute.Name : type.Name;

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            StringBuilder sbSql = new StringBuilder();

            DynamicParameters parameters = new DynamicParameters();
            sbSql.Append($" UPDATE {tableName} SET ");

            IList<string> fields = new List<string>();
            object fieldVal = properties.First(t => t.Name == whereFieldName).GetValue(entity);
            //Type fieldType = null;
            foreach (var prop in properties)
            {
                if (updateFields?.Count > 0)
                {
                    if (updateFields.Contains(prop.Name))
                    {
                        var propVal = prop.GetValue(entity);
                        if (propVal == null)
                            continue;

                        if (prop.PropertyType == typeof(string) && string.IsNullOrWhiteSpace(propVal.ToString()))
                        {
                            continue;
                        }

                        if (prop.Name != whereFieldName)
                        {
                            fields.Add($"{prop.Name} = @{prop.Name} ");
                            parameters.Add($"@{prop.Name}", propVal);
                        }
                        //else
                        //{
                        //    //fieldType = prop.PropertyType;
                        //    fieldVal = propVal;
                        //}
                    }
                }
                else
                {
                    var propVal = prop.GetValue(entity);
                    if (propVal == null)
                        continue;

                    if (prop.PropertyType == typeof(string) && string.IsNullOrWhiteSpace(propVal.ToString()))
                    {
                        continue;
                    }

                    if (prop.Name != whereFieldName)
                    {
                        fields.Add($"{prop.Name} = @{prop.Name} ");
                        parameters.Add($"@{prop.Name}", propVal);
                    }
                    //else
                    //{
                    //    //fieldType = prop.PropertyType;
                    //    fieldVal = propVal;
                    //}
                }
            }

            if (fieldVal != null && fields.Count() > 0)
            {
                sbSql.Append($" {string.Join(",", fields)} ");
                sbSql.Append($" WHERE {whereFieldName} = @{whereFieldName} ");
                parameters.Add($"@{whereFieldName}", fieldVal);
                if (conn == null)
                {
                    //通过反射手段获取连接对象
                    var connProp = repository.GetType().GetProperty("WriteConnection", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (connProp != null)
                    {
                        var connValue = connProp.GetValue(repository);
                        if (connValue != null && connValue is IDbConnection)
                        {
                            conn = connValue as IDbConnection;
                        }
                    }
                }

                if (conn != null)
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    int affectedRow = await conn.ExecuteAsync(sbSql.ToString(), parameters, transaction);
                    return affectedRow > 0;
                }
            }
            return false;
        }


        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="repository"></param>
        /// <param name="whereFieldName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<bool> UpdateNotNullAssignWhereFieldAsync<TEntity>(this IRepository repository, TEntity entity, string whereFieldName, List<string> updateFields, IDbConnection conn = null, IDbTransaction transaction = null)
        //where TEntity : IEntity<TPrimaryKey>
        {

            var type = typeof(TEntity);
            var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            string tableName = tableAttribute != null ? tableAttribute.Name : type.Name;

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            StringBuilder sbSql = new StringBuilder();

            DynamicParameters parameters = new DynamicParameters();
            sbSql.Append($" UPDATE {tableName} SET ");

            IList<string> fields = new List<string>();
            object fieldVal = properties.First(t => t.Name == whereFieldName).GetValue(entity);
            //Type fieldType = null;
            foreach (var prop in properties)
            {
                if (updateFields?.Count > 0)
                {
                    if (updateFields.Contains(prop.Name))
                    {
                        var propVal = prop.GetValue(entity);
                        if (propVal == null)
                            continue;

                        if (prop.PropertyType == typeof(string) && string.IsNullOrWhiteSpace(propVal.ToString()))
                        {
                            continue;
                        }

                        if (prop.Name != whereFieldName)
                        {
                            fields.Add($"{prop.Name} = @{prop.Name} ");
                            parameters.Add($"@{prop.Name}", propVal);
                        }
                        //else
                        //{
                        //    //fieldType = prop.PropertyType;
                        //    fieldVal = propVal;
                        //}
                    }
                }
                else
                {
                    var propVal = prop.GetValue(entity);
                    if (propVal == null)
                        continue;

                    if (prop.PropertyType == typeof(string) && string.IsNullOrWhiteSpace(propVal.ToString()))
                    {
                        continue;
                    }

                    if (prop.Name != whereFieldName)
                    {
                        fields.Add($"{prop.Name} = @{prop.Name} ");
                        parameters.Add($"@{prop.Name}", propVal);
                    }
                    //else
                    //{
                    //    //fieldType = prop.PropertyType;
                    //    fieldVal = propVal;
                    //}
                }
            }

            if (fieldVal != null && fields.Count() > 0)
            {
                sbSql.Append($" {string.Join(",", fields)} ");
                sbSql.Append($" WHERE {whereFieldName} = @{whereFieldName} ");
                parameters.Add($"@{whereFieldName}", fieldVal);
                if (conn == null)
                {
                    //通过反射手段获取连接对象
                    var connProp = repository.GetType().GetProperty("WriteConnection", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (connProp != null)
                    {
                        var connValue = connProp.GetValue(repository);
                        if (connValue != null && connValue is IDbConnection)
                        {
                            conn = connValue as IDbConnection;
                        }
                    }
                }

                if (conn != null)
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    int affectedRow = await conn.ExecuteAsync(sbSql.ToString(), parameters, transaction);
                    return affectedRow > 0;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="repository"></param>
        /// <param name="conn"></param>
        /// <param name="id"></param>
        /// <param name="isContainDeleted"></param>
        /// <param name="tableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<TEntity> GetByIdAsync<TEntity, TPrimaryKey>(this IRepository repository, IDbConnection conn, TPrimaryKey id, bool isContainDeleted = false, string tableName = null, IDbTransaction transaction = null)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            StringBuilder sbsql = new StringBuilder();
            string deleteWhere = string.Empty;
            if (string.IsNullOrWhiteSpace(tableName))
            {
                var tableType = typeof(TEntity);
                var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
                tableName = tableAttr != null ? tableAttr.Name : tableType.Name;
                if (!isContainDeleted)
                {
                    if (tableType.GetInterface(nameof(IDeleted)) != null)
                    {
                        deleteWhere = " AND IFNULL(IsDeleted,0) = 0 ";
                    }
                }
            }
            sbsql.AppendFormat(" SELECT * FROM {0} WHERE {1} = @{1} {2} ", tableName, nameof(IEntity<Guid>.Id), deleteWhere);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            return await conn.QueryFirstAsync<TEntity>(sbsql.ToString(), new { Id = id }, transaction);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="repository"></param>
        /// <param name="conn"></param>
        /// <param name="id"></param>
        /// <param name="isContainDeleted"></param>
        /// <param name="tableName"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<TEntity> GetByIdAsync<TEntity, TPrimaryKey>(this IRepository repository, TPrimaryKey id, bool isContainDeleted = false, string tableName = null)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            StringBuilder sbsql = new StringBuilder();
            string deleteWhere = string.Empty;
            if (string.IsNullOrWhiteSpace(tableName))
            {
                var tableType = typeof(TEntity);
                var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
                tableName = tableAttr != null ? tableAttr.Name : tableType.Name;
                if (!isContainDeleted)
                {
                    if (tableType.GetInterface(nameof(IDeleted)) != null)
                    {
                        deleteWhere = " AND IFNULL(IsDeleted,0) = 0 ";
                    }
                }
            }
            sbsql.AppendFormat(" SELECT * FROM {0} WHERE {1} = @{1} {2} ", tableName, nameof(IEntity<Guid>.Id), deleteWhere);

            //通过反射手段获取连接对象
            var connProp = repository.GetType().GetProperty("WriteConnection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (connProp != null)
            {
                var connValue = connProp.GetValue(repository);
                if (connValue != null && connValue is IDbConnection)
                {
                    var conn = connValue as IDbConnection;
                    using (conn)
                    {
                        return await conn.QueryFirstAsync<TEntity>(sbsql.ToString(), new { Id = id });
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="repository"></param>
        /// <param name="sourceData">处理源对象</param>
        /// <param name="templateTip">必须是"{0}"的形式，{0}占位符</param>
        /// <returns></returns>
        public async static Task HandleDataIsExist<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository,
            IList<Tuple<string, object, string, TPrimaryKey>> sourceData, string templateTip)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var tableType = typeof(TEntity);
            var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
            string tableName = tableAttr != null ? tableAttr.Name : tableType.Name;

            var isdeletedProp = tableType.GetProperty(nameof(IDeleted.IsDeleted), BindingFlags.Public | BindingFlags.Instance);
            bool isHasDeleted = isdeletedProp != null ? true : false;

            Dictionary<string, Tuple<StringBuilder, DynamicParameters>> dic = new Dictionary<string, Tuple<StringBuilder, DynamicParameters>>();
            foreach (var item in sourceData)
            {
                StringBuilder sbSql = new StringBuilder();
                DynamicParameters parameters = new DynamicParameters();
                sbSql.Append($" SELECT 1 FROM {tableName} WHERE {item.Item1} = @{item.Item1} ");
                parameters.Add($"@{item.Item1}", item.Item2);

                if (item.Item3.IsNotNullAndEmpty() && item.Item4.IsNotNullAndEmpty())
                {
                    sbSql.Append($" AND {item.Item3} != @{item.Item3} ");
                    parameters.Add($"@{item.Item3}", item.Item4);
                }
                if (isHasDeleted)
                {
                    sbSql.Append(" AND IFNULL(IsDeleted,0) = 0 ");
                }
                sbSql.Append(";");
                dic.Add(item.Item1, new Tuple<StringBuilder, DynamicParameters>(sbSql, parameters));
            }

            var connProp = repository.GetType().GetProperty("WriteConnection", BindingFlags.Instance | BindingFlags.NonPublic);
            var connValue = connProp.GetValue(repository);
            var readConn = connValue as IDbConnection;
            using (readConn)
            {
                try
                {
                    //Dictionary<string, bool> retDic = new Dictionary<string, bool>();
                    foreach (var item in dic)
                    {
                        var obj = await readConn.ExecuteScalarAsync<object>(item.Value.Item1.ToString(), item.Value.Item2);
                        if (obj != null)
                        {
                            var tempProp = tableType.GetProperty(item.Key, BindingFlags.Instance | BindingFlags.Public);
                            var attr = tempProp.GetCustomAttribute<DescriptionAttribute>();
                            string desc = attr != null ? attr.Description : tempProp.Name;
                            throw new SinoException(string.Format(templateTip, desc));
                        }
                        //retDic.Add(item.Key, obj != null);
                    }
                    //return retDic;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 获取最大编号
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="repository"></param>
        /// <param name="colName"></param>
        /// <param name="totalWidth"></param>
        /// <returns></returns>
        public async static Task<string> GetMaxCode<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, string colName, int totalWidth)
         where TEntity : class, IEntity<TPrimaryKey>, IDeleted
        {
            if (string.IsNullOrWhiteSpace(colName))
            {
                throw new Exception("列名为空");
            }
            StringBuilder sbSql = new StringBuilder();
            var type = typeof(TEntity);
            var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            string tableName = tableAttribute != null ? tableAttribute.Name : type.Name;
            sbSql.Append($" SELECT MAX({colName}) FROM {tableName} WHERE 1=1 ");
            if (type.GetInterface(nameof(IDeleted)) != null)
            {
                sbSql.Append(" AND IFNULL(IsDeleted,0) = 0 ");
            }
            var connProp = repository.GetType().GetProperty("WriteConnection", BindingFlags.Instance | BindingFlags.NonPublic);
            var connValue = connProp.GetValue(repository);
            var conn = connValue as IDbConnection;
            using (conn)
            {
                string maxCode = await conn.QueryFirstAsync<string>(sbSql.ToString());
                maxCode = maxCode.TrimStart('0');
                int intCode;
                if (!string.IsNullOrEmpty(maxCode) && int.TryParse(maxCode, out intCode))
                {
                    intCode += 1;
                }
                else
                {
                    intCode = 1;
                }
                return intCode.ToString().PadLeft(totalWidth, '0');
            }
        }

        public async static Task<string> GetMaxCode<TEntity, TPrimaryKey>(this IRepository repository, string colName, IDbConnection conn, IDbTransaction trans, Func<string, string> handleMaxCode)
         where TEntity : class, IEntity<TPrimaryKey>
        {
            if (string.IsNullOrWhiteSpace(colName))
            {
                throw new Exception("列名为空");
            }
            StringBuilder sbSql = new StringBuilder();
            var type = typeof(TEntity);
            var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            string tableName = tableAttribute != null ? tableAttribute.Name : type.Name;
            sbSql.Append($" SELECT MAX({colName}) FROM {tableName} WHERE 1=1 ");
            if (type.GetInterface(nameof(IDeleted)) != null)
            {
                sbSql.Append(" AND IFNULL(IsDeleted,0) = 0 ");
            }
            string maxCode = await conn.QueryFirstAsync<string>(sbSql.ToString(), null, trans);
            if (handleMaxCode != null)
            {
                return handleMaxCode(maxCode);
            }
            return maxCode;
        }

        public async static Task<string> GetMaxCode<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, string colName, IDbConnection conn, IDbTransaction trans, Func<string, string> handleMaxCode)
         where TEntity : class, IEntity<TPrimaryKey>
        {
            if (string.IsNullOrWhiteSpace(colName))
            {
                throw new Exception("列名为空");
            }
            StringBuilder sbSql = new StringBuilder();
            var type = typeof(TEntity);
            var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            string tableName = tableAttribute != null ? tableAttribute.Name : type.Name;
            sbSql.Append($" SELECT MAX({colName}) FROM {tableName} WHERE 1=1 ");
            if (type.GetInterface(nameof(IDeleted)) != null)
            {
                sbSql.Append(" AND IFNULL(IsDeleted,0) = 0 ");
            }

            if (conn == null)
            {
                var connProp = repository.GetType().GetProperty("WriteConnection", BindingFlags.Instance | BindingFlags.NonPublic);
                var connValue = connProp.GetValue(repository);
                conn = connValue as IDbConnection;
            }
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            string maxCode = await conn.QueryFirstAsync<string>(sbSql.ToString(), null, trans);
            if (handleMaxCode != null)
            {
                return handleMaxCode(maxCode);
            }
            return maxCode;
        }


        private static string GetMySqlDataVal(Type propValType, object val)
        {
            if (val == null)
            {
                return "NULL";
            }

            else if (propValType == typeof(int) || propValType == typeof(int?))
            {
                return val.ToString();
            }
            else if (propValType == typeof(long) || propValType == typeof(long?))
            {
                return val.ToString();
            }
            else if (propValType == typeof(decimal) || propValType == typeof(decimal?))
            {
                return val.ToString();
            }
            else if (propValType == typeof(double) || propValType == typeof(double?))
            {
                return val.ToString();
            }
            else if (propValType == typeof(float) || propValType == typeof(float?))
            {
                return val.ToString();
            }
            else if (propValType == typeof(DateTime))
            {
                return $"'{Convert.ToDateTime(val).ToString("yyyy-MM-dd HH:mm:ss")}'";
            }
            else if (propValType == typeof(DateTime?))
            {
                return (val as DateTime?).HasValue ? $"'{(val as DateTime?).Value.ToString("yyyy-MM-dd HH:mm:ss")}'" : "NULL";
            }
            else if (propValType == typeof(Guid) || propValType == typeof(Guid?))
            {
                return $"'{val.ToString()}'";
            }
            else if (propValType == typeof(string))
            {
                return $"'{val.ToString()}'";
            }
            else if (propValType == typeof(bool) || propValType == typeof(bool?))
            {
                return val.ToString().ToLower();
            }
            return "NULL";
        }

        public async static Task<int> AddAsync<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TEntity entity)
             where TEntity : class, IEntity<TPrimaryKey>
        {
            var connProp = repository.GetType().GetProperty("WriteConnection", BindingFlags.Instance | BindingFlags.NonPublic);
            var connValue = connProp.GetValue(repository);
            var writeConn = connValue as IDbConnection;
            return await AddAsync<TEntity, TPrimaryKey>(entity, writeConn, null);
        }

        public async static Task<int> AddAsync<TEntity, TPrimaryKey>(this IRepository repository, TEntity entity, IDbConnection conn, IDbTransaction trans = null)
        {
            return await AddAsync<TEntity, TPrimaryKey>(entity, conn, trans);
        }

        private async static Task<int> AddAsync<TEntity, TPrimaryKey>(TEntity entity, IDbConnection conn, IDbTransaction trans = null)
        {
            Type modelType = typeof(TEntity);
            var tableAttribute = modelType.GetCustomAttribute<TableAttribute>();
            string tempTableName = tableAttribute != null ? tableAttribute.Name : modelType.Name;
            var properties = modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties != null && properties.Count() > 0)
            {

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                //获取所有公共属性名
                var columnNames = properties.Select(t => t.Name);
                StringBuilder insertSql = new StringBuilder();
                insertSql.Append($" insert into {tempTableName}");
                IList<string> colNames = new List<string>();
                IList<string> vals = new List<string>();
                DynamicParameters parameters = new DynamicParameters();
                foreach (var columnName in columnNames)
                {
                    var prop = properties.First(t => t.Name == columnName);
                    var propVal = prop.GetValue(entity);
                    if (propVal != null)
                    {
                        colNames.Add(columnName);
                        vals.Add($"@{columnName}");
                        parameters.Add($"@{columnName}", propVal);
                    }
                }
                insertSql.Append($"({string.Join(",", colNames)})  values({string.Join(",", vals)});");
                return await conn.ExecuteAsync(insertSql.ToString(), parameters, trans);
            }
            else
            {
                throw new Exception($"该{nameof(TEntity)}类没有可实例化的公共属性");
            }
        }

        public async static Task<int> AddAsync<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TEntity entity, IDbConnection conn, IDbTransaction trans = null)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            return await AddAsync<TEntity, TPrimaryKey>(entity, conn, trans);
        }

        public async static Task<object> BulkAddAsync<TTarget>(this IBulkAddOrUpdate bulk, IList<TTarget> models, int bulkAddRecords = 1000,
            Func<IDbConnection, IDbTransaction, Type, IList<TTarget>, object> beforeAction = null,
            Func<IDbConnection, IDbTransaction, Type, IList<TTarget>, object> afterAction = null, IDbConnection conn = null, IDbTransaction trans = null)
             where TTarget : new()
        {
            object retVal = null;
            int length = 0;
            if (models == null || models.Count() <= 0)
            {
                throw new Exception("数据集为空");
            }
            else
            {
                length = models.Count();
            }

            //实体类名
            Type modelType = typeof(TTarget);
            var tableAttribute = modelType.GetCustomAttribute<TableAttribute>();
            string tempTableName = tableAttribute != null ? tableAttribute.Name : modelType.Name;
            var properties = modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties != null && properties.Count() > 0)
            {

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                //执行批量插入之前是否有操作
                beforeAction?.Invoke(conn, trans, modelType, models);
                //获取所有公共属性名
                var columnNames = properties.Select(t => t.Name);
                int step = 0;
                bool isExactDivision = (length % bulkAddRecords == 0);
                //批量插入数据
                if (isExactDivision)
                {
                    step = length / bulkAddRecords;
                }
                else
                {
                    step = (int)(length / bulkAddRecords) + 1;
                }


                int start = 0;
                int affecedRows = 0;
                for (int i = 0; i < step; i++)
                {
                    start = bulkAddRecords * i;
                    int stepSize = 0;
                    //判断是否为最后一段
                    if (step - i - 1 == 0)
                    {
                        stepSize = length - start;
                    }
                    else
                    {
                        stepSize = bulkAddRecords;
                    }
                    StringBuilder insertSql = new StringBuilder();
                    insertSql.Append($" insert into {tempTableName}({string.Join(",", columnNames)}) values ");
                    IList<string> valSqls = new List<string>();
                    //遍历按步长大小分多次插入数据库
                    for (int j = start; j < start + stepSize; j++)
                    {
                        IList<string> vals = new List<string>();
                        foreach (var columnName in columnNames)
                        {
                            var prop = properties.First(t => t.Name == columnName);
                            var propVal = prop.GetValue(models[j]);
                            vals.Add(GetMySqlDataVal(prop.PropertyType, propVal));
                        }
                        //转换成以,分割的值的字符串
                        string strVal = string.Join(",", vals);
                        valSqls.Add($"({strVal})");
                    }
                    insertSql.AppendFormat("{0};", string.Join(",", valSqls));
                    affecedRows += await conn.ExecuteAsync(insertSql.ToString(), null, trans);
                }
                //如果插入成功的数据与传入的数据量一致
                retVal = affecedRows;
                //1.数据操作对象，2：操作目标表对象类型，3：临时表对象类型，4：批量操作标识数据，5：返回对象
                if (afterAction != null)
                {
                    retVal = afterAction(conn, trans, modelType, models);
                    if (retVal is Task<object>)
                    {
                        retVal = await (retVal as Task<object>);
                    }
                }
                return retVal;
            }
            else
            {
                throw new Exception($"该{nameof(TTarget)}类没有可实例化的公共属性");
            }
        }

        public async static Task<object> BulkAddAsync2<TTarget>(this IBulkAddOrUpdate bulk, IList<TTarget> models, int bulkAddRecords = 1000,
            Func<IDbConnection, IDbTransaction, Type, IList<TTarget>, object> beforeAction = null,
            Func<IDbConnection, IDbTransaction, Type, IList<TTarget>, object> afterAction = null, IDbConnection conn = null, IDbTransaction trans = null)
             where TTarget : new()
        {
            object retVal = null;
            int length = 0;
            if (models == null || models.Count() <= 0)
            {
                throw new Exception("数据集为空");
            }
            else
            {
                length = models.Count();
            }

            //实体类名
            Type modelType = typeof(TTarget);
            var tableAttribute = modelType.GetCustomAttribute<TableAttribute>();
            string tempTableName = tableAttribute != null ? tableAttribute.Name : modelType.Name;
            var properties = modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties != null && properties.Count() > 0)
            {

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                //执行批量插入之前是否有操作
                beforeAction?.Invoke(conn, trans, modelType, models);
                //获取所有公共属性名
                var columnNames = properties.Select(t => t.Name);
                var parameterColNames = properties.Select(t => "@" + t.Name).ToList();
                int step = 0;
                bool isExactDivision = (length % bulkAddRecords == 0);
                //批量插入数据
                if (isExactDivision)
                {
                    step = length / bulkAddRecords;
                }
                else
                {
                    step = (int)(length / bulkAddRecords) + 1;
                }

                int skip = 0;
                int affecedRows = 0;
                for (int i = 0; i < step; i++)
                {
                    skip = bulkAddRecords * i;
                    int stepSize = 0;
                    //判断是否为最后一段
                    if (step - i - 1 == 0)
                    {
                        stepSize = length - skip;
                    }
                    else
                    {
                        stepSize = bulkAddRecords;
                    }
                    StringBuilder insertSql = new StringBuilder();
                    insertSql.Append($" INSERT INTO {tempTableName}({string.Join(",", columnNames)}) VALUES ({string.Join(",", parameterColNames)}); ");
                    affecedRows += await conn.ExecuteAsync(insertSql.ToString(), models.Skip(skip).Take(stepSize).ToList(), trans);
                }
                //如果插入成功的数据与传入的数据量一致
                retVal = affecedRows;
                //1.数据操作对象，2：操作目标表对象类型，3：临时表对象类型，4：批量操作标识数据，5：返回对象
                if (afterAction != null)
                {
                    retVal = afterAction(conn, trans, modelType, models);
                    if (retVal is Task<object>)
                    {
                        retVal = await (retVal as Task<object>);
                    }
                }
                return retVal;
            }
            else
            {
                throw new Exception($"该{nameof(TTarget)}类没有可实例化的公共属性");
            }
        }

        public static void TruncateTable(this IRepository repository, Type type, IDbConnection conn, IDbTransaction trans)
        {
            var tableAttribute = type.GetCustomAttribute<TableAttribute>();
            string tempTableName = tableAttribute != null ? tableAttribute.Name : type.Name;
            if (conn != null && conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            conn.Execute($" TRUNCATE TABLE { tempTableName}; ", transaction: trans);
        }

        public async static Task<object> BulkAddAsync3<TTarget>(this IBulkAddOrUpdate bulk, IList<TTarget> models, int bulkAddRecords = 1000,
          Action<IDbConnection, IDbTransaction, Type, IList<TTarget>> beforeAction = null,
          Action<IDbConnection, IDbTransaction, Type, IList<TTarget>> afterAction = null, IDbConnection conn = null, IDbTransaction trans = null)
           where TTarget : new()
        {
            object retVal = null;
            int length = 0;
            if (models == null || models.Count() <= 0)
            {
                throw new Exception("数据集为空");
            }
            else
            {
                length = models.Count();
            }

            //实体类名
            Type modelType = typeof(TTarget);
            var tableAttribute = modelType.GetCustomAttribute<TableAttribute>();
            string tempTableName = tableAttribute != null ? tableAttribute.Name : modelType.Name;
            var properties = modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties != null && properties.Count() > 0)
            {

                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                //执行批量插入之前是否有操作
                beforeAction?.Invoke(conn, trans, modelType, models);
                //获取所有公共属性名
                var columnNames = properties.Select(t => t.Name);
                var parameterColNames = properties.Select(t => "@" + t.Name).ToList();
                int step = 0;
                bool isExactDivision = (length % bulkAddRecords == 0);
                //批量插入数据
                if (isExactDivision)
                {
                    step = length / bulkAddRecords;
                }
                else
                {
                    step = (int)(length / bulkAddRecords) + 1;
                }

                int skip = 0;
                int affecedRows = 0;
                for (int i = 0; i < step; i++)
                {
                    skip = bulkAddRecords * i;
                    int stepSize = 0;
                    //判断是否为最后一段
                    if (step - i - 1 == 0)
                    {
                        stepSize = length - skip;
                    }
                    else
                    {
                        stepSize = bulkAddRecords;
                    }
                    StringBuilder insertSql = new StringBuilder();
                    insertSql.Append($" INSERT INTO {tempTableName}({string.Join(",", columnNames)}) VALUES ({string.Join(",", parameterColNames)}); ");
                    affecedRows += await conn.ExecuteAsync(insertSql.ToString(), models.Skip(skip).Take(stepSize).ToList(), trans);
                }
                //如果插入成功的数据与传入的数据量一致
                retVal = affecedRows;
                //1.数据操作对象，2：操作目标表对象类型，3：临时表对象类型，4：批量操作标识数据
                afterAction?.Invoke(conn, trans, modelType, models);
                return retVal;
            }
            else
            {
                throw new Exception($"该{nameof(TTarget)}类没有可实例化的公共属性");
            }
        }


        public async static Task<List<TEntity>> GetEntityList<TEntity, TPrimaryKey>(this IRepository repository, bool isContainDeleted = false, string tableName = null, int? timeOut = null)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            StringBuilder sbsql = new StringBuilder();
            string deleteWhere = string.Empty;
            if (string.IsNullOrWhiteSpace(tableName))
            {
                var tableType = typeof(TEntity);
                var tableAttr = tableType.GetCustomAttribute<TableAttribute>();
                tableName = tableAttr != null ? tableAttr.Name : tableType.Name;
                if (!isContainDeleted)
                {
                    if (tableType.GetInterface(nameof(IDeleted)) != null)
                    {
                        deleteWhere = " WHERE IFNULL(IsDeleted,0) = 0 ";
                    }
                }
            }
            sbsql.AppendFormat(" SELECT * FROM {0}  {1}  ", tableName, deleteWhere);

            //通过反射手段获取连接对象
            var connProp = repository.GetType().GetProperty("WriteConnection", BindingFlags.Instance | BindingFlags.NonPublic);
            if (connProp != null)
            {
                var connValue = connProp.GetValue(repository);
                if (connValue != null && connValue is IDbConnection)
                {
                    var conn = connValue as IDbConnection;
                    using (conn)
                    {
                        IEnumerable<TEntity> result = null;
                        if (timeOut.HasValue)
                        {
                            result = await conn.QueryAsync<TEntity>(sbsql.ToString(), commandTimeout: timeOut.Value);
                        }
                        else
                        {
                            result = await conn.QueryAsync<TEntity>(sbsql.ToString());
                        }
                        return result.ToList();
                    }
                }
            }
            return null;
        }
    }
}
