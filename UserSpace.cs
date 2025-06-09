using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ArchiveSearchEngine
{
    public class UserSpace
    {
        private string title_;
        private Page page_;

        public UserSpace(string Title, Page page) { 
            title_ = Title;
            page_ = page;
        }

        public string Title { get { return title_; } }
        public Page Page { get { return page_; } }
    }
}
