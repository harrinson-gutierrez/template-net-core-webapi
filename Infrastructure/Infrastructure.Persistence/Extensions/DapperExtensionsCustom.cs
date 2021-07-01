using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Extensions
{
    public static class DapperExtensionsCustom
    {

        public static async Task<List<TKey>> InsertAllAsync<TKey, TEntity>(this IDbConnection connection, IList<TEntity> entitiesToInsert, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            List<TKey> keys;

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                keys = new List<TKey>();

                if (transaction == null)
                    transaction = connection.BeginTransaction();

                foreach (TEntity entity in entitiesToInsert)
                {
                    var tkey = await connection.InsertAsync<TKey, TEntity>(entity, transaction);

                    if (tkey != null)
                        keys.Add(tkey);
                }

                if (!keys.Count.Equals(entitiesToInsert.Count))
                    throw new ArgumentException(string.Format(@"Entities not insert bulk data @Count - @Count2", new { keys.Count, Count2 = entitiesToInsert.Count }));

                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw e;
            }
            finally
            {
                transaction.Dispose();
            }

            return keys;
        }
    }
}
