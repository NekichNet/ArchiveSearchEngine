using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveSearchEngine
{
    public class User
    {
        private string name_;
        private string password_;
        private int admissionLevel_;



        public string Name { get { return name_; }}
        public string Password { get { return password_; }}
        public int AdmissionLevel { get { return admissionLevel_; }}
        public string AdmissionLevelString { get
            {
                if (admissionLevel_ == 0)
                {
                    return "Обычный пользователь";
                }
                else if (admissionLevel_ == 1)
                {
                    return "Администратор";
                }
                else if (admissionLevel_ == 2)
                {
                    return "Главный администратор";
                }
                else
                {
                    return "Ошибка";
                }
            } }



        public User(string name, string password, int admissionLevel)
        {
            name_ = name;
            password_ = password;
            admissionLevel_ = admissionLevel;
        }

        public User(string name, string password) { 
            name_ = name;
            password_ = password;
            admissionLevel_ = 0;
        }




        public void ChangeName(string newName, bool isChangerIsAdmin)
        {
            if (isChangerIsAdmin) {  name_ = newName; } 
        }
        public void ChangePassword(string newPassword, bool isChangerIsAdmin)
        {
            if (isChangerIsAdmin) { password_ = newPassword; }
        }
        public void ChangeAdmin(string NewName, bool isChangerIsAdmin)
        {
            if (isChangerIsAdmin) { name_ = NewName; }
        }


    }
}
