using Agent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agent.DAO
{
    public class GroupDogDAO
    {
        private Entities _entities = new Entities();
        public IEnumerable<GroupDog> GetAllGroups()
        {
            return (from c in _entities.GroupDog
                    select c);
        }
    }
}