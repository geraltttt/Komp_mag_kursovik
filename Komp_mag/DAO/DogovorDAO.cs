using Agent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agent.DAO
{
    public class DogovorDAO
    {
        private Entities _entities = new Entities();

        public List<Dogovor> GetMyObject(string id)
        {       
            List<Dogovor> myobject = new List<Dogovor>();
            try
            {
                {
                    myobject = _entities.Dogovor
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

        public IEnumerable<Dogovor> GetAllDogovor()
        {
            return (from c in _entities.Dogovor.Include("GroupDog") select c);
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
        public Dogovor getDogovor(int id)
        {
            return (from c in _entities.Dogovor.Include("GroupDog")
                    where c.Id == id
                    select c).FirstOrDefault();
        }


        //
        public void CreateDogovor(Dogovor model)
        {
            try
            {
                using (var ctx = new Entities())
                {
                    string query = "INSERT INTO Dogovor (IDKl, IDAg, IDTr, Date, IDGroup, Described) VALUES(@P0, @P1, @P2, @P3, @P4, @P5)";
                List<object> parameterList = new List<object>{
                    model.IDKl,
                    model.IDAg,
                    model.IDTr,
                    model.Date,
                    model.IDGroup,
                    model.Described
                };
                    object[] parameters = parameterList.ToArray();
                    int result = ctx.Database.ExecuteSqlCommand(query, parameters);
                    Logger.Log.Info("Данные успешно добавлены в Dogovor");
               
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка: ", ex);
            }
        }

      

        public bool updateDogovor( Dogovor Dogov)
        {
            Dogovor originalRecords = getDogovor(Dogov.Id);
            try
            {
               
                originalRecords.IDTr = Dogov.IDTr;
                originalRecords.Date = Dogov.Date;
                originalRecords.IDGroup = Dogov.IDGroup;
                originalRecords.Described = Dogov.Described;

                _entities.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка: ", ex);
                return false;
            }
            return true;
        }
        public bool deleteDogovor(int Id)
        {
            Dogovor originalRecords = getDogovor(Id);
            try
            {
                _entities.Dogovor.Remove(originalRecords);
                _entities.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка: ", ex);
                return false;
            }
            return true;
        }

        public bool UpdateGroup(Dogovor dogovor)
        {
            try
            {
                var Entity = _entities.Dogovor.FirstOrDefault(x => x.Id == dogovor.Id);
                Entity.IDGroup = dogovor.IDGroup;
                Entity.IDAg = dogovor.IDAg;
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