using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace LouNexus.Data.DataBase
{
    public interface IDbConnectionProvider
    {
        IDbConnection CreateConnection();
    }
}
