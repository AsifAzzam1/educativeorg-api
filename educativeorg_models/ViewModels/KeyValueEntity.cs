using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace educativeorg_models.ViewModels
{
    public class KeyValueEntity<T>
    {
        public T Id { get; set; }
        public string Value { get; set; }
        public bool Active { get; set; } = false;
    }
}
