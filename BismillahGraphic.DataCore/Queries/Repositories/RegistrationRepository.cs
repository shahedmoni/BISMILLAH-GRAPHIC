using System.Collections.Generic;
using System.Linq;

namespace BismillahGraphic.DataCore
{
    public class RegistrationRepository : Repository<Registration>, IRegistrationRepository
    {
        public RegistrationRepository(DataContext context) : base(context)
        {
        }
        public int GetRegID_ByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return -1;

            return Context.Registration.FirstOrDefault(r => r.UserName == userName).RegistrationID;
        }

        public ICollection<AdminInfo> GetSubAdminList()
        {
            return Context.Registration.Where(r => r.Type == "Sub-Admin").Select(r => new AdminInfo
            {
                UserName = r.UserName,
                Name = r.Name,
                RegistrationID = r.RegistrationID,
                Type = r.Type,
                FatherName = r.FatherName,
                Address = r.Address,
                DateofBirth = r.DateofBirth,
                Designation = r.Designation,
                Email = r.Email,
                Image = r.Image,
                NationalID = r.NationalID,
                Phone = r.Phone
            }).ToList();
        }

        public ICollection<DDL> SubAdmins()
        {
            return Context.Registration?.Where(r => r.Type == "Sub-Admin").Select(r =>
                new DDL { value = r.RegistrationID, label = r.Name + " (" + r.UserName + ")" }).ToList();
        }

        public AdminBasic GetAdminBasic(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return null;

            return Context.Registration.Where(r => r.UserName == userName).Select(r => new AdminBasic
            {
                Name = r.Name,
                Image = r.Image,
                RegistrationID = r.RegistrationID,
                Type = r.Type
            }).FirstOrDefault();
        }

        public AdminInfo GetAdminInfo(string userName)
        {
            if (string.IsNullOrEmpty(userName)) return null;

            return Context.Registration.Where(r => r.UserName == userName).Select(r => new AdminInfo
            {
                RegistrationID = r.RegistrationID,
                Name = r.Name,
                UserName = r.UserName,
                Type = r.Type,
                Image = r.Image,
                FatherName = r.FatherName,
                Designation = r.Designation,
                DateofBirth = r.DateofBirth,
                NationalID = r.NationalID,
                Address = r.Address,
                Phone = r.Phone,
                Email = r.Email
            }).FirstOrDefault();
        }


        public void UpdateCustom(string userName, AdminInfo reg)
        {
            var r = Context.Registration.FirstOrDefault(u => u.UserName == userName);

            r.FatherName = reg.FatherName;
            r.Name = reg.Name;
            r.Phone = reg.Phone;
            r.Email = reg.Email;
            if (reg.Image != null)
                r.Image = reg.Image;
            r.Address = reg.Address;
            r.Designation = reg.Designation;
            r.NationalID = reg.NationalID;
            r.DateofBirth = reg.DateofBirth;

            Update(r);
        }

    }
}
