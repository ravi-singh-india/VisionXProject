using System;
using System.Configuration;
using System.Threading;
using VisionX.BusinessEntities.Repositories;
using VisionX.BusinessAccessLayer.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;

namespace VisionX.DataAccessLayer.Repositories
{
    public class Dal
    {
        #region "Initialize"

        DBConfigPara DBConfigPara = new DBConfigPara();

        string ConnectionString = "";
        DataTable dt;
        DataSet ds;
        public SqlConnection sqlcon;
        SqlCommand sqlcmd;
        SqlDataAdapter da;
        public SqlTransaction transaction = null;
        Bal bAL = new Bal();
        private object updateStatusLock = new object();
        static readonly object lockObject = new object();

        #endregion

        #region "Constructor"
        public Dal()
        {
            ConnectionString = bAL.ReadConfigPara();
        }

        #endregion

        #region "TransactionFun"

        public void BeginTransaction()
        {
            lock (updateStatusLock)
            {
                Monitor.Enter(lockObject);
                try
                {
                    sqlcon = new SqlConnection(ConnectionString);
                    sqlcon.Open();
                    transaction = sqlcon.BeginTransaction("FMS");
                }
                catch
                {
                    throw;
                }
                finally { Monitor.Exit(lockObject); }
            }
        }
        public void Commit()
        {
            lock (updateStatusLock)
            {
                Monitor.Enter(lockObject);
                try
                {
                    transaction.Commit();
                }
                catch
                {
                    throw;
                }
                finally { if (sqlcon.State == ConnectionState.Open) { sqlcon.Close(); } Monitor.Exit(lockObject); transaction = null; }

            }
        }
        public void Rollback()
        {
            lock (updateStatusLock)
            {
                Monitor.Enter(lockObject);
                try
                {
                    transaction.Rollback();
                }
                catch
                {
                    throw;
                }
                finally { if (sqlcon.State == ConnectionState.Open) { sqlcon.Close(); } Monitor.Exit(lockObject); transaction = null; }
            }
        }
        public DataSet GetDataSet(string storedProcedure, SqlParameter[] sqlParameters)
        {
            lock (updateStatusLock)
            {
                Monitor.Enter(lockObject);
                try
                {
                    using (sqlcon = new SqlConnection(ConnectionString))
                    {
                        ds = new DataSet();
                        sqlcmd = new SqlCommand(storedProcedure, sqlcon);
                        sqlcmd.CommandTimeout = 3600 * 3;
                        da = new SqlDataAdapter(sqlcmd);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        foreach (object obj in sqlParameters)
                        {
                            sqlcmd.Parameters.Add(obj);
                        }
                        da.Fill(ds);
                        return ds;
                    }
                }
                catch
                {
                    throw;
                }
                finally { if (sqlcon.State == ConnectionState.Open) { sqlcon.Close(); } Monitor.Exit(lockObject); }
            }
        }
        public DataSet GetDataSet(string storedProcedure)
        {
            lock (updateStatusLock)
            {
                Monitor.Enter(lockObject);
                try
                {
                    using (sqlcon = new SqlConnection(ConnectionString))
                    {
                        ds = new DataSet();
                        sqlcmd = new SqlCommand(storedProcedure, sqlcon);
                        sqlcmd.CommandTimeout = 3600 * 3;
                        da = new SqlDataAdapter(sqlcmd);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        da.Fill(ds);
                        return ds;
                    }
                }
                catch
                {
                    throw;
                }
                finally { if (sqlcon.State == ConnectionState.Open) { sqlcon.Close(); } Monitor.Exit(lockObject); }
            }
        }
        public DataTable GetDataTable(string storedProcedure)
        {
            lock (updateStatusLock)
            {
                Monitor.Enter(lockObject);
                try
                {
                    using (sqlcon = new SqlConnection(ConnectionString))
                    {
                        dt = new DataTable();
                        sqlcmd = new SqlCommand(storedProcedure, sqlcon);
                        sqlcmd.CommandTimeout = 3600 * 3;
                        da = new SqlDataAdapter(sqlcmd);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        da.Fill(dt);
                        return dt;
                    }
                }
                catch
                {
                    throw;
                }
                finally { if (sqlcon.State == ConnectionState.Open) { sqlcon.Close(); } Monitor.Exit(lockObject); }
            }
        }
        public DataTable GetDataTable(string storedProcedure, SqlParameter[] sqlParameters)
        {
            lock (updateStatusLock)
            {
                Monitor.Enter(lockObject);
                try
                {
                    using (sqlcon = new SqlConnection(ConnectionString))
                    {
                        dt = new DataTable();
                        sqlcmd = new SqlCommand(storedProcedure, sqlcon);
                        sqlcmd.CommandTimeout = 3600 * 3;
                        da = new SqlDataAdapter(sqlcmd);
                        sqlcmd.CommandType = CommandType.StoredProcedure;
                        foreach (object obj in sqlParameters)
                        {
                            sqlcmd.Parameters.Add(obj);
                        }
                        da.Fill(dt);
                        return dt;
                    }
                }
                catch
                {
                    throw;
                }
                finally { if (sqlcon.State == ConnectionState.Open) { sqlcon.Close(); } Monitor.Exit(lockObject); }
            }
        }
        public Int32 InsertUpdateRecords(string storedProcedure, SqlParameter[] sqlParameters)
        {
            lock (updateStatusLock)
            {
                Monitor.Enter(lockObject);
                try
                {
                    if (sqlcon.State == ConnectionState.Closed) { sqlcon.Open(); }
                    sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcon;

                    foreach (object obj in sqlParameters)
                    {
                        sqlcmd.Parameters.Add(obj);
                    }
                    sqlcmd.Transaction = transaction;
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = storedProcedure;
                    sqlcmd.CommandTimeout = 3600 * 3;
                    return Convert.ToInt32(sqlcmd.ExecuteScalar());
                }
                catch
                {
                    throw;
                }
                finally { Monitor.Exit(lockObject); }
            }
        }
        public void InsertRecordsInBulk(string storedProcedure, SqlParameter[] sqlParameters)
        {
            lock (updateStatusLock)
            {
                Monitor.Enter(lockObject);
                try
                {
                    if (sqlcon.State == ConnectionState.Closed) { sqlcon.Open(); }
                    sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcon;

                    foreach (object obj in sqlParameters)
                    {
                        sqlcmd.Parameters.Add(obj);
                    }
                    sqlcmd.Transaction = transaction;
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = storedProcedure;
                    sqlcmd.CommandTimeout = 3600 * 3;
                    sqlcmd.ExecuteScalar();
                }
                catch
                {
                    throw;
                }
                finally { Monitor.Exit(lockObject); }
            }
        }

        #endregion
    }
}
