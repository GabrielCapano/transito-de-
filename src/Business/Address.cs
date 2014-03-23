using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using Business.Base;
using Models = Model.Models._Address;
using Repo = Repository._Address;

namespace Business
{
    public class Address : BaseBussines<Address>
    {
        private Address()
        {   
        }

        public Model.Models._Address.Address GetAddressById(int id)
        {
            return Repo.Address.Instance.GetById(id);
        }

        public List<SelectObject> GetStateToSelect()
        {
            return Repo.State.Instance.GetAllActive().Select(a => new SelectObject(a.Acronym, a.Id.ToString())).ToList();
        }


        public List<SelectObject> GetCitiesToSelect(int stateId)
        {
            return Repo.City.Instance.GetAllActiveByState(stateId).Select(a => new SelectObject(a.Name, a.Id.ToString())).ToList();
        }

    }
}
