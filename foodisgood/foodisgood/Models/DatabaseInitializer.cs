using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using foodisgood.Models;
using System.Data.Entity;
using Microsoft.VisualBasic.ApplicationServices;

namespace foodisgood.Models
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
    }
}