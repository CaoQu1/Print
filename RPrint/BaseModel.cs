using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPrint
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        public virtual event PropertyChangedEventHandler PropertyChanged;
    }
}
