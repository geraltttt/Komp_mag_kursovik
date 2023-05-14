using Agent.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Agent.DAO
{
    public class StrSlDAO : DAO
    {
        private Entities _entities = new Entities();

        public List<StrSl> GetMyObject(string id)
        {
            List<StrSl> myobject = new List<StrSl>();
            try
            {         
                {
                    myobject = _entities.StrSl
                        .Include("GroupDog")
                        .Include("AspNetUsers")
                        .Where(c => c.IDKl == id).ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка: ", ex);
            }
            return myobject;
        }

        public IEnumerable<StrSl> GetAllStrSl()
        {
            return (from c in _entities.StrSl.Include("GroupDog") select c);
        }
        public GroupDog GetGroupDog(int? id)
        {
            if (id != null) //возращает запись по её Id
                return (from c in _entities.GroupDog
                        where c.Id == id
                        select c).FirstOrDefault();
            else // возращает первую запись в таблице
                return (from c in _entities.GroupDog
                        select c).FirstOrDefault();
        }
        public StrSl getStrSl(int id)
        {
            return (from c in _entities.StrSl.Include("GroupDog")
                    where c.Id == id
                    select c).FirstOrDefault();
        }

        public void CreateStrSl(StrSl model)
        {
            try
            {
                using (var ctx = new Entities())
                {
                    string query = "INSERT INTO StrSl (IDKl, IDAg, IDDogv, Date, Described, IDGroup) VALUES(@P0, @P1, @P2, @P3, @P4, @P5)";
                    List<object> parameterList = new List<object>{
                    model.IDKl,
                    model.IDAg,
                    model.IDDogv,
                    model.Date,
                    model.Described,
                    model.IDGroup
                };
                    object[] parameters = parameterList.ToArray();
                    int result = ctx.Database.ExecuteSqlCommand(query, parameters);
                             Logger.Log.Info("Данные успешно добавлены в StrSl");
                  
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка: ", ex);
            }
        }
        public bool updateStrSl( StrSl Str)
        {
            StrSl originalRecords = getStrSl(Str.Id);
        
            try
            {
                originalRecords.IDKl = Str.IDKl;
                originalRecords.IDDogv = Str.IDDogv;
                originalRecords.Date = Str.Date;
                originalRecords.Described = Str.Described;
                originalRecords.IDGroup = Str.IDGroup;
       

                _entities.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка: ", ex);
                return false;
            }
            return true;
        }
        public bool deleteStrSl(int Id)
        {
            StrSl originalStrSl = getStrSl(Id);
            try
            {
                _entities.StrSl.Remove(originalStrSl);
                _entities.SaveChanges();
            }
             catch (Exception ex)
            {
                Logger.Log.Error("Ошибка: ", ex);
                return false;
            }
            return true;
        }
    }
}