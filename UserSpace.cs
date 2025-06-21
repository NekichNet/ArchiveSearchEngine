using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ArchiveSearchEngine
{
    public class UserSpace
    {
        private string title_;
        private Page page_;
        public delegate string Method();
        public Method DelegateMethod { get; set; }

        public UserSpace(string Title, Page page) { 
            title_ = Title;
            page_ = page;
        }
        public UserSpace(string Title, Method method)
        {
            title_ = Title;
            DelegateMethod = method;
        }

        public string Invoke()
        {
            return DelegateMethod.Invoke();
        }


        public string Title { get { return title_; } }
        public Page Page { get { return page_; } }


    }
}
