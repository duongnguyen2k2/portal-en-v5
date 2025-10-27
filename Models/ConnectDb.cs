using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class ConnectDb
    {
        public static DataSet ExecuteDataSetTask(string conString,
            string store,
            string[] thamso,
            object[] giatri)
        {
            DataSet task = ExecuteDataSet(conString, store, thamso, giatri);
            return task;
        }

        public static DataTable ExecuteDataTableTask(string conString,
            string store,
            string[] thamso,
            object[] giatri)
        {
            DataTable task = ExecuteDataTable(conString, store, thamso, giatri);
            return task;
        }

        public static void ExecuteNonQueryTask(string conString,
            string store,
            string[] thamso,
            object[] giatri) => ExecuteNonQuery(conString, store, thamso, giatri);

        public static void ExecuteNonQuery(string conString,
            string store,
            string[] thamso,
            object[] giatri)
        {

            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = store;
                    comm.CommandType = System.Data.CommandType.StoredProcedure;
                    for (int i = 0; i < thamso.Length; i++)
                    {
                        object paramValue = giatri[i];
                        if (paramValue == null) paramValue = DBNull.Value;
                        SqlParameter param = comm.Parameters.AddWithValue(thamso[i], paramValue);
                        if (giatri[i] is DataTable)
                        {
                            param.SqlDbType = SqlDbType.Structured;
                        }

                    }
                    conn.Open();
                    comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
        }

        //ADD BY YDUY - START
        public static object ExecuteNonQueryReturnValueTask(string conString,
            string store,
            string[] thamso,
            object[] giatri)
        {
            return ExecuteNonQueryReturnValue(conString, store, thamso, giatri);
        }

        public static object ExecuteNonQueryReturnValue(string conString,
            string store,
            string[] thamso,
            object[] giatri)
        {

            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = store;
                    comm.CommandType = System.Data.CommandType.StoredProcedure;
                    for (int i = 0; i < thamso.Length; i++)
                    {
                        object paramValue = giatri[i];
                        if (paramValue == null) paramValue = DBNull.Value;
                        SqlParameter param = comm.Parameters.AddWithValue(thamso[i], paramValue);
                        if (giatri[i] is DataTable)
                        {
                            param.SqlDbType = SqlDbType.Structured;
                        }

                    }
                    var returnParameter = comm.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    conn.Open();
                    comm.ExecuteNonQuery();
                    var result = returnParameter.Value;
                    conn.Close();
                    return result;
                }
            }
        }

        public static object ExecuteQueryScalarTask(string conString, string sql)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                conn.Open();
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    return comm.ExecuteScalar();
                }
            }
        }

        public static DataTable ExecuteQuerySQLTask(string conString, string sql)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter()
                    {
                        SelectCommand = comm
                    };
                    //da.SelectCommand = comm;
                    conn.Open();
                    da.Fill(dt);
                    conn.Close();
                    return dt;
                }
            }
        }

        //ADD BY YDUY - END

        public static DataTable ExecuteDataTable(string conString,
            string store,
            string[] thamso,
            object[] giatri)
        {

            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = store;
                    comm.CommandType = System.Data.CommandType.StoredProcedure;
                    for (int i = 0; i < thamso.Length; i++)
                    {
                        comm.Parameters.AddWithValue(thamso[i], giatri[i]);
                    }
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter()
                    {
                        SelectCommand = comm
                    };
                    //da.SelectCommand = comm;
                    conn.Open();
                    da.Fill(dt);
                    conn.Close();
                    return dt;
                }
            }
        }

        public static DataSet ExecuteDataSet(string conString,
            string store,
            string[] thamso,
            object[] giatri)
        {

            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    comm.CommandText = store;
                    comm.CommandType = System.Data.CommandType.StoredProcedure;
                    for (int i = 0; i < thamso.Length; i++)
                    {
                        comm.Parameters.AddWithValue(thamso[i], giatri[i]);
                    }
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter()
                    {
                        SelectCommand = comm
                    };
                    //da.SelectCommand = comm;
                    conn.Open();
                    da.Fill(ds);
                    conn.Close();
                    return ds;
                }
            }
        }

        // ADD BY TAYD - START

        public static DataTable ExecuteQuerySQLWithParamsTask(string conString, string sql, string[] paramNames, object[] values)
        {
            if (paramNames?.Length != values?.Length)
                throw new ArgumentException("Số lượng paramNames phải khớp với values.");

            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    // Thêm parameters an toàn
                    if (paramNames != null)
                    {
                        for (int i = 0; i < paramNames.Length; i++)
                        {
                            comm.Parameters.AddWithValue(paramNames[i], values[i] ?? DBNull.Value);
                        }
                    }

                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(comm))
                    {
                        conn.Open();
                        da.Fill(dt);
                    }
                    return dt;
                }
            }
        }

        public static int ExecuteNonQueryWithParamsTask(string conString, string sql, string[] paramNames, object[] values)
        {
            if (paramNames?.Length != values?.Length)
                throw new ArgumentException("Số lượng paramNames phải khớp với values.");

            using (SqlConnection conn = new SqlConnection(conString))
            {
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    // Thêm parameters an toàn
                    if (paramNames != null)
                    {
                        for (int i = 0; i < paramNames.Length; i++)
                        {
                            comm.Parameters.AddWithValue(paramNames[i], values[i] ?? DBNull.Value);
                        }
                    }

                    conn.Open();
                    return comm.ExecuteNonQuery();
                }
            }
        }

        // ADD BY TAYD - END
    }
}