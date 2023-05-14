using Agent.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Agent.DAO
{
    public class TarifDAO : DAO
    {
        public List<Tarif> GetAllTarif()
        {
            Connect();
            List<Tarif> tarifList = new List<Tarif>();
            try
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Tarif", Connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Tarif tarif = new Tarif();
                    tarif.Id = Convert.ToInt32(reader["Id"]);
                    tarif.Price = Convert.ToInt32(reader["Price"]);
                    tarif.Object = Convert.ToString(reader["Object"]);
                    tarif.Conditions = Convert.ToString(reader["Conditions"]);

                    tarifList.Add(tarif);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка: ", ex);
            }
            finally { Disconnect(); }
            return tarifList;
        }
        
        public bool AddTarif(Tarif tarif)
        {
            bool result = true;
            Connect();
            try
            {
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO  Tarif (Price, Object, Conditions) " +
                    "VALUES (@Price, @Object, @Conditions)", Connection
                    );
                cmd.Parameters.AddWithValue("@Price", tarif.Price);
                cmd.Parameters.AddWithValue("@Object", tarif.Object);
                cmd.Parameters.AddWithValue("@Conditions", tarif.Conditions);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка: ", ex);
                result = false;
            }
            finally { Disconnect(); }
            return result;
        }
        public void EditTarif(Tarif tarif)
        {
            try
            {
                Connect();
                string str = "UPDATE Tarif SET Price = '" + tarif.Price
                    + "', Object = '" + tarif.Object
                    + "', Conditions = '" + tarif.Conditions
                    + "'WHERE Id = " + tarif.Id;
                SqlCommand com = new SqlCommand(str, Connection);
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка: ", ex);
            }
            finally
            {
                Disconnect();
            }
        }
        public void DeleteTarif(int id)
        {
            try
            {
                Connect();
                string str = "DELETE FROM Tarif WHERE Id=" + id;
                SqlCommand com = new SqlCommand(str, Connection);
                com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка: ", ex);
            }
            finally
            {
                Disconnect();
            }
        }

    }
}